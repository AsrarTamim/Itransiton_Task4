using AutoMapper;
using Taks5.Entities;
using Taks5.Models;

namespace LibraryManagementsystem
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<UserAccount, RegistrationViewModel>().ReverseMap();
            CreateMap<UserAccount, UserListViewModel>().ReverseMap();
        }
    }
}
