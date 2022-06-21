namespace LorParser.Models.Pages
{
    public class CommentPage : PageBase<Comment>
    {
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public string NewsUrl { get; set; }
    }
}