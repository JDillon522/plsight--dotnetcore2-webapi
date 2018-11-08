using System.Collections.Generic;
using API.Models;

namespace API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "NYC",
                    Description = "blagh blach",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "A pile of garbage",
                            Description = "the most hiked"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "The mayors office",
                            Description = "Yummm"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Anchorage",
                    Description = "My home",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 3,
                            Name = "Flatop",
                            Description = "the most hiked"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 4,
                            Name = "Chick Fil A",
                            Description = "Yummm"
                        }
                    }
                }
            };
        }
    }
}