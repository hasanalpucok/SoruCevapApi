using Microsoft.Extensions.Hosting;
using SoruCevap.Helper;
using SoruCevap.Models;
using SoruCevap.Models.ViewModel;
using SoruCevap.Service.Abstraction;
using SoruCevapApi;

namespace SoruCevap.Service
{
    public class LikeService : ILikeService
    {
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<Like> _likeRepository;
        public LikeService(IGenericRepository<Post> postRepository, IGenericRepository<Comment> commentRepository, IGenericRepository<Like> likeRepository)
        {
            _postRepository = postRepository;

            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
        }

        public ServiceResult<IEnumerable<bool>> DeleteLike(int id)
        {
            var like = _likeRepository.FindById(id);
            if (like == null)
            {
                return new ServiceResult<IEnumerable<bool>>
                {
                    IsSuccess = false,
                    Message = "Like bulunamadı",
                    errorCode = 2 // Not Found
                };
            }
            _likeRepository.Delete(id);
            _likeRepository.SaveChanges();
            return new ServiceResult<IEnumerable<bool>>
            {
                IsSuccess = true,
                errorCode = 0,
                Message = "Like başarılı şekilde silindi"

            };
        }

        public ServiceResult<IEnumerable<LikeView>> EditLike(LikeView view)
        {
            var like = _likeRepository.FindById(view.Id);
            if (like == null)
            {
                return new ServiceResult<IEnumerable<LikeView>>
                {
                    IsSuccess = false,
                    Message = "Like bulunamadı",
                    errorCode = 2, // Not Found,
                    Data = null
                };
            }
            like.IsLiked = view.IsLiked;
            like.TimeStamp = HelperMethods.GetUnixTimeUT3();
            _likeRepository.SaveChanges();
            return new ServiceResult<IEnumerable<LikeView>>
            {
                IsSuccess = true,
                errorCode = 0,
                Message = "Like başarılı şekilde güncellendi"

            };


        }

        public ServiceResult<IEnumerable<LikeView>> SaveLike(LikeView view)
        {
            if(view.ContentType == LikeView.ItemType.Comment)
            {
                var comment = _commentRepository.FindById(view.ContentId);
                
                if(comment == null)
                {
                    return new ServiceResult<IEnumerable<LikeView>>
                    {
                        IsSuccess = false,
                        Message = "Yorum bulunamadı",
                        errorCode = 2, // Not Found,
                        Data = null
                    };

                }
                else
                {
                    var likeExists = _likeRepository.GetQueryable().FirstOrDefault(like => like.ContentId == view.ContentId && like.ContentType == Like.ItemType.Comment && like.AuthorId == view.AuthorId);
                    if(likeExists != null)
                    {
                        likeExists.IsLiked = view.IsLiked;
                        return new ServiceResult<IEnumerable<LikeView>>
                        {
                            IsSuccess = true,
                            errorCode = 0,
                            Message = "Like başarılı şekilde eklendi"

                        };

                    }



                    var like = new Like { 
                        AuthorId = view.AuthorId,
                        ContentId = view.ContentId,
                        ContentType = Like.ItemType.Comment,
                        Id = 0,
                        IsLiked = true,
                        TimeStamp = HelperMethods.GetUnixTimeUT3(),

                    };
                    _likeRepository.Save(like);
                    return new ServiceResult<IEnumerable<LikeView>>
                    {
                        IsSuccess = true,
                        errorCode = 0,
                        Message = "Like başarılı şekilde eklendi"

                    };



                }
            }
            else
            {
                var post = _postRepository.FindById(view.ContentId);
                if(post == null)
                {
                    return new ServiceResult<IEnumerable<LikeView>>
                    {
                        IsSuccess = false,
                        Message = "Post bulunamadı",
                        errorCode = 2, // Not Found,
                        Data = null
                    };

                }
                else
                {
                    var likeExists = _likeRepository.GetQueryable().FirstOrDefault(like => like.ContentId == view.ContentId && like.ContentType == Like.ItemType.Post && like.AuthorId == view.AuthorId);
                    if (likeExists != null)
                    {
                        likeExists.IsLiked = view.IsLiked;
                        return new ServiceResult<IEnumerable<LikeView>>
                        {
                            IsSuccess = true,
                            errorCode = 0,
                            Message = "Like başarılı şekilde eklendi"

                        };


                    }
                    var like = new Like
                    {
                        AuthorId = view.AuthorId,
                        ContentId = view.ContentId,
                        ContentType = Like.ItemType.Post,
                        Id = 0,
                        IsLiked = true,
                        TimeStamp = HelperMethods.GetUnixTimeUT3(),

                    };
                    _likeRepository.Save(like);
                    return new ServiceResult<IEnumerable<LikeView>>
                    {
                        IsSuccess = true,
                        errorCode = 0,
                        Message = "Like başarılı şekilde eklendi"

                    };

                }

            }

        }
    }
}
