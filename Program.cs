using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Program
    {
        public static string BORDER_FILE = "../../data/borderCZ.json";
        public static string[] MAP_FILES = new string[] {
                "../../data/3.osm",
                "../../data/2.osm",
                "../../data/1.osm",
              
            };

        public static string INTERSECTIONS_FILE = "../../data/intersections.txt";
        public static string OUTPUT_FILE = "../../output/graph.txt";

        public static void Main(string[] args)
        {
   //        InputHandler.CreateGraphFromOSMFiles(BORDER_FILE, MAP_FILES, OUTPUT_FILE);
             PlanarGraph pg = OSMGraphHandler.CreatePlanarGraph
                 (OUTPUT_FILE, INTERSECTIONS_FILE);

                                    //     Graph g = GridGraph.Create(10);
                                    //     PlanarGraph pg = new PlanarGraph(g);
            
             // PlanarSeparator sep = new PlanarSeparator();
             // Dictionary<int, List<long>> sepLevels = sep.Separate(pg);
             // Image im = new Image("Separator");
             // im.GetPlanarGraphBNF( pg.planarNodes.Values.ToList(), pg.planarEdges);
        }
    }
}
