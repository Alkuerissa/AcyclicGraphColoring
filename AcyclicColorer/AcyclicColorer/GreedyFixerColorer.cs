using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcyclicColorer
{
    public class GreedyFixerColorer : Colorer
    {
        public override string Name => "Poprawienie wyniku algorytmu zachłannego";

	    private List<Vertex> LargestFirstOrder;
	    private List<List<Vertex>> Cycles;


	    private StepResult GreedyStep()
	    {
			var v = LargestFirstOrder[stepNumber];
			v.Color = v.FindMinColor();
			return new StepResult(v);
		}


	    private StepResult FixerStep()
	    {
			if (Cycles.Count == 0)
				return new StepResult(null, false);

		    List<Vertex> cycle;
		    List<int> colors;

		    do
		    {
			    cycle = Cycles.First();
			    colors = (from v in cycle select v.Color).Distinct().ToList();
				if (colors.Count != 2)
					Cycles.RemoveAt(0);
		    } while (colors.Count != 2);

		    List<Vertex> recoloringCandidates = (from v in cycle where v.FindMinColor(colors) <= maxColor select v).ToList();
		    if (recoloringCandidates.Count == 0)
			    recoloringCandidates = cycle;

		    var cycleNumbers = (from v in recoloringCandidates select new Tuple<Vertex, int>(v, v.EvenCycles.Union(Cycles).Count()));
		    var vertex = cycleNumbers.Aggregate((agg, next) => next.Item2 > agg.Item2 ? next : agg).Item1;
		    vertex.Color = vertex.FindMinColor(colors);
		    Cycles = Cycles.Except(vertex.EvenCycles).ToList();

			return new StepResult(vertex, Cycles.Count > 0);
		}

        protected override StepResult MakeStep()
        {
	        if (stepNumber < LargestFirstOrder.Count)	
		        return GreedyStep();
            return FixerStep();
        }

	    protected override void Init()
	    {
			Graph.UpdateEvenCycles();
			Cycles = new List<List<Vertex>>(Graph.EvenCycles);
			LargestFirstOrder = new List<Vertex>(Graph.Vertices);
			LargestFirstOrder.Sort((v1, v2) => v2.Edges.Count.CompareTo(v1.Edges.Count));
		}
    }
}
