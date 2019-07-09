using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klein_ApproximateDistanceQueries_0
{
    class Intersections
    {
        
        Graph g;
        SortedDictionary<double, SortedDictionary<double, List<SegmentEvent>>> events =
                 new SortedDictionary<double, SortedDictionary<double, List<SegmentEvent>>>();
        List<SegmentEvent> intersections = new List<SegmentEvent>();

        public void CreateIntersectionsFile(Graph g, string intersectionsOutputFile)  
            //todo: multithr.
        {
            StreamWriter w = new StreamWriter(intersectionsOutputFile);
            try
            {
                List<SegmentEvent> events = GetIntersections(g);
                int co = events.Count;
                w.WriteLine(co);
                foreach (SegmentEvent e in events)
                {
                    w.WriteLine(e.nid0 + "*" + e.nid1 + "*" + e.nnid0 + "*" + e.nnid1);
                    w.WriteLine(e.coordinate.x + "*" + e.coordinate.y);
                }
            }
            catch { };
            w.Close();
        }

        private List<SegmentEvent> GetIntersections(Graph graph)
        {

            CreateEvents();
            foreach (SortedDictionary<double, List<SegmentEvent>> dY in events.Values)
                foreach (List<SegmentEvent> lYX in dY.Values)
                    foreach (SegmentEvent ev in lYX)
                        if (ev.eventType == SegmentEvent.type.Start)
                            CheckAllIntersections(ev);
            return intersections;
        }

        private void CheckAllIntersections(SegmentEvent e)
        {
            double X = e.coordinate.x;
            X = Math.Floor(X * 10);
            X = Math.Floor(X);
            double Y = e.coordinate.y;
            Y = Math.Floor(Y * 10);
            Y = Math.Floor(Y);
            foreach (SegmentEvent ev in events[Y][X])
                Check(e, ev);
            Y = Y - 1;
            if (events.ContainsKey(Y) && events[Y].ContainsKey(X))
                foreach (SegmentEvent ev in events[Y][X])
                    Check(e, ev);

            if (events.ContainsKey(Y) && events[Y].ContainsKey(X - 1))
                foreach (SegmentEvent ev in events[Y][X - 1])
                    Check(e, ev);
            if (events.ContainsKey(Y) && events[Y].ContainsKey(X + 1))
                foreach (SegmentEvent ev in events[Y][X + 1])
                    Check(e, ev);
            Y = Y + 1;
            if (events.ContainsKey(Y) && events[Y].ContainsKey(X - 1))
                foreach (SegmentEvent ev in events[Y][X - 1])
                    Check(e, ev);
            if (events.ContainsKey(Y) && events[Y].ContainsKey(X + 1))
                foreach (SegmentEvent ev in events[Y][X + 1])
                    Check(e, ev);
            Y = Y + 1;
            if (events.ContainsKey(Y) && events[Y].ContainsKey(X))
                foreach (SegmentEvent ev in events[Y][X])
                    Check(e, ev);

            if (events.ContainsKey(Y) && events[Y].ContainsKey(X - 1))
                foreach (SegmentEvent ev in events[Y][X - 1])
                    Check(e, ev);
            if (events.ContainsKey(Y) && events[Y].ContainsKey(X + 1))
                foreach (SegmentEvent ev in events[Y][X + 1])
                    Check(e, ev);
        }


        private void Check(SegmentEvent e, SegmentEvent ev)
        {
            Segment.Vector v;
            if (e.nid0 != ev.nid0 && e.nid0 != ev.nid1 && e.nid1 != ev.nid0 && e.nid1 != ev.nid1)
                if (ev.s0.upperEnd.y > e.s0.lowerEnd.y && e.s0.upperEnd.y > ev.s0.lowerEnd.y)
                    if (e.s0.HasIntersection(ev.s0, out v))
                    {
                        SegmentEvent isEv = new SegmentEvent(SegmentEvent.type.Intersection, v, e.s0, ev.s0);
                        isEv.nid0 = e.nid0;
                        isEv.nid1 = e.nid1;
                        isEv.nnid0 = ev.nid0;
                        isEv.nnid1 = ev.nid1;
                        intersections.Add(isEv);
                    }
        }


        private void CreateEvents()
        {

            foreach (Node n in g.nodes.Values)
            {
                foreach (Node.weightedEdge e in n.neighbourList)
                    CreateEventsFromEdges(n, e);
            }
        }


        private void CreateEventsFromEdges(Node n, Node.weightedEdge e)
        {
            if (!n.inside)
                return;
            Segment.Vector v0 = new Segment.Vector();
            v0.x = n.coordinates.position.longitude;
            v0.y = n.coordinates.position.latitude;
            Segment.Vector v1 = new Segment.Vector();
            v1.x = e.neighbour.coordinates.position.longitude;
            v1.y = e.neighbour.coordinates.position.latitude;
            Segment s = new Segment(v0, v1);
            SegmentEvent eventStart = new SegmentEvent(SegmentEvent.type.Start, s.upperEnd, s);
            eventStart.nid0 = n.id;
            eventStart.nid1 = e.neighbour.id;
            if (eventStart.nid0 == eventStart.nid1)
                return;
            AddEvent(eventStart);
            

        }

        private void AddEvent(SegmentEvent ev)
        {
            double X = ev.coordinate.x;
            X = Math.Floor(X * 10);
            X = Math.Floor(X);
            double Y = ev.coordinate.y;
            Y = Math.Floor(Y * 10);
            Y = Math.Floor(Y);
            if (!events.ContainsKey(Y))
            {
                SortedDictionary<double, List<SegmentEvent>> dict =
                    new SortedDictionary<double, List<SegmentEvent>>();
                events.Add(Y, dict);
            }
            if (!events[Y].ContainsKey(X))
            {
                List<SegmentEvent> list = new List<SegmentEvent>();
                events[Y].Add(X, list);
            }
            events[Y][X].Add(ev);
        }






    }

    

}
