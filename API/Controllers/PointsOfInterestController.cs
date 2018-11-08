using System.Collections.Generic;
using System.Linq;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {

        [HttpGet("{cityId}/points")]
        public IActionResult GetPointsOfInterestsForCity(int cityId)
        {
            CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null) {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/points/{id}")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            PointOfInterestDto point = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (point == null) {
                return NotFound();
            }

            return Ok(point);
        }
    }
}