using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{
    class SegmentsOrdering
    {

        Dictionary<long, Dictionary<long, Node.weightedEdge>> edges;


        Dictionary<long, List<int>> nIdSortedEdgesId;
        Dictionary<int, Node.weightedEdge> eIdEdges;
        int edgesCount = 0;
        Node.weightedEdge[] verticalEdges;
        int id = 0;

        public void ProcessGraph(Graph g)
        {
            edges = new Dictionary<long, Dictionary<long, Node.weightedEdge>>();
            nIdSortedEdgesId = new Dictionary<long, List<int>>();
            eIdEdges = new Dictionary<int, Node.weightedEdge>();
            ProcessNodes(g);


        }



        void ProcessNodes(Graph g)
        {
            foreach (Node n in g.nodes.Values)
            {
                ProcessNode(n);

            }
        }

        void ProcessNode(Node n)
        {

            if (!n.inside)                // jen vnitrni
                return;

            Dictionary<double, Node.weightedEdge> rightSector = new Dictionary<double, Node.weightedEdge>();
            Dictionary<double, Node.weightedEdge> leftSector = new Dictionary<double, Node.weightedEdge>();
            verticalEdges = new Node.weightedEdge[2];
            foreach (Node.weightedEdge e in n.neighbourList)
            {
                if (!e.neighbour.inside)                // jen vnitrni
                    continue;
                ProcessEdge(n, e, leftSector, rightSector);

            }
            List<Node.weightedEdge> edg = new List<Node.weightedEdge>();  //protism. odspod
            if (verticalEdges[1] != null)
                edg.Add(verticalEdges[1]);
            foreach (Node.weightedEdge e in rightSector.Values)
                edg.Add(e);
            if (verticalEdges[0] != null)
                edg.Add(verticalEdges[0]);
            foreach (Node.weightedEdge e in leftSector.Values)
                edg.Add(e);
            AddToEdges(n, edg);
            List<int> sortedEid = new List<int>();
            foreach (Node.weightedEdge e in edg)
                sortedEid.Add(e.id);
            if (!nIdSortedEdgesId.ContainsKey(n.id))
                nIdSortedEdgesId.Add(n.id, sortedEid);  // kontr.na rovnost sorted?
        }

        void ProcessEdge(Node n, Node.weightedEdge e,
           Dictionary<double, Node.weightedEdge> leftSector,
           Dictionary<double, Node.weightedEdge> rightSector)
        {
            Segment.Vector v0 = new Segment.Vector();
            v0.x = n.coordinates.position.longitude;
            v0.y = n.coordinates.position.latitude;
            Segment.Vector v1 = new Segment.Vector();
            v1.x = e.neighbour.coordinates.position.longitude;
            v1.y = e.neighbour.coordinates.position.latitude;
            Segment seg = new Segment(v0, v1);
            if (v0.x > v1.x)
                AddToSector(leftSector, seg.directionNr, e);

            else if (v0.x < v1.x)
                AddToSector(rightSector, seg.directionNr, e);

            else if (v0.y < v1.y)
                verticalEdges[0] = e;
            else
                verticalEdges[1] = e;
        }

        void AddToSector(Dictionary<double, Node.weightedEdge> sector,
            double dirNr, Node.weightedEdge e)
        {
            while (sector.ContainsKey(dirNr))     //mozna chyba
                dirNr = dirNr + 0.00000001;
            sector.Add(dirNr, e);
        }

        void AddToEdges(Node n, List<Node.weightedEdge> edg)
        {
            long a, b;
            foreach (Node.weightedEdge e in edg)
            {

                a = n.id;
                b = e.neighbour.id;

                if (!edges.ContainsKey(a))
                {
                    Dictionary<long, Node.weightedEdge> aEdges
                        = new Dictionary<long, Node.weightedEdge>();
                    edges.Add(a, aEdges);
                }
                if (!edges[a].ContainsKey(b))
                {
                    if ((edges.ContainsKey(b) && edges[b].ContainsKey(a)))
                    {
                        Node.weightedEdge e2 = edges[b][a];
                        e.id = e2.id;
                    }
                    else
                    {
                        e.id = id;
                        id++;
                        eIdEdges.Add(e.id, e);
                    }
                    edges[a].Add(b, e);

                }
            }
        }

        public Dictionary<long, PlanarNode> planarNodes
            = new Dictionary<long, PlanarNode>();
        Dictionary<int, PlanarEdge> planarEdges
            = new Dictionary<int, PlanarEdge>();

        public Dictionary<int, PlanarEdge> CreatePlanarEdges()
        {
            foreach (KeyValuePair<long, Dictionary<long, Node.weightedEdge>> pair in edges)
            {
                long n0 = pair.Key;
                PlanarNode p0 = AddAndGetNode(n0);


                List<int> sortedE = nIdSortedEdgesId[n0];
                int mod = sortedE.Count;
                for (int i = 0; i < mod; i++)
                //foreach (int eid in sortedE)
                {
                    int j = (i + 1) % mod;
                    int k = (i + 2) % mod;
                    int eid = sortedE.ElementAt(j);

                    if (planarEdges.ContainsKey(eid))
                    {
                        planarEdges[eid].neighboursAdjEdges[4] = sortedE.ElementAt(k);
                        planarEdges[eid].neighboursAdjEdges[5] = sortedE.ElementAt(i);
                    }
                    else
                    {

                        long p0neighNid = eIdEdges[eid].neighbour.id;
                        PlanarNode p0neigh = AddAndGetNode(p0neighNid);
                        PlanarEdge npedg = new PlanarEdge(p0, p0neigh,
                            sortedE.ElementAt(k), sortedE.ElementAt(i), eIdEdges[eid].weight);
                        npedg.eid = eid;
                        planarEdges.Add(eid, npedg);
                    }
                }

            }

            foreach (PlanarEdge e in planarEdges.Values)
                e.Update(planarEdges);
            return planarEdges;
        }

        PlanarNode AddAndGetNode(long p0neighNid)
        {
            PlanarNode p0neigh;
            if (!planarNodes.ContainsKey(p0neighNid))
            {
                p0neigh = new PlanarNode(p0neighNid);
                planarNodes.Add(p0neigh.nid, p0neigh);
            }
            else
                p0neigh = planarNodes[p0neighNid];
            return
                p0neigh;
        }

        public void CreatePlanarNodes()
        {

            foreach (KeyValuePair<long, List<int>> pair in nIdSortedEdgesId)
            {
                PlanarNode pn = AddAndGetNode(pair.Key);
                pn.edgesIds = pair.Value;

            }


        }
    }
}
