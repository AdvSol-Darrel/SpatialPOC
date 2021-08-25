using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpatialPOC.Dto
{
    public class CalculatedGeometry
    {
        [Column("geom")]
        public Geometry Geometry { get; set; }
    }
}
