using System.Collections.Generic;

namespace LorParser.Models.Pages
{
    public class PageBase<T>
    {
        public List<T> Items { get; set; }
    }
}