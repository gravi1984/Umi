#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Umi.API.Dtos;
using Umi.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        // check if resource exist; cache
        public async Task<IActionResult> GetTouristRoutes(
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
            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesAsync(parameters.Keyword, parameters.RatingOpt, parameters.RatingValue);
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetTouristRouteById(Guid touristRouteId) // this is FromRoute
        {
            var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
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
        [Authorize(AuthenticationSchemes = "Bearer")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTouristRoute(
            [FromBody] TouristRouteForCreationDto touristRouteForCreationDto
            )
        {

            // Map creationDto to Model
            // _repo _context Add Model
            // _repo _context Save Model
            var touristRouteModel = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRouteModel);
            await _touristRouteRepository.SaveAsync();

            // Map model to readDto
            // return CreatedAtRoute([httpget] name, getby{id}, readDto)
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRouteModel);

            return CreatedAtRoute(
                "GetTouristRouteById", 
                new {touristRouteId = touristRouteToReturn.Id},
                touristRouteToReturn);
        }

        [HttpPut("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateTouristRoute(
            [FromRoute] Guid touristRouteId,
        [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto
        )
        {

            if (! (await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
            {
                return NotFound("not found");
            }
            
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            

            // 1. from repo -> dto
            // 2. update dto
            // 3. update model

            // use input body to update model from repo
            _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);

            await _touristRouteRepository.SaveAsync();

            // 204
            return NoContent();

        }

        [HttpPatch("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PatchUpdateTouristRoute(
            [FromRoute] Guid touristRouteId,
            [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            
            if (! await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("not found");
            }

            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            // map from repo to a Update Dto
            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
            
            // apply input patch to Update 
            // bind modelstate <> dto
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);

            // input Update Dto -> Repo Model
            _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
            
            // do data validation, refer dto rule: TouristRouteForUpdateDto
            if (!TryValidateModel(touristRouteToPatch))
            {
                return ValidationProblem(ModelState);
            }
            
            // must Save to db
            await _touristRouteRepository.SaveAsync();
            
            return NoContent();

        }

        [HttpDelete("{touristRouteId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteTouristRoute(
            [FromRoute] Guid touristRouteId)
        {
            
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
            {
                return NotFound("not found");
            }

            var touristFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            _touristRouteRepository.DeleteTouristRoute(touristFromRepo);

            await _touristRouteRepository.SaveAsync();

            return NoContent();

        }
        
        // DELETE /touristRoute/(1,2,3,4)
        // [HttpDelete ("{touristIds}")]
        //
        // public IActionResult DeleteByIds(
        //     [FromRoute] IEnumerable<Guid> touristIds
        // )
        // {
        //     
        // }
        
        
        
    }
}