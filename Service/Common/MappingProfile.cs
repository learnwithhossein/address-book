using AddressBook.Domain;
using AddressBook.Domain.DTO;
using AutoMapper;

namespace AddressBook.Service.Common
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
