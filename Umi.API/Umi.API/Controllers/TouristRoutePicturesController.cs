using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Umi.API.Dtos;
using Umi.API.Services;

namespace Umi.API.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository =
                touristRouteRepository ?? throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPictureListForTouristRoute(Guid touristRouteId)
        {
            // !!Db operation, extend func in Repo!!
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("tourist Route not exist");
            }
            
            var picuturesFromRepo = _touristRouteRepository.GetPicturesByTouristRouteId(touristRouteId);

            if (picuturesFromRepo == null || picuturesFromRepo.Count() <= 0)
            {
                return NotFound("no picture");
            }

            return Ok(_mapper.Map<IEnumerable<TouristRoutePictureDto>>(picuturesFromRepo));
        }

        [HttpGet("{pictureId}")]
        public IActionResult GetPicture(Guid touristRouteId, int pictureId)
        {
            // check father resource if exist
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("tourist Route not exist");
            }

            var picutureFromRepo = _touristRouteRepository.GetPicture(pictureId);
            
            // check if child resource if exist
            if (picutureFromRepo == null)
            {
                return NotFound("picture not found");
            }

            // return Dto
            return Ok(_mapper.Map<TouristRoutePictureDto>(picutureFromRepo));

        }
    }
}