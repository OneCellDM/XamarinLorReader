using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LorParser.Models.Pages
{
    public class NewsPage : PageBase<NewsPreview>
    {
        public int Offset { get; set; }
    }
}
