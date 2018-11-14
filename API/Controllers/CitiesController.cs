using System.Collections.Generic;
using System.Linq;
using API.Models;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using AutoMapper;

namespace API.Controllers
{
    [Route("api/cities")]
    public class CitiesController: Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities(bool includePoints)
        {
            IEnumerable<City> cityEntities = _cityInfoRepository.GetCities(includePoints);

            if (includePoints)
            {
                IEnumerable<CityDto> results = Mapper.Map<IEnumerable<CityDto>>(cityEntities);
                return Ok(results);

            } else {
                IEnumerable<CityWithoutPointsDto> results = Mapper.Map<IEnumerable<CityWithoutPointsDto>>(cityEntities);
                return Ok(results);
            }
        }


        [HttpGet("/api/cities/{id}", Name = "GetCity")]
        public IActionResult GetCity(int id, bool includePoints = false)
        {
            City city = _cityInfoRepository.GetCity(id, includePoints);

            if (city == null) {
                return NotFound();
            } else {

                if (includePoints) {
                    CityDto cityResult = Mapper.Map<CityDto>(city);
                    return Ok(cityResult);
                } else {
                    CityWithoutPointsDto cityResult = Mapper.Map<CityWithoutPointsDto>(city);
                    return Ok(cityResult);
                }
            }

        }

        [HttpPost("/api/cities")]
        public IActionResult CreateCity([FromBody] CityForCreationDto city)
        {
            if (city == null) {
                return BadRequest(ModelState);
            }

            if (city.Description == city.Name && city.Description != null && city.Name != null) {
                ModelState.AddModelError("Description", "The description must be different from the name");
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            City newCity = Mapper.Map<City>(city);
            _cityInfoRepository.AddCity(newCity);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            };

            CityDto createdCity = Mapper.Map<CityDto>(newCity);

            return CreatedAtRoute("GetCity", new { id = createdCity.Id }, createdCity);
        }
    }
}