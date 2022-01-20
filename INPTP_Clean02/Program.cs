using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INPTP_Clean02.Graph;

namespace INPTP_Clean02
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<string, int> g = new Graph<string, int>();
            g.addNode("start");
            g.addNode("end");
            g.addNode("v1");
            g.addNode("v2");
            g.addNode("v3");

            g.AddUniEdge(100, "start", "end");

            g.AddUniEdge(5, "start", "v1");
            g.AddBiEdge(6, "v2", "v1");
            g.AddUniEdge(7, "v2", "v3");
            g.AddUniEdge(8, "v3", "end");

            g.AddUniEdge(1, "v3", "v1");
            g.AddUniEdge(2, "start", "v2");

            g.removeE(6);

            Djkstra<string, int> d = new Djkstra<string, int>()
            {
                costExtractor = i => i,
                start = "start",
                end = "end",
                graph = g
            };

            var result = d.findRoute();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
