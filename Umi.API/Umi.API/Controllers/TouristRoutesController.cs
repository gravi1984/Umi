#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Umi.API.Dtos;
using Umi.API.Services;
using AutoMapper;
using Umi.API.Models;
using Umi.API.ResourceParameters;

namespace Umi.API.Controllers
{
    // [Route("api/TouristRoutes")]
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        // api/touristRoute?keyword={keyword}
        [HttpGet]
        [HttpHead]
        // check if resource exist; cache
        public IActionResult GetTouristRoutes(
            [FromQuery] TouristRouteResourceParameters parameters  // FromQuery vs FromBody
        ) 
        {
            
            // @"" -> C# string
            // 2 parts: largeThen + 9
            // Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
            // string ratingOpt = "";
            // int ratingValue = -1;
            // Match match = regex.Match(parameters.Rating);
            // if (match.Success)
            // {
            //     ratingOpt = match.Groups[1].Value;
            //     ratingValue = Int32.Parse(match.Groups[2].Value);
            // }
            //
            var touristRoutesFromRepo = _touristRouteRepository.GetTouristRoutes(parameters.Keyword, parameters.RatingOpt, parameters.RatingValue);
            if (!touristRoutesFromRepo.Any())
            {
                return NotFound("no routes found");
            }

            var touristRouteDtos = _mapper.Map<IEnumerable<TouristRouteDto>>(touristRoutesFromRepo);
            return Ok(touristRouteDtos);
            
        }
     
        // api/touristRoute/{touristRouteId}

        [HttpGet("{touristRouteId}", Name = "GetTouristRouteById")]
        [HttpHead("{touristRouteId}")]
        public IActionResult GetTouristRouteById(Guid touristRouteId) // this is FromRoute
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

        [HttpPost]
        public IActionResult CreateTouristRoute(
            [FromBody] TouristRouteForCreationDto touristRouteForCreationDto
            )
        {

            // Map creationDto to Model
            // _repo _context Add Model
            // _repo _context Save Model
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            _touristRouteRepository.Save();

            // Map model to readDto
            // return CreatedAtRoute([httpget] name, getby{id}, readDto)
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            return CreatedAtRoute(
                "GetTouristRouteById", 
                new {touristRouteId = touristRouteToReturn.Id},
                touristRouteToReturn);
        }
        
    }
}