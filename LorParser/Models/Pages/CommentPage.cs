
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LorParser.Models.Pages
{
  
    public class CommentPage:PageBase<Comment>
    {
        public int PageCount { get; set; }
        public int PageNumber { get; set; }    
    }
}
