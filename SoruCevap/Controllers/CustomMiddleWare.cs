using Azure.Core;
using SoruCevap.Helper.Jwt;
using SoruCevap.Models;
using System.Text.RegularExpressions;

namespace SoruCevap.Controllers
{
    public class CustomMiddleWare
    {
        private readonly RequestDelegate _next;

        public CustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // soru cevap protalındaki sorulara herkes erişebilir ve kullanıcı giriş yaparken yetkilendirilme yapılamaz
            if (context.Request.Path.StartsWithSegments("/api/Sign/In") || context.Request.Path.StartsWithSegments("/api/Sign/Up") || context.Request.Path.StartsWithSegments("/api/Sign/Users") || (context.Request.Path.StartsWithSegments("/api/Post") && context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            string token = context.Request.Headers["authorization"].ToString().Replace("Bearer ", "");
            string username = context.Request.Headers["userName"];
            string userId = context.Request.Headers["id"];
            Console.WriteLine(token.ToString() + " " + username + " " + userId);


            // verilen header değerlerinin eksiskiz olup olmadığı kontrolü
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userId))
            {

                string message = "Kullanıcı adı, id veya token bilgisi eksik";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(message);
                return;
            }

            // kullanıcının var olup olmadığı kontrolü

            using(var dbContext = new ApplicationDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserName == username && u.Id == userId);

                if (user == null)
                {

                    string message = "Kullancı kayıtlı değil !";
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(message);
                    
                    return;
                }

            } 

            // verilen token2ın kullanıcıya ait olup olmadığı kontrolü

            if(JwtMethods.IsTokenValid(token,userId,username) == false)
            {
                string message = "Token kullanıcıya ait değil";
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(message);

                return;

            }
            // bundan sonraki aşamada artık istek devam edebilir.
            await _next(context);
            return;
            
            
        }
    }
}
