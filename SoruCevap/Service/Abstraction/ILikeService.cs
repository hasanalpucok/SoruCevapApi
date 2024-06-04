using SoruCevap.Models.Dto;
using SoruCevap.Models.ViewModel;

namespace SoruCevap.Service.Abstraction
{
    public interface ILikeService
    {
        
        public ServiceResult<IEnumerable<LikeView>> EditLike(LikeView view);
        public ServiceResult<IEnumerable<bool>> DeleteLike(int id);
        public ServiceResult<IEnumerable<LikeView>> SaveLike(LikeView view);
    }
}
