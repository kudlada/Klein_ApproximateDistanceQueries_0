using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Klein_ApproximateDistanceQueries_0
{
    class OSMGraphHandler
    {
        public static PlanarGraph CreatePlanarGraph(
            string OSMGraphFile, string intersectionsFile,
            out Graph g)
        {
            g = CreateOSMGraph(OSMGraphFile, intersectionsFile);
            return new PlanarGraph(g);
        }

        public static Graph CreateOSMGraph(
            string OSMGraphFile, string intersectionsFile)
        {
            Graph g = new Graph();
            g.nodes = new Dictionary<long, Node>();
            try
            {
                StreamReader r = new StreamReader(OSMGraphFile);
                Dictionary<long, Dictionary<long, int>> tmpEdges 
                    = ProcessOSMGraphFile(r,g);
               
                Console.WriteLine("Before IS: " + tmpEdges.Keys.Count);
                HandleIntersections(g, tmpEdges, intersectionsFile);
                Console.WriteLine("After IS: " + tmpEdges.Keys.Count);

                AddEdges(g,tmpEdges);
             
            }
            catch
            {
                throw new InvalidDataException();
            }
            
            /*
            maxLat = 51.0404296;
            minLat = 48.5874212;
            maxLon = 18.8446152;
            minLot = 12.1295496
            */
            
            g.SetBounds(48.5874212, 51.0404296, 12.1295496, 18.8446152);
            return g;
        }

        private static Dictionary<long, Dictionary<long, int>> 
            ProcessOSMGraphFile(StreamReader r, Graph g)
        {
            Dictionary<long, Dictionary<long, int>> tmpEdges
                = new Dictionary<long, Dictionary<long, int>>();
            Node n=null;
          
            string line = r.ReadLine();
            string[] words = line.Split(' ');
            bool inside = bool.Parse(words[3]);
            while (!r.EndOfStream)
            {
                if (true)
              //  if (inside)
                {
                    n = new Node(long.Parse(words[0]));
                    n.SetData(double.Parse(words[1]), double.Parse(words[2]));
                    inside = bool.Parse(words[3]);
                    
                    n.inside = inside;
                    g.nodes.Add(n.id, n);
                    tmpEdges[n.id] = new Dictionary<long, int>();
                }
                line = r.ReadLine();
                words = line.Split(' ');
                while (!r.EndOfStream && words.Count() == 2)
                {
                    tmpEdges[n.id][long.Parse(words[0])] 
                            = int.Parse(words[1]);
                    line = r.ReadLine();
                    words = line.Split(' ');
                }
            }
            return tmpEdges;
        }

        private static void AddEdges(
            Graph g, Dictionary<long, Dictionary<long, int>> tmpEdges)
        {
            foreach (KeyValuePair<long, Dictionary<long, int>> pair in tmpEdges)
            {
                Node n = g.nodes[pair.Key];
                foreach (KeyValuePair<long, int> neigh in pair.Value)
                {
                    Node.weightedEdge e 
                        = new Node.weightedEdge(g.nodes[neigh.Key]);
                    e.weight = neigh.Value;
                    n.neighbourList.Add(e);
                }
            }
        }

        private static void HandleIntersections(Graph g, 
            Dictionary<long, Dictionary<long, int>> tmpEdges, 
            string intersectionsFile)
        {
            int count = 0;
            try
            {
                StreamReader r = new StreamReader(intersectionsFile);
                r.ReadLine();
                string line0,line1;
                
                while (!r.EndOfStream)
                {
                    count++;
                    line0 = r.ReadLine();
                    line1 = r.ReadLine();
                    SplitEdges(g, line0, line1, -count, tmpEdges);

                }
            }
            catch
            {
                throw new InvalidDataException();
            }
        }


        private static void SplitEdges(Graph g,
            string line0, string line1, long newNid,
            Dictionary<long, Dictionary<long, int>> tmpEdges)
        {
            Node newNode = new Node(newNid);
            g.nodes.Add(newNid, newNode);
            tmpEdges.Add(newNid, new Dictionary<long, int>());
            string[] words = line0.Split('*');
            long e0 = long.Parse(words[0]);
            long e1 = long.Parse(words[1]);
            long f0 = long.Parse(words[2]);
            long f1 = long.Parse(words[3]);
            words = line1.Split('*');
            double lon = double.Parse(words[0]);
            double lat = double.Parse(words[1]);
            newNode.SetData(lat, lon);
            SplitEdge(g, e0, e1, newNid, tmpEdges);
            SplitEdge(g, f0, f1, newNid, tmpEdges);
            
        }

        private static void SplitEdge(Graph g, 
            long nid0,long nid1, long newNid,
            Dictionary<long, Dictionary<long, int>> tmpEdges)
        {
            if (!(tmpEdges.ContainsKey(nid0) && tmpEdges.ContainsKey(nid1)))
                return;
            Node n0 = g.nodes[nid0];
            Node n1 = g.nodes[nid1];
            Node newNode = g.nodes[newNid];

        
            double weightD0 = n0.GetDistanceUTM(newNode) * 10000;
            int weightI0 = Math.Max(1, (int)weightD0);
            double weightD1 = newNode.GetDistanceUTM(n1) * 10000;
            int weightI1 = Math.Max(1, (int)weightD1);
            double weight = n0.GetDistanceUTM(n1) * 10000;
            int weightI = Math.Max(1, (int)weight);
            int diff =weightI - (weightI0 + weightI1);
           
            if (diff > 0)
                weightI0 = weightI0 + diff;
            else if (diff < 0)
            {
                if (weightI0 > -diff)
                    weightI0 = weightI0 + diff;
                else if (weightI1 > -diff)
                    weightI1 = weightI1 + diff;

            }
            diff = weightI - (weightI0 + weightI1);
            
            if (tmpEdges[nid0].ContainsKey(nid1))
            {
                    tmpEdges[nid0].Remove(nid1);
                    tmpEdges[nid0].Add(newNid,weightI0);
                    tmpEdges[newNid].Add(nid0, weightI0);
            }
            if (tmpEdges[nid1].ContainsKey(nid0))
            {
                tmpEdges[nid1].Remove(nid0);
                tmpEdges[nid1].Add(newNid, weightI1);
                tmpEdges[newNid].Add(nid1, weightI1);
            }
        }
    }
}
