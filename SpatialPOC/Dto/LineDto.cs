using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpatialPOC.Dto
{
    public class LineDto
    {
        public decimal? LineId { get; set; }

        [Column(TypeName = "geometry (line)")]
        public LineString Geometry { get; set; }
    }
}
