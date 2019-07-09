using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{
    class Separation
    {

        List<Node> separators = new List<Node>();

        public void GetImageLayers(Graph graph, Dictionary<long, Node> nodes)
        {
         /*   GetLayersBFS(nodes);
            int u = 0;
            Image im = new Image("Levels");
            im.GetLMImage(graph, separators);*/
        }

        public void GetLayersBFS(Dictionary<long, Node> nodes)
        {
            if (nodes.Count < 30000)
                return;
            Dictionary<int, List<Node>> open = new Dictionary<int, List<Node>>();
            foreach (Node n in nodes.Values)         
            {
                n.state = 0;
                n.distance = int.MaxValue;
                n.pre = null;

            }
            int hk = nodes.Count / 2;
            Node ux = nodes.ElementAt(hk).Value;
            ux.state = 1;
            long id = ux.id;
            int step = 0;
            List<Node> lev = new List<Node>();
            lev.Add(ux);
            open.Add(0,lev);
            int sum = 1;
            int half = nodes.Count / 2;
            int indHalf = -1;
            bool end = false;
            bool found = false;
            while (!end)
            {
                step = step + 1;
                List<Node> lev1 = new List<Node>();
               
                foreach(Node u in lev)
                {
                    foreach (Node.weightedEdge e in u.symList)
                    {
                        if (e.neighbour.state==0)
                        {
                            e.neighbour.state = 1;
                            lev1.Add(e.neighbour);

                        }
                    }
                }
                if (lev1.Count == 0)
                    end = true;
                open.Add(step, lev1);
                sum = sum + lev1.Count;
                if (sum > half)
                {
                    if (indHalf < 0)
                    {
                        indHalf = step;
                        if (true) 
                      
                        {

                            found = true;
                            foreach (Node x in lev1)
                            {
                                
                                if ((lev1.Count < 4 * Math.Sqrt(nodes.Count)))
                                    x.distance = 1;
                                else
                                    x.distance = 0;
                                separators.Add(x);

                            }
                            Dictionary<long, Node> next = new Dictionary<long, Node>();
                            for (int w=0;w<step;w++)
                            {
                                foreach (Node q in open[w])
                                {
                                    next.Add(q.id, q);
                                }
                            }
                            
                            Dictionary<long, Node> next2 = new Dictionary<long, Node>();
                            foreach (Node n in nodes.Values)
                                if (n.state == 0)
                                    next2.Add(n.id, n);
                            GetLayersBFS(next);
                            GetLayersBFS(next2);
                        }
                        end = true;
                    }
                }
                lev = lev1;

            }
        }

    }
}
