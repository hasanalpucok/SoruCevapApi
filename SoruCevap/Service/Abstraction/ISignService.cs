using SoruCevap.Models;
using SoruCevap.Models.Dto;
using SoruCevapApi.Models.ViewModel;

namespace SoruCevap.Service.Abstraction
{
    public interface ISignService
    {
        public Task<ServiceResult<IEnumerable<User>>> SaveUser(RegisterModel model);
        public Task<ServiceResult<IEnumerable<UserSession>>> GetUser(LoginModel model);
        public ServiceResult<IEnumerable<UserDto>> GetUsers();
        public Task<ServiceResult<IEnumerable<UserDto>>> DeleteUser(string id);
    }
}
