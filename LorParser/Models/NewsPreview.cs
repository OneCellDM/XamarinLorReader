using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LorParser.Models
{
    public class Tag
    {
        public string Uri { get; set; }
        public string Title { get; set; }
    }
    public class NewsPreview:NewsBase
    {
        public string Group { get; set; }
      
        public int CommentsCount { get; set; }

        public NewsPreview(NewsBase newsBase)
        {
            this.Text = newsBase.Text;
            this.Author = newsBase.Author;
            this.NewsUri = newsBase.NewsUri;
            this.Time = newsBase.Time;
            this.Title = newsBase.Title;
        }
        public NewsPreview() { }

    }

}
