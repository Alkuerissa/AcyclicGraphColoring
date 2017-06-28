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
	    private List<Cycle> Cycles;


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

		    Cycle cycle;
		    Dictionary<int, int> colors;

		    do
		    {
			    cycle = Cycles.First();
			    colors = cycle.Colors;
				if (colors.Count != 2)
					Cycles.RemoveAt(0);
		    } while (colors.Count != 2);

		    List<Vertex> recoloringCandidates = (from v in cycle.Vertices where v.FindMinNonBreakingColor(colors.Keys) <= maxColor select v).ToList(); //szukamy wierzchołków, które bezpiecznie można pokolorować

		    bool wasSafe = true;
		    if (recoloringCandidates.Count == 0)
		    {
			    recoloringCandidates = cycle.Vertices.ToList();
			    wasSafe = false;
		    }

		    var cycleNumbers = (from v in recoloringCandidates select new Tuple<Vertex, int>(v, v.EvenCycles.Intersect(Cycles).Count()));	
		    var vertex = cycleNumbers.Aggregate((agg, next) => next.Item2 > agg.Item2 ? next : agg).Item1; //znajdujemy wierzchołek w największej liczbie cykli do poprawienia
		    vertex.Color = wasSafe ? vertex.FindMinNonBreakingColor(colors.Keys) : maxColor + 1;
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
			Cycles = new List<Cycle>(Graph.EvenCycles);
			LargestFirstOrder = new List<Vertex>(Graph.Vertices);
			LargestFirstOrder.Sort((v1, v2) => v2.Edges.Count.CompareTo(v1.Edges.Count));
		}
    }
}
