using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Klein_ApproximateDistanceQueries_0
{

    class SegmentEvent
    {
        public enum type { Start, End, Intersection };
        public type eventType;
        public Segment.Vector coordinate;
        public Segment s0;
        public long nid0, nid1, nnid0, nnid1;
        Segment s1;

        public SegmentEvent(type eventT, Segment.Vector coord, Segment seg0)
        {
            Init(eventT, coord, seg0);
        }

        public SegmentEvent(type eventT, Segment.Vector coord, Segment seg0, Segment seg1)
        {
            Init(eventT, coord, seg0);
            s1 = seg1;
        }

        void Init(type eventT, Segment.Vector coord, Segment seg0)
        {
            eventType = eventT;
            coordinate = coord;
            s0 = seg0;
        }


    }
}
