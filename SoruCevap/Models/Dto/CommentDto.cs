namespace SoruCevap.Models.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public string AuthorName { get; set; }
        public int DislikeCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int TimeStamp { get; set; }
    }
}
