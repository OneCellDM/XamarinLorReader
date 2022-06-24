namespace LorParser.Models.Pages
{
    public class NewsPage : PageBase<NewsPreview>
    {
        public int Offset { get; set; }
        public bool IsLast { get; set; }
    }
}