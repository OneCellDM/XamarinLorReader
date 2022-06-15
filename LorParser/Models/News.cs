using LorParser.Models.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace LorParser.Models
{
    public class FullNews:NewsBase
    {
        public CommentPage commentPage { get; set; }
        public FullNews()
        {

        }
        public FullNews(NewsBase newsBase)
        {
            this.Title = newsBase.Title;
            this.Tags = newsBase.Tags;
            
                
            this.Text = newsBase.Text;
            this.Author = newsBase.Author;
            this.NewsUri = newsBase.NewsUri;
            this.Time = newsBase.Time;
        }
    }
}
