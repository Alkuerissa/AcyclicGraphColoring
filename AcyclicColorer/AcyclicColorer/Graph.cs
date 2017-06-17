using System;
using System.Collections.Generic;
using System.IO;
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
	        var lines = File.ReadAllLines(path);
	        if (lines.Length == 0)
				throw new IOException($"Plik {path} jest pusty.");
	        int vertexNumber = int.Parse(lines[0]);
	        for (int i=0; i<vertexNumber; ++i)
				vertices.Add(new Vertex());
	        var cohesionCheck = new bool[vertexNumber];
	        for (int i = 1; i < lines.Length; ++i)
	        {
		        var points = lines[i].Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
				if (points.Length != 2)
					throw new IOException($"Linia {i}: nieprawidłowe wejście: {lines[i]} nie jest w formacie wierzchołek-wierzchołek.");
		        var index1 = int.Parse(points[0]);
		        var index2 = int.Parse(points[1]);
				edges.Add(new Tuple<Vertex, Vertex>(vertices[index1],vertices[index2]));
		        cohesionCheck[index1] = cohesionCheck[index2] = true;
	        }
	        if (!cohesionCheck.All(x => x))
				throw new IOException("Wczytany graf nie jest spójny!");

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
