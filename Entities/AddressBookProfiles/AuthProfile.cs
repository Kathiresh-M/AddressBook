using AutoMapper;
using Entities.Dto;
using Entities.Models;

namespace Entities.AddressBookProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Profiles, ProfilesDto>().ReverseMap();

            CreateMap<ProfileforCreatingDto, Profiles>().ReverseMap();
        }
    }
}
