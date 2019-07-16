using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class BoundarySeparation
    {
        static long srcId = 21311242;
        static int gc = 0;
        static float c = 4f / 5;
        static int nodesCount = 0;

        public static void GetDecompositionTree(PlanarGraph pg, Graph g)

        {
            nodesCount = g.nodes.Count();
            List<PlanarNode> orderNodes = OrderBoundaryNodes(pg, g, srcId);
            PlanarNode src = pg.planarNodes[srcId];    //  .First().Value;
            srcId = src.nid;
            SeparatorCycle sepCycle = new SeparatorCycle();
            RecursiveSeparation(pg, src, orderNodes, sepCycle);

            Console.ReadKey();
        }

        private static List<PlanarNode> OrderBoundaryNodes(
            PlanarGraph pg, Graph g, long srcId)
        {
            Dictionary<long, Node> boundaryNodes
                = new Dictionary<long, Node>();
          
            foreach (PlanarNode n in pg.planarNodes.Values.Where(x => !x.insideCR))
                if (n.edgesIds.Count > 0)
                    boundaryNodes.Add(g.nodes[n.nid].id, g.nodes[n.nid]);
            Graph boundaryGraph = new Graph();
            boundaryGraph.nodes = boundaryNodes;
            Node centre = new Node(-1);
            centre.coordinates = g.nodes[srcId].coordinates;
            foreach (Node n in boundaryNodes.Values)
            {
                Node.weightedEdge e = new Node.weightedEdge(n);
                e.weight = 1;
                centre.neighbourList.Add(e);

            }
            boundaryNodes.Add(centre.id, centre);
            PlanarGraph boundaryPG = new PlanarGraph(boundaryGraph);
            PlanarNode centrePG = boundaryPG.planarNodes[-1];
            List<PlanarNode> result = new List<PlanarNode>();
            foreach (int eid in centrePG.edgesIds)
                result.Insert(0, boundaryPG.planarEdges[eid].GetNeigh(centrePG));
            return result;
        }

        private static void RecursiveSeparation(PlanarGraph pg,
           PlanarNode src,
           List<PlanarNode> orderNodes, SeparatorCycle sepCycle)
        {
            PlanarEdge sepE;
            Bfs.Src_all_bfs(src, pg.planarNodes, pg.planarEdges, true);
            

            if (pg.planarNodes.Count < nodesCount/10
                || !FindSeparator(src, orderNodes, pg, sepCycle, out sepE))
            {
                Console.WriteLine("Leaf:" + gc + "  " + pg.planarNodes.Count);
                return;
            }

            List<PlanarNode> cycleNodes
                = sepCycle.cycle
                .Where(x => pg.planarNodes.Keys.Contains(x))
                .Select(x=> pg.planarNodes[x]).ToList();
            gc++;
            PlanarGraph g0 = new PlanarGraph(sepCycle.inC, cycleNodes, src,
                new Dictionary<int, PlanarEdge>(pg.planarEdges));

            PlanarGraph g1 = new PlanarGraph(sepCycle.outC, cycleNodes, src,
                new Dictionary<int, PlanarEdge>(pg.planarEdges));

            RecursiveSeparation(g0, g0.planarNodes[srcId], orderNodes, sepCycle);
            gc++;
            Bfs.Src_all_bfs(src, pg.planarNodes, pg.planarEdges, true);
            RecursiveSeparation(g1, g1.planarNodes[srcId], orderNodes, sepCycle);


        }

        private static bool FindSeparator(PlanarNode src,
            List<PlanarNode> orderNodes, PlanarGraph pg,
            SeparatorCycle sepCycle, out PlanarEdge f)
        {
            List<PlanarNode> insideBoundaryNodes =
                orderNodes
                .Where(y => pg.planarNodes.ContainsKey(y.nid)).ToList();
            for (int i = 0; i < insideBoundaryNodes.Count - 1; i = i + 1)
                for (int j = i + 1; j < insideBoundaryNodes.Count; j = j + 1)
                {
                    PlanarNode src2 = pg.planarNodes[srcId];
                    Bfs.Src_all_bfs(src2, pg.planarNodes, pg.planarEdges, true);
                    PlanarNode u = pg.planarNodes[insideBoundaryNodes[i].nid];
                    PlanarNode v = pg.planarNodes[insideBoundaryNodes[j].nid];
                    f = new PlanarEdge(u, v);
                    f.eid = -pg.planarEdges.Count - 1;
                    f.trgl = true;
                    pg.planarEdges.Add(f.eid, f);
                    u.edgesIds.Add(f.eid);
                    v.edgesIds.Add(f.eid);
                    //   SeparatorCycle sepCycle = new SeparatorCycle();
                    sepCycle.GetBoundaryCycle(pg, src2, f);

                    int max = (int)(c * pg.planarNodes.Count);
                    if (sepCycle.inC.Count < max
                        && sepCycle.outC.Count < max)
                    {
                        Console.WriteLine(gc + "  " + sepCycle.inC.Count + " " + u.nid + " " + v.nid);
                        //  ResetGraph(pg);
                        return true;
                    }

                    else
                    {
                        u.edgesIds.Remove(f.eid);
                        v.edgesIds.Remove(f.eid);
                    }
                    ResetGraph(pg);
                    //          break;
                }
            f = null;
            if (c == 4f / 5)
            {
                c = 19f / 20;
                return FindSeparator(src, orderNodes, pg, sepCycle, out f);
            }
            else

            {
             //   c = 4f / 5;
                Console.WriteLine("not: " + gc + "  " + pg.planarNodes.Count);
                return FindSeparatorOnTriangulation(src, pg, sepCycle);
            }



        }

        private static bool FindSeparatorOnTriangulation(PlanarNode src,
            PlanarGraph pg,
            SeparatorCycle sepCycle)
        {

            pg = Triangulation.GetTriangulation(pg);
            foreach (PlanarEdge f in Triangulation.triEdges)
            {
                if (((PlanarNode)f.neighboursAdjEdges[0]).nid == srcId
                    || ((PlanarNode)f.neighboursAdjEdges[1]).nid == srcId)
                    continue;
                PlanarNode src2 = pg.planarNodes[srcId];
                Bfs.Src_all_bfs(src2, pg.planarNodes, pg.planarEdges, true);

                sepCycle.GetBoundaryCycle(pg, src2, f);

                int max = (int)(c * pg.planarNodes.Count);
                if (sepCycle.inC.Count < max
                    && sepCycle.outC.Count < max)
                {
                    Console.WriteLine(gc + "  " + sepCycle.inC.Count);
                    //  ResetGraph(pg);
                    return true;
                }

                ResetGraph(pg);
                //          break;
            }



            Console.WriteLine("not: " + gc + "  " + pg.planarNodes.Count);
            return false;
        }

        private static void ResetGraph(PlanarGraph pg)
        {
            foreach (PlanarNode n in pg.planarNodes.Values)
            {
                n.parent = null;
                n.children = new List<PlanarNode>();
                n.dist = int.MaxValue;
                n.state = 0;
                n.insideCycle = false;
            }

        }

    }
}

