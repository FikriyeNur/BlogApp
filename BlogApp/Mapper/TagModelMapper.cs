using AutoMapper;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Mapper
{
    public class TagModelMapper : Profile
    {
        public TagModelMapper()
        {
            CreateMap<Tag, TagModel>().ReverseMap();
        }
    }
}