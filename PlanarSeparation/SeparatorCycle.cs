using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{
    class SeparatorCycle
    {
        int insideDescCost = 0;
        int outsideDescCost = 0;
        bool isSum0inside = false;
        int allCostSum = 0;
        PlanarGraph g;
        PlanarNode commonAnc;
        bool reversed = false;
        internal List<long> cycle;
        Dictionary<long, List<PlanarNode>> cyclesNodes = new Dictionary<long, List<PlanarNode>>();
        List<PlanarEdge> orderedTriE;
        public List<PlanarNode> inC = new List<PlanarNode>();
        public List<PlanarNode> outC = new List<PlanarNode>();
        int order = 0;
        public SeparatorCycle()
        {
           
        }

        public SeparatorCycle(PlanarGraph graph)
        {
            g = graph;
        }

        public bool GetBoundaryCycle(PlanarGraph graph, PlanarNode src,
            PlanarEdge f)  //step 8
        {
            g = graph;

            allCostSum = g.planarNodes.Count; //cost=1
            int index = allCostSum / 2;       
            SetCycleCost(f);
            src.insideCycle = false;
            Bfs.Src_all_bfs(src, g.planarNodes, g.planarEdges, true);
            inC = new List<PlanarNode>();
            outC = new List<PlanarNode>();
            order++;
            foreach (PlanarNode n in g.planarNodes.Values)
            {
                if (cycle.Contains(n.nid))
                    continue;
                if (n.insideCycle)
                {
                    inC.Add(n);
                    n.insideCycle = false;
                }

                else// if (!cycle.Contains(n.nid))
                    outC.Add(n); ;//vc sep


            }

            List<PlanarNode> min;
            if (inC.Count > outC.Count)
                min = outC;
            else
                min = inC;
            // if (!cyclesNodes.ContainsKey(f.eid))
            //      cyclesNodes.Add(f.eid, min);
            return (min.Count > (g.planarNodes.Count) / 3);
            
        }

        public void GetFirstCycle(PlanarGraph graph, PlanarNode src,
            int lev0)  //step 8
        {
            g = graph;
           
            allCostSum = g.planarNodes.Count; //cost=1
            int index = allCostSum / 2;       //od pol.
                                              //   bool found = false;
                                              //    PlanarEdge e = null;
                                              //      for (int i = index; i < g.planarNodes.Count; i++)
                                              //      {
                                              //          foreach (int eid in g.planarNodes.ElementAt(i).Value.edgesIds)
            PlanarEdge f;
             if (order<=1)
            {
                f = g.planarEdges[1630674];
            }
            else
            {
                if (orderedTriE == null)
                    Init(lev0);
                if (orderedTriE.Count == 0)
                    return;
                f = orderedTriE[0];
                orderedTriE.RemoveAt(0);
            }
            
                                           
           
          

            SetCycleCost(f);
            src.insideCycle = false;
            Bfs.Src_all_bfs(src,g.planarNodes, g.planarEdges,true);
            inC = new List<PlanarNode>();
            outC = new List<PlanarNode>();
            order++;
            foreach (PlanarNode n in g.planarNodes.Values)
            {
                if (cycle.Contains(n.nid))
                    continue;
                if (n.insideCycle)
                {
                    inC.Add(n);
                    n.insideCycle = false;
                }
                   
                else// if (!cycle.Contains(n.nid))
                    outC.Add(n); ;//vc sep


            }

            List<PlanarNode> min;
            if (inC.Count > outC.Count)
                min = outC;
            else
                min = inC;
           // if (!cyclesNodes.ContainsKey(f.eid))
           //      cyclesNodes.Add(f.eid, min);
            if (min.Count < g.planarNodes.Count / 13)
            {
                GetFirstCycle(g, src, lev0);
            }
            
                

        }

        private void Init(int lev0)
        {
            if (orderedTriE == null)
            {
                orderedTriE = new List<PlanarEdge>();
                foreach (PlanarEdge e in Triangulation.triEdges)
                {

                    GetDistDiff((PlanarNode)e.neighboursAdjEdges[0],
                        (PlanarNode)e.neighboursAdjEdges[1], e, lev0);

                }
                foreach (List<PlanarEdge> tr in cyclesSizes.OrderByDescending(x => x.Key)
                    .Select(x => x.Value))
                {
                    orderedTriE = orderedTriE.Concat(tr).ToList();
                    if (orderedTriE.Count > 1000)
                        break;
                }

            }
            else if (orderedTriE.Count == 0)
                return;

        }

        public void RecursiveSeparation(PlanarGraph graph, PlanarNode src,
            int lev0)
        {
            List<long> c = new List<long>();
            PlanarSeparator sep = new PlanarSeparator();
            sep.Separate(ref graph, out c);
            Console.WriteLine(graph.planarNodes.Count);
            Console.ReadKey();
             GetFirstCycle(graph, src, lev0);
            List<PlanarNode> cycleNodes
                = cycle.Select(x => graph.planarNodes[x]).ToList();
            PlanarGraph g0 = new PlanarGraph(inC, cycleNodes, src,
                new Dictionary<int, PlanarEdge> (graph.planarEdges));
           // PlanarGraph g1 = new PlanarGraph(outC, cycleNodes, src, graph.planarEdges);
            RecursiveSeparation(g0, g0.planarNodes.ElementAt(0).Value, 10000);
          //  RecursiveSeparation(g1, g1.planarNodes.ElementAt(0).Value, 10000);
        }



        public  List<string> outputs = new List<string>();

        public void GetCycle_U_V(PlanarGraph graph, PlanarNode u, PlanarNode v)
        {
            g = graph;
          //  GetCycle(u, v);
        }

        Dictionary<int, List<PlanarEdge>> cyclesSizes = new Dictionary<int, List<PlanarEdge>> ();


        private void GetDistDiff(PlanarNode u, PlanarNode v, PlanarEdge e,int lev0)
        {
            if (u.dist == int.MaxValue || v.dist == int.MaxValue)
                return;
            if (u.dist <lev0 && v.dist < lev0)
                return;
            int sz = Math.Abs(u.dist - v.dist);
            if (!cyclesSizes.ContainsKey(sz))
                cyclesSizes.Add(sz, new List<PlanarEdge>());
            cyclesSizes[sz].Add(e);
            
        }

        public List<PlanarNode> SeparateByEdge(
            PlanarEdge f, PlanarNode src,
            out List<PlanarNode> inC, out List<PlanarNode> outC)
        {
            Bfs.Src_all_bfs(src, g.planarNodes, g.planarEdges, true);
            SetCycleCost(f);
            Bfs.Src_all_bfs(src, g.planarNodes, g.planarEdges, true);
            inC = new List<PlanarNode>();
            outC = new List<PlanarNode>();
            List<PlanarNode> sep = new List<PlanarNode>();
            foreach (PlanarNode n in g.planarNodes.Values)
            {
                if (cycle.Contains(n.nid))
                    continue;
                if (n.insideCycle)
                {
                    inC.Add(n);
                    n.insideCycle = false;
                }
                else 
                    outC.Add(n);
              
            }
            return sep;
        }

        private void GetCycle(PlanarNode u, PlanarNode v, PlanarEdge e)
        {
            
            cycle = new List<long>();
            PlanarNode x = u;
            int state = -3;

            while (x.parent != null)
            {
                cycle.Add(x.nid);
                x.state = state;//cycle
                x = x.parent;

            }
            cycle.Add(x.nid);
            x.state = state;
            commonAnc = x;
            x = v;
            int size = 1;
            
            while (x.parent != null)
            {
                cycle.Add(x.nid);
                size++;
                x.state = state;
                if (x.parent.state == -3 && state == -3)
                {
                    commonAnc = x.parent;
                    break;
                   
                }
                
                x = x.parent;

            }
            return;
            x = u;
            state = 0;

            while (x.parent != null)
            {
                x.state = state;//cycle
                x = x.parent;

            }
        //    if (!cyclesSizes.ContainsKey(size))
        //        cyclesSizes.Add(size, e);
        }

        public void SetCycleCost(PlanarEdge e)
        {
            PlanarNode u = (PlanarNode)e.neighboursAdjEdges[0];
            PlanarNode v = (PlanarNode)e.neighboursAdjEdges[1];
            GetCycle(u, v,e);
            
            

            //////
            
            int sum0 = 0;
            int sum1 = 0;
            PlanarEdge xe = new PlanarEdge();
            PlanarEdge parU = null;
            PlanarEdge parV = null;
            try
            {
                foreach (int eid in u.edgesIds)
                    if (g.planarEdges.ContainsKey(eid)
                        && g.planarEdges[eid].GetNeigh(u).nid == u.parent.nid)
                    {

                        parU = g.planarEdges[eid];
                        parU.eid = eid;

                    }
                
                foreach (int eid in v.edgesIds)
                    if (g.planarEdges.ContainsKey(eid)
                        && g.planarEdges[eid].GetNeigh(v).nid == v.parent.nid)
                    {
                        parV = g.planarEdges[eid];
                        parV.eid = eid;
                    }

            }
            catch (Exception ex)
            {
                return;
            }
          //  if (u.parent.parent == null && v.parent.parent == null&&false)
          //      sum0 = GetDescCostFromXToZAroundY(u,u.parent, e, out xe);
          //  else
          //  {
                e.inTree = true;
                PlanarNode last;
            try {
                SetCycleCostUpward(ref u, parU, e, out last, ref sum0, ref sum1);

       //         SetCycleCostAroundAnc(u, last, ref sum0, ref sum1);
                SetCycleCostAroundAnc(commonAnc, last, ref sum0, ref sum1);
                reversed = true;
                SetCycleCostUpward(ref v, parV, e, out last, ref sum0, ref sum1);
            }
                catch 
            {
                return;
            }
                e.inTree = false;
                
          //  }
            
            if (sum0 > sum1)
            {
                isSum0inside = true;
                insideDescCost = sum0;
                outsideDescCost = sum1;
            }
            else
            {
                insideDescCost = sum1;
                outsideDescCost = sum0;
            }


   /*          while (insideDescCost > 2 * outsideDescCost)
            {
                e = ExpandCycle((PlanarNode)e.neighboursAdjEdges[0], (PlanarNode)e.neighboursAdjEdges[1], e);    //(v,w,e)
            }
    */            
        }

        private void SetCycleCostUpward(ref PlanarNode n, PlanarEdge pn, PlanarEdge nch, 
            out PlanarNode last, ref int sum0, ref int sum1)
        {
            PlanarNode par = n.parent;
            last = n;
            //       while (par!=null && par.nid!=commonAnc.nid)
            while (n != null && n.nid != commonAnc.nid)
            {
                SetCycleCostAroundN(n, pn, nch, ref sum0, ref sum1);
                nch = pn;
                last = n;
                n = n.parent;
                if (n.parent == null )//||(reversed&&n.parent.state==-3))
                    break;
                foreach (int eid in n.edgesIds)
                    if (g.planarEdges.ContainsKey(eid) &&
                         g.planarEdges[eid].GetNeigh(n).nid == n.parent.nid)
                    {
                        pn = g.planarEdges[eid];
                        break;
                    }

            }
         
        }

        private void SetCycleCostAroundN(PlanarNode n, PlanarEdge pn, PlanarEdge nch,
            ref int sum0, ref int sum1)
        {
            int[] sums = SplitEdges(n.edgesIds, pn, nch,n);
            if (!reversed)
            {
                sum0 = sum0+ sums[0];
                sum1 = sum1 + sums[1];
            }
            else
            {
                sum0 = sum0 + sums[1];
                sum1 = sum1 + sums[0];
            }

        }

        private void SetCycleCostAroundAnc(PlanarNode anc, PlanarNode last, ref int sum0, ref int sum1)
        {
           // return;
            PlanarEdge pn=null;
            PlanarEdge nch=null;
            foreach (int eid in anc.edgesIds)
            {
                if (g.planarEdges[eid].GetNeigh(anc).state == -3)  //count=2
                {
                    if (g.planarEdges[eid].GetNeigh(anc).nid == last.nid)
                    {
                        nch = g.planarEdges[eid];
                        if (pn==null)
                            pn = nch; //src muze mit jen jednu hr.
                    }
                    else
                    {
                        pn = g.planarEdges[eid];
                    }
                        
                }
            }
            ;
            SetCycleCostAroundN(anc,pn,nch, ref sum0, ref sum1);
        }

        private int[] SplitEdges(List<int>eids, PlanarEdge pn, PlanarEdge nch, PlanarNode n)
        {
            int ipn = eids.IndexOf(pn.eid);
            int inch = eids.IndexOf(nch.eid);
            List<int> pn_nch=new List<int>();
            List<int> nch_pn = new List<int>();
            int[] result = new int[] { 0, 0 };
            if (ipn<inch)
            {
                pn_nch = eids.GetRange(ipn + 1, inch - (ipn + 1));
                nch_pn = eids.GetRange(0,ipn );
                nch_pn.AddRange(eids.GetRange(inch+1, eids.Count- (inch + 1)));
                
            }
            else if (ipn > inch)
            {
                nch_pn = eids.GetRange(inch + 1, ipn - (inch + 1));
                pn_nch = eids.GetRange(0, inch);
                pn_nch.AddRange(eids.GetRange(ipn + 1, eids.Count - (ipn + 1)));
            }
            else
            {
                pn_nch = new List<int>(eids);
                pn_nch.Remove(pn.eid);
            }

            foreach (int eid in pn_nch.Where(ei => g.planarEdges.ContainsKey(ei)))
            {
                result[0] = result[0] + GetDescCost(n, g.planarEdges[eid].GetNeigh(n));
                if (!reversed)
                    g.planarEdges[eid].GetNeigh(n).insideCycle = true;
            }
                
            foreach (int eid in nch_pn.Where(ei => g.planarEdges.ContainsKey(ei)))
            {
                result[1] = result[1] + GetDescCost(n, g.planarEdges[eid].GetNeigh(n));
                if (reversed)
                    g.planarEdges[eid].GetNeigh(n).insideCycle = true;
            }
               
            return result;
        }

        private void SetCycleCostUpwardOld(PlanarNode n, PlanarEdge e, ref int sum0, ref int sum1)
        {
            Console.WriteLine("SetCycleCostUpward" + n.nid);
            PlanarNode neigh = n;
            List<PlanarEdge> nn = new List<PlanarEdge>();
            while (n.state == -3)
            {
                nn.Add(e);
                neigh = e.GetNeigh(n);
                if (neigh.state == -2 && reversed)   //anc
                    break;
                e = SetCostAroundNode(n, e, ref sum0, ref sum1);
                Console.WriteLine(sum0 + " " + sum1);
                if (neigh.state == -2)   //anc
                    break;
                n = neigh;
            }
            
        }

        private PlanarEdge SetCostAroundNode(PlanarNode v0, PlanarEdge e, ref int sum0, ref int sum1)
        {
            List<PlanarEdge> nn = new List<PlanarEdge>();
            PlanarNode v = e.GetNeigh(v0);
            if (v.state != -3 && v.state != -2)
                return null;
            nn.Add(e);
            
            e = g.planarEdges[e.GetNextEdgeId(v)];
            PlanarNode w = e.GetNeigh(v);

            int st0 = -1;
            int st1 = -2;
            if (reversed)
            {
                st0 = -2;
                st1 = -1;
            }
            while (((w.state != -3) && 
                (w.state != -2)) || ((w.state == -3 || w.state == -2) && !e.inTree))     {
                nn.Add(e);
                if (e.inTree)
                    sum0 = sum0 + GetDescCost(v, w);
                e.state = st0;
                if (w.parent == null)
                    return e;
                // e = e.GetNextEdge(v, out v0);
                e = g.planarEdges[e.GetNextEdgeId(v)];
                w = e.GetNeigh(v);
            }
            
            PlanarEdge result = e;
            //          e = e.GetNextEdge(v, out v0);
            e.GetNextEdgeId(v);
            w = e.GetNeigh(v);
            while (((w.state != -3) && 
                (w.state != -2)) || ((w.state == -3 || w.state == -2) && !e.inTree))
            {
                if (e.inTree)
                    sum1 = sum1 + GetDescCost(v, w);
                e.state = st1;
                //      e = e.GetNextEdge(v, out v0);
                e = g.planarEdges[e.GetNextEdgeId(v)];
                w = e.GetNeigh(v);
            }
            if (v0.nid == w.nid)
                return nn.Last();

            //    if (e.inTree)
            //        sum1 = sum1 + GetDescCost(v, w);
            return result;
        }

        

        private PlanarEdge ExpandCycle(PlanarNode v, PlanarNode w, PlanarEdge e)
        {
            PlanarNode y, n;
            PlanarEdge yv, yw, next;
            if (isSum0inside)
            {
                //  yv =g.planarEdges[ e.GetNextEdgeId(v)];
                yw = g.planarEdges[e.GetNextEdgeId(w)];
                y = yw.GetNeigh(w);
                yv = g.planarEdges[yw.GetNextEdgeId(y)];
            }
            else
            {
                yv = g.planarEdges[e.GetNextEdgeId(v)];
              //  yw = g.planarEdges[e.GetNextEdgeId(w)];
                y = yv.GetNeigh(v);
                yw = g.planarEdges[yv.GetNextEdgeId(w)];
            }
            if (yv.inTree)
            {
                next=ExpandCycleWithTriangle(y, yw, yv);
                cycle.Add(next.GetNeigh(y).nid);
              //  next = yw;
            }
            else if (yw.inTree)
            {
                ExpandCycleWithTriangle(y, yv, yw);
                next = yv;
            }
            else
            {
                SetCycleCost(yv);
                return yv;
               // int sum0_v = sum0_v;
              //  SetCycleCost()
               // next = ExpandCycleWithNewCycle(y, yv, yw);

                //  dopocitat a dosadit inscost outscost
            }
            return next;
        }

        private PlanarEdge ExpandCycleWithTriangle(PlanarNode y,
            PlanarEdge yx, PlanarEdge treeEdge)
        {
            y.state = -3;
            PlanarEdge next =g.planarEdges[ yx.GetNextEdgeId(y)];
           //     int yDescCost = GetInsideDescCostForNextTwoEdges(y,next, yx);
            int yDescCost =GetDescCost(y, next.GetNeigh(y));
            insideDescCost = insideDescCost - yDescCost;
            outsideDescCost = outsideDescCost + yDescCost;
            return next;
        }

        

        private PlanarEdge ExpandCycleWithNewCycle(PlanarNode y,
            PlanarEdge yv, PlanarEdge yw)
        {
            PlanarNode z = y;
            PlanarNode zchild = y;
            int totalPathCost = 0;
            while (z.parent!=null&& z.parent.state > -2)
            {
                z.state = -3;
                totalPathCost = totalPathCost + z.cost;
                zchild = z;
                z = z.parent;
            }
            PlanarNode v = yv.GetNeigh(y);
            PlanarNode w = yw.GetNeigh(y);
            ///
            bool left_yz_path = Has_left_yz_path(z, zchild);
            //
            PlanarEdge result_y_e;
            int[] insideCycleCost = ScanBothNewCycles(
                left_yz_path, v, w, y, yv, yw, out result_y_e);
            int yvInsideCycleCost = insideCycleCost[0];
            int ywInsideCycleCost = insideCycleCost[1];
            int insideCostOfSmallCycle;
            PlanarNode smallCycleLeave, bigCycleLeave;
            if (result_y_e.GetNeigh(y).nid == v.nid)
            {
                smallCycleLeave = v;
                bigCycleLeave = w;
                insideCostOfSmallCycle = yvInsideCycleCost;
            }
            else
            {
                smallCycleLeave = w;
                bigCycleLeave = v;
                insideCostOfSmallCycle = ywInsideCycleCost;
            }
            int costOfBigCycle = 
                GetInsideCostOfBigCycle(insideCostOfSmallCycle, insideDescCost, totalPathCost);
            return result_y_e;
        }



        private int[] ScanBothNewCycles(bool left_yz_path,
            PlanarNode v, PlanarNode w, PlanarNode y,
            PlanarEdge yv, PlanarEdge yw, out PlanarEdge result_y_e)
        {
            bool end = false;
            bool v_left_yz_path = left_yz_path;
            bool w_left_yz_path = left_yz_path;
            PlanarNode r_v, l_v, r_w, l_w;
            InitNewCycleLeaves(v, y, out r_v, out l_v, left_yz_path);
            InitNewCycleLeaves(w, y, out r_w, out l_w, left_yz_path);
            PlanarNode y_v = y;
            PlanarNode y_w = y;
            PlanarNode last_l_v = y;
            PlanarNode last_l_w = y;
            PlanarNode z_v = l_v.parent;
            PlanarNode z_w = r_w.parent;
            PlanarEdge ev = yv;
            PlanarEdge ew = yw;
            int costInside_yv = 0;
            int costInside_yw = 0;
            result_y_e = null;
            while (!end)                  //zmena oproti alg -jde str.po vrch., ne jedn.hranach
            {
                ev = ScanCycle(r_v, yv, ref y_v, ref z_v, 
                    ref last_l_v, ev, ref v_left_yz_path, ref costInside_yv, ref end);
                if (end)
                {
                    result_y_e = yv;
                    break;
                }
                ew = ScanCycle(r_w, yw, ref y_w, ref z_w, 
                    ref last_l_w, ew, ref w_left_yz_path, ref costInside_yw, ref end);
                if (end)
                {
                    result_y_e = yw;
                }
            }
            return new int[] { costInside_yv, costInside_yw };

        }

        private void InitNewCycleLeaves(PlanarNode u, PlanarNode y,
            out PlanarNode right, out PlanarNode left, bool left_yz_path)
        {
            if (left_yz_path)
            {
                left = y;
                right = u;
            }
            else
            {
                left = u;
                right = y;
            }
        }

        private PlanarEdge ScanCycle(PlanarNode r, PlanarEdge rl,
            ref PlanarNode y, ref PlanarNode z, ref PlanarNode last_l, PlanarEdge xy,
            ref bool left_yz_path, ref int sum, ref bool end)
        {
            PlanarEdge yz;
            sum = sum + GetInsideDescCostFromXToZAroundY(y, z, xy, out yz, left_yz_path);
            if (yz.GetNeigh(y).state != -3)
            {
                if (left_yz_path)
                {
                    left_yz_path = false;
                    last_l = y;
                    y = rl.GetNeigh(r);            //rigth branch -leave
                    z = y.parent;
                    return rl;
                }
                end = true;
                sum = sum + GetDescCostFromXToZAroundY(yz.GetNeigh(y), last_l, yz, out yz);
                z = yz.GetNeigh(y);


            }
            return yz;
        }

        private bool Has_left_yz_path(PlanarNode z, PlanarNode zchild)
        {
            int lastEid = -1;
            foreach (int eid in z.edgesIds)
                if (g.planarEdges[eid].GetNeigh(z).nid == zchild.nid)
                {
                    lastEid = eid;
                    break;
                }
            PlanarEdge e = g.planarEdges[g.planarEdges[lastEid].GetNextEdgeId(z)];
            PlanarNode u = e.GetNeigh(z);
            while (u.state > -2)
            {
                e = g.planarEdges[e.GetNextEdgeId(z)];
              
                u = e.GetNeigh(z);
            }
            if (u.parent.nid == z.nid)
                return false;
            return true;
        }

        private int GetInsideDescCostForNextTwoEdges(PlanarNode y,   //smazat/test
            PlanarEdge e, PlanarEdge f)
        {
            int descCost = 0;
            foreach (int eid in y.edgesIds)
            {
                if (eid == e.eid || eid == f.eid)
                    continue;
                PlanarEdge ye = g.planarEdges[eid];
                if (ye.inTree)
                {
                    descCost = descCost + GetDescCost(y, ye.GetNeigh(y));
                }
                    
            }
            return descCost;
        }


        private int GetInsideCostOfBigCycle(int smallCycleInsideDescCost,
            int originCycleInsideDescCost, int pathCost)
        {
            return (smallCycleInsideDescCost + originCycleInsideDescCost + pathCost);

        }

        private int GetInsideDescCostFromXToZAroundY(PlanarNode y,
             PlanarNode z, PlanarEdge xy, out PlanarEdge yz, bool leftpath)
        {
            if (leftpath)
                return GetDescCostFromZToXAroundY(y, z, xy, out yz);
            return GetDescCostFromXToZAroundY(y, z, xy, out yz);
        }


        private int GetDescCostFromXToZAroundY(PlanarNode y,   //rigth path
             PlanarNode z, PlanarEdge xy, out PlanarEdge yz)
        {
            int descCost = 0;
            yz = xy;
            PlanarNode w = yz.GetNeigh(y);
            while (w.nid != z.nid)
            {
                yz = g.planarEdges[yz.GetNextEdgeId(y)];
                
                w = yz.GetNeigh(y);
                descCost = descCost + GetDescCost(y, w);
            }
            return descCost;
        }

        private int GetDescCostFromZToXAroundY(PlanarNode y,   // left path
             PlanarNode z, PlanarEdge xy, out PlanarEdge yz)
        {
            yz = xy;
            PlanarNode w = yz.GetNeigh(y);
            while (w.nid != z.nid)
            {
                yz = g.planarEdges[yz.GetNextEdgeId(y)];
               
                w = yz.GetNeigh(y);
            }
            PlanarEdge e = yz;
            PlanarEdge e0;
            int descCost = GetDescCostFromXToZAroundY(y, xy.GetNeigh(y), e, out e0);
            return descCost;
        }

        private int GetDescCost(PlanarNode v, PlanarNode w)
        {
                
            if (v.parent!=null && v.parent.nid == w.nid)
                return allCostSum - v.cost;
            if (w.parent != null && w.parent.nid == v.nid)
                return w.cost;
            return 0; //not tree
        }
    }
}
