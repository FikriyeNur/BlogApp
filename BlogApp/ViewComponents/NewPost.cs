using AutoMapper;
using BlogApp.Data.Abstract.IRepository;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class NewPost : ViewComponent
    {
        private IPostRepository _postRepository;
        private IMapper _mapper;

        public NewPost(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var postList = await _postRepository.Posts()
                            .Where(x => x.IsActive)
                            .OrderByDescending(p => p.PublishedDate)
                            .Take(4)
                            .ToListAsync();
            var model = _mapper.Map<List<PostModel>>(postList);
            return View(model);
        }
    }
}