using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{
    class SeparatorUtils
    {
        public static void UpdateNextEdges(PlanarNode src, Dictionary<int, PlanarEdge> planarEdges) //jen jeden smer
        {

            List<int> next = new List<int>(src.edgesIds);
            next.RemoveAt(0);
            next.Add(src.edgesIds.First());
            foreach (int eid in src.edgesIds)
            {
                UpdateNextEdges(src, planarEdges[eid], planarEdges[next[0]]);
            }
        }


        private static void UpdateNextEdges(PlanarNode src, PlanarEdge e, PlanarEdge nextE)
        {
            if (src == e.neighboursAdjEdges[0])
                e.neighboursAdjEdges[2] = nextE;
            else if (src == e.neighboursAdjEdges[1])
                e.neighboursAdjEdges[3] = nextE;
            else
                throw new Exception();


        }

        public static void RemoveEdges(PlanarGraph g, PlanarNode src, int minLevel, int maxLevel)  //jde udelat ryz=chleji primo v alg.sep.
        {
            foreach (PlanarNode n in g.planarNodes.Values)
            {
                if (n.parent != null && !g.planarNodes.ContainsKey(n.parent.nid))
                    n.parent = src;

            }

            List<int> keys = new List<int>(g.planarEdges.Keys);
            foreach (int key in keys)
            {
                PlanarEdge e = g.planarEdges[key];
                PlanarNode n0 = (PlanarNode)e.neighboursAdjEdges[0];
                PlanarNode n1 = (PlanarNode)e.neighboursAdjEdges[1];
                if (IsOutsideLevels(n0.dist, n1.dist, minLevel, maxLevel))
                    g.planarEdges.Remove(key);
                else
                {
                    if (!g.planarNodes.ContainsKey(n0.nid))
                        g.planarNodes.Add(n0.nid, n0);
                    if (!g.planarNodes.ContainsKey(n1.nid))
                        g.planarNodes.Add(n1.nid, n1);
                }
            }
            foreach (PlanarEdge e in g.planarEdges.Values)
            {
                CheckNextEdges(g, e);  //smazat

            }
            foreach (PlanarNode n in g.planarNodes.Values)
            {

                CheckEdgesIds(g, n);
            }
        }

        private static bool IsOutsideLevels(int dist0, int dist1, int minLevel, int maxLevel)
        {
            if (dist0 + dist1 < minLevel)
                return true;
            return (dist0 != 0 && dist1 != 0
                && ((dist0 <= minLevel || dist1 <= minLevel)
                     ||
                     (dist0 >= maxLevel || dist1 >= maxLevel))
                     );
        }

        private static void CheckNextEdges(PlanarGraph g, PlanarEdge e)
        {
            PlanarEdge ne = (PlanarEdge)e.neighboursAdjEdges[2];
            if (!g.planarEdges.Keys.Contains(ne.eid))
                e.neighboursAdjEdges[2] = FindAndSetNewNextEdge(g, (PlanarNode)e.neighboursAdjEdges[0], ne);
            //          ne = (PlanarEdge)e.neighboursAdjEdges[4];
            //          if (!g.planarEdges.Keys.Contains(ne.eid))
            //              e.neighboursAdjEdges[4] = FindAndSetNewNextEdge(g, (PlanarNode)e.neighboursAdjEdges[1], ne);

        }


        private static void CheckEdgesIds(PlanarGraph g, PlanarNode n)
        {
            List<int> tmp = new List<int>();
            foreach (int eid in n.edgesIds)
            {
                if (g.planarEdges.ContainsKey(eid))
                    tmp.Add(eid);
            }
            n.edgesIds = tmp;
        }

        private static PlanarEdge FindAndSetNewNextEdge(PlanarGraph g, PlanarNode n, PlanarEdge ne)
        {
            int ind = n.edgesIds.FindIndex(x => x == ne.eid);
            int neid = ne.eid;
            while (!g.planarEdges.ContainsKey(neid))
            {

                n.edgesIds.Remove(neid);
                if (ind == n.edgesIds.Count)
                    ind = n.edgesIds.Count() - 1;
                neid = n.edgesIds.ElementAt(ind);
            }
            ne = g.planarEdges[neid];
            return ne;
        }

        private static bool IsOnBoundary(int dist0, int dist1, int minLevel, int maxLevel)  //nepouzito
        {
            return (dist0 != 0 && dist1 != 0
                && (dist0 == minLevel || dist0 == maxLevel
                     || dist1 == minLevel || dist1 == maxLevel));
        }



        public static void SetAllSumsOfDescNodesIncludeItself(PlanarGraph g,PlanarNode root,
            int cost)
        {
            Dictionary<long, List<PlanarNode>> children = new Dictionary<long, List<PlanarNode>>();
            

            foreach (PlanarNode pn in g.planarNodes.Values)
                if (pn.parent != null)
                {
                    if (!children.Keys.Contains(pn.parent.nid))
                        children[pn.parent.nid] = new List<PlanarNode>();
                    children[pn.parent.nid].Add(pn);

                }
                
            SetSumOfDescNodesIncludeItself(g, root, children);

        }

        private static int SetSumOfDescNodesIncludeItself(PlanarGraph g, PlanarNode pn,
            Dictionary<long, List<PlanarNode>> children)
        {
            if (!children.Keys.Contains(pn.nid))
            {
                pn.cost = 1;
                return 1;
            }
            int sum = 1;
            foreach (PlanarNode n in children[pn.nid])
            {
                sum = sum + SetSumOfDescNodesIncludeItself(g, n, children);
            }
            pn.cost = sum;
            return sum;
        }


        private static void GetFacesAndTriangulation(PlanarGraph g)
        {
            foreach (PlanarNode n in g.planarNodes.Values)
            {
                Console.WriteLine(n.nid);
                foreach (int i in n.edgesIds)
                    Console.WriteLine
                        (((PlanarNode)(g.planarEdges[i].neighboursAdjEdges[0])).nid + ", " +
                       ((PlanarNode)(g.planarEdges[i].neighboursAdjEdges[1])).nid);
                Console.WriteLine();
            }
            Console.ReadKey();
            foreach (PlanarEdge e in g.planarEdges.Values)
                e.state = 0;

            foreach (PlanarNode n in g.planarNodes.Values)
            {
                PlanarEdge e;
                List<int> eids = new List<int>(n.edgesIds);
                foreach (int eid in eids)
                {
                    try
                    {
                        e = g.planarEdges[eid];
                        if (e.state == 2 ||
                            !g.planarNodes.Keys.Contains(e.GetNeigh(n).nid))
                            continue;
                    }
                    catch
                    {
                        continue;
                    }


                    GetFaceAndTriangulate(g, e, true);

                }

            }
        }

        private static void GetFaceAndTriangulate(PlanarGraph g, PlanarEdge e0, bool forward)
        {
            List<long> nids = new List<long>();
            PlanarNode n0 = (PlanarNode)e0.neighboursAdjEdges[0];
            PlanarNode n1 = (PlanarNode)e0.neighboursAdjEdges[1];

            PlanarNode n2, n3;
            int i0 = n1.edgesIds.IndexOf(e0.eid);   //TODO: metoda
            if (i0 == n1.edgesIds.Count - 1)
                i0 = 0;
            else
                i0++;
            PlanarEdge e1 = g.planarEdges[n1.edgesIds.ElementAt(i0)];
            n2 = e1.GetNeigh(n1);
            i0 = n2.edgesIds.IndexOf(e1.eid);   //TODO: metoda
            if (i0 == n2.edgesIds.Count - 1)
                i0 = 0;
            else
                i0++;
            PlanarEdge e2 = g.planarEdges[n2.edgesIds.ElementAt(i0)];
            n3 = e2.GetNeigh(n2);
            if (n3.nid == n0.nid)
                return;                   //triangle


            PlanarEdge ne;

            if (n0 == n2 && n1 != n3)
            {
                ne = new PlanarEdge(n1, n3, e0);  //n1.edgesIds==<n0>
                ne.eid = g.planarEdges.Keys.Max() + 1;
                n1.edgesIds.Insert(0, ne.eid);
                n3.edgesIds.Insert(Math.Max(0, n3.edgesIds.IndexOf(e2.eid) - 1), ne.eid);
                g.planarEdges.Add(ne.eid, ne);
                return;
            }
            else if (n1 != n3)
                ne = new PlanarEdge(n0, n2, e0);
            else
                throw new NotImplementedException();
            //return;

            ne.eid = g.planarEdges.Keys.Max() + 1;
            if (n0.edgesIds[0] == e0.eid)
            {

                n0.edgesIds.Add(ne.eid);
            }
            else
            {
                int ind3 = n0.edgesIds.IndexOf(e0.eid) - 1;

                n0.edgesIds.Insert(ind3 + 1, ne.eid);
            }
            n2.edgesIds.Insert(n2.edgesIds.IndexOf(e2.eid), ne.eid);
            g.planarEdges.Add(ne.eid, ne);

            /*   if (e3.neighboursAdjEdges[0] == n0)
                   e3.neighboursAdjEdges[2] = ne;
               else
                   e3.neighboursAdjEdges[4] = ne;
   */

            e0.state++;
            e1.state++;
            nids.Add(n0.nid);
            nids.Add(n1.nid);
            nids.Add(n2.nid);
            nids.Add(n3.nid);
            while (n3.nid != n0.nid)

            {
                nids.Add(n3.nid);
                n2 = n3;
                e2 = e2.GetNextEdge(n2, out n3);
                e2.state++;
                n3 = e2.GetNeigh(n2);

            }
        }


    }
}
