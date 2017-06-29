using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcyclicColorer
{
    public class GreedyCheckerColorer : Colorer
    {
        public override string Name => "Algorytm zachłanny ze sprawdzeniami w każdym kroku";

        private Graph NewGraph;

		protected override void Init()
		{
            NewGraph = new Graph();
		}

		protected override StepResult MakeStep()
		{
		    var oldV = Graph.Vertices[stepNumber];
            NewGraph.AddVertex(oldV);
            NewGraph.UpdateEvenCycles();
            var v = NewGraph.Vertices[stepNumber];
		    var banned = (from e in v.Edges select e.Color);
		    foreach (var c in v.EvenCycles)
		    {
		        if (c.Colors.Keys.Count == 2)
		            banned = banned.Concat(c.Colors.Keys);
		    }
		    v.Color = v.FindMinColor(banned.ToList());
            oldV.Color = v.Color;
            return new StepResult(oldV, stepNumber < Graph.Vertices.Count - 1);
        }


    }
}
