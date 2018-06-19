using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDto> Cities;

        public CitiesDataStore()
        {
            Cities = new List<CityDto>
            {
                new CityDto
                {
                    Id = 1, Name = "New York", Description = "The one with the big park",
                    PointsOfInterestDtos =
                    {   new PointOfInterestDto
                    {
                        Id = 1, Name = "Central Park", Description = "The most visitied urban park in the USA"
                    },
                        new PointOfInterestDto
                        {
                            Id = 2, Name = "Empire State Building", Description = "A 102-storey "
                        }
                    }
                },
                new CityDto
                {
                    Id = 2, Name = "Antwerp", Description = "The one with the cathedral that was never finished",
                    PointsOfInterestDtos =
                    {   new PointOfInterestDto
                        {
                            Id = 3, Name = "Cathedral of Our Lady", Description = "A Gothic style cathedral"
                        },
                        new PointOfInterestDto
                        {
                            Id = 4, Name = "Antwerp Central Station", Description = "The finest example of railway architecture in Belgum "
                        }
                    }

                },
                new CityDto
                {
                    Id = 3, Name = "Paris", Description = "The one with the big tower",
                    PointsOfInterestDtos =
                    {   new PointOfInterestDto
                        {
                            Id = 5, Name = "Eiffel Tower", Description = "A wrought iron tower"
                        },
                        new PointOfInterestDto
                        {
                            Id = 6, Name = "The Louvre", Description = "The world's largest museum"
                        }
                    }
                },
            };
        }
    }
}
