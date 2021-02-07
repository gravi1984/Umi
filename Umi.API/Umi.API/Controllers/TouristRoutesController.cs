using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Umi.API.Dtos;
using Umi.API.Services;
using AutoMapper;

namespace Umi.API.Controllers
{
    // [Route("api/TouristRoutes")]
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTouristRoutes()
        {
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutes();
            if (!touristRoutesFromRepo.Any())
            {
                return NotFound("no routes found");
            }

            var touristRouteDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            return Ok(touristRouteDtos);
            
        }

        // api/touristRoute/{touristRouteId}

        [HttpGet("{touristRouteId}")]
        public IActionResult GetTouristRouteById(Guid touristRouteId)
        {
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (touristRoutesFromRepo == null)
            {
                return NotFound("no route found with provided id");
            }

            // Model -> Dto: Auto Mapper

            var touristRouteDto = _mapper.Map<TouristRouteDto>(touristRoutesFromRepo);

            // var touristROuteDto = new TouristRouteDto()
            // {
            //     Id = touristRoutesFromRepo.Id,
            //     Title = touristRoutesFromRepo.Title,
            //     Description = touristRoutesFromRepo.Description,
            //     Price = touristRoutesFromRepo.OriginalPrice * (decimal) (touristRoutesFromRepo.DiscountPresent ?? 1),
            //     TravelDays = touristRoutesFromRepo.TravelDays.ToString()
            // };
            
            
            return Ok(touristRouteDto);
        }
    }
}