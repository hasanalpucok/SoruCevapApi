using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SoruCevap.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // SQLite veritabanı bağlantısı için bağlantı dizesini burada belirtin
            optionsBuilder.UseSqlServer("Server=localhost\\HASANALP;Database=master;Trusted_Connection=True;TrustServerCertificate=True");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Post tablosunun primary key tanımı
            modelBuilder.Entity<Post>()
                .HasKey(p => p.Id);

            // Comment tablosunun primary key tanımı
            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);

            // User ve Post arasındaki ilişki (Bir kullanıcının birden çok post'u olabilir)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.AuthorId).OnDelete(DeleteBehavior.Cascade); // Kaskad silmeyi devre dışı bırak;

            // User ve Comment arasındaki ilişki (Bir kullanıcının birden çok yorumu olabilir)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.AuthorId).OnDelete(DeleteBehavior.Cascade); // Kaskad silmeyi devre dışı bırak;

            // User ve Like arasındaki ilişki (Bir kullanıcının birden çok beğenisi olabilir)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.AuthorId).OnDelete(DeleteBehavior.Cascade);



            // Post ve Comment arasındaki ilişki (Bir postun birden çok yorumu olabilir)
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId).OnDelete(DeleteBehavior.Cascade); // Kaskad silmeyi devre dışı bırak;

           
        }


    }

}


