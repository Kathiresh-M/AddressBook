using AutoMapper;
using Entities.Dto;
using Entities.Models;

namespace Entities.AddressBookProfiles
{
    public class AddressBookProfile : Profile
    {
        public AddressBookProfile()
        {
            CreateMap<RefSet, RefSetDto>().ReverseMap();

            CreateMap<RefTerm, RefTermDto>().ReverseMap();

            CreateMap<RefSetTerm, RefTermDto>().ReverseMap();

            //Authentication profiles
            CreateMap<Profiles, ProfilesDto>().ReverseMap();

            CreateMap<ProfileforCreatingDto, Profiles>().ReverseMap();
        }
    }
}