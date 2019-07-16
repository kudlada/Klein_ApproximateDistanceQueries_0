using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Decomposition  //jen test
    {
        static long[] separationLeaves = new long[] {
            3682132103, 75053080, 34073500, 479245107 };

        public static void GetDecomposition(PlanarGraph g, PlanarNode src)
        {
            PlanarSeparator sep = new PlanarSeparator();
            List<long> cycle;
       //     Bfs.Src_all_bfs(g.planarNodes.First().Value, g.planarEdges, true);
            SeparatorCycle c = new SeparatorCycle();
            c.GetCycle_U_V(g, g.planarNodes[3682132103], g.planarNodes[34073500]);
            cycle = c.cycle;

            Dictionary<long, PlanarNode> nodes
                = new Dictionary<long, PlanarNode>(g.planarNodes);

            List<long> nullKeys = new List<long>();
            foreach (KeyValuePair<long, PlanarNode> pair in nodes)
            {
                PlanarNode n = pair.Value;

                if (n == null || n.edgesIds.Count == 0)
                {
                    nullKeys.Add(pair.Key);

                    continue;
                }
                n.state = 0;
                n.dist = 0;
            }
            foreach (long nid in nullKeys)
                nodes.Remove(nid);
            foreach (long nid in cycle)
                nodes.Remove(nid);
            Dictionary<long, List<long>> components = new Dictionary<long, List<long>>();
            foreach (PlanarNode x in nodes.Values)
            {
                if (x.state > 0)
                    continue;
             //   Bfs.Src_all_bfs(x, g.planarEdges, false);
                List<long> comp = Bfs.part0;
                components.Add(x.nid, comp);

            }
            int max = 0;
            long maxKey = 0;

            foreach (KeyValuePair<long, List<long>> pair in components)
                if (nodes.ContainsKey(pair.Key) && pair.Value.Count > max)
                {
                    max = pair.Value.Count;
                    maxKey = pair.Key;
                }
            PlanarNode s0 = nodes.Last().Value;


        }

    }


}
