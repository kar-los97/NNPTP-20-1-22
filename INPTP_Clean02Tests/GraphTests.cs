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
    public class GraphTests
    {
        [TestMethod()]
        public void EmptyGraphShouldNotHaveAnyNodes()
        {
            Graph<int, int> graph = new Graph<int, int>();

            // do nothing

            Assert.IsFalse(graph.HasNode(10));
        }

        [TestMethod()]
        public void AddNodeTest()
        {
            Graph<int, int> graph = new Graph<int, int>();

            graph.addNode(10);

            Assert.IsTrue(graph.HasNode(10));
        }

        [TestMethod()]
        public void AddOrientedEdgeTest()
        {
            Graph<int, int> graph = new Graph<int, int>();

            graph.AddNode(10);
            graph.AddNode(20);
            graph.AddOrientedEdge(100, 10, 20);

            Assert.IsTrue(graph.HasEdge(100));
        }

        [TestMethod()]
        public void AddUniorientedEdgeTest()
        {
            Graph<int, int> graph = new Graph<int, int>();

            graph.AddNode(10);
            graph.AddNode(20);
            graph.AddUnorientedEdge(100, 10, 20);

            Assert.IsTrue(graph.HasEdge(100));
        }

        [TestMethod()]
        public void RemoveEdgeTest()
        {
            Graph<int, int> graph = new Graph<int, int>();

            graph.AddNode(10);
            graph.AddNode(20);
            graph.AddOrientedEdge(100, 10, 20);

            Assert.IsTrue(graph.HasEdge(100));

            graph.RemoveEdge(100);

            Assert.IsFalse(graph.HaseEdge(100));
        }

        [TestMethod()]
        public void RemoveNodeTest()
        {
            Graph<int, int> graph = new Graph<int, int>();

            graph.AddNode(10);
            graph.AddNode(20);

            graph.IsTrue(graph.HasNode(10));

            graph.removeNode(10);

            graph.IsFalse(graph.HasNode(10));
        }
    }
}