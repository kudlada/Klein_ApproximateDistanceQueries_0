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
        

        public static void Src_all_bfs(PlanarNode src, Dictionary<int, PlanarEdge> planarEdges)
        {
            Src_all_bfs(src, planarEdges, true);
        }

        public static void Src_all_bfs(PlanarNode src, Dictionary<int, PlanarEdge> planarEdges,
            bool parentUpdate)
        {
            List<PlanarNode> opened = new List<PlanarNode>();
            opened.Add(src);
            if (!parentUpdate)
                {
                    part0 = new List<long>();
                    part0.Add(src.nid);
                }
           
            while (opened.Count > 0)
                Close(opened,planarEdges, parentUpdate);
        }

        private static void Close(List<PlanarNode> opened, 
            Dictionary<int, PlanarEdge> planarEdges, bool parentUpdate)
        {
            
            PlanarNode n = opened[0];
            opened.RemoveAt(0);
            n.state = 2;
            foreach (int eid in n.edgesIds)
            {
                if (!planarEdges.ContainsKey(eid))
                    continue;
                PlanarEdge e = planarEdges[eid];
                PlanarNode neigh = e.GetNeigh(n);
                if (neigh.state == 0)
                {
                    e.parent = n;
                    e.inTree = true;
                    if (parentUpdate)
                        neigh.parent = n;
                    else
                        part0.Add(n.nid);
                    neigh.dist = n.dist + e.w;
                    neigh.state = 1;
                    opened.Add(neigh);
                }
            }

        }
    }
}
