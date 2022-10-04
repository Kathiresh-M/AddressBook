using AutoMapper;
using Entities.AddressBookProfiles;

namespace Entities
{
    public class AutoMapperConfiguration
    {
            public MapperConfiguration Configure()
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AddressBookProfile>();
                });
                return config;
        }
        }
    }
