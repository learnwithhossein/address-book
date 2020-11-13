using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain;
using Domain.DTO;

namespace Service.Common
{
   public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserToGetDto>();
            CreateMap<Contact, ContactToGetDto>().ForMember(x=>x.UserFullName
                , options =>
            {
                options.MapFrom(x=>$"{x.FirstName} {x.LastName}");
            });
            CreateMap<Contact, ContactToGetSimple>();

        }
    }
}
