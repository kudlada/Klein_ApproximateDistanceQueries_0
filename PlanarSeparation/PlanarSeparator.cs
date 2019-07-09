using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{
    class PlanarSeparator
    {
        Dictionary<long, PlanarNode> planarNodes;
        Dictionary<int, PlanarEdge> planarEdges;
        double k = 0;  // L(0..lev1)
        public int lev0, lev1, lev2;
        PlanarNode src, newSrc;
        public PlanarGraph shrGraph;  //shrinked
        Dictionary<long, PlanarNode> shrNodes = new Dictionary<long, PlanarNode>();

        public Dictionary<int, List<long>> Separate(PlanarGraph pg, out List<long> cycle)
        {
            return Separate(pg.planarNodes, pg.planarEdges, out cycle);
        }


        public Dictionary<int, List<long>> Separate(
            Dictionary<long, PlanarNode> plNodes,
            Dictionary<int, PlanarEdge> plEdges, out List<long> cycle)
        {
            planarNodes = plNodes;
            planarEdges = plEdges;
            src = planarNodes.First().Value;
            Bfs.Src_all_bfs(src, planarEdges);
            List<int> tri;
            // step 3, 4, 5
            Dictionary<int, List<long>> levels = CreateMainLevels(src.nid);
            PlanarGraph trg = Triangulation.GetTriangulation(
                new PlanarGraph(plNodes, plEdges), out tri);
            foreach (int eid in planarEdges.Keys.Reverse<int>())
            {
                break;
                PlanarEdge e = trg.planarEdges[eid];
                if (e.inTree)
                    continue;
                PlanarNode u = (PlanarNode)e.neighboursAdjEdges[0];
                PlanarNode v = (PlanarNode)e.neighboursAdjEdges[1];
                List<long> pathU = new List<long>();
                List<long> pathV = new List<long>();
                PlanarNode x = u;

                while (x.parent != null)
                {
                    x.state = -3;
                    pathU.Add(x.nid);
                    x = x.parent;
                }
                pathV = new List<long>();
                x = v;

                while (x.parent != null && x.parent.state != -3)
                {
                    pathV.Add(x.nid);
                    x = x.parent;
                }
                pathV.Add(x.nid);
            }

            //step 6

            planarNodes = trg.planarNodes;
            planarEdges = trg.planarEdges;

            GoAroundBnfTree(levels);

            SeparatorUtils.UpdateNextEdges(newSrc, plEdges);
            SeparatorUtils.UpdateNextEdges(src, planarEdges);
            shrNodes.Add(newSrc.nid, newSrc);
            //shrGraph = new PlanarGraph(shrNodes, planarEdges);

            SeparatorUtils.RemoveEdges(shrGraph, newSrc, lev0, lev2);
            SeparatorUtils.SetAllSumsOfDescNodesIncludeItself(shrGraph, 1);

            SeparatorCycle sepCycle = new SeparatorCycle();
            sepCycle.GetFirstCycle(trg);



            src = newSrc;
            cycle = sepCycle.cycle;
            return levels;

        }






        void CloseEdge(PlanarNode n, int dist, List<PlanarEdge> opened)
        {
            PlanarEdge e = opened[0];
            opened.RemoveAt(0);
            e.state = 2;

            PlanarNode neigh0, neigh1, neigh;
            PlanarEdge e2 = e.GetNextEdge(e.parent, out neigh0);

            if (e2.state == 0)
            {
                neigh = e2.GetNeigh(e.parent);

                e2.state = 1;
                e2.dist = e.parent.dist + e2.w;
                if (neigh.dist == 0)
                {
                    neigh.dist = e2.dist;
                    e2.parent = e.parent;
                    neigh.parent = e2.parent;
                    opened.Add(e2);
                    CloseEdge(null, 0, opened);
                }

            }
            PlanarEdge f2 = e.GetNextEdge(neigh0, out neigh1);

            if (f2.state == 0)
            {

                neigh1 = f2.GetNeigh(neigh0);

                f2.state = 1;
                f2.dist = e.dist + f2.w;
                if (neigh1.dist == 0)
                {
                    neigh1.dist = f2.dist;
                    f2.parent = neigh0;
                    neigh1.parent = f2.parent;
                    opened.Add(f2);
                }


            }

        }

        Dictionary<int, List<long>> CreateMainLevels(long srcNid)
        {
            Dictionary<int, int> levelsSizes;
            Dictionary<int, List<long>> levels = GetLevels(out levelsSizes);        // kdyz nemam velkou komp...
            List<long> unreach = levels[0];   // +src
            levels[0] = new List<long>();
            levels[0].Add(srcNid);

            lev1 = GetFirstLevelWithGrTotalSum(planarNodes.Count / 2, levelsSizes);
            SetLevel0(levelsSizes);
            SetLevel2(levelsSizes);
            /*     Console.WriteLine(lev0+" "+ lev1 + " " + lev2 + " " +k);
                 foreach (KeyValuePair<int, List<long>> pa in levels)
                 {
                     Console.WriteLine(pa.Key);
                     foreach (long lg in pa.Value)
                         Console.Write(lg +", ");
                     Console.WriteLine();
                 }*/
            return levels;
        }

        Dictionary<int, List<long>> GetLevels(out Dictionary<int, int> levelsSizes)
        {
            Dictionary<int, List<long>> levels = new Dictionary<int, List<long>>();
            foreach (PlanarNode n in planarNodes.Values)
            {
                int dist = n.dist;
                if (!levels.ContainsKey(dist))
                    levels.Add(dist, new List<long>());
                levels[dist].Add(n.nid);

            }
            levelsSizes = new Dictionary<int, int>();
            foreach (KeyValuePair<int, List<long>> pair in levels)
                levelsSizes.Add(pair.Key, pair.Value.Count);
            return levels;
        }

        public int GetFirstLevelWithGrTotalSum(int max, Dictionary<int, int> levelsSizes) // <= 
        {
            int totalSum = 0;

            foreach (int key in levelsSizes.Keys)
            {
                totalSum = totalSum + levelsSizes[key];
                if (totalSum > max)
                {
                    k = totalSum;
                    return key;
                }
            }
            return -1;
        }

        public void SetLevel0(Dictionary<int, int> levelsSizes)
        {
            int totalSum = 0;
            int levDiff;
            double max = (2 * Math.Sqrt(k));           //          lev0 <= lev1
            var reversedKeysFromLev1 = levelsSizes.Keys.OrderBy(key => key).Reverse();
            reversedKeysFromLev1 = reversedKeysFromLev1.SkipWhile(key => key > lev1);
            foreach (int key in reversedKeysFromLev1)
            {

                levDiff = 2 * (lev1 - key);
                totalSum = levelsSizes[key] + levDiff;   // nejvyssi lev0: L(lev0) + 2*(lev1-lev0) <= 2* odm.k
                if (totalSum <= max)
                {
                    lev0 = key;
                    break;
                }
            }

        }

        public void SetLevel2(Dictionary<int, int> levelsSizes)
        {
            int totalSum = 0;
            int levDiff;
            double max = 2 * Math.Sqrt(planarNodes.Count - k);           //          lev2 >= lev1 + 1
            var keysFromLev1 = levelsSizes.Keys.OrderBy(x => x).SkipWhile(key => key < lev1 + 1);
            foreach (int key in keysFromLev1)
            {

                levDiff = 2 * (key - lev1 - 1);
                totalSum = levelsSizes[key] + levDiff;   // nejnizsi lev2: L(lev2) + 2*(lev2-lev1 -1) <= 2* odm.(n-k)
                if (totalSum <= max)
                {
                    lev2 = key;
                    break;
                }
            }

        }

        void GoAroundBnfTree(Dictionary<int, List<long>> levels)
        {
            List<PlanarEdge> nottree = new List<PlanarEdge>();
            foreach (PlanarEdge e in planarEdges.Values)
                e.state = 0;


            Dictionary<long, bool> table = new Dictionary<long, bool>();
            bool b = false;
            foreach (KeyValuePair<int, List<long>> pair in levels)
            {
                if ((pair.Key >= lev2))
                {
                    foreach (long nid in pair.Value)
                        table.Add(nid, false);
                    continue;
                }

                if (pair.Key <= lev0)
                    b = true;
                else
                    b = false;
                foreach (long nid in pair.Value)
                {
                    table.Add(nid, b);
                    if (pair.Key > lev0)
                        shrNodes.Add(nid, planarNodes[nid]);
                }

            }

            newSrc = new PlanarNode(0);
            newSrc.edgesIds = new List<int>();
            GoAroundNode(src, planarEdges[src.edgesIds.First()], table);
        }

        void GoAroundNode(PlanarNode n0, PlanarEdge e0, Dictionary<long, bool> table)
        {

            e0.state++;
            if (e0.state > 2)
                return;

            PlanarNode m;
            PlanarNode n1 = e0.GetNeigh(n0);
            PlanarEdge e1 = planarEdges[e0.GetNextEdgeId(n1)];
            if (e1.parent == null)
            {
                Check(e1, n1, table);
                m = e1.GetNeigh(n1);
                GoAroundNode(m, e1, table);
            }
            else
            {
                if (n1.dist > lev0)
                {
                    Check(e1, n1, table);  // never -pro kontrolu
                }

                else
                {
                    PlanarNode n2 = e1.GetNeigh(n1);
                    while (n2 != null && n2.dist > lev0)
                    {

                        PlanarEdge f = planarEdges[e1.GetNextEdgeId(n1)];
                        Check(e1, n1, table);
                        n2 = f.GetNeigh(n1);
                        e1 = f;
                    }
                }
                GoAroundNode(n1, e1, table);
            }
        }




        bool Check(PlanarEdge e, PlanarNode n, Dictionary<long, bool> table)
        {

            PlanarNode neigh = e.GetNeigh(n);


            if (table[neigh.nid])
                return false;
            table[neigh.nid] = true;
            neigh.parent = newSrc;
            e.UpdateNeighbour(n, newSrc);    //dodelat src vs newsrc
            e.inTree = true;                 //bude v shr


            newSrc.edgesIds.Add(e.eid);

            return true;

        }


    }
}
