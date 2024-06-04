namespace SoruCevap.Models.ViewModel
{
    public class PostView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }

        public string Category {  get; set; }
    }
}
