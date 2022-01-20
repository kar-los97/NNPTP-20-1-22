using Microsoft.VisualStudio.TestTools.UnitTesting;
using INPTP_Clean02.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPTP_Clean02.Graph.Tests
{
    [TestClass()]
    public class DijkstraTest
    {
        [TestMethod()]
        public void FindRouteTest()
        {
            Graph<int, int> graph = new Graph<int, int>();
            graph.AddNode(0);
            graph.AddNode(1);
            graph.AddNode(2);


            graph.AddUnoirentedEdge(10,0,1);
            graph.AddUnorientedEdge(10,1,2);

            Dijkstra<int, int> dijkstra = new Dijkstra<int, int>()
            {
                constExtractor = i => i,
                start = 0,
                end = 2,
                graph = graph
            };

            List<int> result = dijkstra.FindRoute();
        }
    }
}