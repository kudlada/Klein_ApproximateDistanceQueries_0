using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{
    class Dijkstra //Fin
    {
        Node s, t;
        int openC, scanC;
        long lmId;
        public enum algType { simple, aStar, LM, all };
        
        public List<Node> GetShortestPath(long sid, long tid, algType type,
            Dictionary<long, Node> nodes, out int op, out int sc)
        {

            double D = 0;
            t = nodes[tid];
            s = nodes[sid];
            bool found = false;
            PriorityQueue<Node> opened = new PriorityQueue<Node>();
           // SortedList<double, Node> opened = new SortedList<double, Node>();
            Init(ref nodes, type, false);
            Open(0, ref s, null, ref opened);
            Search(opened, type, ref found, ref D);

            List<Node> path = new List<Node>();
            if (found)
                path = GetPath(t);
            op = openC;
            sc = scanC;

            // Image im = new Image(na);
            // im.GetImageFile(g, path, s, t);

            return path;
        }

        void Init(ref Dictionary<long, Node> nodes, algType type, bool recursive)
        {
            if (!recursive)  //never rec. -smazat
            {
                lmId = 0;
                openC = 0;
                scanC = 0;
            }
            if (type == algType.LM)
                lmId = LMInit();
            foreach (Node n in nodes.Values)
            {
                if (!recursive) //never rec. -smazat
                {
                    n.state = 0;
                    n.distance = int.MaxValue ;
                }
                switch (type)
                {
                    case algType.aStar:
                        n.pot = n.coordinates.GetDist(t.coordinates.utm)*10000; break;
                    case algType.LM:
                        {
                            if (n.LMdist.TryGetValue(lmId, out double di))
                                n.pot = di - t.LMdist[lmId];
                            else
                                n.pot = 0;
                            break;
                        }
                    default: n.pot = 0; break;

                }
                n.pre = null;
            }
            if (type != algType.simple && type != algType.all)
            {
                foreach (Node n in nodes.Values)
                {
                    foreach (Node.weightedEdge e in n.neighbourList)
                    {
                        Node v = e.neighbour;
                        int dd = (int)(e.weight - n.pot + v.pot);
                        if (dd >= 0)
                            e.weight = dd;
                        else
                            e.weight = 0;
                    }

                }
            }


        }


        /*
            Landmarks lms = new Landmarks(g.minLat, g.maxLat, g.minLon, g.maxLon);
            LM = lms.GenerateLatticeLM(50, g.nodes);               
            d.LM = LM;
            d.g = g;
            lms.GetDistTrees(LM, ref g.nodes);
        */

        public List<Node> LM;
        public Graph g;

        long LMInit()
        {
            long lmId = 0;
            Landmarks landmarks = new Landmarks();
            lmId = landmarks.GetNearestLM(LM, s, t);
            return lmId;
        }



        void Open(double d, ref Node n, Node pre, ref PriorityQueue<Node> opened)
        {
            if (n.state == 0)
                openC++;
            n.state = 1;
            n.pre = pre;
            n.distance = d;
            opened.Add((int)d, n);
   

        }

        private void Search(PriorityQueue<Node> opened,
            algType type, ref bool found, ref double D)
        {
            Node last;
            while (opened.Count > 0 && !found)
            {

                last = opened.RemoveMin();
                D = last.distance;
                //...
                last.state = 2;
                scanC++;
                if (type != algType.all)
                {
                    if (last.id == t.id)
                    {
                        found = true;
                        break;
                    }
                }
                else
                {
                    if (!last.LMdist.ContainsKey(s.id))
                        last.LMdist.Add(s.id, D);
                    else
                        last.LMdist[s.id] = D;
                }
                    
                List<Node.weightedEdge> edges = last.neighbourList;
               // if (type == algType.all)
               //     edges = last.backList;

                foreach (Node.weightedEdge e in edges)
                {
                    if (e.neighbour.inSeparator)
                        continue;
                    if (e.neighbour.state == 0)
                        Open(D + e.weight, ref e.neighbour, last, ref opened);
                    else if (e.neighbour.distance > D + e.weight)
                    {
                        opened.RemoveElement((int)e.neighbour.distance);
                        Open(D + e.weight, ref e.neighbour, last, ref opened);
                    }
                }

            }
        }

        void AddIntoSortedList(ref SortedList<double, Node> openNodes, Node n)  //zkontrolovat na presnost
        {
            double epsilon = 0.00000000001;
            double dist = n.distance;
            Node outN;
            while (openNodes.TryGetValue(dist, out outN))
            {

                dist = dist + epsilon;

            }
            n.distance = dist;
            openNodes.Add(dist, n);
        }

        List<Node> GetPath(Node t)
        {
            List<Node> path = new List<Node>();
            Node v = t;
            while (v.id != s.id)
            {
                path.Add(v);
                v = v.pre;
                if (v == null)
                    break;
            }
            path.Add(v);
            return path;
        }
    }
}
