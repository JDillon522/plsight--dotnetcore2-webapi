using System;
using System.Collections.Generic;
using System.Linq;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        // private CityInfoContext _ctx;
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        public PointsOfInterestController(
            // CityInfoContext ctx,
            ILogger<PointsOfInterestController> logger,
            IMailService mailService
        )
        {
            // _ctx = ctx;
            _logger = logger;
            _mailService = mailService;
        }

        [HttpGet("{cityId}/points")]
        public IActionResult GetPointsOfInterestsForCity(int cityId)
        {
            try
            {
                CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null) {
                    _logger.LogInformation($"City with id {cityId} was not found");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"EXCEPTION getting points for city with id {cityId}", ex);
                return StatusCode(500, "Uber problem");
            }
        }

        [HttpGet("{cityId}/points/{id}", Name = "GetPointOfInterest")]
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

        [HttpPost("{cityId}/points")]
        public IActionResult CreatePoint(int cityId, [FromBody] PointOfInterestForCreationDto point)
        {
            if (point == null) {
                return BadRequest(ModelState);
            }

            if (point.Description == point.Name) {
                ModelState.AddModelError("Description", "The description must be different from the name");
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            int maxPointId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            PointOfInterestDto newPoint = new PointOfInterestDto()
            {
                Id = ++maxPointId,
                Name = point.Name,
                Description = point.Description
            };

            city.PointsOfInterest.Add(newPoint);
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = newPoint.Id }, newPoint);
        }

        [HttpPut("{cityId}/points/{pointId}")]
        public IActionResult UpdatePoint(int cityId, int pointId, [FromBody] PointOfInterestForUpdateDto point)
        {
            if (point == null) {
                return BadRequest(ModelState);
            }

            if (point.Description == point.Name) {
                ModelState.AddModelError("Description", "The description must be different from the name");
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            PointOfInterestDto oldPoint = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);
            if (oldPoint == null) {
                return NotFound();
            }

            oldPoint.Name = point.Name;
            oldPoint.Description = point.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/points/{id}")]
        public IActionResult PatchPoint(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> point)
        {
             if (point == null) {
                return BadRequest(ModelState);
            }

            CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            PointOfInterestDto oldPoint = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (oldPoint == null) {
                return NotFound();
            }

            PointOfInterestForUpdateDto pointToPatch = new PointOfInterestForUpdateDto()
            {
                Name = oldPoint.Name,
                Description = oldPoint.Description
            };

            point.ApplyTo(pointToPatch, ModelState);

            TryValidateModel(pointToPatch);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            oldPoint.Name = pointToPatch.Name;
            oldPoint.Description = pointToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{cityId}/points/{id}")]
        public IActionResult DeletePoint(int cityId, int id)
        {
            CityDto city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null) {
                return NotFound();
            }

            PointOfInterestDto point = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (point == null) {
                return NotFound();
            }

            city.PointsOfInterest.Remove(point);
            _mailService.Send($"Delete Point {id}", $"Deleted point \"{point.Name}\" with id {point.Id}");
            return NoContent();
        }
    }
}