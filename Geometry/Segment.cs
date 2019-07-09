using Klein_ApproximateDistanceQueries_0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{
    class Segment
    {


        public SegmentEquation equation;
        public struct Vector
        { public double x; public double y; };

        static List<Segment> overlap;
        static List<Segment> duplicate;
        public Vector upperEnd;
        public Vector lowerEnd;
        Vector direction;
        public double directionNr;
        double minT = -1;
        double maxT = 0;
        bool vertical = false;
        public Vector rEnd;
        public Vector lEnd;
        public Segment(Vector a, Vector b)
        {
            SetEnds(a, b);
        }

        public Segment(Node n0, Node n1)
        {
            Segment.Vector a = new Segment.Vector();
            a.x = n0.coordinates.position.longitude;
            a.y = n0.coordinates.position.latitude;
            Segment.Vector b = new Segment.Vector();
            b.x = n1.coordinates.position.longitude;
            b.y = n1.coordinates.position.latitude;
            SetEnds(a, b);
        }


        void SetEnds(Vector a, Vector b)
        {
            if ((a.y > b.y) || (a.y == b.y && a.x < b.x))     //  -- ?
            {
                upperEnd = a;
                lowerEnd = b;
            }
            else
            {
                upperEnd = b;
                lowerEnd = a;
            }
            if ((a.x > b.x) || (a.x == b.x && a.y > b.y))     //  -- ?
            {
                rEnd = a;
                lEnd = b;
            }
            else
            {
                lEnd = a;
                rEnd = b;
            }

            direction.x = upperEnd.x - lowerEnd.x;
            direction.y = upperEnd.y - lowerEnd.y;
            if (direction.x != 0)
                directionNr = direction.y / direction.x;
            else
                vertical = true;
            equation = new SegmentEquation();
            equation.dir = direction;
            equation.a = upperEnd;
        }

        public bool HasIntersection(Segment s, out Vector intersection)   //point intersection below line
        {

            if (!PossibleIntersection(s))
            {
                intersection.x = -1;
                intersection.y = -1;
                return false;
            }
            intersection.x = 0;
            intersection.y = 0;
            if (vertical && s.vertical)
            {
                if (upperEnd.x == s.upperEnd.x)// !!
                    GetColinearOverlap(s);
                return false;
            }
            if (vertical)
                return HasVerticalIntersection(this, s, ref intersection);
            if (s.vertical)
                return HasVerticalIntersection(s, this, ref intersection);
            return HasNonVerticalIntersection(s, ref intersection);
        }

        bool PossibleIntersection(Segment s)
        {
            return (lEnd.x <= s.rEnd.x && s.lEnd.x <= rEnd.x);
        }

        bool HasVerticalIntersection(Segment vert, Segment s, ref Vector intersection)
        {
            double t = (vert.upperEnd.x - s.upperEnd.x) / s.direction.x;
            double at = Math.Abs(t);
            if (0 <= at && at <= 1)
            {
                intersection.x = vert.upperEnd.x;
                intersection.y = s.upperEnd.y + t * s.direction.y;
                return true;
            }
            return false;
        }

        bool HasNonVerticalIntersection(Segment s, ref Vector intersection)
        {
            double a = lowerEnd.y - upperEnd.y;
            double b = upperEnd.x - lowerEnd.x;
            double c = a * upperEnd.x + b * upperEnd.y;
            double sa = s.lowerEnd.y - s.upperEnd.y;
            double sb = s.upperEnd.x - s.lowerEnd.x;
            double sc = sa * s.upperEnd.x + sb * s.upperEnd.y;

            double det = a * sb - sa * b;

            if (det == 0)
            {
                //par.
                return false;


            }
            else
            {
                intersection.x = (sb * c - b * sc) / det;
                intersection.y = (a * sc - sa * c) / det;
                double minX = Math.Min(upperEnd.x, lowerEnd.x);
                double maxX = Math.Max(upperEnd.x, lowerEnd.x);
                if (!(minX < intersection.x && intersection.x < maxX))
                    return false;

                minX = Math.Min(s.upperEnd.x, s.lowerEnd.x);
                maxX = Math.Max(s.upperEnd.x, s.lowerEnd.x);
                if (!(minX < intersection.x && intersection.x < maxX))
                    return false;


                double x = equation.GetXCoordinate(intersection.y);



                return true;

            }


        }

        void GetColinearOverlap(Segment s)
        {
            return;
            if (upperEnd.y == s.upperEnd.y && s.lowerEnd.y == lowerEnd.y)
            {
                duplicate.Add(this);
                duplicate.Add(s);

            }
            else if (upperEnd.y == s.lowerEnd.y || s.upperEnd.y == lowerEnd.y)
            {

                overlap.Add(this);
                overlap.Add(s);
            }
        }

        bool ContainsPoint(Vector v)
        {
            double t = (v.x - upperEnd.x) / direction.x;
            return (v.y == upperEnd.y + t);
        }
    }

    class SegmentEquation
    {
        public Segment.Vector dir;
        public Segment.Vector a;

        public double GetXCoordinate(double Y)   // Y .. horiz. line
        {
            if (dir.y == 0)
                return a.x; //horiz.
            double t = (Y - a.y) / dir.y;
            return (a.x + t * dir.x);
        }
    }


}
