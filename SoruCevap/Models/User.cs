using Microsoft.AspNetCore.Identity;

namespace SoruCevap.Models
{
    public class User : IdentityUser
    {

        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }


    }
}
