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
            Graph<string, int> graph = new Graph<string, int>();
            graph.AddNode("start");
            graph.AddNode("end");
            graph.AddNode("v1");
            graph.AddNode("v2");
            graph.AddNode("v3");

            graph.AddOrientedEdge(100, "start", "end");

            graph.AddOrientedEdge(5, "start", "v1");
            graph.AddUnorientedEdge(6, "v2", "v1");
            graph.AddOrientedEdge(7, "v2", "v3");
            graph.AddOrientedEdge(8, "v3", "end");

            graph.AddOrientedEdge(1, "v3", "v1");
            graph.AddOrientedEdge(2, "start", "v2");

            graph.RemoveEdge(6);

            Dijkstra<string, int> dijkstra = new Dijkstra<string, int>()
            {
                costExtractor = i => i,
                start = "start",
                end = "end",
                graph = graph
            };

            var result = dijkstra.FindRoute();
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
