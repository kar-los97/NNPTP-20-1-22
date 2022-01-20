using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INPTP_Clean02.Graph
{
    /*
     * Graph represents mathematical structure Directed Graph. Graph holds user
     * defined data objects (N, E) at each node and edge. These objects also works 
     * as a key, so they must be unique.
     * 
     * TODO: create a GIT repository, for each change create a GIT commit, 
     * cleanup code, add unit tests for all graph operations, including invalid 
     * states. In error cases Graph and other classes should throw relevant exception
     * objects. Test Dijkstra's algorithm. These classes are part of public API - add
     * basic documentation.
     * 
     * For GIT - you can use portable version from: https://git-scm.com/download/win
     */
    public class Graph<N, E> where N : IComparable<N> where E : IComparable<E>
    {
        private Dictionary<N, Node<N, E>> vertexes = new Dictionary<N, Node<N, E>>();
        private Dictionary<E, Edge<N, E>> edges = new Dictionary<E, Edge<N, E>>();

        /// <summary>
        /// Method add node to graph
        /// </summary>
        /// <param name="node">Node to add</param>
        public void AddNode(N node)
        {
            Node<N, E> n = new Node<N, E>()
            {
                data = node
            };

            vertexes.Add(node, n);
        }

        /// <summary>
        /// Method add oriented edge to graph
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void AddOrientedEdge(E edge, N source, N destination)
        {
            Edge<N, E> e = new Edge<N, E>()
            {
                data = edge
            };

            Node<N, E> a1 = GetNode(source);
            Node<N, E> a2 = GetNode(destination);

            e.source = a1;
            e.destination = a2;

            a1.neighbors.Add(e);

            edges.Add(edge, e);
        }

        /// <summary>
        /// Method add unoriented edge to graph
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public void AddUnorientedEdge(E edge, N source, N destination)
        {
            Edge<N, E> e = new Edge<N, E>()
            {
                data = edge
            };

            Node<N, E> a1 = GetNode(source);
            Node<N, E> a2 = GetNode(destination);

            e.source = a1;
            e.destination = a2;

            a1.neighbors.Add(e);
            a2.neighbors.Add(e);

            edges.Add(edge, e);
        }

        /// <summary>
        /// Method remove edge from graph
        /// </summary>
        /// <param name="edge">edge to remove</param>
        public void RemoveEdge(E edge)
        {
            Edge<N, E> e = GetEdge(edge);
            edges.Remove(edge);

            e.source.neighbors.Remove(e);
            e.destination.neighbors.Remove(e);

            e.source = e.destination = null;
        }

        /// <summary>
        /// Method remove edge from graph
        /// </summary>
        /// <param name="edge">edge to remove</param>
        public void RemoveEdge(Edge<N,E> edge)
        {
            edges.Remove(edge.data);
            edge.source = edge.destination = null;
        }

        /// <summary>
        /// Method remove node from graph
        /// </summary>
        /// <param name="node">node to remove</param>
        public void RemoveNode(N node)
        {
            Node<N, E> n = GetNode(node);

            foreach (var e in n.neighbors)
            {
                RemoveEdge(e);
                Node<N, E> o = e.source == n ? e.destination : e.source;
                o.neighbors.Remove(e);
            }
        }

        /// <summary>
        /// Method return edge
        /// </summary>
        /// <param name="edge">Edge to get</param>
        /// <returns>Edge</returns>
        internal Edge<N,E>GetEdge(E edge)
        {
            return edges[edge];
        }

        /// <summary>
        /// Method return node
        /// </summary>
        /// <param name="key">key of node</param>
        /// <returns>Node</returns>
        internal Node<N,E> GetNode(N key)
        {
            return vertexes[key];
        }

        /// <summary>
        /// Method verifies that graph have node with key
        /// </summary>
        /// <param name="key">key of node</param>
        /// <returns>true - graph have node, false - graph haven't node</returns>
        public bool HasNode(N key)
        {
            return vertexes.ContainsKey(key);
        }

        /// <summary>
        /// Method verifies that graph have edge with key
        /// </summary>
        /// <param name="key">key of edge</param>
        /// <returns>true - graph have edge, false - graph haven't edge</returns>
        public bool HasEdge(E key)
        {
            return edges.ContainsKey(key);
        }
    }

    public class Node<N, E>
    {
        public List<Edge<N, E>> neighbors;
        public N data;

        public Node()
        {
            neighbors = new List<Edge<N, E>>();
        }
    }

    public class Edge<N, E>
    {
        public Node<N, E> source;
        public Node<N, E> destination;
        public E data;

        public Node<N,E> Opposite(Node<N,E> n)
        {
            return n == source ? destination : source;
        }

    }

    /*
     * Dijsktras path finding algorithm.
     * 
     * TODO: code cleanup, create unit tests
     *
     */
    public class Dijkstra<N, E> where N : IComparable<N> where E : IComparable<E>
    {
        public Func<E, double> costExtractor;// func getWeightOfEdge(edge)
        public N start;
        public N end;
        public Graph<N, E> graph;

        private HashSet<N> finalized = new HashSet<N>();
        private Dictionary<N, double> costs = new Dictionary<N, double>();
        private Dictionary<N, N> prev = new Dictionary<N, N>();

        public List<N> FindRoute()
        {
            costs.Add(start, 0);

            while (costs.Where(p =>!finalized.Contains(p.Key)).Count() != 0)
            {
                double minimum = double.MaxValue;
                N nodeMinimum = default(N);

                foreach (var item in costs)
                {
                    if (!finalized.Contains(item.Key) && item.Value < minimum)
                    {
                        nodeMinimum = item.Key;
                        minimum = item.Value;
                    }
                }

                finalized.Add(nodeMinimum);

                foreach (var item in graph.GetNode(nodeMinimum).neighbors)
                {
                    Node<N, E> nextNode = item.Opposite(graph.GetNode(nodeMinimum));

                    if (finalized.Contains(nextNode.data))
                        continue;

                    double nodeCost = minimum + costExtractor(item.data);

                    if (costs.ContainsKey(nextNode.data))
                    {
                        double ocost = costs[nextNode.data];
                        if (nodeCost < ocost)
                        {
                            costs[nextNode.data] = nodeCost;
                            prev[nextNode.data] = nodeMinimum;
                        }
                    } else
                    {
                        costs.Add(nextNode.data, nodeCost);
                        prev[nextNode.data] = nodeMinimum;
                    }
                }
            }

            List<N> route = new List<N>();

            N t = end;
            while (t != null)
            {
                route.Add(t);
                t = prev.ContainsKey(t) ? prev[t] : default(N);
            }

            route.Reverse();
            return route;
        }
    }
}
