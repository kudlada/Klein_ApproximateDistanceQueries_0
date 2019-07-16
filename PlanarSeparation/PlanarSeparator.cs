using System;
using System.Collections.Generic;
using System.IO;
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
        PlanarNode src;//, newSrc;
     //   public PlanarGraph shrGraph;  //shrinked
     //   Dictionary<long, PlanarNode> shrNodes = new Dictionary<long, PlanarNode>();

      /*  public Dictionary<int, List<long>> Separate(PlanarGraph pg, out List<long> cycle)
        {
            return Separate(pg.planarNodes, pg.planarEdges, out cycle);
        }
*/
 



        public Dictionary<int, List<long>> Separate(
         ref   PlanarGraph pg, out List<long> cycle)
        {
            planarNodes = pg.planarNodes;
            planarEdges = pg.planarEdges;
            src = planarNodes.First().Value;
            Bfs.Src_all_bfs(src,pg.planarNodes, planarEdges);
            
            // step 3, 4, 5
            Dictionary<int, List<long>> levels = CreateMainLevels(src.nid);
            //step 6
        //    PlanarGraph shrinkedGraph = pg;// CreateShrinkedGraph(levels, pg,src.nid);
     //       foreach (long nid in levels[int.MaxValue])
     //           shrinkedGraph.planarNodes.Remove(nid);
            //step 7
       //     Bfs.Src_all_bfs(src, shrinkedGraph.planarEdges);
            SeparatorUtils.SetAllSumsOfDescNodesIncludeItself(pg,src, 1);
            if (Triangulation.triEdges==null||Triangulation.triEdges.Count==0)
                pg = Triangulation.GetTriangulation(pg);
     //       PlanarEdge xx = shrinkedGraph.planarEdges[1630674];
            //step 8
       //     SeparatorCycle sepCycle = new SeparatorCycle();
      //      StreamWriter w = new StreamWriter("C:\\Users\\L\\Desktop\\triCycles2.txt");
            try
            {
      //          sepCycle.GetFirstCycle(shrinkedGraph, src, lev0);
            }
            catch { };
       //     foreach (string s in sepCycle.outputs)
       //         w.WriteLine(s);
       //     w.Close();
            cycle = null;
            return new Dictionary<int, List<long>>();

            /*

            PlanarGraph trg = Triangulation.GetTriangulation(
                new PlanarGraph(plNodes, plEdges));
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

            /////////////////////////

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
            */
        }


        public DecompositionTree<DecompositionTreeItem, LeafDistTable> 
            GetSeparationTree(PlanarGraph pg, PlanarNode src) //1610738   78794
        {
            pg = Triangulation.GetTriangulation(pg);
            PlanarEdge ww = pg.planarEdges[1630674];
            Dictionary<PlanarEdge, int> cyclesTriEdges
                = new Dictionary<PlanarEdge, int>();
            StreamReader r = new StreamReader("C:\\Users\\L\\Desktop\\triCycles.txt");
            while (!r.EndOfStream)
            {
                string line = r.ReadLine();
                string[] spl = line.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);
                cyclesTriEdges.Add(
                    pg.planarEdges[int.Parse(spl[0])], int.Parse(spl[0]));
            }
            int max = cyclesTriEdges.Values.Max();
            KeyValuePair<PlanarEdge,int> maxTriE 
                = cyclesTriEdges.Where(x => x.Value == max).First();
            cyclesTriEdges.Remove(maxTriE.Key);
           
            SeparatorCycle cycle = new SeparatorCycle(pg);
            List<PlanarNode> inC;
            List<PlanarNode> outC;
            List<PlanarNode> sep = cycle.SeparateByEdge(
             pg.planarEdges[1630674], pg.planarNodes.First().Value,out inC,out outC);
            return null;
      //      DecompositionTreeItem rootIte
        }

        //step 6
        private PlanarGraph CreateShrinkedGraph(Dictionary<int, List<long>> levels,
            PlanarGraph pg, long srcNid)
        {
            Dictionary<long, PlanarNode> originalNodes
                = new Dictionary<long, PlanarNode>(pg.planarNodes);
            Dictionary<long, PlanarNode> shrNodes = new Dictionary<long, PlanarNode>();
            foreach (int key in levels.Keys)
            {
                if (key >= lev0)// && key <= lev2)
                    foreach (long nid in levels[key])
                    {
                        PlanarNode n = pg.planarNodes[nid];
                        shrNodes.Add(n.nid, n);
                    }
                        
            }
            PlanarNode src2 =originalNodes[srcNid];
            shrNodes.Add(src2.nid, src2);
            foreach (PlanarNode n in shrNodes.Values)
            {
                break;
                List<int> newEdgesIds = new List<int>();
                foreach (int eid in n.edgesIds)
                {
                    PlanarEdge e = pg.planarEdges[eid];
                    PlanarNode neigh = e.GetNeigh(n);
                    if (shrNodes.ContainsKey(neigh.nid))
                        newEdgesIds.Add(eid);
                        
                }
                n.edgesIds = newEdgesIds;
            }
            Dictionary<int, PlanarEdge> shrEdges = new Dictionary<int, PlanarEdge>();
            foreach (PlanarEdge e in pg.planarEdges.Values)
            {
                break;
                if (shrNodes.ContainsKey(
                    ((PlanarNode)e.neighboursAdjEdges[0]).nid)
                    &&
                    shrNodes.ContainsKey(
                    ((PlanarNode)e.neighboursAdjEdges[1]).nid))
                    shrEdges.Add(e.eid, e);
            }
            GoAroundBnfTree(pg.planarNodes,levels, src2);
            return new PlanarGraph(shrNodes, shrEdges);
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
            List<long> unreach =new List<long>( levels[0]);   // +src
            levels[0] = new List<long>();
            levels[0].Add(srcNid);
        //    levels[int.MaxValue] = unreach;
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
                int dist = n.dist;// (n.dist/100)*100;  // n.dist;
                if (!levels.ContainsKey(dist))
                    levels.Add(dist, new List<long>());
                levels[dist].Add(n.nid);

            }
    //        List<long> unreach = levels[0];   // +src
    //        levels[0] = new List<long>();
    //        levels[int.MaxValue] = unreach;
            levelsSizes = new Dictionary<int, int>();
            foreach (KeyValuePair<int, List<long>> pair in levels)
                levelsSizes.Add(pair.Key, pair.Value.Count);
            return levels;
        }

        public int GetFirstLevelWithGrTotalSum(int max, Dictionary<int, int> levelsSizes) // <= 
        {
            int totalSum = 0;

            foreach (int key in levelsSizes.Keys.OrderBy(x=>x))
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

        void GoAroundBnfTree(Dictionary<long,PlanarNode> nodes,
            Dictionary<int, List<long>> levels, 
            PlanarNode src2)
        {
            List<PlanarEdge> nottree = new List<PlanarEdge>();
            foreach (PlanarEdge e in planarEdges.Values)
                e.state = 0;


            Dictionary<long, bool> table = new Dictionary<long, bool>();
            bool b = false;
            foreach (PlanarNode n in nodes.Values)
            // foreach (KeyValuePair<int, List<long>> pair in levels)
            // {
            /*    if ((pair.Key >= lev2))
                {
                    foreach (long nid in pair.Value)
                        table.Add(nid, false);
                    continue;
                }
*/
            //  if (pair.Key == int.MaxValue)
            //      continue;
            { 
                if (n.dist < lev0)
                    b = true;
                else
                    b = false;
            
                    table.Add(n.nid, b);
                    
            }

        //    newSrc = new PlanarNode(0);
        //    newSrc.edgesIds = new List<int>();
            GoAroundNode(src2, planarEdges[src.edgesIds.First()], src, table);
        }

        void GoAroundNode(PlanarNode n0, PlanarEdge e0,
            PlanarNode src2, Dictionary<long, bool> table)
        {

            e0.state++;
            if (e0.state > 2)
                return;

            PlanarNode m;
            PlanarNode n1 = e0.GetNeigh(n0);
            PlanarEdge e1 = planarEdges[e0.GetNextEdgeId(n1)];

            if (e1.parent == null)
            {
                Check(e1, n1,src2, table);
                m = e1.GetNeigh(n1);
                GoAroundNode(m, e1, src2, table);
            }
            else
            {
                if (n1.dist > lev0)
                {
                    Check(e1, n1,src2, table);  // never -pro kontrolu
                }

                else
                {
                    PlanarNode n2 = e1.GetNeigh(n1);
                    while (n2 != null && n2.dist > lev0)
                    {

                        PlanarEdge f = planarEdges[e1.GetNextEdgeId(n1)];
                        Check(e1, n1,src, table);
                        n2 = f.GetNeigh(n1);
                        e1 = f;
                    }
                }
                GoAroundNode(n1, e1, src, table);
            }
        }


        bool Check(PlanarEdge e, PlanarNode n, PlanarNode src2, Dictionary<long, bool> table)
        {
            PlanarNode neigh = e.GetNeigh(n);
            if (table[neigh.nid])
                return false;
            table[neigh.nid] = true;
            e.UpdateNeighbour(n, src2);   
            //        
            src2.edgesIds.Add(e.eid);
            return true;

        }


    }
}
