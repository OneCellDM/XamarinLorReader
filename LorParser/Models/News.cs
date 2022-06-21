using LorParser.Models.Pages;

namespace LorParser.Models
{
    public class FullNews : NewsBase
    {
        public FullNews()
        {
        }

        public FullNews(NewsBase newsBase)
        {
            Title = newsBase.Title;
            Tags = newsBase.Tags;


            Text = newsBase.Text;
            Author = newsBase.Author;
            NewsUri = newsBase.NewsUri;
            Time = newsBase.Time;
        }

        public CommentPage commentPage { get; set; }
    }
}