using AutoMapper;
using BlogApp.Data.Abstract.IRepository;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class TagMenu : ViewComponent
    {
        private  ITagRepository _tagRepository;
        private IMapper _mapper;

        public TagMenu(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tagList = await _tagRepository.Tags().ToListAsync();
            var model = _mapper.Map<List<TagModel>>(tagList);
            return View(model);
        }
    }
}