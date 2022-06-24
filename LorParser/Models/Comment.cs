namespace LorParser.Models
{
    public class Comment
    {
        public string UserPic { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public Reply ReplyOn { get; set; }
    }
}