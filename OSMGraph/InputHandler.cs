using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Klein_ApproximateDistanceQueries_0
{
    class InputHandler
    {
        

        public static void CreateGraphFromOSMFiles(string borderFile, string[] mapFiles,
            string outputFile)
        {
            
            List<long> seps = new List<long>();   //pro test. sep.
            Graph g = GetGraph(borderFile,mapFiles);
            Preprocessing.MergeEdges(g,seps);
            WriteGraphToFile(g, outputFile); 
        }

        private static Graph GetGraph(string borderFile, string[] mapFiles)
        {
            Reader reader = new Reader();
            foreach (string fileName in mapFiles)
                reader.ReadFile(fileName,true);
            reader.ReadFile(borderFile, false);
            reader.graph.SetWeights();                  //??
            return reader.graph;
        }

        private static void WriteGraphToFile(Graph g, string outputFile)
        {
            StreamWriter w = new StreamWriter(outputFile);
            foreach (Node n in g.nodes.Values)
            {
                w.WriteLine("{0} {1} {2} {3}",
                    n.id, n.coordinates.position.latitude, n.coordinates.position.longitude, n.inside);
                foreach (Node.weightedEdge e in n.neighbourList)
                    w.WriteLine("{0} {1}", e.neighbour.id, e.weight);
            }
            w.Close();
        }
    }
}
