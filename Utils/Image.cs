using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Klein_ApproximateDistanceQueries_0
{
    class Image
    {
        StreamWriter file;  // smazat nepotrebne metody
        int unitSizeW = 0;
        int unitSizeH = 0;
        int height = 0;
        int width = 0;
        string name;
        public Image(string nm)
        {
            width = 2800;
            height = 1700;
            name = "C:/Users/L/Desktop/" + nm + ".svg";
            file = new StreamWriter(name);
        }



        void GetColorTestImage(int count)
        {
            Node s = new Node(0);
            Node t = new Node(1);
            WriteHeader(s, t, 50, count);
            AddPoint(15, 15, "darkgrey", 10);
            AddPoint(55, 15, "red", 10);
            AddPoint(75, 15, "blue", 10);
            AddPoint(95, 15, "yellow", 10);
            AddPoint(155, 15, "black", 10);
            AddPoint(175, 15, "green", 10);
            AddPoint(15, 55, "lightblue", 10);
            WriteLastTagAndClose();

        }

        public string GetColor(string color)
        {
            string svgColor;
            switch (color)
            {
                case "darkgrey":
                    svgColor = "#7f7f7f"; break;
                case "red":
                    svgColor = "#aa1e1e"; break;
                case "blue":
                    svgColor = "#1a6184"; break;
                case "yellow":
                    svgColor = "#f9e20c"; break;

                case "black":
                    svgColor = "#000000"; break;
                case "green":
                    svgColor = "#599900"; break;

                case "lightblue":
                    svgColor = "#cce3f9"; break;

                default:
                    svgColor = "#000000"; break;
            }
            return svgColor;
        }

        public void WriteHeader(Node s, Node t, int frame, int count)
        {
            width = (int)(width * 0.6);
            height = (int)(height * 0.8);
            double dist = t.distance * 1000000;
            dist = Math.Truncate(dist);
            dist = dist / 1000;
            string ds = dist.ToString().Replace(',', '.');
            file.WriteLine("<svg width=\"{0}\" height=\"{1}\" xmlns=\"http://www.w3.org/2000/svg\" style=\"vector-effect: non-scaling-stroke;\" stroke=\"null\">", width, height);
            file.WriteLine("<g stroke = \"null\" >");
            file.WriteLine("<title stroke = \"null\" > background </title >");
            file.WriteLine("<rect stroke = \"#000000\" fill = \"#f9f9f9\" id = \"canvas_background\" width=\"{0}\" height=\"{1}\" y = \"0\" x = \"0\" />", width, height);

            file.WriteLine("<rect stroke = \"#000000\" fill = \"#f9f9f9\" id=\"svg_2\" width=\"{0}\" height=\"{1}\" y = \"50\" x = \"50\" />", (width - (2 * (frame - 1))), height - (2 * (frame - 1)));
            file.WriteLine("</g >");
            file.WriteLine("<g stroke = \"null\" >");
            file.WriteLine("<title stroke = \"null\" > Layer 1 </title > ");
            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\" font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"20\" x=\"50\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" fill=\"#000000\">OSM Intersection Of Ways.</text>");
            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\" font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"40\" x=\"50\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" fill=\"#000000\">Count of nodes:    {0}.</text>", count);

        }

        public void AddPoint(double x, double y, string color, double size)
        {
            string colorSvg = GetColor(color);
            string Xs = x.ToString().Replace(',', '.');
            string Ys = y.ToString().Replace(',', '.');
            string Ss = size.ToString().Replace(',', '.');
            string stroke = "0.8";
            if (size == 2)
                stroke = "1";
            if (size > 2)
                stroke = "1.2";
            file.WriteLine
                ("<ellipse opacity=\"100\" stroke=\"{2}\" style=\"vector-effect: non-scaling-stroke;\" ry=\"{3}\" rx=\"{3}\" id=\"svg_1\" cy=\"{0}\" cx=\"{1}\" stroke-width=\"{4}\" fill=\"{2}\"/>", Xs, Ys, colorSvg, Ss, stroke);
        }

        public void AddLine(double x1, double y1, double x2, double y2, string color, double size)
        {
            string colorSvg = GetColor(color);
            string X1s = x1.ToString().Replace(',', '.');
            string Y1s = y1.ToString().Replace(',', '.');
            string X2s = x2.ToString().Replace(',', '.');
            string Y2s = y2.ToString().Replace(',', '.');
            string Ss = size.ToString().Replace(',', '.');
            string opacity = "0.5";
            if (size > 0.6)
                opacity = "0.5";

            file.WriteLine
                ("<line opacity=\"{5}\" stroke=\"{0}\" stroke-linecap=\"null\" stroke-linejoin=\"null\" id=\"svg_3\" y2=\"{1}\" x2=\"{2}\" y1=\"{3}\" x1=\"{4}\" stroke-width=\"{5}\" fill=\"none\"/>", colorSvg, Y2s, X2s, Y1s, X1s, Ss, opacity);
        }

        public void WriteLastTagAndClose()
        {
            file.WriteLine(" </g>");
            file.WriteLine("</svg>");
            file.Close();
        }


        public void GetLMImage(Graph graph, List<Node> separators)
        {
            return;
            height = 800;//800
            width = 1700;//1700   width = (int)(graph.resizePar * graph.lonDiff);
            int frame = 50;
            Node s = separators[0];
            Node t = separators[1];
            WriteHeader(s, t, frame, graph.nodes.Count);
            List<Node> one = new List<Node>();

            foreach (Node n1 in graph.nodes.Values)
            {
                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                string color = "black";


                double x = n1.coordinates.pictureCrd1;
                double y = n1.coordinates.pictureCrd2;
                if (!n1.inside)
                    AddPoint(y, x, color, 0.8);

            }
            foreach (Node n1 in separators)
            {
                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                string color = "red";
                if (n1.distance == 1)
                    color = "green";

                double x = n1.coordinates.pictureCrd1;
                double y = n1.coordinates.pictureCrd2;

                AddPoint(y, x, color, 0.8);

            }
            WriteLastTagAndClose();
        }

        public void GetLMImage(Graph graph, List<Node> LM, Node s, Node t)
        {
            return;
            height = 800;//800
            width = 1700;//1700   width = (int)(graph.resizePar * graph.lonDiff);
            int frame = 50;
            WriteHeader(s, t, frame, graph.nodes.Count);
            List<Node> one = new List<Node>();

            foreach (Node n1 in graph.nodes.Values)
            {
                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                string color = "black";
                if (n1.state == 10)
                    color = "red";

                double x = n1.coordinates.pictureCrd1;
                double y = n1.coordinates.pictureCrd2;
                // if (!n1.inside)
                AddPoint(y, x, color, 0.8);

            }
            foreach (Node n1 in LM)
            {
                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                string color = "black";
                //  if (n1.state == 10)
                color = "red";

                double x = n1.coordinates.pictureCrd1;
                double y = n1.coordinates.pictureCrd2;
                // if (!n1.inside)
                AddPoint(y, x, color, 0.8);

            }
            WriteLastTagAndClose();
        }
        public void GetImageFile(Graph graph, List<Node> path, Node s, Node t)
        {
            // return;
            height = 800;//800
            width = 1700;//1700   width = (int)(graph.resizePar * graph.lonDiff);
            int frame = 50;
            WriteHeader(s, t, frame, graph.nodes.Count);
            List<Node> one = new List<Node>();

            foreach (Node n1 in graph.nodes.Values)
            {
                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                string color = "black";
                if (n1.state == 10)
                    color = "red";

                double x = n1.coordinates.pictureCrd1;
                double y = n1.coordinates.pictureCrd2;
                if (!n1.inside)
                    AddPoint(y, x, color, 0.8);
                //  else if (n1.state==-100)
                //      AddPoint(y, x, "blue", 5);

            }
            double size = 0.9;
            int cE = 0;
            foreach (Node n1 in graph.nodes.Values)
            {
                if (true)// (n1.inside)

                {
                    foreach (Node.weightedEdge edge in n1.neighbourList)
                    {
                        Node n2 = edge.neighbour;
                        int state = n1.state + n2.state;
                        string color;

                        switch (state)
                        {
                            case 0:
                                color = "black"; size = 0.4; break;
                            case 1:
                                color = "red"; size = 1;

                                break;
                            case 2:
                                color = "red"; cE++; break;
                            case 3:
                                color = "blue"; size = 1; break;
                            case 4:
                                color = "blue"; size = 0.7; break;

                            default:
                                color = "blue"; size = 0.7; break;

                        }
                        if (((state == 16) && (edge.weight > 0)) | (color == "red"))
                            AddLine(n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2,
                        n2.coordinates.pictureCrd1, n2.coordinates.pictureCrd2, color, size);
                        else if (((state == 36) && (edge.weight > 0)) | (color == "red"))
                            AddLine(n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2,
                        n2.coordinates.pictureCrd1, n2.coordinates.pictureCrd2, "red", size);
                    }
                }

            }

            int count = path.Count;
            for (int i = 0; i < count - 1; i++)
            {
                Node n1 = path[i];
                Node n2 = path[i + 1];

                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                n2.SetData2(graph.maxLat, graph.minLon);
                n2.Resize(graph.resizePar);
                AddLine(n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2,
                             n2.coordinates.pictureCrd1, n2.coordinates.pictureCrd2, "yellow", 2.3);
            }

            //  AddPoint(s.coordinates.pictureCrd2, s.coordinates.pictureCrd1, "green", 5);
            //  AddPoint(t.coordinates.pictureCrd2, t.coordinates.pictureCrd1, "red", 5);
            WriteLastTagAndClose();
        }

        public void Get2(Graph graph, List<Node> path, Node s, Node t)
        {
            int frame = 50;
            file = new StreamWriter("C:/Users/L/Desktop/minGraph/3/finBi.svg");
            WriteHeader(s, t, 50, graph.nodes.Count);
            List<Node> one = new List<Node>();

            foreach (Node n1 in graph.nodes.Values)
            {

                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                string color = "black";
                if (n1.state == 10)
                    color = "red";

                double x = n1.coordinates.pictureCrd1;
                double y = n1.coordinates.pictureCrd2;

            }

            foreach (Node n1 in graph.nodes.Values)
            {

                foreach (Node.weightedEdge edge in n1.neighbourList)
                {
                    double size = 0.9;

                    Node n2 = edge.neighbour;
                    int state = n1.state1 + n1.state2;
                    string color;

                    switch (state)
                    {
                        case 1:
                            if (n1.distance1 < n1.distance2)
                                color = "blue";
                            else
                                color = "yellow";
                            break;
                        case 2:
                            if (n1.distance1 < n1.distance2)
                                color = "blue";
                            else
                                color = "yellow"; break;
                        case 3:
                            if (n1.distance1 < n1.distance2)
                                color = "blue";
                            else
                                color = "yellow"; break;
                        case 4:
                            if (n1.distance1 < n1.distance2)
                                color = "blue";
                            else
                                color = "yellow"; ; break;
                        case 10:
                            color = "red"; break;
                        case 200:
                            color = "red";
                            size = 0.8;
                            break;



                        default:
                            color = "black"; size = 0.3; break;

                    }
                    n2.SetData2(graph.maxLat, graph.minLon);
                    n2.Resize(graph.resizePar);
                    if ((n1.coordinates.pictureCrd1 > frame && n1.coordinates.pictureCrd1 < (width - frame) &&
                                (n1.coordinates.pictureCrd2 > frame && n1.coordinates.pictureCrd2 < (height - frame) &&
                                (n2.coordinates.pictureCrd1 > frame && n2.coordinates.pictureCrd1 < (width - frame) &&
                                (n2.coordinates.pictureCrd2 > frame && n2.coordinates.pictureCrd2 < (height - frame))))))

                        AddLine(n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2,
                             n2.coordinates.pictureCrd1, n2.coordinates.pictureCrd2, color, size);
                }

            }

            int count = path.Count;

            for (int i = 0; i < count - 1; i++)
            {
                Node n1 = path[i];
                Node n2 = path[i + 1];

                n1.SetData2(graph.maxLat, graph.minLon);
                n1.Resize(graph.resizePar);
                n2.SetData2(graph.maxLat, graph.minLon);
                n2.Resize(graph.resizePar);
                AddLine(n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2,
                             n2.coordinates.pictureCrd1, n2.coordinates.pictureCrd2, "yellow", 2);
            }
            AddPoint(s.coordinates.pictureCrd2, s.coordinates.pictureCrd1, "green", 3);
            AddPoint(t.coordinates.pictureCrd2, t.coordinates.pictureCrd1, "red", 3);
            WriteLastTagAndClose();
        }
        ///////////////////////////////
        public void GetPlanarGraphBNF(Graph graph,
            List<PlanarNode> plNodes, Dictionary<int, PlanarEdge> plE)
        {
            Dictionary<long, Node> nodes = graph.nodes;
            Node s = nodes[plNodes.First().nid];
            file = new StreamWriter(name);
            WriteHeader(s, s, 50, plNodes.Count);
            List<Node> one = new List<Node>();
            foreach (Node n in nodes.Values)
            {
                //   n.SetData2(graph.maxLat, graph.minLon);
                n.Resize(10);
                // if (!n.inside)
                //     AddPoint(n.coordinates.pictureCrd2, n.coordinates.pictureCrd1, "black", 0.3);
            }
            foreach (PlanarEdge e in plE.Values)
            {
                PlanarNode p0 = (PlanarNode)e.neighboursAdjEdges[0];
                PlanarNode p1 = (PlanarNode)e.neighboursAdjEdges[1];

                Node n0 = nodes[p0.nid];
                Node n1 = nodes[p1.nid];
                String color = "black";
                //             if (pe.parent != null)
                AddLine(n0.coordinates.pictureCrd1, n0.coordinates.pictureCrd2,
                             n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2, color, 2);


            }
            AddPoint(s.coordinates.pictureCrd2, s.coordinates.pictureCrd1, "red", 3.0);

            WriteLastTagAndClose();
        }


        public void GetPlanarGraphBNFLevels(Graph graph,
            List<PlanarNode> plNodes, Dictionary<int, PlanarEdge> plE,
            Dictionary<int, List<long>> levels, int lev, int lev0, int lev1, Node s)
        {
            Dictionary<long, Node> nodes = graph.nodes;
            //Node s = nodes[plNodes.First().nid];
            file = new StreamWriter("C:/Users/L/Desktop/bnf_nur_1.svg");
            WriteHeader(s, s, 50, plNodes.Count);
            List<Node> one = new List<Node>();
            foreach (Node n in nodes.Values)
            {
                //   n.SetData2(graph.maxLat, graph.minLon);
                n.Resize(10);
                // if (!n.inside)
                //     AddPoint(n.coordinates.pictureCrd2, n.coordinates.pictureCrd1, "black", 0.3);
            }
            foreach (List<long> level in levels.Values)
                foreach (long nid in level)
                // foreach (PlanarNode p0 in plNodes)
                {
                    PlanarNode p0 = plNodes.Where(x => x.nid == nid).First();// plNodes[nid];
                    String color = "black";
                    if (p0.dist >= lev1)
                        color = "yellow";
                    else if (p0.dist >= lev0)
                        color = "red";
                    else if (p0.dist > lev)
                        color = "blue";
                    Node n = nodes[p0.nid];
                    AddPoint(n.coordinates.pictureCrd2, n.coordinates.pictureCrd1, color, 2);
                    Node n41 = nodes[41];
                    AddPoint(n41.coordinates.pictureCrd2, n41.coordinates.pictureCrd1, color, 2);
                    foreach (int eid in p0.edgesIds)
                    {

                        try
                        {
                            PlanarEdge pe = plE[eid];
                            Node n1 = nodes[pe.GetNeigh(p0).nid];

                            if (levels[pe.GetNeigh(p0).dist].Contains(pe.GetNeigh(p0).nid))
                                //   if (pe.parent!=null)
                                //  if (pe.state==2)
                                if (pe.inTree)
                                    AddLine(n.coordinates.pictureCrd1, n.coordinates.pictureCrd2,
                                                n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2, color, 2);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            PlanarNode sp = plNodes.Where(x => x.nid == s.id).First();
            foreach (int eid in sp.edgesIds)
            {
                PlanarEdge e = plE[eid];
                PlanarEdge pe = plE[eid];
                Node n = nodes[pe.GetNeigh(sp).nid];
                Node n1 = nodes[sp.nid];
                //       if (pe.parent != null)
                //          if (pe.state >= 0)
                AddLine(n.coordinates.pictureCrd1, n.coordinates.pictureCrd2,
                           n1.coordinates.pictureCrd1, n1.coordinates.pictureCrd2, "red", 2);


            }
            //       AddPoint(s.coordinates.pictureCrd2, s.coordinates.pictureCrd1, "red", 3.0);
            AddPoint(graph.nodes[sp.nid].coordinates.pictureCrd2, graph.nodes[sp.nid].coordinates.pictureCrd1, "red", 3.0);
            WriteLastTagAndClose();
        }
        /// 
        double GetPointCoordinates(double lat, double lon, out double y, Graph graph)
        {
            double distanceH = graph.maxLat - lat;
            double distanceW = lon - graph.minLon;
            double x = (distanceW * unitSizeW);


            y = (distanceH * unitSizeH) + 5000;

            return x;
        }

        void CountUnitSize(Graph graph)
        {
            double distX, distY;
            distX = (graph.maxLon) - (graph.minLon);
            distY = (graph.maxLat) - (graph.minLat);
            int pbWidth = 500;
            unitSizeW = (int)((pbWidth) / (distX));
            unitSizeH = (int)((500) / (distY)) * 2;
            int pbHeigth = (int)(distY * unitSizeH);
        }
    }
}