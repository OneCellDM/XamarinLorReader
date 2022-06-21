namespace LorParser.Models
{
    public class Tag
    {
        public string Uri { get; set; }
        public string Title { get; set; }
    }

    public class NewsPreview : NewsBase
    {
        public NewsPreview(NewsBase newsBase)
        {
            Text = newsBase.Text;
            Author = newsBase.Author;
            NewsUri = newsBase.NewsUri;
            Time = newsBase.Time;
            Title = newsBase.Title;
        }

        public NewsPreview()
        {
        }

        public string Group { get; set; }

        public int CommentsCount { get; set; }
    }
}