using SoruCevap.Models.Dto;
using SoruCevap.Models.ViewModel;
using SoruCevapApi.Models.ViewModel;

namespace SoruCevap.Service.Abstraction
{
    public interface IPostService
    {
        public ServiceResult<IEnumerable<PostDto>> GetPosts();
        public ServiceResult<IEnumerable<PostDto>> GetPostById(int id);
        public ServiceResult<IEnumerable<PostDto>> EditPost(PostView view);
        public ServiceResult<IEnumerable<bool>> DeletePost(int id);
        public ServiceResult<IEnumerable<PostDto>> SavePost(PostView view);


    }
}
