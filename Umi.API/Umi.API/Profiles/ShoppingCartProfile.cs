using AutoMapper;
using Umi.API.Dtos;

namespace Umi.API.Models.Profiles
{
    public class ShoppingCartProfile : Profile
    {

        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<LineItem, LineItemDto>();
        }
        
    }
}