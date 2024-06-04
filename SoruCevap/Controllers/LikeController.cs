using Microsoft.AspNetCore.Mvc;
using SoruCevap.Models.ViewModel;
using SoruCevap.Service.Abstraction;

namespace SoruCevap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController: ControllerBase
    {
        private readonly ILikeService _likeService;
        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        public IActionResult SaveLike(LikeView view)
        {
            
            try
            {
                var result = _likeService.SaveLike(view);
                return Ok(result);


            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult UpdateLike(LikeView view)
        {
            try
            {
                var result = _likeService.EditLike(view);
                return Ok(result);

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLike(int id)
        {
            try
            {
                var result = _likeService.DeleteLike(id);
                return Ok(result);

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

    }
}
