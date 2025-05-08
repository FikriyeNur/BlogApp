using AutoMapper;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Mapper
{
    public class UserModelMapper : Profile
    {
        public UserModelMapper()
        {
            CreateMap<User, UserModel>().ReverseMap();
        }      
    }
}