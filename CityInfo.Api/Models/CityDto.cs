using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfPointsOfInterest => PointsOfInterestDtos.Count;

        public ICollection<PointOfInterestDto> PointsOfInterestDtos { get; set; } = 
        new List<PointOfInterestDto>();
        
    }
}
