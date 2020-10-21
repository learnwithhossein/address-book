using AutoMapper;
using Domain;
using Domain.DTO;

namespace Service.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactToGetSimpleDto>();
            
            CreateMap<Contact, ContactToGetDto>()
                .ForMember(x => x.UserFullName, options =>
                {
                    options.MapFrom(x => $"{x.FirstName} {x.LastName}");
                }
            );

            CreateMap<User, UserToGetDto>();
        }
    }
}
