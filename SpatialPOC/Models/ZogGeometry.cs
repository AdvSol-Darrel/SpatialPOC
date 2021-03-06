using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

#nullable disable

namespace SpatialPOC.Models
{
    public partial class ZogGeometry
    {
        public decimal Id { get; set; }
        public Geometry Geometry { get; set; }
        public int? Srid { get; set; }
        public string GeometryType { get; set; }
    }
}
