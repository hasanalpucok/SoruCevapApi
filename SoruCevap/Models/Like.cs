namespace SoruCevap.Models
{
    public class Like
    {
        public int Id { get; set; } = 0;
        public int ContentId { get; set; } // Beğenilen öğenin kimliği (örneğin post veya comment)
        public ItemType ContentType { get; set; } // Beğenilen öğenin türü (post veya comment)
        public string AuthorId { get; set; } // Beğeni atan kullanıcının kimliği
        public int TimeStamp { get; set; } // Beğeninin zaman damgası
        public bool IsLiked { get; set; } = false;

        // Bir enum, beğenilen öğenin türünü belirtmek için kullanılabilir
        public enum ItemType
        {
            Post,
            Comment
        }
       
        public virtual User? User { get; set; }
    }
}
