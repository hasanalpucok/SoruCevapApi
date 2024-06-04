using SoruCevap.Models.Dto;
using SoruCevap.Models.ViewModel;
using SoruCevap.Models;
using SoruCevap.Service.Abstraction;
using SoruCevapApi;
using SoruCevap.Helper;

namespace SoruCevap.Service
{
    public class CommentService:ICommentService
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<Post> _postRepository;

        public CommentService(IGenericRepository<Comment> commentRepository, IGenericRepository<Post> postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        public ServiceResult<IEnumerable<bool>> DeleteComment(int id)
        {
            var comment = _commentRepository.FindById(id);
            if (comment == null)
            {
                return new ServiceResult<IEnumerable<bool>>
                {
                    IsSuccess = false,
                    Message = "Yorum bulunamadı",
                    errorCode = 2 // Not Found
                };
            }

            _commentRepository.Delete(id);
            _commentRepository.SaveChanges();

            return new ServiceResult<IEnumerable<bool>>
            {
                IsSuccess = true,
                Data = new List<bool> { true },
                Message = "Yorum başarıyla silindi"
            };
        }

        public ServiceResult<IEnumerable<CommentDto>> EditComment(CommentView view)
        {
            // Yorumu bul
            var comment = _commentRepository.FindById(view.Id);
            if (comment == null)
            {
                return new ServiceResult<IEnumerable<CommentDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Yorum bulunamadı",
                    errorCode = 404 // Not Found
                };
            }

            // Yorumu güncelle
            comment.Content = view.Content;
            comment.TimeStamp = HelperMethods.GetUnixTimeUT3();

            _commentRepository.SaveChanges();

            // Güncellenmiş yorum DTO'sunu oluştur
            var updatedCommentDto = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                AuthorName = comment.User?.UserName, // Yorumun yazar adını al
                DislikeCount = 0, // Örnek olarak sıfır ata, gerekirse güncelle
                LikeCount = 0, // Örnek olarak sıfır ata, gerekirse güncelle
                TimeStamp = comment.TimeStamp
            };

            return new ServiceResult<IEnumerable<CommentDto>>
            {
                IsSuccess = true,
                Data = new List<CommentDto> { updatedCommentDto },
                Message = "Yorum başarıyla güncellendi"
            };
        }

        public ServiceResult<IEnumerable<CommentDto>> SaveComment(CommentView view)
        {
            // Yorum yapılacak olan post'un var olduğunu kontrol et
            var existingPost = _postRepository.FindById(view.PostId);
            if (existingPost == null)
            {
                return new ServiceResult<IEnumerable<CommentDto>>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = "Yorum yapılacak post bulunamadı",
                    errorCode = 404 // Not Found
                };
            }
            

            // Yeni bir Comment oluştur
            var newComment = new Comment
            {
                Content = view.Content,
                AuthorId = view.AuthorId,
                PostId = view.PostId,
                TimeStamp = HelperMethods.GetUnixTimeUT3()
            };

            // Comment'u kaydet
            _commentRepository.Save(newComment);

            // Yeni comment DTO'sunu oluştur
            var newCommentDto = new CommentDto
            {
                Id = newComment.Id,
                Content = newComment.Content,
                PostId = newComment.PostId,
                AuthorName = newComment.User?.UserName, // Yorumun yazar adını al
                DislikeCount = 0, // Örnek olarak sıfır ata, gerekirse güncelle
                LikeCount = 0, // Örnek olarak sıfır ata, gerekirse güncelle
                TimeStamp = newComment.TimeStamp
            };

            return new ServiceResult<IEnumerable<CommentDto>>
            {
                IsSuccess = true,
                Data = new List<CommentDto> { newCommentDto },
                Message = "Yorum başarıyla kaydedildi"
            };
        }

    }
}
