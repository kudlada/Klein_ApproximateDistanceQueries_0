using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Klein_ApproximateDistanceQueries_0
{
    public  class Node : IComparable<Node>
    {

        public long id;
        public double pot;
        public double distance = 0;
        public double distance1 = 0;
        public double distance2 = 0;
        public double distHeur = 0;
        public double distHeur2 = 0;
        public int state,state1,state2=0;
        public int countOfForward = 0;
        public bool inSeparator = false;
        public bool inside = true;
        public Dictionary<long,double> LMdist = new Dictionary<long, double>();

        public class weightedEdge
        {
            public Node neighbour;
            public string oneway;
            public int weight=0;
            public double weight2 = 0;
            public int state = 0;
            public int id;
            public weightedEdge(string onewayTag)
            {
                oneway = onewayTag;
            }
            public weightedEdge(Node n)
            {
                neighbour=n;
            }
            public void SetWeight(int w)
            {
                weight = w;
            }
        }

        public double Weight=1;
        internal Coordinates coordinates = new Coordinates(0, 0);
        public List<weightedEdge> neighbourList = new List<weightedEdge>();
        public List<weightedEdge> backList = new List<weightedEdge>();
        public List<weightedEdge> symList = new List<weightedEdge>();
        public List<weightedEdge> mList = new List<weightedEdge>();
        public Node pre,pre2;

        public Node(long Id)
        {
            id = Id;
        }

        public int CompareTo(Node x)
        {
            if (x == null)
                return 1;
            return (distance.CompareTo(x.distance));
           
        }

        public void SetData(double Lat, double Lon)
        {
            coordinates = new Coordinates(Lat, Lon);
            coordinates.utm = coordinates.ConvertToUTM(Lat, Lon);
 

        }

        public void SetData2( double maxLat, double minLon)
        {
         
            double[] maxLatc = coordinates.ConvertToUTM(maxLat, coordinates.position.longitude);
            double[] minLonc = coordinates.ConvertToUTM(coordinates.position.latitude, minLon);
            coordinates.pictureCrd1 =coordinates.GetDist(minLonc);
            coordinates.pictureCrd2 =coordinates.GetDist(maxLatc);
          
        }
        public void Resize(double resizePar)
        {
            coordinates.pictureCrd1 = coordinates.pictureCrd1 * resizePar + 65;
            coordinates.pictureCrd2 = coordinates.pictureCrd2 * resizePar + 65;
        }

        public bool IsInNeighbourList(List<Node.weightedEdge> edges, out Node.weightedEdge edge )
        {
            edge = null;
            foreach (Node.weightedEdge e in edges)
            {
                if (id==e.neighbour.id)
                {
                    edge = e;
                    return true;
                }
            }
            return false;
        }

        public double GetDistance(Node neighbour)
        {
            double latN = neighbour.coordinates.position.latitude;
            double lonN = neighbour.coordinates.position.longitude;
            double d = coordinates.GetDist(latN, lonN);
            return d;
        }
        public double GetDistanceUTM(Node neighbour)
        {
            double dist = coordinates.GetDist(neighbour.coordinates.utm);
            return dist;
        }

        public void AddNeighbour(Node neighbour, string oneWayTag)
        {
            weightedEdge edge = new weightedEdge(oneWayTag);
            edge.neighbour = neighbour;
            if (oneWayTag != "backward")
                countOfForward++;
            neighbourList.Add(edge);
        }

        public void AddBack(Node neighbour, string oneWayTag)
        {
            weightedEdge edge = new weightedEdge(oneWayTag);
            edge.neighbour = neighbour;
            
            backList.Add(edge);
        }

        public void AddSym(Node neighbour)
        {
            weightedEdge edge = new weightedEdge(neighbour);
            symList.Add(edge);
            
        }

        public void Add(Node neighbour, int w)
        {
            weightedEdge edge = new weightedEdge(neighbour);
            edge.weight = w;
            neighbourList.Add(edge);
        }

    }

   class Graph
    {
        public Dictionary<long, Node> nodes = new Dictionary<long, Node>();
     
      
      
        public double minLat;
        public double maxLat;
        public double minLon;
        public double maxLon;
        public double latDiff = 0;
        public double lonDiff = 0;
        int help = 0;
        public double resizePar = 0;
        bool hasBounds = false;
        public void GetEdges(string oneWay,string nodesDictS)
        {
            help++;
            int k = 0;
            char[] sep = new char[]
                { ',', ' ', '\t', '\r' };

            string[] splitAr = nodesDictS.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            Node node1, node2;
            long id1, id2;
            id1 = long.Parse(splitAr[k]);
            node1 = new Node(id1);

            if (!nodes.ContainsKey(id1))
                nodes.Add(id1, node1);
            node1 = nodes[id1];
           
            while (k < splitAr.Length - 1)         
            {
                k++;
                id2 = long.Parse(splitAr[k]);
                
                node2 = new Node(id2);
           
                if (!nodes.ContainsKey(id2))
                    nodes.Add(id2, node2);
                node2 = nodes[id2];
                bool newSym = true;
                foreach (Node.weightedEdge e in node1.symList)
                    if (e.neighbour.id == id2)
                        newSym = false;
                if (newSym)
                {
                    node1.AddSym(node2);
                    node2.AddSym(node1);
                }
                switch (oneWay)
                {
                    case "no":
                    
                        node1.AddNeighbour(node2,oneWay);
                        node2.AddNeighbour(node1,oneWay);
                        node1.AddBack(node2,oneWay);
                        node2.AddBack(node1,oneWay);
                        break;
                    case "yes":
                        node1.AddNeighbour(node2,oneWay);
                        node2.AddBack(node1, oneWay);
                        break;
                    case "-1":
                        node2.AddNeighbour(node1,oneWay);
                        node2.AddBack(node1, oneWay);
                        break;

                    default:
                        node1.AddNeighbour(node2,oneWay);
                        node2.AddBack(node1, oneWay);
                        break;

                }
               
                node1 = node2;
                id1 = id2;
                
            }

          
           
        }

        public void SetWeights()
        {
            
            foreach (Node n1 in nodes.Values)
                for (int i=0;i<n1.symList.Count;i++)
             
                {
                    Node.weightedEdge edge = n1.symList[i];
                    double weightD = n1.GetDistanceUTM(edge.neighbour)*10000;
                    int weight =Math.Max(1,(int)weightD);
                    edge.SetWeight(weight);

                    Node b = edge.neighbour;
             
                }
        }

        public void SetBounds(double minLatAct, double maxLatAct, double minLonAct, double maxLonAct)  //predelat
        {
            if (hasBounds)
                return;
            hasBounds = true;
            minLat = minLatAct;
            maxLat = maxLatAct;
            minLon = minLonAct;
            maxLon = maxLonAct;
            Coordinates cN = new Coordinates(maxLat, minLon); 
            cN.utm = cN.ConvertToUTM(cN.position.latitude, cN.position.longitude);
            Coordinates cS = new Coordinates(minLat, minLon);
            cS.utm = cS.ConvertToUTM(cS.position.latitude, cS.position.longitude);
            latDiff = cN.GetDist(cS.utm);
            Coordinates cE = new Coordinates(minLat, maxLon); 
            cE.utm = cE.ConvertToUTM(cE.position.latitude, cE.position.longitude);
            Coordinates cW = new Coordinates(minLat, minLon);
            cW.utm = cW.ConvertToUTM(cW.position.latitude, cW.position.longitude);
            lonDiff = cE.GetDist(cW.utm);
            resizePar=500/(latDiff);
            
        }
       
       
    }

  
        
}

