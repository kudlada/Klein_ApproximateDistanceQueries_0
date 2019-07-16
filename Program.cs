using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Program
    {
        public static string BORDER_FILE = "../../data/borderCZ_2.json";
        public static string[] MAP_FILES = new string[] {
                "../../data/3.osm",
                "../../data/2.osm",
                "../../data/1.osm",

            };

        public static string INTERSECTIONS_FILE = "../../data/intersections.txt";
        public static string OUTPUT_FILE = "../../output/graph.txt";

        public static void Main(string[] args)
        {
            Graph g = OSMGraphHandler.CreateOSMGraph(
                OUTPUT_FILE, INTERSECTIONS_FILE);
            AlgorithmComparison.Go(g);


            //    InputHandler.CreateGraphFromOSMFiles(BORDER_FILE, MAP_FILES, OUTPUT_FILE);
            //    PlanarGraph pg = OSMGraphHandler.CreatePlanarGraph(
            //         OUTPUT_FILE, INTERSECTIONS_FILE,
            //         out Graph g);
            //    BoundarySeparation.GetDecompositionTree(pg,g);

            //    Graph g = GridGraph.Create(20);
            //    PlanarGraph pg = new PlanarGraph(g);
            //    List<int> tri = new List<int>();
            //    List<long> cycle = new List<long>();
            //    PlanarGraph gr=   Triangulation.GetTriangulation(pg);
            //    SeparatorCycle c = new SeparatorCycle();
            //    Dictionary<int, List<long>> sepLevels = sep.Separate(pg,out cy);
            //    TestingGraphs.PlanarUtilsTest.TestCountOfEdges(30);
        }
    }
}
