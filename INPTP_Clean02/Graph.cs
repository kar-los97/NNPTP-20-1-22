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

        public void AddNode(N node)
        {
            Node<N, E> n = new Node<N, E>()
            {
                data = node
            };

            vertexes.Add(node, n);
        }

        public void AddUniEdge(E edge, N src, N dst)
        {
            Edge<N, E> e = new Edge<N, E>()
            {
                data = edge
            };

            Node<N, E> a1 = GetNode(src);
            Node<N, E> a2 = GetNode(dst);

            e.adj1 = a1;
            e.adj2 = a2;

            a1.adj.Add(e);

            edges.Add(edge, e);
        }

        public void AddBiEdge(E edge, N src, N dst)
        {
            Edge<N, E> e = new Edge<N, E>()
            {
                data = edge
            };

            Node<N, E> a1 = GetNode(src);
            Node<N, E> a2 = GetNode(dst);

            e.adj1 = a1;
            e.adj2 = a2;

            a1.adj.Add(e);
            a2.adj.Add(e);

            edges.Add(edge, e);
        }

        public void RemoveEdge(E edge)
        {
            Edge<N, E> e = GetEdge(edge);
            edges.Remove(edge);

            e.adj1.adj.Remove(e);
            e.adj2.adj.Remove(e);

            e.adj1 = e.adj2 = null;
        }

        public void RemoveEdge(Edge<N,E> edge)
        {
            edges.Remove(edge.data);
            edge.adj1 = edge.adj2 = null;
        }

        public void RemoveNode(N node)
        {
            Node<N, E> n = GetNode(node);

            foreach (var e in n.adj)
            {
                RemoveEdge(e);
                Node<N, E> o = e.adj1 == n ? e.adj2 : e.adj1;
                o.adj.Remove(e);
            }
        }

        internal Edge<N,E>GetEdge(E edge)
        {
            return edges[edge];
        }

        internal Node<N,E> GetNode(N key)
        {
            return vertexes[key];
        }

        public bool HasNode(N key)
        {
            return vertexes.ContainsKey(key);
        }

        public bool HasEdge(E key)
        {
            return edges.ContainsKey(key);
        }
    }

    public class Node<N, E>
    {
        public List<Edge<N, E>> adj;
        public N data;

        public Node()
        {
            adj = new List<Edge<N, E>>();
        }
    }

    public class Edge<N, E>
    {
        public Node<N, E> adj1;
        public Node<N, E> adj2;
        public E data;

        public Node<N,E> Opposite(Node<N,E> n)
        {
            return n == adj1 ? adj2 : adj1;
        }

    }

    /*
     * Dijsktras path finding algorithm.
     * 
     * TODO: code cleanup, create unit tests
     *
     */
    public class Djkstra<N, E> where N : IComparable<N> where E : IComparable<E>
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
                double min = double.MaxValue;
                N nmin = default(N);

                foreach (var item in costs)
                {
                    if (!finalized.Contains(item.Key) && item.Value < min)
                    {
                        nmin = item.Key;
                        min = item.Value;
                    }
                }

                finalized.Add(nmin);

                foreach (var item in graph.GetNode(nmin).adj)
                {
                    Node<N, E> nn = item.Opposite(graph.GetNode(nmin));

                    if (finalized.Contains(nn.data))
                        continue;

                    double ncost = min + costExtractor(item.data);

                    if (costs.ContainsKey(nn.data))
                    {
                        double ocost = costs[nn.data];
                        if (ncost < ocost)
                        {
                            costs[nn.data] = ncost;
                            prev[nn.data] = nmin;
                        }
                    } else
                    {
                        costs.Add(nn.data, ncost);
                        prev[nn.data] = nmin;
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
