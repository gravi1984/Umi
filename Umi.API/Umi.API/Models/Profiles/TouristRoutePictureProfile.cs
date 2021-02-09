using AutoMapper;
using Umi.API.Dtos;

namespace Umi.API.Models.Profiles
{
    public class TouristRoutePictureProfile : Profile
    {
        public TouristRoutePictureProfile()
        {
            // Auto Mapper
            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();

            // add mapping in Profile => AutoMapper
            CreateMap<TouristRoutePictureForCreationDto, TouristRoutePicture>();
        }
    }
}