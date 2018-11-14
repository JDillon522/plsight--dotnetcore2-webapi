using System;
using System.Collections.Generic;
using System.Linq;
using API.Entities;
using API.Models;
using API.Services;
using AutoMapper;
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
        private ICityInfoRepository _cityInfoRepo;
        public PointsOfInterestController(
            // CityInfoContext ctx,
            ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepo
        )
        {
            // _ctx = ctx;
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepo = cityInfoRepo;
        }

        [HttpGet("{cityId}/points")]
        public IActionResult GetPointsOfInterestsForCity(int cityId)
        {
            try
            {
                IEnumerable<Point> points = _cityInfoRepo.GetPoints(cityId);

                if (points == null) {
                    _logger.LogInformation($"City with id {cityId} was not found");
                    return NotFound();
                }

                List<PointOfInterestDto> pointsResult = Mapper.Map<List<PointOfInterestDto>>(points);

                return Ok(points);

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
            Point point = _cityInfoRepo.GetPoint(cityId, id);

            if (point == null) {
                return NotFound();
            }

            PointOfInterestDto pointResult = Mapper.Map<PointOfInterestDto>(point);

            return Ok(pointResult);
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

            if (!_cityInfoRepo.CityExists(cityId)) {
                return NotFound();
            }

            Point newPoint = Mapper.Map<Point>(point);
            _cityInfoRepo.AddPoint(cityId, newPoint);

            if (!_cityInfoRepo.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            };

            PointOfInterestDto createdPoint = Mapper.Map<PointOfInterestDto>(newPoint);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = createdPoint.Id }, createdPoint);
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

            if (!_cityInfoRepo.CityExists(cityId)) {
                return NotFound();
            }

            Point oldPoint = _cityInfoRepo.GetPoint(cityId, pointId);
            if (oldPoint == null) {
                return NotFound();
            }

            Point updatedPointEntity = Mapper.Map(point, oldPoint);

            if (!_cityInfoRepo.Save()) {
                return StatusCode(500, "A problem occured updating your point");
            }
            PointOfInterestDto updatedPoint = Mapper.Map<PointOfInterestDto>(updatedPointEntity);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = pointId }, updatedPoint);
        }

        [HttpPatch("{cityId}/points/{id}")]
        public IActionResult PatchPoint(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> pointDoc)
        {
             if (pointDoc == null) {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepo.CityExists(cityId)) {
                return NotFound();
            }

            Point oldPoint = _cityInfoRepo.GetPoint(cityId, id);
            if (oldPoint == null) {
                return NotFound();
            }

            PointOfInterestForUpdateDto pointToPatch = Mapper.Map<PointOfInterestForUpdateDto>(oldPoint);

            pointDoc.ApplyTo(pointToPatch, ModelState);

            TryValidateModel(pointToPatch);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            Point updatedPointEntity = Mapper.Map(pointToPatch, oldPoint);

            if (!_cityInfoRepo.Save()) {
                return StatusCode(500, "A problem occured updating your point");
            }
            PointOfInterestDto updatedPoint = Mapper.Map<PointOfInterestDto>(updatedPointEntity);

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = id }, updatedPoint);
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