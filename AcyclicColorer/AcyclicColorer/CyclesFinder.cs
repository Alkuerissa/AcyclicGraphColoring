using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcyclicColorer
{
	public class CyclesFinder
	{
		private Graph Graph;
		private List<List<Vertex>> Cycles = new List<List<Vertex>>();

		public CyclesFinder(Graph graph)
		{
			Graph = graph;
		}

		public List<List<Vertex>> FindCycles() //metoda oparta o DFS
		{
			foreach (var v in Graph.Vertices)
				FindNewCycles(new List<Vertex>{v});

			return Cycles;
		}

		private void FindNewCycles(List<Vertex> path)
		{
			var n = path[path.Count - 1];

			foreach (var x in n.Edges)
			{
				if (!path.Contains(x)) //dodajemy x do ścieżki
				{
					var p = new List<Vertex>(path);
					p.Add(x);
					FindNewCycles(p);
				}
				else if ((path.Count > 2) && (x == path[0])) //znaleźliśmy cykl
				{
					var p = Normalize(path);
					var invP = Invert(p);
					if (IsNew(p) && IsNew(invP))
						Cycles.Add(p);
				}
			}
		}

		private bool AreEqual(List<Vertex> l, List<Vertex> r)
		{
			if (l.Count != r.Count)
				return false;

			bool ret = true;
			for (int i = 0; i < l.Count; ++i)	//równość ze zgodną kolejnością
				if (l[i].Index != r[i].Index)
				{
					return false;
				}
			return true;
		}

		private List<Vertex> Invert(List<Vertex> path)
		{
			var p = new List<Vertex>(path);
			p.Reverse();
			return Normalize(p);
		}

		private List<Vertex> Normalize(List<Vertex> path) //obrócenie tak, żeby cykl zaczynał się od najmniejszego wierzchołka
		{
			int minPos = 0;
			for (int i = 0; i < path.Count; ++i)
			{
				if (path[i].Index < path[minPos].Index)
					minPos = i;
			}

			return path.Skip(minPos).Concat(path.Take(minPos)).ToList();
		}

		private bool IsNew(List<Vertex> path)
		{
			foreach (var c in Cycles)
				if (AreEqual(c, path))
					return false;

			return true;
		}
	}
}
