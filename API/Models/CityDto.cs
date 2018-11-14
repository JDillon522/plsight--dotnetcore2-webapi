using System.Collections.Generic;

namespace API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfPointsOfInterest
        {
            get {
                return PointsOfInterest.Count;
            }

            set {}
        }
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
    }

    public class CityWithoutPointsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CityForCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}