using System.Collections.Generic;
using System.Linq;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.FirstOrDefault(c => c.Id == cityId) != null;
        }

        public IEnumerable<City> GetCities(bool includePoints = false)
        {
            if (includePoints)
            {
                return _context.Cities.Include(c => c.PointsOfInterest).OrderBy(c => c.Name).ToList();
            }
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePoints)
        {
            if (includePoints) {
                return _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefault();
            }
            return _context.Cities.FirstOrDefault(c => c.Id == cityId);
        }

        public Point GetPoint(int cityId, int pointId)
        {
            return _context.Cities
                        .Include(c => c.PointsOfInterest)
                        .Where(c => c.Id == cityId)
                        .FirstOrDefault()
                        .PointsOfInterest.FirstOrDefault(p => p.Id == pointId);
        }

        public IEnumerable<Point> GetPoints(int cityId)
        {
            return _context.Points.Where(p => p.CityId == cityId).ToList();
        }

        public void AddPoint(int cityId, Point point)
        {
            City city = GetCity(cityId, false);
            city.PointsOfInterest.Add(point);
        }

        public void AddCity(City city)
        {
            _context.Cities.Add(city);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}