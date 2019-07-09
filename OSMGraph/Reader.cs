using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Klein_ApproximateDistanceQueries_0
{
    class Reader
    {
        StreamReader file;
        double minLat = 50.10;          // presne behem nacteni s.
        double maxLat = 50.10;          // todo: regex misto "   "
        double minLon = 14.40;
        double maxLon = 14.40;

        public Graph graph = new Graph();

        public Reader() { }


        public void ReadFile(string inputFileName, bool inside)
        {

            try
            {
                file = new StreamReader(inputFileName);
            }
            catch (IOException)
            {
                return;

            }
            string line;
            const string way = "      \"type\": \"way\",";
            long id;
            StringBuilder SBid;
            StringBuilder SBnodes;
            while (!file.EndOfStream)
            {

                line = file.ReadLine();
                if ((line == way) | (line == "  \"type\": \"way\","))
                {
                    SBid = new StringBuilder();
                    char c = ' ';
                    while (c != ':')
                        c = (char)file.Read();
                    c = (char)file.Read();
                    c = (char)file.Read();
                    while (c != ',')
                    {
                        SBid.Append(c);
                        c = (char)file.Read();

                    }
                    id = long.Parse(SBid.ToString());


                    line = file.ReadLine();
                    line = file.ReadLine();
                    line = file.ReadLine();
                    SBnodes = new StringBuilder();
                    while ((line != "      ],") && (line != "      ]")
                        && (line != "  ],"))
                    {

                        SBnodes.Append(line);
                        line = file.ReadLine();
                    }
                    string oneWay = "no";
                    bool found = false;
                    while ((line != "      }") && (line != "  }")
                        && (line != "  }") && (!found) && (!file.EndOfStream))
                    {
                        line = file.ReadLine();
                        string[] split = line.Split('\"');
                        if (split.Length > 2 && split[1] == "oneway")
                        {
                            oneWay = split[3];
                            found = true;
                        }

                    }
                    graph.GetEdges(oneWay, SBnodes.ToString());

                }
                else if ((line == "      \"type\": \"node\",")
                        | (line == "  \"type\": \"node\","))
                {

                    char[] sep = new char[] { ' ', ',' };
                    line = file.ReadLine();
                    string[] words = line.Split(
                        sep, StringSplitOptions.RemoveEmptyEntries);
                    long idNode = long.Parse(words[1]);

                    line = file.ReadLine();
                    words = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                    double lat = double.Parse(words[1]);
                    if (lat < minLat)
                        minLat = lat;
                    if (lat > maxLat)
                        maxLat = lat;
                    line = file.ReadLine();
                    words = line.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                    double lon = double.Parse(words[1]);
                    if (lon < minLon)
                        minLon = lon;
                    if (lon > maxLon)
                        maxLon = lon;

                    Node node = new Node(idNode);
                    Node nodeOut;

                    node.SetData(lat, lon);
                    if (!graph.nodes.TryGetValue(idNode, out nodeOut))
                    {
                        graph.nodes.Add(idNode, node);
                        node.inside = inside;

                    }
                    else
                    {
                        nodeOut.SetData(lat, lon);
                        node.inside = inside;

                    }
                }


            }

            List<long> deletedC = new List<long>();


            graph.SetBounds(minLat, maxLat, minLon, maxLon);

        }



    }
}
