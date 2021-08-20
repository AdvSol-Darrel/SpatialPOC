using System;
using System.Collections.Generic;

#nullable disable

namespace SpatialPOC.Models
{
    public partial class Topology
    {
        public Topology()
        {
            Layers = new HashSet<Layer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Srid { get; set; }
        public double Precision { get; set; }
        public bool Hasz { get; set; }

        public virtual ICollection<Layer> Layers { get; set; }
    }
}
