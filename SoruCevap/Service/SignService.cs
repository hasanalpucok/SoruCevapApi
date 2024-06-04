using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SoruCevap.Models;
using SoruCevap.Service.Abstraction;
using SoruCevapApi.Models.ViewModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SoruCevap.Helper.Jwt;
using SoruCevap.Models.Dto;
using Microsoft.EntityFrameworkCore;
using SoruCevapApi;

namespace SoruCevap.Service
{
    public class SignService : ISignService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IGenericRepository<Post> _postRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<Like> _likeRepository;
        



        public SignService(UserManager<User> userManager, SignInManager<User> signInManager, IGenericRepository<Post> postRepository, IGenericRepository<Comment> commentRepository, IGenericRepository<Like> likeRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
           
        }

        public async Task<ServiceResult<IEnumerable<UserDto>>> DeleteUser(string id)
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();
            result.Data = null;

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                result.Message = "User not found.";
                result.errorCode = 404;
                result.IsSuccess = false;
                return result;
            }

            //Kullanıcıya ait verileri de silebiliriz
            var soruListesi = _postRepository.GetQueryable().Where(x=> x.AuthorId ==  user.Id).ToList();
            if(soruListesi != null)
            {
                soruListesi.ForEach(soru =>
                {
                    _postRepository.Remove(soru);

                });
                
               
            }
            // Yorumları varsa silebiliriz
            var yorumListesi = _commentRepository.GetQueryable().Where(c => c.AuthorId == user.Id).ToList();
            if(yorumListesi != null)
            {
                yorumListesi.ForEach(yorum => { 
                    _commentRepository.Remove(yorum);
                });

            }
            //Begenileri varsa silebiliriz
            var begeniListesi = _likeRepository.GetQueryable().Where(l => l.AuthorId == user.Id).ToList();
            if(begeniListesi != null)
            {
                begeniListesi.ForEach(begeni => { 
                    _likeRepository.Remove(begeni);
                });
            }



            var deleteResult = await _userManager.DeleteAsync(user);
            if (deleteResult.Succeeded)
            {
                result.IsSuccess = true;
                result.Message = "User deleted successfully.";
            }
            else
            {
                result.Message = "Error deleting user.";
                result.errorCode = 500;
                result.IsSuccess = false;
            }

            return result;
        }
        public ServiceResult<IEnumerable<UserDto>> GetUsers()
        {
            try
            {
                var users = _userManager.Users
                                         .Select(u => new UserDto
                                         {
                                             Id = u.Id,
                                             Name = u.UserName,
                                             Email = u.Email
                                         }).ToList();

                return new ServiceResult<IEnumerable<UserDto>>()
                {
                    Data = users,
                    errorCode = 0,
                    IsSuccess = true,
                    Message = "Kullanicilar basariyla getirildi"



                };

            }
            catch (Exception)
            {

                return new ServiceResult<IEnumerable<UserDto>>()
                {
                    Data = null,
                    errorCode = 1,
                    IsSuccess = false,
                    Message = "Kullanicilar alinirken hata olustu"



                };
            }
        }

        // Kullanıcı girişi için metot
        public async Task<ServiceResult<IEnumerable<UserSession>>> GetUser(LoginModel model)
        {
            // Kullanıcı adı ve parola ile giriş yapılır
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                // Giriş başarılı ise JWT token oluşturulur
                var user = await _userManager.FindByNameAsync(model.UserName);
                var token = JwtMethods.CreateToken(user);
                var userSession = new UserSession
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Authorization = token
                };
                // Kullanıcının rollerini bir diziye al
                var roles = await _userManager.GetRolesAsync(user);

                // Rolleri bir virgülle ayırarak tek bir dizeye dönüştür
                var userRoles = string.Join(", ", roles);

                // Kullanıcı adını ve rollerini içeren bir mesaj oluştur
                var message = $"Kullanıcı: {user.UserName}, Roller: {userRoles}";
                return new ServiceResult<IEnumerable<UserSession>>
                {
                    IsSuccess = true,
                    Data = new List<UserSession> { userSession },
                    Message = message,
                };
            }

            return new ServiceResult<IEnumerable<UserSession>>
            {
                IsSuccess = false,
                Data = null,
                Message = "Geçersiz kullanıcı adı veya parola",
                errorCode = 3 // Geçersiz yetki veya yetkilendirme
            };
        }

        

        // Kullanıcı kaydı için metot
        public async Task<ServiceResult<IEnumerable<User>>> SaveUser(RegisterModel model)
        {
            // Yeni bir kullanıcı oluşturulur
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            // Kullanıcı oluşturma işlemi gerçekleştirilir
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                /*
                // Kullanıcıya başlangıçta rol ekleyebiliriz
                var roles = new string[] { "Admin", "Moderator" };

                // Assign the roles to the user
                var addToRoleResult = await _userManager.AddToRolesAsync(user, roles); */
                var data = new ServiceResult<IEnumerable<User>>
                {
                    IsSuccess = true,
                    Data = null,
                    errorCode = 0,
                    Message = "Succes"
                };
                return  data;
            }

            return new ServiceResult<IEnumerable<User>>
            {
                IsSuccess = false,
                Data = null,
                Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                errorCode = 1 // Geçersiz istek
            };
        }

        // JWT token oluşturma metodu
       
    }
}
