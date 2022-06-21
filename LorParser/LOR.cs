using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using LorParser.Models;
using LorParser.Models.Pages;

namespace LorParser
{
    public static class LOR
    {
        public enum HttpProtocolEnum
        {
            Http,
            Https
        }

        public static string Domain { get; set; } = "www.linux.org.ru";
        public static HttpProtocolEnum HttpProtocol { get; set; } = HttpProtocolEnum.Https;

        public static async Task<NewsPage> GetNewsList(int offset = 0)
        {
            try
            {
                var res = await Utils.GetRequest(LorUriBuilder($"news/?offset={offset}"));

                var document = res.StrToNode();

                var parsedPage = Parser.NewsPageParse(document);

                parsedPage.Offset = parsedPage.Items.Count + offset;

                return parsedPage;
            }
            catch (HttpRequestException httpex)
            {
                throw httpex;
            }
            catch (Exception ex)
            {
                throw new ParserException(ex.Message);
            }
        }

        public static async Task<FullNews> GetNews(NewsPreview newsPreview) => 
            await GetNews(newsPreview.NewsUri);
        
        public static async Task<CommentPage> GetCommentPage(NewsBase newsBase, int page) =>
               await GetCommentPage(newsBase.NewsUri,page);
        
        public static async Task<CommentPage> GetCommentPage(string uri, int page)
        {
            var uris = LorUriBuilder(uri) + "/page" + page;
            var res = await Utils.GetRequest(uris);

            var parsedData = Parser.CommentPageParse(res.StrToNode());
            parsedData.NewsUrl = uri;
            return parsedData;
        }
        public static async Task<FullNews> GetNews(string uri)
        {
            var res = await Utils.GetRequest(LorUriBuilder(uri));
            var node = res.StrToNode();
            var parsedData = Parser.NewsParse(node);

            if (parsedData.commentPage!=null) 
                parsedData.commentPage.NewsUrl = parsedData?.NewsUri;
            return parsedData;
        }

        public static string LorUriBuilder(string uri)
        {
            var _uri = uri;
            if (uri == null)
                return null;

            var pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            var rgx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (rgx.IsMatch(_uri)) return _uri;

            if (_uri.StartsWith("/")) _uri = uri.Remove(0, 1);
            _uri = HttpProtocol.ToString().ToLower() + "://" + Domain + @"/" + _uri;

            if (rgx.IsMatch(_uri)) return _uri;

            throw new UriFormatException(uri);
        }

       
    }
}