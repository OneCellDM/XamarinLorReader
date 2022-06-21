using HtmlAgilityPack;
using LorParser.Models;
using LorParser.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LorParser
{

    public static class Parser
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

                commentPage.PageCount = htmlNode?.SelectSingleNode("//div[@class='nav']")?
                    .ChildNodes
                    .Count - 2 ?? 0;

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
            
            return new FullNews(newsBase) { commentPage = commentPage };
        }

        public static NewsPage NewsPageParse(HtmlNode htmlNode)
        {
            IEnumerable<NewsPreview> Parse(HtmlNodeCollection articles)
            {
                if (articles != null)
                    foreach (var article in articles)
                        yield return ParserUtils.NewsPreviewParse(article);
            }

            return new NewsPage
            {
                Items = Parse(htmlNode?.SelectNodes("//article")).ToList(),

            };
        }



        private static class ParserUtils
        {
           

            public static Comment CommentParse(HtmlNode commentNode)
            {
                if (commentNode == null)
                    return null;

                var comment = new Comment();

                var titleNode = commentNode.SelectSingleNode("div[@class='title']");

                if (titleNode?.InnerLength > 0)
                {
                    var replyTime = titleNode?.SelectSingleNode("time")?.InnerText;
                    var replyA = titleNode?.SelectSingleNode("a");

                    var href = replyA?.Attributes["href"]?.Value;

                    comment.ReplyOn = new Reply
                    {
                        Time = replyTime ?? null,
                        Uri = LOR.LorUriBuilder(href) ?? null,
                        Name = titleNode.InnerText ?? null
                    };
                }

                var msg = commentNode?.SelectSingleNode("div[@class='msg-container']");
                var photo = msg?.SelectSingleNode("div[@class='userpic']/img");

                comment.UserPic = LOR.LorUriBuilder(photo?.Attributes["src"]?.Value) ?? null;

                var commentBodyNode = msg.SelectSingleNode("div[@class='msg_body message-w-userpic']");

                if (commentBodyNode != null)
                {
                    var textItems = commentBodyNode.ChildNodes.TakeWhile(x => x.Name != "div")
                        .Select(x => x.InnerHtml);

                    foreach (var item in textItems) comment.Text += item;

                    var signNode = commentBodyNode.SelectSingleNode("div[@class='sign']");
                    if (signNode != null) comment.Date = signNode.SelectSingleNode("time")?.InnerText;
                    comment.Name = signNode.SelectSingleNode("a")?.InnerText;
                }

                return comment;
            }

            public static IEnumerable<Tag> TagsParse(HtmlNode htmlNode)
            {
                var tagsNodes = (htmlNode?.SelectSingleNode("p[@class='tags']") ??
                                htmlNode?.SelectSingleNode("//p[@class='tags']"))?
                                .SelectNodes("a");
                if (tagsNodes != null)
                    foreach (var tag in tagsNodes)
                        yield return new Tag
                        {
                            Title = tag.InnerText,
                            Uri = tag.GetAttributeValue("href", "")
                        };
            }

            public static NewsBase NewsBaseParse(HtmlNode newsNode)
            {
                var commentCount = 0;
                var a = newsNode?.SelectSingleNode("h1//a") ??
                        newsNode?.SelectSingleNode("//h1[@itemprop='headline']//a");
                if (a is null)
                {
                    a = newsNode?.SelectSingleNode("a");
                    var cIndex = newsNode.InnerText.LastIndexOf("(");

                    var nbspIndex = newsNode.InnerText.LastIndexOf("&nbsp;");

                    if (cIndex > 0 && nbspIndex > 0)
                    {
                        var str = newsNode.InnerText.Substring(cIndex + 1, nbspIndex - cIndex - 1);
                        int.TryParse(str, out commentCount);
                    }
                }

                var url = a?.GetAttributeValue("href", string.Empty);

                var title = a?.InnerText;

                var body = GetBody(newsNode);

                var text = GetText(body);

                var signNode = GetSignNode(body);

                var author = signNode?.SelectSingleNode("a")?.InnerText;
                var time = signNode?.SelectSingleNode("time")?.InnerText;

                return new NewsBase
                {
                    Text = text?.InnerText ?? "",
                    Author = author ?? "",
                    Time = time ?? "",
                    Title = title ?? "",
                    NewsUri = url ?? "",
                    Tags = TagsParse(newsNode).ToList() ?? null
                };
            }

            public static NewsPreview NewsPreviewParse(HtmlNode newsNode)
            {
                var commentCount = 0;

                var group = newsNode?.SelectSingleNode("div[@class='group']")?.InnerText;

                var body = GetBody(newsNode);

                var comments = body?.SelectSingleNode("div[@class='nav']//a")?.InnerText?.Split('&');

                if (comments != null && comments.Length > 0) int.TryParse(comments[0], out commentCount);

                return new NewsPreview(NewsBaseParse(newsNode))
                {
                    Group = group,
                    CommentsCount = commentCount
                };
            }

            public static HtmlNode GetBody(HtmlNode newsNode)
            {
                return newsNode?.SelectSingleNode("div[@class='entry-body']") ??
                       newsNode?.SelectSingleNode("//div[@class='msg_body']");
            }

            public static HtmlNode GetText(HtmlNode bodyNode)
            {
                return bodyNode?.SelectSingleNode("div[@class='msg']") ??
                       bodyNode?.SelectSingleNode("//div[@itemprop='articleBody']");
            }

            public static HtmlNode GetSignNode(HtmlNode bodyNode)
            {
              
                return bodyNode?.SelectSingleNode("div[@class='sign']") ?? bodyNode?.SelectSingleNode("footer/div[@class='sign']");
            }
        }
    }
}
