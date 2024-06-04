namespace SoruCevap.Models.ViewModel
{
    public class CommentView
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int TimeStamp { get; set; }
    }
}
