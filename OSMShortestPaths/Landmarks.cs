using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Klein_ApproximateDistanceQueries_0
{
    class Landmarks
    {
        double minLat;
        double maxLat;
        double minLon;
        double maxLon;
        

        public Landmarks()
        { }

        public Landmarks(double minLatIn, double maxLatIn, double minLonIn, double maxLonIn)
        {
            minLat = minLatIn;
            maxLat = maxLatIn;
            minLon = minLonIn;
            maxLon = maxLonIn;


        }

        public void GetDistTrees(List<Node> LM, ref Dictionary<long, Node> nodes)
        {
            Dijkstra d = new Dijkstra();
            foreach (Node n in LM)
            {
                Node s;
                nodes.TryGetValue(n.id, out s);
                d.GetShortestPath(n.id, 0, Dijkstra.algType.all, nodes, out int op, out int sc);

            }
        }

        public List<Node> GenerateLatticeLM(double d, Dictionary<long, Node> nodes)
        {
            Dictionary<Tuple<double, double>, List<long>> parts = new Dictionary<Tuple<double, double>, List<long>>();
            Tuple<double, double> t;
            Coordinates cN = new Coordinates(maxLat, minLon);
            cN.utm = cN.ConvertToUTM(cN.position.latitude, cN.position.longitude);
            Coordinates cS = new Coordinates(minLat, minLon);
            cS.utm = cS.ConvertToUTM(cS.position.latitude, cS.position.longitude);
            double diffLat = cN.GetDist(cS.utm);
            Coordinates cE = new Coordinates(minLat, maxLon);
            cE.utm = cE.ConvertToUTM(cE.position.latitude, cE.position.longitude);
            Coordinates cW = new Coordinates(minLat, minLon);
            cW.utm = cW.ConvertToUTM(cW.position.latitude, cW.position.longitude);
            double diffLon = cE.GetDist(cW.utm);

            double h = (diffLat / d);

            double w = (diffLon / d);
            for (int j = 0; j <= (int)w; j++)
            {
                for (int i = 0; i <= (int)h; i++)
                {
                    List<long> list = new List<long>();
                    t = new Tuple<double, double>(i, j); //array 

                    parts.Add(t, list);
                }
            }
            double fractH = (maxLat - minLat) / h;
            double fractW = (maxLon - minLon) / w;
            foreach (Node n in nodes.Values)
            {
                if (n.coordinates.position.latitude == 0 || n.coordinates.position.longitude == 0)
                    continue;
                int hn = (int)((n.coordinates.position.latitude - minLat) / fractH);
                int wn = (int)((n.coordinates.position.longitude - minLon) / fractW);
                t = new Tuple<double, double>(hn, wn);
                parts[t].Add(n.id);
            }
            Random random = new Random(0);
            int r;
            List<Node> latticeLM = new List<Node>();


            foreach (List<long> list in parts.Values)
                if (list.Count > 0)

                {
                    r = random.Next(list.Count);
                    long id = list[r];
                    Node n = nodes[id];

                    latticeLM.Add(n);
                }


            return latticeLM;
        }


        public long GetNearestLM(List<Node> LM, Node s, Node t)
        {
            Node lm = t;
            double dist = 100000;
            Line lineST = new Line(s.coordinates.position.longitude, s.coordinates.position.latitude,
                t.coordinates.position.longitude, t.coordinates.position.latitude);


            foreach (Node n in LM)
            {
                double D;
                if (lineST.IsBehind(n.coordinates.position.longitude, n.coordinates.position.latitude))
                    if (t.LMdist.TryGetValue(n.id, out D))
                        if (D < dist)
                        {
                            lm = n;
                            dist = t.LMdist[n.id];
                        }
            }
            if (lm == t)
            {
                foreach (Node n in LM)
                {
                    double D;
                    if (t.LMdist.TryGetValue(n.id, out D))
                        if (D < dist)
                        {
                            lm = n;
                            dist = t.LMdist[n.id];
                        }
                }
            }
            return lm.id;
        }

        class Line
        {
            double A = 1;
            double B = 0;
            Line perp0, perp1;
            bool B1gtB0 = false;
            double X1;
            double Y1;
            Line(double a, double b)
            {
                A = a;
                B = b;
            }

            public Line(double x0, double y0, double x1, double y1) //nikdy 0
            {
                A = (y1 - y0) / (x1 - x0);
                B = y0 - (A * x0);
                perp0 = new Line(-1 / A, (x0 / A) - y0);
                perp1 = new Line(-1 / A, (x1 / A) - y1);
                B1gtB0 = (perp1.B > perp0.B);
                X1 = x1;
                Y1 = y1;
            }

            public bool IsBehind(double x, double y)
            {
                double xyB = -(x * perp1.A) + y;
                if (B1gtB0 && xyB > perp1.B)
                    return true;
                if (!B1gtB0 && xyB < perp1.B)
                    return true;
                return false;
            }
        }
    }
}
