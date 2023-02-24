using AutoMapper;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;

namespace BusinessLayer.Automapper;

public class UserProfile:Profile
{
	public UserProfile()
	{
        CreateMap<UserDTO, User>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.products, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(dest => dest.products, opt => opt.Ignore())
            .ForMember(dest => dest.OrdersTables, opt => opt.Ignore())
            .ReverseMap();


    }
}
