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

        public void GetFirstCycle(PlanarGraph graph)  //step 8
        {
            g = graph;
            
            allCostSum = g.planarNodes.Count; //cost=1
            int index = allCostSum / 2;       //od pol.
            bool found = false;
            PlanarEdge e = null;
            for (int i = index; i < g.planarNodes.Count; i++)
            {
                foreach (int eid in g.planarNodes.ElementAt(i).Value.edgesIds)
                {
                    if (!g.planarEdges[eid].inTree)
                    {

                        e = g.planarEdges.ElementAt(i).Value;
                        if (((PlanarNode)e.neighboursAdjEdges[0]).dist == 0
                            || ((PlanarNode)e.neighboursAdjEdges[1]).dist == 0) //jde o src
                                                                               
                            continue;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }
                    
            if (!found)
            {
                for (int i = index - 1; i >= 0; i--)
                {
                    foreach (int eid in g.planarNodes.ElementAt(i).Value.edgesIds)

                        if (!g.planarEdges[eid].inTree)
                        {
                            found = true;
                            e = g.planarEdges.ElementAt(i).Value;
                            if (((PlanarNode)e.neighboursAdjEdges[0]).dist == 0
                                || ((PlanarNode)e.neighboursAdjEdges[1]).dist == 0) //jde o src
                                                                                    // || e.state == 0) //triang.hrana
                                continue;
                            break;
                        }

                    if (found)
                        break;
                }

            }
                 
            SetCycleCost(e);


        }

        public void GetCycle_U_V(PlanarGraph graph, PlanarNode u, PlanarNode v)
        {
            g = graph;
            GetCycle(u, v);
        }

        private void GetCycle(PlanarNode u, PlanarNode v)
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

            while (x.parent != null)
            {
                cycle.Add(x.nid);

                if (x.state == -3 && state == -3)
                {
                    commonAnc = x;
                    break;
                   
                }
                x.state = state;
                x = x.parent;

            }
        }

        private void SetCycleCost(PlanarEdge e)
        {
            GetCycle((PlanarNode)e.neighboursAdjEdges[0], 
                (PlanarNode)e.neighboursAdjEdges[1]);
            
            return;

            //////
            /*
            x.state = state;
            int sum0 = 0;
            int sum1 = 0;
            PlanarEdge xe = new PlanarEdge();
            PlanarEdge parU = null;
            foreach (int eid in u.edgesIds)
                if (g.planarEdges[eid].GetNeigh(u).nid==u.parent.nid)
                {
                    parU = g.planarEdges[eid];
                }
            PlanarEdge parV = null;
            foreach (int eid in v.edgesIds)
                if (g.planarEdges[eid].GetNeigh(v).nid == v.parent.nid)
                {
                    parV = g.planarEdges[eid];
                }
            if (u.parent.parent == null && v.parent.parent == null&&false)
                sum0 = GetDescCostFromXToZAroundY(u,u.parent, e, out xe);
            else
            {
                e.inTree = true;
                PlanarNode last;
                SetCycleCostUpward(ref u, parU,e, out last, ref sum0, ref sum1);
                SetCycleCostAroundAnc(u, last, ref sum0, ref sum1);
                
                reversed = true;
                SetCycleCostUpward(ref v, parV, e, out last, ref sum0, ref sum1);
                e.inTree = false;
                
            }
            
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
             while (insideDescCost > 2 * outsideDescCost)
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
            while (par!=null)
            {
                SetCycleCostAroundN(n, pn, nch, ref sum0, ref sum1);
                nch = pn;
                last = n;
                n = n.parent;
                if (n.parent == null ||(reversed&&n.parent.state==-3))
                    break;
                foreach (int eid in n.edgesIds)
                    if (g.planarEdges[eid].GetNeigh(n).nid == n.parent.nid)
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
            PlanarEdge pn=null;
            PlanarEdge nch=null;
            foreach (int eid in anc.edgesIds)
            {
                if (g.planarEdges[eid].GetNeigh(anc).state == -3)  //count=2
                {
                    if (g.planarEdges[eid].GetNeigh(anc).nid == last.nid)
                    {
                        nch = g.planarEdges[eid];
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
           
            foreach (int eid in pn_nch)
                result[0] = result[0] + GetDescCost(n, g.planarEdges[eid].GetNeigh(n));
            foreach (int eid in nch_pn)
                result[1] = result[1] + GetDescCost(n, g.planarEdges[eid].GetNeigh(n));
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
                next = ExpandCycleWithNewCycle(y, yv, yw);

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
