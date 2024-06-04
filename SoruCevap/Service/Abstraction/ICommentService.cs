using SoruCevap.Models.Dto;
using SoruCevap.Models.ViewModel;

namespace SoruCevap.Service.Abstraction
{
    public interface ICommentService
    {
        public ServiceResult<IEnumerable<bool>> DeleteComment(int id);
        public ServiceResult<IEnumerable<CommentDto>> EditComment(CommentView view);
        public ServiceResult<IEnumerable<CommentDto>> SaveComment(CommentView view);
    }
}
