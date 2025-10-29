using AutoMapper;
using CarRentalWebApplication.Areas.Identity.Pages.Account;
using CarRentalWebApplication.Areas.Identity.Pages.Account.Manage;
using CarRentalWebApplication.Models;

namespace CarRentalWebApplication.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterModel.InputModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpperInvariant()));

            CreateMap<IndexModel.InputModel, User>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<User, IndexModel.InputModel>();
        }
    }
}
