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

        public virtual DbSet<ZogGeometry> ZogGeometries { get; set; }
        public virtual DbSet<ZogWkt> ZogWkts { get; set; }

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

                entity.Property(e => e.GeometryType)
                    .HasColumnType("character varying")
                    .HasColumnName("geometry_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Srid).HasColumnName("srid");
            });

            modelBuilder.Entity<ZogWkt>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ZogWKT", "gis_test");

                entity.HasIndex(e => e.Id, "zog_geometry_id_idx");

                entity.HasIndex(e => e.Id, "zoggeometry_id_idx")
                    .IsUnique();

                entity.Property(e => e.GeometryType)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("geometry_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.WktGeometry)
                    .IsRequired()
                    .HasColumnName("wkt_geometry");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
