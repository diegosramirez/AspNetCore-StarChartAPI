using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarChart.Models
{
    public class CelestialObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrbitedObjectId { get; set; }
        [NotMapped]
        public IList<CelestialObject> Satellites { get; set; }
        public TimeSpan OrbitalPeriod { get; set; }
    }
}
