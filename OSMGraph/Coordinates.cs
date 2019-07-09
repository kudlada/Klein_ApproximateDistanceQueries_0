using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using ProjNet.Converters;

namespace Klein_ApproximateDistanceQueries_0
{
    class Coordinates
    {
        public enum DistanceType { Miles, Kilometers };
        public Position position;
        public double[] utm;

        public Coordinates(double Lat, double Lon)      // TODO: zrusit position.Lat ..!
        {
            Position p = new Position();
            p.Latitude = Lat;
            p.Longitude = Lon;
            position = p;
            utm = ConvertToUTM(Lat, Lon);
        }

        public double[] ConvertToUTM(double lat, double lon)
        {
            CoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
            CoordinateSystem utm33 = ProjectedCoordinateSystem.WGS84_UTM(33, true);
            var fact = new CoordinateTransformationFactory();
            var transformation = fact.CreateFromCoordinateSystems(wgs84, utm33);
            double[] utm = transformation.MathTransform.Transform(new double[] { lon, lat });
            return utm;
        }



        public struct Position   //zmenit
        {
            public double latitude;   //private !!!
            public double longitude;
            public double Latitude
            { get { return latitude; } set { latitude = value; } }
            public double Longitude
            { get { return longitude; } set { longitude = value; } }
        }

        public double pictureCrd1 = 0;
        public double pictureCrd2 = 0;

        public void SetPictureCrd(int crd1, int crd2)
        {
            pictureCrd1 = crd1;
            pictureCrd2 = crd2;
        }

        public double GetDist(double lat1, double long1)
        {

            Position pos1 = new Position();
            pos1.Latitude = lat1;
            pos1.Longitude = long1;
            double lat2 = position.Latitude;
            double long2 = position.Longitude;
            Position pos2 = new Position();
            pos2.Latitude = lat2;
            pos2.Longitude = long2;

            Haversine hv = new Haversine();
            double result = hv.Distance(pos1, pos2, DistanceType.Kilometers);
            result = result * 1000;                                             // KM -> M
            return result;
        }

        public double GetDist(double[] utmIn)
        {
            double x = Math.Pow(utm[0] - utmIn[0], 2);
            double y = Math.Pow(utm[1] - utmIn[1], 2);
            double distance = (Math.Sqrt(x + y)) / 1000;
            return distance;
        }

        class Haversine
        {

            public double Distance(Position pos1, Position pos2, DistanceType type)
            {

                double R = (type == DistanceType.Kilometers) ? 3960 : 6371;
                double latDif = pos2.Latitude - pos1.Latitude;

                double longDif = pos2.Longitude - pos1.Longitude;

                double dLat = this.toRadian(latDif);
                double dLon = this.toRadian(longDif);

                double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(this.toRadian(pos1.Latitude)) * Math.Cos(this.toRadian(pos2.latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
                double d = R * c;

                return d;
            }


            private double toRadian(double val)
            {
                return (Math.PI / 180) * val;
            }


        }

    }
}