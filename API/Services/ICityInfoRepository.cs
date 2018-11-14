using System.Collections.Generic;
using System.Linq;
using API.Entities;

namespace API.Services
{
    public interface ICityInfoRepository
    {
        bool CityExists(int cityId);
        IEnumerable<City> GetCities(bool includePoints);
        City GetCity(int cityId, bool includePoints);
        IEnumerable<Point> GetPoints(int cityId);
        Point GetPoint(int cityId, int pointId);

        void AddPoint(int cityId, Point point);
        void AddCity(City city);
        bool Save();
    }
}