using System;
using System.Collections.Generic;
using System.IO;

namespace graphImage
{
    class Program
    {
        

        static StreamWriter file;
        static double max;
        static int boxRange = 50;
   static     double XX;
        static int[][] tabs = new int[11][];
        

  static      SortedList<int, ItemTOC>[] tcs = new SortedList<int, ItemTOC>[10];
        static void Main(string[] args)
        {
            // StreamReader r = new StreamReader("C:/Users/L/Desktop/tuples20B.txt");

            List<String> inputs = new List<string>();
            inputs.Add("C:/Users/L/Desktop/all.txt");

            
            List<ItemTOC> datas = new List<ItemTOC>();
           
            for (int i = 0; i < 10; i++)
                tcs[i] = new SortedList<int, ItemTOC>();
            for (int i = 0; i < 10; i++)
                tabs[i] = new int[10];
            foreach (String name in inputs)
            {
                ItemTOC toc;
                double op;
                StreamReader r = new StreamReader(name);
                while (!r.EndOfStream)
                {
                    String s = r.ReadLine();
                    String[] spl = s.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries);
                    for (int i=0;i<9;i++)
                    {
                        op =double.Parse(spl[i]);
                        int o =(int) Math.Floor(op);
                        toc = new ItemTOC(ItemTOC.types.simple,o,0);
                        toc.d = i * 50;
                       datas.Add(toc);
                        tcs[i].Add(-o, toc);
                        tabs[i][toc.ord] = o;
                    }
                    s = r.ReadLine();
                    if (s == null)
                        break;
                    spl = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < 9; i++)
                    {
                        op = double.Parse(spl[i]);
                        int o = (int)Math.Floor(op);
                        toc = new ItemTOC(ItemTOC.types.aStar, o, 0);
                        toc.d = i * 50;
                      datas.Add(toc);
                        tcs[i].Add(-o, toc);
                        tabs[i][toc.ord] = o;
                    }
                    s = r.ReadLine();
                    spl = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < 9; i++)
                    {
                        op = double.Parse(spl[i]);
                        int o = (int)Math.Floor(op);
                        toc = new ItemTOC(ItemTOC.types.l50, o, 0);
                        toc.d = i * 50;
                        tcs[i].Add(-o, toc);
                        datas.Add(toc);
                        tabs[i][toc.ord] = o;
                    }
                    s = r.ReadLine();
                    spl = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < 9; i++)
                    {
                        op = double.Parse(spl[i]);
                        int o = (int)Math.Floor(op);
                        toc = new ItemTOC(ItemTOC.types.l40, o, 0);
                        toc.d = i * 50;
                        datas.Add(toc);
                        tcs[i].Add(-o, toc);
                        tabs[i][toc.ord] = o;
                    }
                    s = r.ReadLine();
                    spl = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < 9; i++)
                    {
                        op = double.Parse(spl[i]);
                        int o = (int)Math.Floor(op);
                        toc = new ItemTOC(ItemTOC.types.l30, o, 0);
                        toc.d = i * 50;
                        datas.Add(toc);
                        tcs[i].Add(-o, toc);
                        tabs[i][toc.ord] = o;
                    }
                    s = r.ReadLine();
                    spl = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < 9; i++)
                    {
                        op = double.Parse(spl[i]);
                        int o = (int)Math.Floor(op);
                        toc = new ItemTOC(ItemTOC.types.l25, o, 0);
                        toc.d = i * 50;
                        datas.Add(toc);
                        tcs[i].Add(-o, toc);
                        tabs[i][toc.ord] = o;
                    }
                    s = r.ReadLine();
                    spl = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < 9; i++)
                    {
                        op = double.Parse(spl[i]);
                        int o = (int)Math.Floor(op);
                        toc = new ItemTOC(ItemTOC.types.l20, o, 0);
                        toc.d = i * 50;
                        datas.Add(toc);
                        tcs[i].Add(-o, toc);
                        tabs[i][toc.ord] = o;
                    }


                }
            }
           

            file = new StreamWriter("C:\\Users\\L\\Desktop\\War.svg");
            GetGraph(datas);


        }

     static Dictionary<int, List<ItemTOC>> GetBoxes(List<ItemTOC> items)
        {
            Dictionary<int, List<ItemTOC>> boxes = new Dictionary<int, List<ItemTOC>>();
            int dist;
            foreach (ItemTOC item in items)
            {
                dist =(int) ((item.d)/boxRange)*boxRange;
                if (boxes.ContainsKey(dist))
                    boxes[dist].Add(item);
                else
                {
                    List<ItemTOC> distItems = new List<ItemTOC>();
                    distItems.Add(item);
                    boxes.Add(dist, distItems);
                }
            }
            return boxes;
        }


        /*

        static List<ItemTOC> GetAvgDatas(Dictionary<int, List<ItemTOC>> boxes)
        {
            List<ItemTOC> datas = new List<ItemTOC>();
            foreach (KeyValuePair<int, List<ItemTOC>> box in boxes)
            {
                int d = box.Key;
                int simOpen = 0;
                int simCl = 0;
                int simCount = 0;
                int euclOpen = 0;
                int euclCl = 0;
                int euclCount = 0;
                int lmOpen = 0;
                int lmCl = 0;
                int lmCount = 0;
                int lmOpensev = 0;
                int lmClsev = 0;
                int lmCountsev = 0;
                foreach (ItemTOC item in box.Value)
                {
                    switch (item.type)
                    {
                        case ItemTOC.types.simple:
                            simOpen = simOpen + item.opened;
                            simCl = simCl + item.closed;
                            simCount++;
                            break;
                       
                        default:
                            lmOpen = lmOpen + item.opened;
                            lmCl = lmCl + item.closed;
                            lmCount++;
                            break;
                    }
                }
            
            ItemTOC simAvg = new ItemTOC(ItemTOC.types.simple, simOpen / simCount, simCl / simCount);
            simAvg.d = d;
            datas.Add(simAvg);
            ItemTOC euclAvg = new ItemTOC(ItemTOC.types.aStarEuclid, euclOpen / euclCount, euclCl / euclCount);
            euclAvg.d = d;
            datas.Add(euclAvg);
            ItemTOC lmAvg = new ItemTOC(ItemTOC.types.aStarLMforty, lmOpen / lmCount, lmCl / lmCount);
            lmAvg.d = d;
            datas.Add(lmAvg);
                if (lmCountsev > 0)
                {
                    ItemTOC lmAvgsev = new ItemTOC(ItemTOC.types.aStarLMseventy, lmOpensev / lmCountsev, lmClsev / lmCountsev);
                    lmAvgsev.d = d;
                    datas.Add(lmAvgsev);
                }
        }
            return datas;
        }

        */

        static List<ItemTOC> GetVar(Dictionary<int, List<ItemTOC>> boxes, List<ItemTOC> avgDatas)
        {
            List<ItemTOC> var = new List<ItemTOC>();
            Dictionary<int, int> simAvg = new Dictionary<int, int>();
            Dictionary<int, int> euclAvg = new Dictionary<int, int>();
            Dictionary<int, int> lmAvg = new Dictionary<int, int>();
            Dictionary<int, int> lmAvgsev = new Dictionary<int, int>();
            foreach (ItemTOC item in avgDatas)
            {
               /* if (item.type == ItemTOC.types.simple)
                    simAvg.Add((int)item.d, item.opened);
                else if (item.type == ItemTOC.types.aStarEuclid)
                    euclAvg.Add((int)item.d, item.opened);
                else if (item.type == ItemTOC.types.aStarLMseventy)
                   lmAvgsev.Add((int)item.d, item.opened);
                else
                    lmAvg.Add((int)item.d, item.opened); */
            }
            foreach (KeyValuePair<int, List<ItemTOC>> box in boxes)
            {
                int d = box.Key;
                long simVar = 0;

                long euclVar = 0;
                long lmVar = 0;
                long lmVarsev = 0;
                int simAvgOpen = simAvg[d];
                int euclAvgOpen = euclAvg[d];
                int lmAvgOpen = lmAvg[d];
                int lmAvgOpensev = lmAvgsev[d];

                foreach (ItemTOC item in box.Value)
                {

                    switch (item.type)
                    {
                        case ItemTOC.types.simple:
                            simVar = simVar + ((item.opened - simAvgOpen) * (item.opened - simAvgOpen));
                            break;

                    /*    case ItemTOC.types.aStarEuclid:
                            euclVar = euclVar + ((item.opened - euclAvgOpen) * (item.opened - euclAvgOpen));
                            break;

                        case ItemTOC.types.aStarLMseventy:
                            lmVarsev = lmVarsev + ((item.opened - lmAvgOpensev) * (item.opened - lmAvgOpensev));
                            break;
*/
                        default:
                            lmVar = lmVar + ((item.opened - lmAvgOpen) * (item.opened - lmAvgOpen));
                            break;

                    }
                }
                
                simVar=simVar/(box.Value.Count/4);
                simVar =(long) Math.Sqrt(simVar);

                ItemTOC varsimItem = new ItemTOC(ItemTOC.types.simple,(int)  simAvgOpen, (int)simVar);
                varsimItem.d = d;
                var.Add(varsimItem);

                /*      euclVar = euclVar / (box.Value.Count / 4);
                      euclVar = (long)Math.Sqrt(euclVar);
                      ItemTOC vareuclItem = new ItemTOC(ItemTOC.types.aStarEuclid,(int)  euclAvgOpen,(int)euclVar);
                      vareuclItem.d = d;
                      var.Add(vareuclItem);

                      lmVar = lmVar / (box.Value.Count / 4);
                      lmVar = (long)Math.Sqrt(lmVar);
                      ItemTOC varlmItem = new ItemTOC(ItemTOC.types.aStarLMforty,(int) lmAvgOpen,(int)lmVar);
                      varlmItem.d = d;
                      var.Add(varlmItem);

                      lmVarsev = lmVarsev / (box.Value.Count / 4);
                      lmVarsev = (long)Math.Sqrt(lmVarsev);
                      ItemTOC varlmItemsev = new ItemTOC(ItemTOC.types.aStarLMseventy, (int)lmAvgOpensev, (int)lmVarsev);
                      varlmItemsev.d = d;
                      var.Add(varlmItemsev); */
            }
            return var;
        }

        static void GetGraph(List<ItemTOC> function)
        {
            int height = 800;
            int width = 800;// 1190;
            int tab = 30;

            
                max = 0;
                file.WriteLine
                    ("<svg width=\"{0}\" height=\"{1}\" xmlns=\"http://www.w3.org/2000/svg\" style=\"vector-effect: non-scaling-stroke;\" stroke=\"null\">", width+10, height);

            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\" " +
                "font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"67\" x=\"25\" fill-opacity=\"null\" " +
                "stroke-opacity=\"null\" stroke-width=\"0\" fill=\"#000000\">Count of visited nodes</text>");

            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\" font-family=\"Helvetica\" " +
                "font-size=\"11\" id=\"svg_1\" y=\"105\" x=\"25\" " +
                "fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" fill=\"#000000\">650 000</text>");

          
          /*  file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
               " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"584\" x=\"1152\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
               "fill=\"#000000\">Distance</text>");
*/
            foreach (ItemTOC toc in function)
                    if (toc.d > max)
                    {
                        max = toc.d;
                    };
            double unitX = 600 / max;// 1000 / max;
            max = 0;
            foreach (ItemTOC toc in function)
            {
                if (toc.opened > max)
                {
                    max = toc.opened;
                };
                if (toc.closed > max)
                {
                    max = toc.closed;
                };
            }
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx!!
            max = 650000;

            double unitY = (1500 / max) * 0.3;// (1100 / max)*0.3;

            //  foreach (ItemTOC toc in function)
            //  {
            //    AddLine(50 + tab + unitX * toc.d, 550 - unitY * toc.opened, 50 + tab + unitX * toc.d, 550, "black");
            //    AddLine(50 + tab + unitX * (toc.d + boxRange), 550 - unitY * toc.opened, 50 + tab + unitX * (toc.d + boxRange), 550, "black");
            //   }
            //   ItemTOC to = new ItemTOC(ItemTOC.types.simple,0,0);
            //   to.d = 529;
            //   function.Add(to);
            //    to = new ItemTOC(ItemTOC.types.simple, 650000, 650000);
            //    to.d = 1.5;
            //    function.Add(to);
            //        function.RemoveAt(function.Count - 1);
            ItemTOC tc=null;// = tcs[0]. ;
            
            for (int i = 0; i < 7; i++)
            {
                tabs[7][i] = (tabs[8][i] + tabs[9][i]+ tabs[7][i])/2;// ( tcs[6].Values[i].opened + tcs[7].Values[i].opened + tcs[8].Values[i].opened)/3;

            }
                for (int i = 0; i < 9; i++)
                foreach (ItemTOC toc  in tcs[i].Values)
            {
                    if (toc.type == ItemTOC.types.l20)  //// forestgreen #228B22
                        continue;
                    if (toc.type == ItemTOC.types.l30)  //// forestgreen #228B22
                        continue;
                    if (tc == null)
                        tc = toc;
                String color = "#000000";                                     // , red #8B0000
                if (toc.type == ItemTOC.types.aStar)                                       // , darkcyan #008B8B
                    color = "#8B0000";// "#000080";             
                else if (toc.type == ItemTOC.types.l25) // midnightblue #191970
                    color = "#191970";
                else if (toc.type == ItemTOC.types.l50)  //// forestgreen #228B22
                    color = "#4682B4"; //"#B22222";
            //    else if (toc.type == ItemTOC.types.l30)  //// dimgray #696969, 
            //        color = "#6495ED"; //"#B22222";
                else if (toc.type == ItemTOC.types.l40) //cornflowerblue #6495ED
                    color = "#008080"; //"#B22222";
                                       //     AddPoint(50 + tab + unitX * toc.d, 550 - unitY * toc.opened,color);
                                       //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
                                       //     AddRectangle(50 + tab + unitX * toc.d, 50 + tab + unitX * (toc.d+boxRange), 550 - unitY * toc.opened, color,550);
           //     AddFilledRectangle(50 + tab + unitX * (toc.d), 550 - unitY * (toc.opened + toc.closed), unitX * boxRange, unitY * toc.opened, color);
                    try
                    {
                        int rr = toc.ord;// tcs[i].IndexOfKey(-toc.opened);
                  int  t = tabs[((int)toc.d / 50)+1][rr];
                        int t1 = tabs[((int)toc.d / 50) ][rr];
                        if (toc.d ==0)

                            AddLine(50 + tab + unitX * (toc.d + 50), 550 - unitY * (t),
                               50 + tab + unitX * (toc.d+25), 550 - unitY * (t1),
                               color);
                        else if (toc.d < 400)

                            AddLine(50 + tab + unitX * (toc.d + 50), 550 - unitY * (t),
                               50 + tab + unitX * (toc.d), 550 - unitY * (t1),
                               color);
                        else
                        {
                             XX =10+( 50 + tab + unitX * (toc.d));
                            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
               " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"{0}\" x=\"{1}\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
               "fill=\"#000000\">{2}</text>", 550 - unitY * (t1), XX, toc.label);
                        }
                    }
                    
                    //A* Landmarks: count of landmarks 
                    catch (Exception ex)
                    { };
                    
                    
                    
                    //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            }
            AddPoint(50 + tab, 550 - unitY * 650000, "black");
            AddPoint(50 + tab+unitX*500, 550 , "black");
            AddLine(50 + tab, 550, XX, 550, "black");

            
            //  AddLine(50 + tab, 550, 1255 + tab, 550, "black");
            AddLine(50 + tab, 550, 50 + tab, 80, "black");
            XX = XX - 20;

            AddPoint(50 + tab + unitX * 100, 550, "black");
            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
               " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"{0}\" x=\"{1}\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
               "fill=\"#000000\">{2}</text>", 570, 50 + tab + unitX * 100 - 20, "100 km");
            AddPoint(50 + tab + unitX * 200, 550, "black");
            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
               " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"{0}\" x=\"{1}\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
               "fill=\"#000000\">{2}</text>", 570, 50 + tab + unitX * 200 - 20, "200 km");
            AddPoint(50 + tab + unitX * 300, 550, "black");
            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
               " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"{0}\" x=\"{1}\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
               "fill=\"#000000\">{2}</text>", 570, 50 + tab + unitX * 300 - 20, "300 km");

            AddPoint(50 + tab + unitX * 400, 550, "black");
            file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
               " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"{0}\" x=\"{1}\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
               "fill=\"#000000\">{2}</text>", 570 , XX, "400 km");
            //     AddPoint(50 + tab + unitX * 2, 585, "#000000");
            //     AddPoint(50 + tab + unitX * 2, 600, "#4169E1");
            //     AddPoint(50 + tab + unitX * 2, 615, "#B22222");
            /*       AddFilledRectangle(50 + tab + unitX * 2,  585, 50, unitY*10000,"#000000");
                   AddFilledRectangle(50 + tab + unitX * 2, 600, 50, unitY * 10000, "#000099");
                   AddFilledRectangle(50 + tab + unitX * 2,615, 50, unitY * 10000, "#45B39D");
                   AddFilledRectangle(50 + tab + unitX * 2, 615, 50, unitY * 10000, "#000033");

                   file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
                      " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"588\" x=\"150\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
                      "fill=\"#000000\">Dijkstra</text>");
                   file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
                      " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"603\" x=\"150\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
                      "fill=\"#000000\">A* Euclid</text>");
                   file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
                      " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"618\" x=\"150\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
                      "fill=\"#000000\">A* Landmarks: count of landmarks = 40</text>");
                   file.WriteLine("<text font-weight=\"bold\" stroke=\"#426373\" xml:space=\"preserve\" text-anchor=\"start\"" +
                      " font-family=\"Helvetica\" font-size=\"11\" id=\"svg_1\" y=\"618\" x=\"150\" fill-opacity=\"null\" stroke-opacity=\"null\" stroke-width=\"0\" " +
                      "fill=\"#000000\">A* Landmarks: count of landmarks = 73</text>");
                      */
            WriteLastTagAndClose();
        }

        static void AddRectangle(double x0,double x1, double y, String color,  double zerY)
        {
            AddLine(x0, y, x1, y, color);
            AddLine(x0, y, x0, zerY, color);
            AddLine(x1, y, x1, zerY, color);
        }

        static void AddFilledRectangle(double x, double y, double width, double heigth, String color)
        {
            
            file.WriteLine("<rect x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\" fill =\"{4}\" stroke=\"{4}\" stroke-width=\"2\" opacity=\"0.7\" /> ", x, y, width, heigth, color);
          //  AddLine(x, y + heigth / 2, x + width, y + heigth / 2, color);
        }

        static void AddPoint(double x, double y, String color)
        {
            file.WriteLine
                ("<ellipse opacity=\"1\" stroke=\"#000000\" style=\"vector-effect: non-scaling-stroke;\" ry=\"2.5\" rx=\"2.5\" id=\"svg_1\" cy=\"{1}\" cx=\"{0}\" stroke-width=\"1\" fill =\"{2}\"/>", x, y,color);
        }

        static void AddLine(double x1, double y1, double x2, double y2, String color)
        {

            file.WriteLine
                ("<line opacity=\"0.9\" stroke=\"{4}\" stroke-linecap=\"null\" stroke-linejoin=\"null\" id=\"svg_3\" y2=\"{0}\" x2=\"{1}\" y1=\"{2}\" x1=\"{3}\" stroke-width=\"1.8\" fill=\"{4}\"/>", y2, x2, y1, x1,color);
        }

        static void WriteLastTagAndClose()
        {
            file.WriteLine("</svg>");
            file.Close();
        }

    }
}