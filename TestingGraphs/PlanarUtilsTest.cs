using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0.TestingGraphs
{
    class PlanarUtilsTest
    {
        public static void GetImageOfTriangulation(int N)
        {
            Graph g;
            PlanarGraph pg = CreateGridAndTriangulation(N, out g);
            Image im = new Image("Triangulation_"+N);
            im.GetPlanarGraphBNF(g, pg.planarNodes.Values.ToList(), pg.planarEdges);
        }

        public static void TestCountOfEdges(int maxN)
        {
            int right = 0;
            for (int i=3;i<=maxN;i++)
            {
                PlanarGraph pg = CreateGridAndTriangulation(i);
                if (pg.planarEdges.Count != 3 * (pg.planarNodes.Count - 2))
                    Console.WriteLine("Error for N=" + i + ". |E|=" + pg.planarEdges.Count);
                else
                    right++;
            }
            Console.WriteLine("Correct "+right+" out of " +(maxN-2));
            Console.ReadKey();
        }

        private static PlanarGraph CreateGridAndTriangulation(int N, out Graph g)
        {
            g = GridGraph.Create(N);
            PlanarGraph pg = new PlanarGraph(g);
            return Triangulation.GetTriangulation(pg);
        }

        private static PlanarGraph CreateGridAndTriangulation(int N)
        {
            Graph g;
            return CreateGridAndTriangulation(N,out g);
        }
    }
}
