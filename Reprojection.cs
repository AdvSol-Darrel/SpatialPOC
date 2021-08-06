﻿using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Diagnostics;

namespace SpatialPOC
{
    class Reprojection
    {
        static protected GeometryFactory _geometryFactory;
        
        const string srid3005 = "PROJCS[\"NAD83 / BC Albers\",GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]],PROJECTION[\"Albers_Conic_Equal_Area\"],PARAMETER[\"standard_parallel_1\",50],PARAMETER[\"standard_parallel_2\",58.5],PARAMETER[\"latitude_of_center\",45],PARAMETER[\"longitude_of_center\",-126],PARAMETER[\"false_easting\",1000000],PARAMETER[\"false_northing\",0],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"3005\"]]";
        const string srid4326 = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\", \"7030\"]],AUTHORITY[\"EPSG\", \"6326\"]],PRIMEM[\"Greenwich\", 0,AUTHORITY[\"EPSG\", \"8901\"]],UNIT[\"degree\", 0.01745329251994328,AUTHORITY[\"EPSG\", \"9122\"]],AUTHORITY[\"EPSG\", \"4326\"]]";
        
        sealed class MTF : ICoordinateSequenceFilter
        {
            private readonly MathTransform _mathTransform;

            public MTF(MathTransform mathTransform) => _mathTransform = mathTransform;

            public bool Done => false;
            public bool GeometryChanged => true;
            public void Filter(CoordinateSequence seq, int i)
            {
                double x = seq.GetX(i);
                double y = seq.GetY(i);
                double z = seq.GetZ(i);

                _mathTransform.Transform(ref x, ref y, ref z);

                seq.SetX(i, x);
                seq.SetY(i, y);
                seq.SetZ(i, z);
            }
        }

        static void Main(string[] args)
        {
            _geometryFactory = new GeometryFactory();

            /*ReprojectNAD83toWGS84();
            ReprojectWGS84toNAD83();*/

            //Calculate3DDistanceBetween2Points();

        }

        private static void Calculate3DDistanceBetween2Points()
        {
            //first figure out which UTM zone our coordinates are in
            int utm = (int)Math.Floor((-123.6142205 + 180) / 6) % 60 + 1;

            var originCS = GeographicCoordinateSystem.WGS84;
            var targetCS = ProjectedCoordinateSystem.WGS84_UTM(utm, true);

            var transFactory = new CoordinateTransformationFactory();
            var transform = transFactory.CreateFromCoordinateSystems(originCS, targetCS);

            var pointA = new CoordinateZ(-123.6142205, 48.8012582, 157.6479456);
            var pointB = new CoordinateZ(-123.6121899, 48.8001664, 148.4268458);

            /*var coordA = _geometryFactory.CreatePoint(new CoordinateZ(-123.6142205, 48.8012582, 157.6479456));
            var coordB = _geometryFactory.CreatePoint(new CoordinateZ(-123.6121899, 48.8001664, 148.4268458));
            
            var newCoordA = Transform(coordA, transform.MathTransform);
            var newCoordB = Transform(coordB, transform.MathTransform);
            
            double result = newCoordA.Distance3D(newCoordB);*/

            double result = pointA.Distance3D(pointB);

            Console.WriteLine($"3D Distance between {pointA} and {pointB} is {result}");
        }

        private static void ReprojectNAD83toWGS84()
        {
            //var point = _geometryFactory.CreatePoint(new CoordinateZ(992000.68836478, 922777.27310679, 1000));
            var point = _geometryFactory.CreatePoint(new CoordinateZ(992000.6918534045, 922777.2878385643, 1000.0055629117414));
            Console.WriteLine($"3005 LS - {point.ToText()}");

            var rpGeom = ReprojectGeometry(point, srid3005, srid4326);

            Console.WriteLine($"4326 LS - {rpGeom.ToText()}");
        }

        private static void ReprojectWGS84toNAD83()
        {

            //var point = _geometryFactory.CreatePoint(new CoordinateZ(-123.6121899, 48.8001664, 148.4268458));
            var point = _geometryFactory.CreatePoint(new CoordinateZ(-126.12032035209688, 53.3114257414847, 1000.0054282639176));
            Console.WriteLine($"4326 LS - {point.ToText()}");

            var rpGeom = ReprojectGeometry(point, srid4326, srid3005);

            Console.WriteLine($"3005 LS - {rpGeom.ToText()}");
        }

        public static Geometry ReprojectGeometry(Geometry geom, string FromWKT, string ToWKT)
        {
            var SourceCoordSystem = new CoordinateSystemFactory().CreateFromWkt(FromWKT);
            var TargetCoordSystem = new CoordinateSystemFactory().CreateFromWkt(ToWKT);

            var trans = new CoordinateTransformationFactory().CreateFromCoordinateSystems(SourceCoordSystem, TargetCoordSystem);

            var projGeom = Transform(geom, trans.MathTransform);

            return projGeom;
        }

        static Geometry Transform(Geometry geom, MathTransform transform)
        {
            geom = geom.Copy();
            geom.Apply(new MTF(transform));
            return geom;
        }

        
    }
}