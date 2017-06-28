using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcyclicColorer
{
    public class ChordalColorer : Colorer
    {
        public override string Name => "Dopełnienie do grafu triangulowanego";

		private List<Vertex> LargestFirstOrder;
		private List<Cycle> Cycles;
	    private int n;
	    private Graph GraphCopy;

	    private StepResult AddEdgeStep(int step)
	    {
		    var cycle = Cycles[step];

			var cycleNumbers = (from v in cycle.Vertices select new Tuple<Vertex, int>(v, v.EvenCycles.Intersect(Cycles).Count()));
			var vertex = cycleNumbers.Aggregate((agg, next) => next.Item2 > agg.Item2 ? next : agg).Item1;
			StringBuilder text = new StringBuilder($"Dodani sąsiedzi dla wierzchołka {vertex.Index}: ");
		    foreach (var c in vertex.EvenCycles)
		    {
			    foreach (var v in c.Vertices)
			    {
				    if (!vertex.Edges.Contains(v))
				    {
						GraphCopy.AddEdge(vertex, v);
					    text.Append($"{v.Index},");
				    }
				}
		    }
		    if (Verbose)
		    {
			    if (text[text.Length - 1] == ',')
			    {
					text[text.Length - 1] = '.';
					Console.WriteLine(text.ToString());
				}	    
		    }
				
			return new StepResult(null);
		}

	    private StepResult GreedyStep(int step)
	    {
			var v = LargestFirstOrder[step];
			v.Color = v.FindMinColor();
			return new StepResult(v);
		}

		protected override StepResult MakeStep()
        {
			if (stepNumber < Cycles.Count)
				return AddEdgeStep(stepNumber);
	        if (stepNumber == Cycles.Count)
	        {
				LargestFirstOrder.Sort((v1, v2) => v2.Edges.Count.CompareTo(v1.Edges.Count));
				return new StepResult(null);
			}
			if (stepNumber < Cycles.Count + 1 + GraphCopy.Vertices.Count)
				return GreedyStep(stepNumber - Cycles.Count - 1);

	        for (int i = 0; i < GraphCopy.Vertices.Count; ++i)
		        Graph.Vertices[i].Color = GraphCopy.Vertices[i].Color;

            return new StepResult(null, false);
        }

	    protected override void Init()
	    {
			GraphCopy = new Graph(Graph);
			GraphCopy.UpdateEvenCycles();
			Cycles = new List<Cycle>(GraphCopy.EvenCycles);
		    n = Cycles.Count;
			LargestFirstOrder = new List<Vertex>(GraphCopy.Vertices);
		}
    }
}
