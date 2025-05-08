using AutoMapper;
using BlogApp.Data.Abstract.IRepository;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class TagController : Controller
    {
        private readonly IMapper _mapper;
        private ITagRepository _tagRepository;

        public TagController(IMapper mapper, ITagRepository tagRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListTag()
        {
            var tags = await _tagRepository.Tags().ToListAsync();
            var model = _mapper.Map<List<TagModel>>(tags);
            return View(model);
        }

        [HttpGet("AddTag")]
        public IActionResult AddTag()
        {
            return View();
        }

        [HttpPost("AddTag")]
        public async Task<IActionResult> AddTag(TagModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<Tag>(model);
                if (await _tagRepository.Tags().AnyAsync(x => x.Text == tag.Text || x.Url == tag.Url))
                {
                    TempData["TagExistsMessage"] = "Etiket ve Url zaten mevcut!";
                    return View(model);
                }

                await _tagRepository.AddTagAsync(tag);
                return RedirectToAction("Index", "Post");
            }

            return View(model);
        }

        [HttpGet("EditTag/{id}")]
        public async Task<IActionResult> EditTag(int id)
        {
            var tag = await _tagRepository.Tags().FirstOrDefaultAsync(x => x.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<TagModel>(tag);
            return View(model);
        }

        [HttpPost("EditTag/{id}")]
        public async Task<IActionResult> EditTag(int id, TagModel model)
        {
            if (id != model.TagId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<Tag>(model);
                if (await _tagRepository.Tags().AnyAsync(x => x.Text == tag.Text || x.Url == tag.Url))
                {
                    TempData["TagExistsMessage"] = "Etiket ve Url zaten mevcut!";
                    return View(model);
                }

                await _tagRepository.UpdateTagAsync(tag);
                return RedirectToAction("Index", "Post");
            }

            return View(model);
        }

        [HttpGet("DeleteTag/{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _tagRepository.Tags().FirstOrDefaultAsync(x => x.TagId == id);
            if (tag == null)
            {
                return NotFound();
            }

            await _tagRepository.DeleteTagAsync(id);
            return RedirectToAction("ListTag");
        }
    }
}