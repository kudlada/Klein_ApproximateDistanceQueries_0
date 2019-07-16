using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphImage
{
    class ItemTOC
    {
        public enum types
        {
            simple=0,aStar,l50,l40,l30,l25,l20
        }
        public string[] labels=new string[] {"Dijkstra", "A* Euclid", "A*:  49 Landmarks", "A*:  73 Landmarks",
        "A*: 122 Landmarks", "A*: 164 Landmarks","A*: Landmarks LL120"};
        public int ord;
        public types type;
        public double d;
        public int opened;
        public int closed;
        public string color;
        public string label;

        public ItemTOC(types T,int o,int c)
        {
            type = T;
            opened = o;
            closed = c;
            ord = (int)type;
            label = labels[ord];
        }
    }
}
