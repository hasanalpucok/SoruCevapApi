namespace SoruCevap.Models.ViewModel
{
    public class LikeView
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public int ContentId { get; set; }
        public ItemType ContentType { get; set; }
        public int TimeStamp { get; set; }
        public bool IsLiked { get; set; } = false;


        public enum ItemType
        {
            Post,
            Comment
        }
    }
}
