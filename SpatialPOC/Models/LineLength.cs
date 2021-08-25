using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SpatialPOC.Dto
{
    public class LineLength
    {
        [Column("id")]
        public decimal LineId { get; set; }

        public double? Length { get; set; }
    }
}