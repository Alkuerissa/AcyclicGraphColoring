using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace AcyclicColorer
{
    public class Graph
    {
        public List<Vertex> Vertices { get; protected set; }
        protected Dictionary<int, Vertex> indexVerticesMapping;
        protected int maxIndex;

	    protected bool AreCyclesValid = false;

	    private List<Cycle> _evenCycles;

	    public List<Cycle> EvenCycles
	    {
		    get
		    {
			    if (!AreCyclesValid)
					UpdateEvenCycles();
			    return _evenCycles;
		    }
		    protected set { _evenCycles = value; }
	    }

        public Graph()
        {
            Vertices = new List<Vertex>();
            maxIndex = 0;
            indexVerticesMapping = new Dictionary<int, Vertex>();
            EvenCycles = null;
        }

	    public Graph(IEnumerable<Vertex> startVertices, List<Tuple<Vertex, Vertex>> edges)
        {        
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
			EvenCycles = null;
		}

	    public void AddVertex(Vertex vertex)
	    {
		    var v = vertex.Copy();
		    Vertices.Add(v);
	        indexVerticesMapping[v.Index] = v;

            foreach (var e in vertex.Edges)
		    {
                if (indexVerticesMapping.ContainsKey(e.Index))
				    AddEdge(indexVerticesMapping[v.Index],
						    indexVerticesMapping[e.Index]);
			}

		    AreCyclesValid = false;
	    }

        public void AddEdge(Vertex from, Vertex to)
        {
            if (to.Edges.Contains(from))
                return;
            from.AddEdge(to);
            to.AddEdge(from);
        }

        public void RemoveEdge(Vertex from, Vertex to)
        {
            from.RemoveEdge(to);
            to.RemoveEdge(from);
        }

        public void ResetColors()
        {
            foreach (var vertex in Vertices)
            {
                vertex.Color = -1;
            }
        }

	    public void UpdateEvenCycles()
	    {
			AreCyclesValid = true;
			FindEvenCycles();

		    foreach (var vertex in Vertices)
		    {
			    if (vertex.EvenCycles != null)
					vertex.EvenCycles.Clear();
				else
					vertex.EvenCycles = new List<Cycle>();
		    }
			
		    foreach (var cycle in EvenCycles)
		    {
			    foreach (var vertex in cycle.Vertices)
			    {
				    vertex.EvenCycles.Add(cycle);
			    }
		    }	    
	    }

	    private void FindEvenCycles()
	    {
			var finder = new CyclesFinder(this);
		    EvenCycles = (from c in finder.FindCycles() where (c.Count%2 == 0) select new Cycle(c)).ToList();
	    }

		public IEnumerable<Tuple<Vertex, Vertex>> GetAllEdges()
		{
			foreach (var v in Vertices)
				foreach (var e in v.Edges)
					if (e.Index > v.Index)  //graf jest nieskierowany, a chcemy zwracać tylko raz każdą krawędź
						yield return new Tuple<Vertex, Vertex>(v, e);
		}

		public Graph(Graph graph)
        {
            Vertices = new List<Vertex>();
            maxIndex = graph.maxIndex;
            foreach (var vertex in graph.Vertices)
            {
                Vertices.Add(vertex.Copy());
            }
            CreateVerticesDictionary();
            foreach (var vertex in graph.Vertices)
            {
                foreach (var edgeEnd in vertex.Edges)
                {
                    AddEdge(indexVerticesMapping[vertex.Index],
                            indexVerticesMapping[edgeEnd.Index]);
                }
            }
	        if (graph.EvenCycles != null)
	        {
		        EvenCycles = new List<Cycle>();
		        foreach (var cycle in graph.EvenCycles)
		        {
			        var c = new List<Vertex>();
			        foreach (var v in cycle.Vertices)
				        c.Add(indexVerticesMapping[v.Index]);
			        EvenCycles.Add(new Cycle(c));
		        }
	        }
	        else
		        EvenCycles = null;
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
	        for (int i = 1; i < lines.Length; ++i)
	        {
		        var points = lines[i].Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
				if (points.Length != 2)
					throw new IOException($"Linia {i}: nieprawidłowe wejście: {lines[i]} nie jest w formacie wierzchołek-wierzchołek.");
		        var index1 = int.Parse(points[0]);
		        var index2 = int.Parse(points[1]);
				edges.Add(new Tuple<Vertex, Vertex>(vertices[index1],vertices[index2]));
	        }
            var graph = new Graph(vertices, edges);
            if (!graph.IsCoherent())
				throw new IOException("Wczytany graf nie jest spójny!");
            return graph;
        }

        protected bool IsCoherent()
        {
            if (Vertices.Count == 0)
                return true;
            var visited = new HashSet<int>();
            DFS(Vertices.First(), visited);
            return visited.Count == Vertices.Count;
        }

        protected void DFS(Vertex current, HashSet<int> visited)
        {
            visited.Add(current.Index);
            foreach (var neighbor in current.Edges)
            {
                if (!visited.Contains(neighbor.Index))
                    DFS(neighbor, visited);
            }
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
