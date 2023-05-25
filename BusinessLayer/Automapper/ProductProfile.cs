using AutoMapper;
using DataAcessLayer.DTO;
using DataAcessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Automapper;
public class ProductProfile : Profile
{
    // Id = product.Id,
    //ProductName = product.ProductName,
    //ProductDescription = product.ProductDescription,
    //price = product.price,
    //CategoryID = product.CategoryID,
    //UserId = userId,
    //CreatedAt = DateTimeOffset.Now,
    //CreatedBy = userId,
    public ProductProfile()
    {
        CreateMap<ProductDTO,Products>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.ProductDescription))
            .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.price))
            .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
            .ForMember(dest => dest.user, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDetail, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ReverseMap()
            ;
    }
}
