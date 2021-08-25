using Microsoft.EntityFrameworkCore;
using SpatialPOC.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpatialPOC.Context
{
    public partial class AppDbContext : ScaffoldDbContext
    {
        public virtual DbSet<LineLength> LineLengths { get; set; }
        public virtual DbSet<CalculatedGeometry> CalculatedGeometries { get; set; }

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<ScaffoldDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LineLength>().HasNoKey();
            modelBuilder.Entity<CalculatedGeometry>().HasNoKey();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }
    }
}
