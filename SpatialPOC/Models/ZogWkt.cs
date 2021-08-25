using System;
using System.Collections.Generic;

#nullable disable

namespace SpatialPOC.Models
{
    public partial class ZogWkt
    {
        public decimal Id { get; set; }
        public string WktGeometry { get; set; }
        public string GeometryType { get; set; }
    }
}
