using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Diagnostics;

namespace SpatialPOC
{
    public class Reprojection
    {
        static protected GeometryFactory _geometryFactory;
        
        const string srid3005 = "PROJCS[\"NAD83 / BC Albers\",GEOGCS[\"NAD83\",DATUM[\"North_American_Datum_1983\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[\"EPSG\",\"6269\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4269\"]],PROJECTION[\"Albers_Conic_Equal_Area\"],PARAMETER[\"standard_parallel_1\",50],PARAMETER[\"standard_parallel_2\",58.5],PARAMETER[\"latitude_of_center\",45],PARAMETER[\"longitude_of_center\",-126],PARAMETER[\"false_easting\",1000000],PARAMETER[\"false_northing\",0],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH],AUTHORITY[\"EPSG\",\"3005\"]]";
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

        static Reprojection()
        {
            _geometryFactory = new GeometryFactory();
        }

        public void ReprojectNAD83toWGS84(LineString line)
        {
            //var point = new GeometryFactory().CreatePoint(new CoordinateZ(992000.68836478, 922777.27310679, 1));
            Console.WriteLine($"3005 LS - {line.ToText()}");

            var rpGeom = ReprojectGeometry(line, srid3005, srid4326);

            Console.WriteLine($"4326 LS - {line.ToText()}");
        }

        public void ReprojectNAD83toWGS84()
        {
            var point = new GeometryFactory().CreatePoint(new CoordinateZ(992000.68836478, 922777.27310679, 1));
            Console.WriteLine($"3005 LS - {point.ToText()}");

            var rpGeom = ReprojectGeometry(point, srid3005, srid4326);

            Console.WriteLine($"4326 LS - {rpGeom.ToText()}");
        }

        public void ReprojectWGS84toNAD83(LineString line)
        {
            //var point = new GeometryFactory().CreatePoint(new CoordinateZ(-123.6121899, 48.8001664, 148.4268458));
            Console.WriteLine($"4326 LS - {line.ToText()}");

            var rpGeom = ReprojectGeometry(line, srid4326, srid3005);

            Console.WriteLine($"3005 LS - {rpGeom.ToText()}");
        }

        public void ReprojectWGS84toNAD83()
        {

            var point = new GeometryFactory().CreatePoint(new CoordinateZ(-123.6121899, 48.8001664, 148.4268458));
            Console.WriteLine($"4326 LS - {point.ToText()}");

            var rpGeom = ReprojectGeometry(point, srid4326, srid3005);

            Console.WriteLine($"3005 LS - {rpGeom.ToText()}");
        }

        private static Geometry ReprojectGeometry(Geometry geom, string FromWKT, string ToWKT)
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