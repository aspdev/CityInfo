using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace CityInfo.Api.Controllers
{
    //[Produces("application/json")]
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {

        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _repo;

        

       
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService, ICityInfoRepository repo)
        {
            _logger = logger;
            _mailService = mailService;
            _repo = repo;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                
                if (!_repo.CityExists(cityId))
                {
                    _logger.LogInformation($"The city with id {cityId} wasn't found when accessing points of interest");
                    return NotFound();
                }


                var points = _repo.GetPointsOfInterest(cityId);

                var result = AutoMapper.Mapper.Map<IEnumerable<PointOfInterestDto>>(points);

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Exception while getting point of interest");
                return StatusCode(500, "A problem happened while handling your request");
            }
        }

        [HttpGet("{cityId}/pointofinterest/{pointId}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int pointId)
        {
            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _repo.GetPointOfInterest(cityId, pointId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var result = AutoMapper.Mapper.Map<PointOfInterestDto>(pointOfInterest);

            return Ok(result);


        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, 
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }


            var finalPointOfInterest = AutoMapper.Mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _repo.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if (!_repo.Save())
            {
                return StatusCode(500, "A problem happened while handling this request");
            }


            var createdPointOfInterestToReturn = AutoMapper.Mapper
                .Map<PointOfInterestDto>(finalPointOfInterest);
            

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId, pointId = createdPointOfInterestToReturn.Id
            }, createdPointOfInterestToReturn);

        }

        [HttpPut("{cityId}/pointofinterest/{pointId}")]
        public IActionResult UpdatePointOfInterest(int cityId, int pointId,
            [FromBody] PointOfInterestUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _repo.GetPointOfInterest(cityId, pointId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            AutoMapper.Mapper.Map(pointOfInterest, pointOfInterestEntity);


            if (!_repo.Save())
            {
                return StatusCode(500, "A problem happened while tracking your request");
            }


            return NoContent();
        }

        [HttpPatch("{cityId}/pointofInterest/{pointId}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int pointId,
            [FromBody] JsonPatchDocument<PointOfInterestUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterestDtos
                .FirstOrDefault(p => p.Id == pointId);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;


            return NoContent();


        }

        [HttpDelete("{cityId}/pointofinterest/{pointId}")]
        public IActionResult DeletePointOfInterest(int cityId, int pointId)
        {
            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _repo.GetPointOfInterest(cityId, pointId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _repo.DeletePointOfInterest(pointOfInterestEntity);

            if (!_repo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request");
            }

            
            _mailService.Send("Message to Admin", "A point of interest has been removed from dataStore!");

            return NoContent();

        }
    }
}