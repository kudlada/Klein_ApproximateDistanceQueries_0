using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Bfs
    {

        public static List<long> part0;


        public static void Src_all_bfs(PlanarNode src, Dictionary<long, PlanarNode> planarN,
            Dictionary<int, PlanarEdge> planarEdges)
        {
            Src_all_bfs(src,planarN, planarEdges, true);
        }

        public static void Src_all_bfs(PlanarNode src, Dictionary<long, PlanarNode> planarN,
            Dictionary<int, PlanarEdge> planarEdges,
            bool parentUpdate)
        {

            foreach (PlanarNode n in planarN.Values)
            {
                if (parentUpdate)
                {
                    n.parent = null;
                    n.children = new List<PlanarNode>();
                    n.dist = int.MaxValue;
                    n.state = 0;
                }
            }
            PriorityQueue<PlanarNode> opened = new PriorityQueue<PlanarNode>();
        //    List<PlanarNode> opened = new List<PlanarNode>();
            opened.Add(0,src);
            src.dist = 0;
            if (!parentUpdate)
            {
                part0 = new List<long>();
                part0.Add(src.nid);
            }

            while (opened.Count > 0)
                Close(opened, planarEdges, parentUpdate);
        }

        private static void Close(PriorityQueue<PlanarNode> opened,
            Dictionary<int, PlanarEdge> planarEdges, bool parentUpdate)
        {

            PlanarNode n = opened.RemoveMin();
       //     opened.RemoveAt(0);
            n.state = 2;
            foreach (int eid in n.edgesIds)
            {
                if (!planarEdges.ContainsKey(eid))
                    continue;
                PlanarEdge e = planarEdges[eid];
                if (e.trgl)
                    continue;
                PlanarNode neigh = e.GetNeigh(n);
                if (neigh == null)
                    continue;
                if (neigh.dist > n.dist + e.w)
                {
                    e.parent = n;
                    e.inTree = true;
                    if (parentUpdate)
                    {
                        neigh.parent = n;
                        if (!neigh.insideCycle)
                            neigh.insideCycle = neigh.parent.insideCycle;


                      //  n.children.Add(neigh);
                    }

                    else
                        part0.Add(n.nid);
                    neigh.dist = n.dist + e.w;
                    neigh.state = 1;
                    opened.Add(neigh.dist, neigh);
                }
            }

        }
    }
}
