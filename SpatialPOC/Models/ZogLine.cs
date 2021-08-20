using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

#nullable disable

namespace SpatialPOC.Models
{
    public partial class ZogLine
    {
        public decimal? LineId { get; set; }
        public Geometry Geometry { get; set; }
    }
}
