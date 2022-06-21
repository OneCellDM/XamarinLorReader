using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace LorParser
{
    public static class Exstensions
    {
        public static HtmlNode StrToNode(this string data)
        {
            var document = new HtmlDocument();
            document.LoadHtml(data);
            return document.DocumentNode;
        }
    }
}
