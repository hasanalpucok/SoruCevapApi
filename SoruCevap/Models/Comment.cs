namespace SoruCevap.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string AuthorId { get; set; }
        public int PostId { get; set; }
        public int TimeStamp { get; set; }  

        public virtual Post? Post { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }


    }
}
