using HtmlAgilityPack;
using LorParser.Models;
using LorParser.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LorParser
{
    public static class LOR
    {
        public enum HttpProtocolEnum
        {
            Http,
            Https,
        }
        public static string Domain { get; set; } = "www.linux.org.ru";
        public static HttpProtocolEnum HttpProtocol { get; set; } = HttpProtocolEnum.Https;
        public static async Task<Models.Pages.NewsPage> GetNewsList(int offset = 0)
        {
            try
            {
               

                var res = await Utils.GetRequest(LorUriBuilder($"news/?offset={offset}"));

                var document = Parser.ParserUtils.StrToNode(ref res);
               
                var parsedPage = Parser.NewsPageParse(document);

                parsedPage.Offset = parsedPage.Items.Count + offset;

                return parsedPage;
            }
            catch (HttpRequestException httpex)
            {
                throw httpex;
            }
            catch(Exception ex)
            {
                throw new ParserException(ex.Message);
            }
        }
        public static async Task<Models.FullNews> GetNews(NewsPreview newsPreview)
        {
           var res = await GetNews(newsPreview.NewsUri);
            
           return new FullNews();
        }
        public static async Task<Models.FullNews> GetNews(string uri)
        {
           

            var res = await Utils.GetRequest(LorUriBuilder(uri));
            var node = Parser.ParserUtils.StrToNode(ref res);
            var res2 = Parser.NewsParse(node);
            return res2;
           
        }

        private static string LorUriBuilder(string uri)
        {
            string _uri = uri;
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (rgx.IsMatch(_uri)) return _uri;

            if (_uri.StartsWith("/"))
            {
                _uri = uri.Remove(0, 1);
            }
            _uri = HttpProtocol.ToString().ToLower() + "://" + Domain + @"/" + _uri;

            if (rgx.IsMatch(_uri)) return _uri;

            throw new UriFormatException(uri);


        }
        private static class Parser
        {
            public static CommentPage CommentPageParse(HtmlNode htmlNode)
            {
                CommentPage commentPage = null;

                var commentNodes = htmlNode?.SelectNodes("//article[@itemprop='comment']");

                if (commentNodes != null)
                {

                    commentPage = new CommentPage();

                    commentPage.PageNumber = int.Parse(htmlNode?.SelectSingleNode("//strong[@class='page-number']")?
                                                           .InnerText ?? "0");

                    commentPage.PageCount = (htmlNode?.SelectSingleNode("//div[@class='nav']")?
                                                       .ChildNodes
                                                       .Count - 2) ?? 0;

                    commentPage.Items = new List<Comment>();

                    foreach (var commentNode in commentNodes)
                    {
                        var commentParsed = ParserUtils.CommentParse(commentNode);

                        if (commentParsed != null)
                            commentPage.Items.Add(commentParsed);
                    }

                }
                return commentPage;

            }
            public static FullNews NewsParse(HtmlNode newsNode)
            {

                var newsBase = ParserUtils.NewsBaseParse(newsNode);
                var commentPage = CommentPageParse(newsNode);

                return new FullNews(newsBase) { commentPage = commentPage, };

            }
            public static NewsPage NewsPageParse(HtmlNode htmlNode)
            {
                IEnumerable<NewsPreview> Parse(HtmlNodeCollection articles)
                {
                    if (articles != null)
                    {
                        foreach (var article in articles)
                        {
                            
                          yield return ParserUtils.NewsPreviewParse(article);
                           
                        }
                    }
                }

                return new NewsPage() 
                { 
                    Items = Parse(htmlNode?.SelectNodes("//article")).ToList()
                };
              
            }
           
            public static class ParserUtils
            {
                public static HtmlNode StrToNode(ref string data)
                {
                    HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                    document.LoadHtml(data);
                    return document.DocumentNode;
                }
                public static Comment CommentParse(HtmlNode commentNode)
                {
                    if (commentNode == null)
                        return null;

                    Comment comment = new Comment();

                    var titleNode = commentNode.SelectSingleNode("div[@class='title']");

                    if (titleNode?.InnerLength > 0)
                    {
                        var replyTime = titleNode?.SelectSingleNode("time")?.InnerText;
                        var replyA = titleNode?.SelectSingleNode("a");

                        var href = replyA?.Attributes["href"]?.Value;

                        comment.ReplyOn = new Reply()
                        {
                            Time = replyTime ?? null,
                            Uri = href ?? null,
                            Name = titleNode.InnerText ?? null,
                        };

                    }

                    var msg = commentNode?.SelectSingleNode("div[@class='msg-container']");
                    var photo = msg?.SelectSingleNode("div[@class='userpic']/img");

                    comment.UserPic = photo?.Attributes["src"]?.Value ?? null;

                    var commentBodyNode = msg.SelectSingleNode("div[@class='msg_body message-w-userpic']");

                    if (commentBodyNode != null)
                    {

                        var textItems = commentBodyNode.ChildNodes.TakeWhile(x => x.Name != "div").
                                                                   Select(x => x.InnerHtml);

                        foreach (var item in textItems) comment.Text += item;

                        var signNode = commentBodyNode.SelectSingleNode("div[@class='sign']");
                        if (signNode != null)
                        {
                            comment.Date = signNode.SelectSingleNode("time")?.InnerText;
                        }

                    }

                    return comment;
                }
                public static IEnumerable<Tag> TagsParse(HtmlNode htmlNode)
                {
                    var tagsNodes = htmlNode?.SelectSingleNode("p[@class='tags']")?.SelectNodes("a");
                    if (tagsNodes != null)
                    {
                        foreach (var tag in tagsNodes)
                        {
                            yield return (new Tag()
                            {
                                Title = tag.InnerText,
                                Uri = tag.GetAttributeValue("href", ""),
                            });
                        }

                    }

                }
                public static NewsBase NewsBaseParse(HtmlNode newsNode)
                {
                    int commentCount = 0;
                    var a = newsNode?.SelectSingleNode("h1//a") ?? newsNode?.SelectSingleNode("//h1[@itemprop='headline']//a");
                    if (a is null)
                    {
                        a = newsNode?.SelectSingleNode("a");
                        int cIndex = newsNode.InnerText.LastIndexOf("(");

                        int nbspIndex = newsNode.InnerText.LastIndexOf("&nbsp;");

                        if (cIndex > 0 && nbspIndex > 0)
                        {
                            var str = newsNode.InnerText.Substring((cIndex + 1), (nbspIndex - cIndex) - 1);
                            int.TryParse(str, out commentCount);
                        }

                    }
                    
                    string url = a?.GetAttributeValue("href", string.Empty);

                    string title = a?.InnerText;

                    var body = GetBody(newsNode);

                    var text = GetText(body);

                    var signNode = GetSignNode(body);

                    string author = signNode?.SelectSingleNode("a")?.InnerText;
                    string time = signNode?.SelectSingleNode("time")?.InnerText;

                    return new NewsBase()
                    {
                        Text = text?.InnerText ?? "",
                        Author = author ?? "",
                        Time = time ?? "",
                        Title = title ?? "",
                        NewsUri = url ?? "",
                        Tags = TagsParse(newsNode).ToList() ?? null,
                    };
                }
                public static NewsPreview NewsPreviewParse(HtmlNode newsNode)
                {

                    List<Tag> tagsList = new List<Tag>();

                    int commentCount = 0;

                    string group = newsNode?.SelectSingleNode("div[@class='group']")?.InnerText;

                    var body = ParserUtils.GetBody(newsNode);

                    var comments = body?.SelectSingleNode("div[@class='nav']//a")?.InnerText?.Split('&');

                    if (comments != null && comments.Length > 0)
                    {
                        int.TryParse(comments[0], out commentCount);
                    }

                    return new NewsPreview(ParserUtils.NewsBaseParse(newsNode))
                    {
                        Group = group,
                        CommentsCount = commentCount,
                    };
                }
                public static HtmlNode GetBody(HtmlNode newsNode)
                {
                    return newsNode?.SelectSingleNode("div[@class='entry-body']")?? newsNode?.SelectSingleNode("//div[@class='msg_body']");
                }
                public static HtmlNode GetText(HtmlNode bodyNode)
                {
                    return bodyNode?.SelectSingleNode("div[@class='msg']")??bodyNode?.SelectSingleNode("//div[@itemprop='articleBody']");
                }
                public static HtmlNode GetSignNode(HtmlNode bodyNode)
                {
                    return bodyNode?.SelectSingleNode("div[@class='sign']") ?? 
                           bodyNode?.SelectSingleNode("footer")?.SelectSingleNode("/div[@itemprop='sign']");
                }
            }
        }
       
    }
  
}