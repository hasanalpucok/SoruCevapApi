using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SoruCevap.Helper;
using SoruCevap.Models;
using SoruCevap.Models.Dto;
using SoruCevap.Models.ViewModel;
using SoruCevap.Service.Abstraction;
using SoruCevapApi;

namespace SoruCevap.Service
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IGenericRepository<Like> _likeRepository;
        private readonly IGenericRepository<Comment> _commentRepository;

        public PostService(IGenericRepository<Post> postRepository, IGenericRepository<Like> likeRepository, IGenericRepository<Comment> commentRepository)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _commentRepository = commentRepository;
        }

        public ServiceResult<IEnumerable<bool>> DeletePost(int id)
        {
            var post = _postRepository.GetQueryable().Include(p => p.Comments).Include(p => p.Likes).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return new ServiceResult<IEnumerable<bool>>
                {
                    IsSuccess = false,
                    Message = "Post bulunamadı",
                    errorCode = 404 // Not Found
                };
            }
        


            _postRepository.Remove(post);
            _postRepository.SaveChanges();

            return new ServiceResult<IEnumerable<bool>>
            {
                IsSuccess = true,
                Data = new List<bool> { true },
                Message = "Post başarıyla silindi"
            };
        }

        public ServiceResult<IEnumerable<PostDto>> EditPost(PostView view)
        {
            // Post'u bul
            var post = _postRepository.FindById(view.Id);
            if (post == null)
            {
                return new ServiceResult<IEnumerable<PostDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Post bulunamadı",
                    errorCode = 2 // Not Found
                };
            }

            // Post'u güncelle
            post.Title = view.Title;
            post.Description = view.Description;
            post.Category = view.Category;
            post.TimeStamp = HelperMethods.GetUnixTimeUT3();

            _postRepository.SaveChanges();

            // Güncellenmiş post DTO'sunu oluştur
            var updatedPostDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                AuthorName = post.User?.UserName, // Yazarın adını al
                Category = post.Category,
                LikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == post.Id && x.ContentType == Like.ItemType.Post && x.IsLiked),
                DislikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == post.Id && x.ContentType == Like.ItemType.Post && !x.IsLiked),


                Comments = _commentRepository.GetQueryable().Include(c => c.User).Where(p => p.PostId == post.Id).Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    PostId = c.PostId,
                    AuthorName = c.User.UserName ?? "", // Yorum yapanın adını al
                    DislikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == c.Id && x.ContentType == Like.ItemType.Comment && !x.IsLiked), // Örnek olarak sıfır ata, gerekirse güncelle
                    LikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == c.Id && x.ContentType == Like.ItemType.Comment && x.IsLiked), // Örnek olarak sıfır ata, gerekirse güncelle
                    TimeStamp = c.TimeStamp
                }).ToList(),

            };

            return new ServiceResult<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Data = new List<PostDto> { updatedPostDto },
                Message = "Post başarıyla güncellendi"
            };
        }

        public ServiceResult<IEnumerable<PostDto>> GetPostById(int id)
        {
            var post =  _postRepository.GetQueryable().Include(p => p.User).Include(p => p.Comments).FirstOrDefault( x => x.Id == id);
            if (post == null)
            {
                return new ServiceResult<IEnumerable<PostDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Post bulunamadı",
                    errorCode =2 // Not Found
                };
            }

            var postDto = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                LikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == post.Id && x.ContentType == Like.ItemType.Post && x.IsLiked),
                DislikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == post.Id && x.ContentType == Like.ItemType.Post && !x.IsLiked),
                AuthorName = post.User?.UserName, // Yazarın adını al
                Category = post.Category,
                Comments = _commentRepository.GetQueryable().Include(c => c.User).Where(p => p.PostId == post.Id).Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    PostId = c.PostId,
                    AuthorName = c.User.UserName ?? "", // Yorum yapanın adını al
                    DislikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == c.Id && x.ContentType == Like.ItemType.Comment && !x.IsLiked), // Örnek olarak sıfır ata, gerekirse güncelle
                    LikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == c.Id && x.ContentType == Like.ItemType.Comment && x.IsLiked), // Örnek olarak sıfır ata, gerekirse güncelle
                    TimeStamp = c.TimeStamp
                }).ToList(),
                
            };

            return new ServiceResult<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Data = new List<PostDto> { postDto },
                Message = "Post başarıyla bulundu"
            };
        }

        public ServiceResult<IEnumerable<PostDto>> GetPosts()
        {
            var posts = _postRepository.GetQueryable().Include(p => p.User).Include(p => p.Comments).ToList();

            var postDtos = posts.Select(post => new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                AuthorName = post.User?.UserName, // Yazarın adını al
                Category = post.Category,
                LikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == post.Id && x.ContentType == Like.ItemType.Post && x.IsLiked),
                DislikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == post.Id && x.ContentType == Like.ItemType.Post && !x.IsLiked),
                Comments = _commentRepository.GetQueryable().Include(c => c.User).Where(p => p.PostId == post.Id).Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    PostId = c.PostId,
                    AuthorName = c.User.UserName ?? "", // Yorum yapanın adını al
                    DislikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == c.Id && x.ContentType == Like.ItemType.Comment && !x.IsLiked), // Örnek olarak sıfır ata, gerekirse güncelle
                    LikeCount = _likeRepository.GetQueryable().Count(x => x.ContentId == c.Id && x.ContentType == Like.ItemType.Comment && x.IsLiked), // Örnek olarak sıfır ata, gerekirse güncelle
                    TimeStamp = c.TimeStamp
                }).ToList(),
            }).ToList();
            

            return new ServiceResult<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Data = postDtos,
                Message = "Postlar başarıyla bulundu"
            };
        }

        public ServiceResult<IEnumerable<PostDto>> SavePost(PostView view)
        {
            // Aynı başlıkta başka bir post var mı kontrol et
            var existingPost = _postRepository.GetQueryable().FirstOrDefault(p => p.Title == view.Title);
            if (existingPost != null)
            {
                return new ServiceResult<IEnumerable<PostDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Aynı başlıkta başka bir post zaten var",
                    errorCode = 2 // Bad Request
                };
            }

            // Yeni bir Post oluştur
            var newPost = new Post
            {
                Title = view.Title,
                Description = view.Description,
                Category = view.Category,
                AuthorId = view.AuthorId, // Bu kısmın nasıl doldurulacağına dikkat edin
                TimeStamp = HelperMethods.GetUnixTimeUT3() // Şu anki zamanı ata
            };

            // Post'u kaydet
            _postRepository.Save(newPost);

            // Yeni post DTO'sunu oluştur
            var newPostDto = new PostDto
            {
                Id = newPost.Id,
                Title = newPost.Title,
                Description = newPost.Description,
                AuthorName = newPost.User?.UserName, // Yazarın adını al
                Category = newPost.Category,
                
                Comments = new List<CommentDto>() // Yorumlar henüz eklenmediği için boş liste ata
            };

            return new ServiceResult<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Data = new List<PostDto> { newPostDto },
                Message = "Post başarıyla kaydedildi"
            };
        }
    }
}
