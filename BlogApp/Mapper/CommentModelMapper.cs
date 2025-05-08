using AutoMapper;
using BlogApp.Entity;
using BlogApp.Models;

namespace BlogApp.Mapper
{
    public class CommentModelMapper : Profile
    {
        public CommentModelMapper()
        {
            CreateMap<Comment, CommentModel>().ReverseMap();

            CreateMap<Comment, CommentViewModel>()
                     .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserModel
                     {
                         UserId = src.User.UserId,
                         UserName = src.User.UserName,
                         Image = src.User.Image,               
                     }))
                     .ReverseMap();
        }
    }
}