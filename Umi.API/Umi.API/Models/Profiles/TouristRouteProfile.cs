using System.Collections.Specialized;
using AutoMapper;
using Umi.API.Dtos;

namespace Umi.API.Models.Profiles
{
    public class TouristRouteProfile : Profile
    {
        public TouristRouteProfile()
        {
            // same name field: Auto
            // Project some filed Model -> Dto: ForMember
            CreateMap<TouristRoute, TouristRouteDto>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.OriginalPrice * (decimal) (src.DiscountPresent ?? 1))
                )
                .ForMember(
                    dest => dest.TravelDays,
                    opt => opt.MapFrom(src => src.TravelDays.ToString())
                )
                .ForMember(
                    dest => dest.TripType,
                    opt => opt.MapFrom(src => src.TripType.ToString()))
                .ForMember(
                    dest => dest.DepartureCity,
                    opt => opt.MapFrom(src => src.DepartureCity.ToString())
                );

        }
        }
    }


