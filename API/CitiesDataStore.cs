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
                    Description = "blagh blach"
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Anchorage",
                    Description = "My home"
                }
            };
        }
    }
}