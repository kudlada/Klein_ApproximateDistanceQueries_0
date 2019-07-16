using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{


    class PlanarGraph
    {
        internal Dictionary<int, PlanarEdge> planarEdges;
        internal Dictionary<long, PlanarNode> planarNodes;
        public int lev0, lev1, lev2;
        public Dictionary<int, List<long>> levels;


        public PlanarGraph(Graph g)
        {

            SegmentsOrdering ord = new SegmentsOrdering();
         //   Node xx =g.nodes[74122365];
            ord.ProcessGraph(g);
            foreach (Node n in g.nodes.Values)
                foreach (Node.weightedEdge e in n.neighbourList)
                    if (e.weight <= 0)
                        throw new InvalidDataException();               
            planarEdges = ord.CreatePlanarEdges();
            foreach (PlanarEdge e in planarEdges.Values)
                if (e.w <= 0)
                    break;
            ord.CreatePlanarNodes();  
            planarNodes = ord.planarNodes;
            PlanarNode xp =planarNodes[74122365];
            foreach (Node n in g.nodes.Values)
            {
                planarNodes[n.id].insideCR = n.inside;
                if (n.inside == false)
                    continue;
            }
                
        }

        public PlanarGraph(List<PlanarNode> nodes, List<PlanarNode> sepCycle,
            PlanarNode src, Dictionary<int, PlanarEdge> planarE)
        {
            PlanarNode newSrc = new PlanarNode(src.nid);
            newSrc.edgesIds = new List<int>();
            planarNodes = new Dictionary<long, PlanarNode>();
            planarNodes.Add(newSrc.nid, newSrc);
            planarEdges = new Dictionary<int, PlanarEdge>();
            //        Dictionary<long, List<int>> toUpdate = new Dictionary<long, List<int>>();
            //        foreach (PlanarNode n in nodes)
            ///           toUpdate.Add(n.nid,new List<int>());
            foreach (PlanarNode n in nodes)
            {
                planarNodes.Add(n.nid, n);
                foreach (int eid in n.edgesIds)
                {
                    if (!planarEdges.ContainsKey(eid)&& planarE.ContainsKey(eid))
                    {
                        PlanarEdge e = new PlanarEdge(planarE[eid]);
                        planarEdges[eid] = e;
                    }
                }
            }
            int has = sepCycle.IndexOf(src);
            foreach (PlanarNode s in sepCycle)
            {
                if (planarNodes.ContainsKey(s.nid))
                    has = has;
               // List<int> toRemove = new List<int>();
                foreach (int eid in s.edgesIds)
                {
                    //   PlanarEdge e =  planarE[eid];
                    if (!planarEdges.ContainsKey(eid))
                        continue;
                    PlanarEdge eNew = planarEdges[eid];
                    PlanarNode neigh = eNew.GetNeigh(s);
                    eNew.eid = eid;
                    if (neigh == null)
                        break;
                    if (neigh!= null && planarNodes.ContainsKey(neigh.nid)
                        && neigh.parent!=null&& neigh.parent.nid==s.nid)
                    {
                        eNew.UpdateNeighbour(s, newSrc);
                        newSrc.edgesIds.Add(eid);
                        eNew.w = eNew.w + neigh.dist;
                       
                    }
              //      if (toUpdate.ContainsKey(neigh.nid))
              //          toUpdate[neigh.nid].Add(eid);
                    
                }
                
            }
       //     planarEdges.First().Value.w = 111111;


            /* List<int> eIds = new List<int>();
             foreach (int eid in src.edgesIds)
             {
                 if (newSrc.edgesIds.Contains(eid))
                     eIds.Add(eid);


             }
             */
            //  newSrc.edgesIds = eIds;

        }

        internal PlanarGraph(
            Dictionary<long, PlanarNode> planarN, Dictionary<int, PlanarEdge> planarE)
        {
            planarNodes = planarN;
            planarEdges = planarE;
        }

    }

    class PlanarNode
    {
        public long nid;  //shodny s puv.
        public int dist;
        public int cost = 0;
        public List<int> edgesIds;
        public int state = 0;
        public PlanarNode parent;
        public List<PlanarNode> children=new List<PlanarNode>();
        public bool insideCR = false;
        public bool insideCycle = false;
        public PlanarNode(long id)
        {
            nid = id;
        }
    }

    class PlanarEdge
    {
        public bool inTree = false;
       
        public bool trgl = false;   
        public int eid;
        public int w;
        public int dist;
        public object[] neighboursAdjEdges = new object[6];    //jen prvni dva potr. long
        public PlanarNode parent;                  // List<int> orderedEdges0;
        public int state = 0;
        PlanarEdgeRepre repre;


        public PlanarEdge()
        {
            
        }

        public PlanarEdge(PlanarNode n0, PlanarNode n1)
        {
            repre = new PlanarEdgeRepre(n0, n1);
            neighboursAdjEdges = repre.arr;
        }

        public PlanarEdge(PlanarNode n0, PlanarNode n1, int n0EAid, int n0EBid, int weigth)
        {
            repre = new PlanarEdgeRepre(n0, n1, n0EAid, n0EBid);
            neighboursAdjEdges = repre.arr;
            w = weigth;
        }

        public PlanarEdge(PlanarNode n0, PlanarNode n1, PlanarEdge n0e)
        {
            repre = new PlanarEdgeRepre(n0, n1, n0e);
            neighboursAdjEdges = repre.arr;
            
        }

        public PlanarEdge(PlanarEdge e)
        {
            PlanarEdgeRepre repre = new PlanarEdgeRepre(
                (PlanarNode) e.neighboursAdjEdges[0], (PlanarNode)e.neighboursAdjEdges[1]);
            neighboursAdjEdges = repre.arr;
            w =e.w;
            trgl = e.trgl;
            inTree = e.inTree;
            eid = e.eid;
        }

        public PlanarEdge(PlanarEdge e, PlanarNode oldNeigh, PlanarNode newNeigh)
        {
            new PlanarEdge(e);
            UpdateNeighbour(oldNeigh, newNeigh);
            newNeigh.edgesIds.Add(eid);
            w = oldNeigh.dist + e.w;
            
        }

        public PlanarNode GetNeigh(PlanarNode n)
        {
            if (n.nid == ((PlanarNode)neighboursAdjEdges[0]).nid)
                return (PlanarNode)neighboursAdjEdges[1];
            else if (n.nid == ((PlanarNode)neighboursAdjEdges[1]).nid)
                return (PlanarNode)neighboursAdjEdges[0];
            return null;

        }

        public void Update(Dictionary<int, PlanarEdge> planarEdges)
        {
            repre.Update(planarEdges);
        }

        public void UpdateNeighbour(PlanarNode oldN, PlanarNode newN)
        {
            if (oldN == neighboursAdjEdges[0])
            {
             /*   PlanarEdge e =(PlanarEdge) neighboursAdjEdges[3];
                PlanarNode n2 = e.GetNeigh(oldN);
                if (n2 == e.neighboursAdjEdges[0])
                    e.neighboursAdjEdges[2] = neighboursAdjEdges[2];
                else
                    e.neighboursAdjEdges[4] = neighboursAdjEdges[2];*/
                neighboursAdjEdges[0] = newN;
            }
            else if (oldN == neighboursAdjEdges[1])
            {
              /*  PlanarEdge e = (PlanarEdge)neighboursAdjEdges[5];
                PlanarNode n2 = e.GetNeigh(oldN);
                if (n2 == e.neighboursAdjEdges[0])
                    e.neighboursAdjEdges[2] = neighboursAdjEdges[4];
                else
                    e.neighboursAdjEdges[4] = neighboursAdjEdges[4];
                */
                neighboursAdjEdges[1] = newN;
            }
                
        }

        public int GetNextEdgeId(PlanarNode n)
        {
            int index = n.edgesIds.IndexOf(eid);
            if (index == n.edgesIds.Count - 1)
                return n.edgesIds[0];
            else
                return n.edgesIds[index + 1];
        }

        public PlanarEdge GetNextEdge(PlanarNode n)
        {

            if (n == neighboursAdjEdges[0])
                return (PlanarEdge)neighboursAdjEdges[2];
            if (n == neighboursAdjEdges[1])
                return (PlanarEdge)neighboursAdjEdges[4];
            return null;

        }

        public PlanarEdge GetNextEdge(PlanarNode n, out PlanarNode neigh) //smazat ?
        {
            
            if (n == neighboursAdjEdges[0])
            {
                neigh = (PlanarNode)neighboursAdjEdges[1];
                return (PlanarEdge)neighboursAdjEdges[2];
            }
            if (n == neighboursAdjEdges[1])
            {
                neigh = (PlanarNode)neighboursAdjEdges[0];
                return (PlanarEdge)neighboursAdjEdges[3];
            }
            neigh = null;
            return null;

        }
        /*

             public int GetNextNeighEdgeId(long nid, out long nidOther)  // dalsi hranu pro jiny uzel nez nid!
             {
                 if (nid == neighboursAdjEdges[1])
                 {
                     nidOther = neighboursAdjEdges[0];
                     return (int)neighboursAdjEdges[2];
                 }

                 if (nid == neighboursAdjEdges[0])
                 {
                     nidOther = neighboursAdjEdges[1];
                     return (int)neighboursAdjEdges[4];
                 }
                 nidOther = -1;
                 return -1;

             }
     */

    }

    class PlanarEdgeRepre
    {
        public object[] arr = new object[6];
        int n0_EAid, n0_EBid;

        public PlanarEdgeRepre(PlanarNode n0, PlanarNode n1, int n0EAid, int n0EBid)// PlanarEdge n0EA, PlanarEdge n0EB)
        {
            arr[0] = n0;
            arr[1] = n1;
            n0_EAid = n0EAid;
            n0_EBid = n0EBid;
            arr[4] = -1;
            arr[5] = -1;
        }

        public PlanarEdgeRepre(PlanarNode n0, PlanarNode n1, PlanarEdge n0e)
        {
            arr[0] = n0;
            arr[1] = n1;
            arr[2] = n0e;
            //arr[4] = -1;
            //arr[5] = -1;
        }

        public PlanarEdgeRepre(PlanarNode n0, PlanarNode n1)
        {
            arr[0] = n0;
            arr[1] = n1;
            
        }

        public void Update(Dictionary<int, PlanarEdge> planarEdges)
        {
            arr[2] = planarEdges[n0_EAid];
            arr[3] = planarEdges[n0_EBid];   //predelat TODO!!!  
          /*  if ((long)arr[4]>-1)
            {
                arr[4] = planarEdges[(int)arr[4]];
                arr[5] = planarEdges[(int)arr[5]];
            }
           */ 
        }

        public PlanarNode GetNextEdgeId(PlanarNode n)
        {
            if (n == arr[0])
            {
                return (PlanarNode)arr[2];
            }

            if (n == arr[1])
            {
                return (PlanarNode)arr[4];
            }
            return null;

        }
    }
}
