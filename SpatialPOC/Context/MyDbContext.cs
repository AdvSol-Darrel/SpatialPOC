using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SpatialPOC.Models;

#nullable disable

namespace SpatialPOC.Context
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobRunDetail> JobRunDetails { get; set; }
        public virtual DbSet<Layer> Layers { get; set; }
        public virtual DbSet<Topology> Topologies { get; set; }
        public virtual DbSet<ZogLine> ZogLines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("hstore")
                .HasPostgresExtension("pg_cron")
                .HasPostgresExtension("pgrouting")
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("postgis_raster")
                .HasPostgresExtension("postgis_topology")
                .HasAnnotation("Relational:Collation", "C.UTF-8");

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("job", "cron");

                entity.Property(e => e.Jobid)
                    .HasColumnName("jobid")
                    .HasDefaultValueSql("nextval('cron.jobid_seq'::regclass)");

                entity.Property(e => e.Active)
                    .IsRequired()
                    .HasColumnName("active")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Command)
                    .IsRequired()
                    .HasColumnName("command");

                entity.Property(e => e.Database)
                    .IsRequired()
                    .HasColumnName("database")
                    .HasDefaultValueSql("current_database()");

                entity.Property(e => e.Nodename)
                    .IsRequired()
                    .HasColumnName("nodename")
                    .HasDefaultValueSql("'localhost'::text");

                entity.Property(e => e.Nodeport)
                    .HasColumnName("nodeport")
                    .HasDefaultValueSql("inet_server_port()");

                entity.Property(e => e.Schedule)
                    .IsRequired()
                    .HasColumnName("schedule");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasDefaultValueSql("CURRENT_USER");
            });

            modelBuilder.Entity<JobRunDetail>(entity =>
            {
                entity.HasKey(e => e.Runid)
                    .HasName("job_run_details_pkey");

                entity.ToTable("job_run_details", "cron");

                entity.Property(e => e.Runid)
                    .HasColumnName("runid")
                    .HasDefaultValueSql("nextval('cron.runid_seq'::regclass)");

                entity.Property(e => e.Command).HasColumnName("command");

                entity.Property(e => e.Database).HasColumnName("database");

                entity.Property(e => e.EndTime)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("end_time");

                entity.Property(e => e.JobPid).HasColumnName("job_pid");

                entity.Property(e => e.Jobid).HasColumnName("jobid");

                entity.Property(e => e.ReturnMessage).HasColumnName("return_message");

                entity.Property(e => e.StartTime)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("start_time");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Username).HasColumnName("username");
            });

            modelBuilder.Entity<Layer>(entity =>
            {
                entity.HasKey(e => new { e.TopologyId, e.LayerId })
                    .HasName("layer_pkey");

                entity.ToTable("layer", "topology");

                entity.HasIndex(e => new { e.SchemaName, e.TableName, e.FeatureColumn }, "layer_schema_name_table_name_feature_column_key")
                    .IsUnique();

                entity.Property(e => e.TopologyId).HasColumnName("topology_id");

                entity.Property(e => e.LayerId).HasColumnName("layer_id");

                entity.Property(e => e.ChildId).HasColumnName("child_id");

                entity.Property(e => e.FeatureColumn)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("feature_column");

                entity.Property(e => e.FeatureType).HasColumnName("feature_type");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.SchemaName)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("schema_name");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("table_name");

                entity.HasOne(d => d.Topology)
                    .WithMany(p => p.Layers)
                    .HasForeignKey(d => d.TopologyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("layer_topology_id_fkey");
            });

            modelBuilder.Entity<Topology>(entity =>
            {
                entity.ToTable("topology", "topology");

                entity.HasIndex(e => e.Name, "topology_name_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('topology_id_seq'::regclass)");

                entity.Property(e => e.Hasz).HasColumnName("hasz");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.Precision).HasColumnName("precision");

                entity.Property(e => e.Srid).HasColumnName("srid");
            });

            modelBuilder.Entity<ZogLine>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("zog_lines", "gis_test");

                entity.Property(e => e.Geometry).HasColumnName("geometry");

                entity.Property(e => e.LineId).HasColumnName("line_id");
            });

            modelBuilder.HasSequence("jobid_seq", "cron");

            modelBuilder.HasSequence("runid_seq", "cron");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
