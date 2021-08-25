using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SpatialPOC.Models;

#nullable disable

namespace SpatialPOC.Context
{
    public partial class ScaffoldDbContext : DbContext
    {
        public ScaffoldDbContext()
        {
        }

        public ScaffoldDbContext(DbContextOptions<ScaffoldDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ZogGeometry> ZogGeometry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("hstore")
                .HasPostgresExtension("pg_cron")
                .HasPostgresExtension("pgrouting")
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("postgis_raster")
                .HasPostgresExtension("postgis_topology")
                .HasAnnotation("Relational:Collation", "C.UTF-8");

            modelBuilder.Entity<ZogGeometry>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("zog_geometry", "gis_test");

                entity.Property(e => e.Geometry).HasColumnName("geometry");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SRId).HasColumnName("srid");

                entity.Property(e => e.GeometryType).HasColumnName("geometry_type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
