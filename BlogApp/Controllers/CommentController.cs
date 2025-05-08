using System.Security.Claims;
using AutoMapper;
using BlogApp.Data.Abstract.IRepository;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class CommentController : Controller
    {
        private ICommentRepository _commentRepository;
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<JsonResult> AddOrUpdateComment(CommentViewModel model)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var commentModel = _mapper.Map<Comment>(model);
            var update = false;

            if (commentModel != null)
            {
                commentModel.User = await _userRepository.Users().FirstOrDefaultAsync(x => x.UserId == userId) ?? throw new InvalidOperationException("User not found");

                if (commentModel.CommentId > 0)
                {
                    update = true;
                    await _commentRepository.UpdateCommentAsync(commentModel);
                }
                else
                {
                    await _commentRepository.AddCommentAsync(commentModel);
                }
            }
            else
            {
                return Json(new { success = false });
            }

            var returnModel = _mapper.Map<CommentViewModel>(commentModel);
            return Json(new
            {
                success = true,
                update,
                returnModel
            });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.Comments()
                                    .Include(x => x.Post)
                                    .FirstOrDefaultAsync(x => x.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            await _commentRepository.DeleteCommentAsync(comment.CommentId);
            return Json(new { success = true });
        }
    }
}