using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class LeafDistTable  // small subset
    {
        Dictionary<long, Dictionary<long, int>> distancies 
            = new Dictionary<long, Dictionary<long, int>>();

        internal LeafDistTable(Dictionary<long, Node> nodes, 
            List<long> nodesSubset)
        {
            Dijkstra djk = new Dijkstra();
            int op, sc;
            foreach (long id in nodesSubset)
            {
                Node n=nodes[id];
                djk.GetShortestPath(n.id, -1, Dijkstra.algType.all, 
                    nodes, out op, out sc);
                foreach (Node x in nodes.Values)
                {
                    Add(n.id, x.id, (int)x.distance);
                }
            }   
        }

        private void Add(long nid0, long nid1, int dist)
        {
            long first, second;
            OrderKeys(nid0, nid1, out first, out second);
            if (!distancies.ContainsKey(first))
                distancies.Add(first, new Dictionary<long, int>());
            else if (!distancies.ContainsKey(first)) 
                distancies[first].Add(second, dist);
        }

        internal int GetDistance(long nid0, long nid1)
        {
            long first, second;
            OrderKeys(nid0, nid1, out first, out second);
            return distancies[first][second];             // try?
        }

        private void OrderKeys(long nid0, long nid1,
           out long first, out long second)
        {
            if (nid0 < nid1)
            {
                first = nid0;
                second = nid1;
            }
            else
            {
                first = nid1;
                second = nid0;
            }
        }
    }
}
