using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class AlgorithmComparison
    {


        public static void Go(Graph g)   // test
        {
            Dijkstra dij = new Dijkstra();
            Dictionary<int, int> mess = new Dictionary<int, int>();
            Init(g, mess);
            Landmarks lms = new Landmarks(g.minLat, g.maxLat, g.minLon, g.maxLon);
            List<Node> LM = lms.GenerateLatticeLM(80, g.nodes);
            dij.LM = LM;
            dij.g = g;
            lms.GetDistTrees(LM, ref g.nodes);
            List<Tuple<long, long>> tuples = GetRandomTuples(1000, g.nodes);
            // pro 10000 skalovat podle dist.
            StreamWriter wr = new StreamWriter("C:/Users/L/Desktop/tup_P.txt");
            foreach (Tuple<long, long> tup in tuples)
            {
                long sId = tup.Item1;
                Node s = g.nodes[sId];
                long tId = tup.Item2; Node t = g.nodes[tId];
                int sc = 0; int ope = 0;

                List<Node> path0 = dij.GetShortestPath(
                    sId, tId, Dijkstra.algType.simple, g.nodes, out ope, out sc);

                double d0 = t.distance;
                wr.WriteLine("{0}x{1}x{2}", path0.Count, ope, sc);

                List<Node> path2 = dij.GetShortestPath(
                    sId, tId, Dijkstra.algType.LM, g.nodes, out ope, out sc);
                d0 = (int)t.distance;
                wr.WriteLine("{0}x{1}x{2}", sId, tId, d0);
                if (path0.Count != path2.Count)
                    wr.WriteLine("Error.");
                wr.WriteLine("{0}x{1}x{2}", path2.Count, ope, sc);

                double lDist = t.distance;
                double lS = s.pot;
                double lT = t.pot;
                foreach (Node n in g.nodes.Values)
                {
                    foreach (Node.weightedEdge e in n.neighbourList)
                    {

                        e.weight = mess[e.id];
                    }
                    foreach (Node.weightedEdge e in n.backList)
                    {

                        e.weight = mess[e.id];
                    }
                }
            }
            wr.Close();
        }


        private static void Init(Graph g, Dictionary<int, int> mess)
        {
            int k = 0;
            foreach (Node n in g.nodes.Values)
                n.backList = new List<Node.weightedEdge>();
            foreach (Node n in g.nodes.Values)
            {
                List<long> nbs = new List<long>();
                List<Node.weightedEdge> nnewe = new List<Node.weightedEdge>();
                foreach (Node.weightedEdge e in n.neighbourList)
                {
                    if (nbs.Contains(e.neighbour.id))
                        e.state = 8;
                    else
                    {
                        e.id = k;
                        mess.Add(k, e.weight);
                        k++;
                        nnewe.Add(e);

                        nbs.Add(e.neighbour.id);
                    }
                }
                n.neighbourList = nnewe;

            }
            foreach (Node n in g.nodes.Values)
            {
                foreach (Node.weightedEdge e in n.neighbourList)
                {

                    Node v = e.neighbour;
                    Node.weightedEdge ed = new Node.weightedEdge(n);
                    ed.weight = e.weight;
                    ed.id = e.id;
                    v.backList.Add(ed);

                    bool newSym = true;                                 //SYM
                    foreach (Node.weightedEdge es in n.symList)
                        if (es.neighbour.id == v.id)
                            newSym = false;
                    if (newSym)
                    {
                        n.AddSym(v);

                    }

                    newSym = true;                                 //SYM
                    foreach (Node.weightedEdge es in v.symList)
                        if (es.neighbour.id == n.id)
                            newSym = false;
                    if (newSym)
                    {
                        v.AddSym(n);

                    }
                }

            }
        }



        private static List<Tuple<long, long>> GetRandomTuples(int count, Dictionary<long, Node> nodes)
        {
            int max = nodes.Count;
            Random random = new Random(0);
            // 21 11 4 30 33 38 88 100 1 3 13(500) 66(1000..) 70 72
            int r;
            Tuple<long, long> t;
            List<Tuple<long, long>> list = new List<Tuple<long, long>>();
            for (int i = 0; i < count; i++)
            {
                r = random.Next(max);
                long sId = nodes.ElementAt(r).Key;
                r = random.Next(max);
                long tId = nodes.ElementAt(r).Key;
                t = new Tuple<long, long>(sId, tId);
                list.Add(t);
            }
            return list;
        }



        // *****************************************************************************************************************************
        /*
                public Graph GetTestGraph(int N)
                {
                    Graph testGraph = new Graph();
                    double diff = 0.002;
                    double lat = 50.05;
                    double lon = 14.40;
                    Node n1, n2;
                    lat = lat + diff;
                    lon = lon + diff;
                    for (int i = 0; i < N; i++)
                    {
                        n1 = new Node(i);
                        lat = lat + diff;
                        lon = lon + diff;
                        testGraph.nodesInFrame.Add(i, n1);
                    }

                    lat = lat + diff;
                    lon = lon + diff;
                    for (int i = 0; i < N-1 ; i++)
                    {
                        testGraph.nodesInFrame.TryGetValue(i, out n1);
                        testGraph.nodesInFrame.TryGetValue(i + 1, out n2);
                        n1.AddNeighbour(n2,"yes");
                        n2.AddNeighbour(n1,"yes");
                    }
                    return testGraph;
                }

                public void WriteNodesID(Graph graph)
                {
                    StreamWriter w=new StreamWriter("C:/Users/Lada/Desktop/CONTROLNodes.txt");
                    w.WriteLine("TOTAL COUNT:   {}", graph.nodes.Count);
                    foreach (Node n in graph.nodes.Values)
                    {
                        w.WriteLine(n.id);
                    }
                    w.Close();
                }


            }

            */
    }

}