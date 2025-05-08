using AutoMapper;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Mapper
{
    public class PostModelMapper : Profile
    {
        public PostModelMapper()
        {
            CreateMap<Post, PostModel>().ReverseMap();
        }
    }
}