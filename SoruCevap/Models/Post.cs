using SoruCevap.Helper;

namespace SoruCevap.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AuthorId { get; set; }
        public string Category { get; set; } = string.Empty;

        public int TimeStamp { get; set; } = HelperMethods.GetUnixTimeUT3();

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual User? User { get; set; }

    }
}
