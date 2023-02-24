using AutoMapper;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Automapper;
public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDTO, Roles>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
            .ForMember(dest => dest.ID ,opt => opt.MapFrom(src => src.ID))
            .ForMember(dest =>dest.users ,opt => opt.Ignore()).ReverseMap();
       
    }
}
