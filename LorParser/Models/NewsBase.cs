﻿using System.Collections.Generic;

namespace LorParser.Models
{
    public class NewsBase
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public List<Tag> Tags { get; set; }
        public string Time { get; set; }
        public string NewsUri { get; set; }
        public string ImageUri { get; set; }
    }
}