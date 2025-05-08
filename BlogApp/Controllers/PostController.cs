using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using BlogApp.Data.Abstract.IRepository;
using BlogApp.Entity;
using BlogApp.Helpers;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly IMapper _mapper;
        private IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private IUserRepository _userRepository;
        private ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository, ITagRepository tagRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string tagUrl, int page = 1)
        {
            var pageSize = 3;
            var skip = (page - 1) * pageSize;

            var claims = User.Claims;

            var query = _postRepository.Posts()
                        .Include(x => x.Tags)
                        .Where(x => (string.IsNullOrEmpty(tagUrl) || x.Tags.Any(t => t.Url == tagUrl)) && x.IsActive);

            var totalPosts = await query.CountAsync();
            var postList = await query
                                .Skip(skip)
                                .Take(pageSize)
                                .ToListAsync();

            foreach (var post in postList)
            {
                post.Content = post.Content!.Length > 300 ? post.Content.Substring(0, 300) + "..." : post.Content;
            }

            var model = _mapper.Map<List<PostModel>>(postList);
            var totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TagUrl = tagUrl;

            return View(model);
        }

        [HttpGet("{Detail}/{url}")]
        public async Task<IActionResult> Detail(string url)
        {
            var post = await _postRepository.Posts()
                            .Include(x => x.User)
                            .Include(x => x.Tags)
                            .Include(x => x.Comments)
                            .ThenInclude(x => x.User)
                            .FirstOrDefaultAsync(x => x.Url == url);
            var model = _mapper.Map<PostModel>(post);
            return View(model);
        }

        [HttpGet("PostList")]
        [Authorize]
        public async Task<IActionResult> PostList()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);
            var posts = _postRepository.Posts();

            if (string.IsNullOrEmpty(role) && role != "Admin")
            {
                posts = posts.Where(x => x.UserId == userId);
            }

            await posts.ToListAsync();
            var model = _mapper.Map<List<PostModel>>(posts).OrderByDescending(x => x.PublishedDate).ToList();
            return View(model);
        }

        [HttpGet("AddPost")]
        [Authorize]
        public async Task<IActionResult> AddPost()
        {
            await GetTagSelectListAsync();
            return View();
        }

        [HttpPost("AddPost")]
        [Authorize]
        public async Task<IActionResult> AddPost(PostModel model)
        {
            if (ModelState.IsValid)
            {
                var post = _mapper.Map<Post>(model);

                if (!string.IsNullOrEmpty(model.SelectedTags))
                {
                    var selectedTagStrings = JsonSerializer.Deserialize<List<string>>(model.SelectedTags);
                    var selectedTagIds = selectedTagStrings!.Select(tagId => int.Parse(tagId)).ToList();
                    var tags = await _tagRepository.Tags().Where(tag => selectedTagIds.Contains(tag.TagId)).ToListAsync();
                    post.Tags = tags;
                }

                post.PublishedDate = DateTime.Now;
                post.UserId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
                post.Image = await SaveImageHelper.SaveImageAsync(model.ImageFile!);
                await _postRepository.AddPostAsync(post);

                if (User.FindFirstValue(ClaimTypes.Role) == "Admin")
                {
                    post.IsActive = model.IsActive;
                }
                else
                {
                    TempData["PostAddMessage"] = "Post başarıyla eklendi! Admin onayından sonra yayınlanacaktır.";
                }

                return RedirectToAction("Index");
            }

            await GetTagSelectListAsync();
            ViewBag.SelectedTags = model.SelectedTags;
            return View(model);
        }

        [HttpGet("EditPost/{id}")]
        [Authorize]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.Posts()
                            .Include(x => x.Tags)
                            .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<PostModel>(post);
            ViewBag.SelectedTagIds = post.Tags.Select(t => t.TagId).ToList();
            await GetTagSelectListAsync();
            return View(model);
        }

        [HttpPost("EditPost/{id}")]
        [Authorize]
        public async Task<IActionResult> EditPost(int id, PostModel model)
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            List<Tag> tags = new ();

            if (id != model.PostId)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.SelectedTags))
            {
                var selectedTagStrings = JsonSerializer.Deserialize<List<string>>(model.SelectedTags);
                var selectedTagIds = selectedTagStrings!.Select(tagId => int.Parse(tagId)).ToList();
                tags = await _tagRepository.Tags().Where(tag => selectedTagIds.Contains(tag.TagId)).ToListAsync();
            }

            if (ModelState.IsValid)
            {
                var postInDb = await _postRepository.Posts().FirstOrDefaultAsync(x => x.PostId == id);
                if (postInDb == null)
                {
                    return NotFound();
                }

                await _postRepository.DeletePostTagAsync(postInDb.PostId);
                postInDb.Tags.AddRange(tags);

                if (model.ImageFile != null)
                {
                    postInDb.Image = await SaveImageHelper.SaveImageAsync(model.ImageFile);
                }

                if (role == "Admin")
                {
                    postInDb.IsActive = model.IsActive;
                }
                else
                {
                    TempData["PostEditMessage"] = "Post başarıyla güncellendi! Admin onayından sonra yayınlanacaktır.";
                }

                postInDb.PublishedDate = DateTime.Now;
                await _postRepository.UpdatePostAsync(postInDb);
                return RedirectToAction("PostList");
            }

            ViewBag.SelectedTagIds = tags.Select(t => t.TagId).ToList();
            await GetTagSelectListAsync();
            return View(model);
        }

        [HttpGet("DeletePost/{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.Posts().FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<PostModel>(post);
            return View(model);
        }

        [HttpPost("DeletePost/{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            await _postRepository.DeletePostAsync(id);
            return RedirectToAction("PostList");
        }

        private async Task GetTagSelectListAsync()
        {
            ViewBag.Tags = new SelectList(await _tagRepository.Tags().ToListAsync(), "TagId", "Text");
        }
    }
}