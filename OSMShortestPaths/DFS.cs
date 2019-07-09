using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class DFS
    {
   
        public void GetComponents(Graph g, StreamWriter w)
        {
            List<Node> stack = new List<Node>();
            List<Node> components = new List<Node>();
            /*  foreach (Node n in g.nodes.Values)
                  if (n.state == 0)
                      directedDFS(ref n);*/
            foreach (Node n in g.nodes.Values)
                n.state = 0;
            foreach (Node n in stack)
                if (n.state == 0)
                    backwardDFS(n, ref g, components, w);
        }

        public void backwardDFS(Node n, ref Graph g,
            List<Node> components, StreamWriter w)
        {
            List<Node> open = new List<Node>();
            n.state = 1;
            open.Add(n);
            components.Add(n);
            int size = 1;

            while (open.Count > 0)
            {

                Node node = open[0];
                open.RemoveAt(0);
                foreach (Node.weightedEdge e in node.backList)
                {
                    if (e.neighbour.state == 0)
                    {
                        e.neighbour.state = 1;
                        Node n2 = e.neighbour;
                        open.Insert(0, n2);
                        size++;
                    }


                }

                node.state = 2;

            }
            w.WriteLine("{0}:    {1}:   size:{2}", components.Count, n.id, size);

        }

        public void directedBFS(ref Node n, Dictionary<long, Node> nodes)
        {
            SortedList<double, Node> open = new SortedList<double, Node>();
            foreach (Node u in nodes.Values)
            {
                u.state = 0;
                u.distance = 1000000;
                u.pre = null;

            }
            long id = n.id;
            n.state = 1;
            n.distance = 0;
            AddIntoSortedList(ref open, n);
            double dist = 0;
            n.LMdist.Add(id, dist);

            while (open.Count > 0)
            {
                Node node = open.ElementAt(0).Value;
                dist = node.distance;
                open.Remove(dist);

                foreach (Node.weightedEdge e in node.backList)
                {
                    if (e.neighbour.state == 0)
                    {
                        e.neighbour.state = 1;
                        e.neighbour.distance = dist + e.weight;
                        AddIntoSortedList(ref open, e.neighbour);
                    }
                    else if (e.neighbour.state >= 1 && e.neighbour.distance > dist + e.weight)
                    {
                        open.Remove(e.neighbour.distance);
                        e.neighbour.distance = dist + e.weight;
                        
                        AddIntoSortedList(ref open, e.neighbour);

                    }

                }
                node.LMdist[id] = node.distance;
                node.state = 2;

            }


          

        }


        void AddIntoSortedList(ref SortedList<double, Node> openNodes, Node n)  
            //zkontrolovat na presnost  | pouzit heap jako v Dijkstra
        {
            double epsilon = 0.00000000001;
            double dist = n.distance;
            Node outN;
            while (openNodes.TryGetValue(dist, out outN))
            {
                dist = dist + epsilon;
            }
            n.distance = dist;
            openNodes.Add(dist, n);
        }
    }
}
