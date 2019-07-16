using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class GridGraph
    {
        static double lat0, lon0;
        static int weight=1;

       

        public static Graph Create(int n)
        {
            lat0 = 45;
            lon0 = 15;
            Graph gridGraph = new Graph();
            gridGraph.nodes = new Dictionary<long, Node>();
            Node[][] grid = new Node[n][];
            for (int i = 0; i < n; i++)
            {
                grid[i] = GetRow(n, i);
            }
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    Node u = grid[i][j];
                    Node v = grid[i + 1][j];
                    Node w = grid[i][j + 1];
                    u.Add(v,weight);
                    v.Add(u, weight);
                    u.Add(w, weight);
                    w.Add(u, weight);
                }
                Node lu = grid[i][n - 1];
                Node lv = grid[i + 1][n - 1];
                Node lw = grid[n - 1][i];
                Node lz = grid[n - 1][i + 1];
                lu.Add(lv,weight);
                lv.Add(lu,weight);
                lw.Add(lz,weight);
                lz.Add(lw,weight);
            }

            for (int i = 0; i < n; i++)

                for (int j = 0; j < n; j++)
                    gridGraph.nodes.Add(grid[i][j].id, grid[i][j]);
            return gridGraph;

        }

        static Node[] GetRow(int n, int i)
        {
            Node[] row = new Node[n];
            int start = n * i;
            double step = 3;
            double lat = lat0 + step * i;
            double lon = lon0;
            for (int k = start; k < start + n; k++)
            {
                Node u = new Node(k);
                u.coordinates.position.Latitude = lat +k; //bez +k/2
                u.coordinates.position.Longitude = lon; 
                lon = lon + step *3 ;  //bez *3
                u.coordinates.pictureCrd2 = u.coordinates.position.latitude;
                u.coordinates.pictureCrd1 = u.coordinates.position.longitude;
                row[k - start] = u;
            }
            return row;


        }

       
    }
}



