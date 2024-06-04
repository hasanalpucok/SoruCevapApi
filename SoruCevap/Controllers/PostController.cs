using Microsoft.AspNetCore.Mvc;
using SoruCevap.Models.ViewModel;
using SoruCevap.Service.Abstraction;

namespace SoruCevap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController: ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public IActionResult GetPosts()
        {
            var result = _postService.GetPosts();
            try
            {
                return Ok(result);
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            var result = _postService.GetPostById(id);
            try
            {
                return Ok(result);

            }
            catch (Exception)
            {

                return BadRequest();
            }
            
            
           
        }

        [HttpPost]
        public IActionResult SavePost(PostView view)
        {
            var result = _postService.SavePost(view);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult EditPost(PostView view)
        {
            var result = _postService.EditPost(view);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePost(int id)
        {
            var result = _postService.DeletePost(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
