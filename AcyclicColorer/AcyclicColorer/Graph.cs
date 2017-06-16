using System;
using System.Collections.Generic;
using System.Linq;

namespace AcyclicColorer
{
    public class Graph
    {
        public List<Vertex> Vertices { get; protected set; }
        public bool Directed { get; protected set; }

        protected Dictionary<int, Vertex> indexVerticesMapping;
        protected int maxIndex;
        

        public Graph(IEnumerable<Vertex> startVertices, List<Tuple<Vertex, Vertex>> edges, bool directed=false)
        {
            Directed = directed;
            Vertices = new List<Vertex>();
            maxIndex = 0;
            foreach (var vertex in startVertices)
            {
                Vertices.Add(vertex);
                vertex.Index = maxIndex++;
            }
            CreateVerticesDictionary();

            foreach (var edge in edges)
            {
                AddEdge(edge.Item1, edge.Item2);
            }
        }

        public void AddEdge(Vertex from, Vertex to)
        {
            from.AddEdge(to);
            if (!Directed)
                to.AddEdge(from);
        }

        public void RemoveEdge(Vertex from, Vertex to)
        {
            from.RemoveEdge(to);
            if (!Directed)
                to.RemoveEdge(from);
        }

        public void ResetColors()
        {
            foreach (var vertex in Vertices)
            {
                vertex.Color = -1;
            }
        }

        public Graph(Graph graph)
        {
            Directed = graph.Directed;
            Vertices = new List<Vertex>();
            maxIndex = graph.maxIndex;
            foreach (var vertex in graph.Vertices)
            {
                Vertices.Add(vertex.Copy());
            }
            CreateVerticesDictionary();
            foreach (var vertex in graph.Vertices)
            {
                foreach (var edgeEnd in vertex.ExitingEdges)
                {
                    AddEdge(indexVerticesMapping[vertex.Index],
                            indexVerticesMapping[edgeEnd.Index]);
                }
            }
        }

        public static Graph FromFile(string path)
        {
            var vertices = new List<Vertex>();
            var edges = new List<Tuple<Vertex, Vertex>>();
            return new Graph(vertices, edges);
        }

        protected void CreateVerticesDictionary()
        {
            indexVerticesMapping = new Dictionary<int, Vertex>();
            foreach (var vertice in Vertices)
            {
                indexVerticesMapping[vertice.Index] = vertice;
            }

        }
        
    }
}
