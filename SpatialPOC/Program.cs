using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpatialPOC.Context;
using SpatialPOC.Mappings;
using SpatialPOC.Services;

namespace SpatialPOC
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        static void Main(string[] args)
        {
            #region Service Provider Setup

            string connectionString = "host=localhost;port=5432;database=gis;user id=docker;password=docker";
            var svcCollection = new ServiceCollection();
            svcCollection.AddSingleton<ZogLineService>();
            svcCollection.AddDbContext<MyDbContext>(options => {
                options.UseNpgsql(connectionString, o => o.UseNetTopologySuite());
            });
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToModel());
                cfg.AddProfile(new ModelToEntity());
            });

            var mapper = mappingConfig.CreateMapper();
            svcCollection.AddSingleton(mapper);


            ServiceProvider svcProvider = svcCollection.BuildServiceProvider();

            #endregion

            #region Reproject
            Console.WriteLine("Setup Complete!");

            var reprojection = new Reprojection();

            Console.WriteLine("\nPerforming Reprojections NAD83 to WGS84");
            reprojection.ReprojectNAD83toWGS84();

            Console.WriteLine("\nPerforming Reprojections WGS84 to NAD83\n");
            reprojection.ReprojectWGS84toNAD83();
            #endregion

            #region Get Line info from DB
            var zogLineSvc = svcProvider.GetService<ZogLineService>();
            
            var lines = zogLineSvc.GetLines();
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    reprojection.ReprojectNAD83toWGS84(line.Geometry);
                }
            }
            
            Console.ReadLine();
            #endregion  
        }
    }
}