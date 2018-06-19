using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    //[Produces("application/json")]
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _repo;

        public CitiesController(ICityInfoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {

            var entities = _repo.GetCities();

            var results = AutoMapper.Mapper
                .Map<IEnumerable<CityWithoutPointsOfInterestDto>>(entities);

            return Ok(results);

        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {

            var city = _repo.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var res = AutoMapper.Mapper.Map<CityDto>(city);

                return Ok(res);
            }

            var result = AutoMapper.Mapper.Map<CityWithoutPointsOfInterestDto>(city);

            return Ok(result);


            /*var cityToReturn = CitiesDataStore.Current.Cities
                .FirstOrDefault(city => city.Id == id);

            if (cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);*/
        }



    }
}