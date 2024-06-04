using Microsoft.AspNetCore.Mvc;
using SoruCevap.Models.ViewModel;
using SoruCevap.Service.Abstraction;

namespace SoruCevap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public IActionResult SaveComment(CommentView view)
        {

            try
            {
                var result = _commentService.SaveComment(view);
                return Ok(result);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        [HttpPut]
        public IActionResult EditComment(CommentView view)
        {

            try
            {
                var result = _commentService.EditComment(view);
                return Ok(result);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {

            try
            {
                var result = _commentService.DeleteComment(id);
                return Ok(result);

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }
    }
}
