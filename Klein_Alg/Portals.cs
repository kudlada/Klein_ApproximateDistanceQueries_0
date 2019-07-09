using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{


    class Portals
    {
        List<Node> separatorNodes = new List<Node>();
        List<Node> path0, path1;
        // staci index -predelat pri zmene repre z 4.casti

        static double epsilon = 15d / 128;
        double alpha = 1d / (1 + epsilon);
        Dictionary<long, int> pathDist0, pathDist1;


        internal Portals(List<Node> pth0, List<Node> pth1)
        {
            separatorNodes = path0.Concat(path1).ToList();
            path0 = pth0;
            path1 = pth1;
        }

        internal void Create_PortalsDistanciesTableForSeparator
            (BTNode<DecompositionTreeItem, LeafDistTable> btNode,
            Dictionary<long, Node> nodes,
            NodesToSeparatorPathsTable mainTable)
        {
            Create_SeparatorDistanciesTable(nodes);
            foreach (Node x in nodes.Values)
            {
                Dictionary<int, Node> portalsX =
                    ChoosePortalsForX(x, path0, pathDist0, -1);
                List<int[]> table0 = new List<int[]>();
                foreach (Node p in portalsX.Values)
                    table0.Add(new int[]
                    {(int) x.LMdist[p.id], pathDist0[p.id]});
                mainTable.table[x.id].Add(btNode, new Dictionary<int, List<int[]>>());
                mainTable.table[x.id][btNode][0] = table0;
                portalsX = ChoosePortalsForX(x, path1, pathDist1, 1);
                List<int[]> table1 = new List<int[]>();
                foreach (Node p in portalsX.Values)
                    table1.Add(new int[]
                    {(int) x.LMdist[p.id], pathDist0[p.id]});
                mainTable.table[x.id][btNode][1] = table1;

            }

        }

        private void Create_SeparatorDistanciesTable
            (Dictionary<long, Node> nodes)
        {

            foreach (Node n in separatorNodes)
                n.inSeparator = true;
            Dijkstra djk = new Dijkstra();
            Node src = path0.Last();
            djk.GetShortestPath(src.id, -1, Dijkstra.algType.simple,
                nodes, out int op, out int sc);
            pathDist0 = SetDistToRoot(src, path0);
            pathDist1 = SetDistToRoot(src, path1);
            Landmarks lms = new Landmarks();
            lms.GetDistTrees(separatorNodes, ref nodes);  //bez ref
        }

        private Dictionary<long, int> SetDistToRoot(Node src, List<Node> path)
        {
            Dictionary<long, int> pathDist = new Dictionary<long, int>();
            foreach (Node x in path)
                pathDist.Add(x.id, (int)x.distance);
            return pathDist;
        }

        private Node ChooseZ0ForX(Node x, List<Node> path, out int i0)
        {
            int min = int.MaxValue;
            Node z0 = null;
            i0 = -1;
            foreach (Node y in path)
                if (x.LMdist[y.id] < min)
                {
                    min = (int)x.LMdist[y.id];
                    z0 = y;
                    i0 = path.IndexOf(y);
                }
            return z0;
        }

        private Dictionary<int, Node> ChoosePortalsForX(Node x, List<Node> path,
            Dictionary<long, int> pathDist, int i)
        {
            Dictionary<int, Node> portalsX = new Dictionary<int, Node>();
            int i0;
            Node z0 = ChooseZ0ForX(x, path, out i0);
            portalsX.Add(0, z0);

            List<Node> subpath = new List<Node>(path);
            while (subpath.Count > 0)
            {
                subpath = subpath
                    .GetRange(i0 + 1, path.Count - 1 - (i0 + 1));
                foreach (Node z in subpath)
                {
                    double A = alpha * (x.LMdist[portalsX[i + 1].id]
                        + pathDist[portalsX[i + 1].id] - pathDist[z.id]);
                    if (x.LMdist[z.id] < A)
                    {
                        portalsX.Add(i, z);
                        i--;
                        i0 = subpath.IndexOf(z);
                        break;
                    }
                }
            }
            i = 1;
            subpath = new List<Node>(path);
            while (subpath.Count > 0)
            {
                subpath = subpath
                    .TakeWhile(n => n.id != z0.id).ToList();
                foreach (Node z in subpath)
                {
                    double A = alpha * (x.LMdist[portalsX[i - 1].id]
                        - pathDist[portalsX[i - 1].id] + pathDist[z.id]);
                    if (x.LMdist[z.id] < A)
                    {
                        portalsX.Add(i, z);
                        i++;
                        i0 = subpath.IndexOf(z);
                        break;
                    }
                }
            }
            return portalsX;
        }
    }
}
