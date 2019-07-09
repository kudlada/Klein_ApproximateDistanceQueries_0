using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Triangulation
    {
        static int newId = 0;
        public static PlanarGraph GetTriangulation(
            PlanarGraph g, out List<int> tri)  //sloz. 
        {
            
            
            tri = new List<int>();
            PlanarGraph trnG = new PlanarGraph(g.planarNodes,
                new Dictionary<int, PlanarEdge>( g.planarEdges));
            newId= g.planarEdges.Keys.Max() + 1;
            foreach (PlanarNode n in trnG.planarNodes.Values)
            {
              
                if (n.edgesIds.Count < 2)
                    continue;
                for (int i = 0; i < n.edgesIds.Count - 1; i++)
                    GetTriangulationFromTo(tri,trnG,n, n.edgesIds[i], n.edgesIds[i + 1]);
                GetTriangulationFromTo(tri,trnG, n, n.edgesIds[n.edgesIds.Count - 1],
                    n.edgesIds[0]);
            }
       
            return trnG;
        }

        private static void GetTriangulationFromTo(List<int> tri , PlanarGraph g,
            PlanarNode n1,int eid0,int eid1)
        {
            PlanarEdge e0 = g.planarEdges[eid0];
      
            PlanarEdge e1 = g.planarEdges[eid1];
            PlanarNode n0 = e0.GetNeigh(n1);
            PlanarNode n2 = e1.GetNeigh(n1);
            if (!n2.edgesIds.Contains(e1.eid))
                return;
            int fid = e1.GetNextEdgeId(n2);
            PlanarEdge f = g.planarEdges[fid];
            PlanarNode n3 = f.GetNeigh(n2);
            if (n0.nid == n3.nid)
                return;        //triangle
            PlanarEdge te = new PlanarEdge(n0,n2,0,0,0);
            te.trgl = true;
            te.eid = newId;
            newId++;
            InsertAsFirst(n0, eid0, te.eid);
            InsertAsSecond(n2, eid1, te.eid);
            g.planarEdges.Add(te.eid, te);
            tri.Add(te.eid);
        }

        private static void InsertAsSecond(PlanarNode n, int eid, int newEid)
        {
            int i0 = n.edgesIds.IndexOf(eid);
            if (i0 == n.edgesIds.Count - 1)
                n.edgesIds.Add(newEid);
            else
                n.edgesIds.Insert(i0+1, newEid);

        }

        private static void InsertAsFirst(PlanarNode n, int eid, int newEid)
        {
            if (n.edgesIds.Contains(eid))
              n.edgesIds.Insert(n.edgesIds.IndexOf(eid), newEid);
            
        }
    }
}
