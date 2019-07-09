using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Preprocessing
    {


        public static void MergeEdges(Graph g, List<long> sep)
        {
            int weigth = 0;
            Node last, next;
            Dictionary<long, Node> nodesM = new Dictionary<long, Node>();
            foreach (Node n in g.nodes.Values)
                n.neighbourList = n.symList;
            foreach (Node n in g.nodes.Values)
                if (!n.inside)
                {
                    Node outN;
                    if (!nodesM.TryGetValue(n.id, out outN))
                        nodesM.Add(n.id, n);
                }
                else if (HasHighDegree(n, sep))
                {

                    List<Node.weightedEdge> path = new List<Node.weightedEdge>();

                    foreach (Node.weightedEdge e in n.neighbourList)
                    {
                        last = n;
                        path = new List<Node.weightedEdge>();
                        bool end = false;
                        next = e.neighbour;

                        weigth = e.weight;
                        e.state = 1;
                        path.Add(e);
                        while ((!end))
                        {

                            if ((!HasHighDegree(next, sep)))
                            {

                                if (next.neighbourList.Count == 0)
                                {
                                    end = true;
                                    break;
                                }
                                Node.weightedEdge e2 = next.neighbourList[0];
                                if ((e2.state == 0) && (e2.neighbour.id != last.id))
                                {

                                    last = next;
                                    next = e2.neighbour;
                                }
                                else if (next.neighbourList.Count > 1) //never
                                {
                                    e2 = next.neighbourList[1];
                                    last = next;
                                    next = e2.neighbour;
                                }
                                else //pro kontrolu
                                {
                                    end = true;
                                    break;
                                }
                                path.Add(e2);
                                e2.state = 1;
                                weigth = weigth + e2.weight;
                            }
                            else
                            {
                                end = true;

                            }

                        }
                        foreach (Node.weightedEdge e3 in path)
                            e3.state = 0;
                        Node.weightedEdge edge = new Node.weightedEdge(next);
                        edge.SetWeight(weigth);
                        n.mList.Add(edge);



                    }
                    nodesM.Add(n.id, n);


                }
            double dN = 0;

            foreach (Node n in nodesM.Values)
            {


                List<Node.weightedEdge> backList2 = new List<Node.weightedEdge>();

                n.backList = backList2;

                n.symList = new List<Node.weightedEdge>();
                n.neighbourList = n.mList;
                n.mList = new List<Node.weightedEdge>();
                foreach (Node.weightedEdge e in n.neighbourList)
                {
                    Node.weightedEdge f = new Node.weightedEdge(n);
                    f.weight = e.weight;
                    e.neighbour.backList.Add(f);
                }
            }
            g.nodes = nodesM;
        }

        private static bool HasHighDegree(Node n, List<long> sep)
        {
            if (sep.Contains(n.id))
                return true;
            if (n.backList.Count == 1 && n.neighbourList.Count == 1)
            {
                if (n.backList[0].neighbour.id != n.neighbourList[0].neighbour.id)
                    return false;
                else
                    return true;
            }
            else if (n.backList.Count == 2 && n.neighbourList.Count == 2)
            {
                int count = 0;
                if (n.backList[0].neighbour.id == n.neighbourList[0].neighbour.id)
                    count++;
                if (n.backList[0].neighbour.id == n.neighbourList[1].neighbour.id)
                    count++;
                if (n.backList[1].neighbour.id == n.neighbourList[0].neighbour.id)
                    count++;
                if (n.backList[1].neighbour.id == n.neighbourList[1].neighbour.id)
                    count++;
                if (count > 1)
                    return false;
            }
            return true;
        }

        private static void AddCycle(List<Node.weightedEdge> cycle, ref Dictionary<long, Node> mergedNodes, Node s)
        {
            Node n1, n2;
            n1 = s;


            foreach (Node.weightedEdge e in cycle)
            {

                n2 = e.neighbour;

                n1.mList.Add(e);
                n1 = n2;

            }
        }
    }
}
