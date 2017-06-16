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

        public GreedyCheckerColorer()
        {
        }

        
        protected override StepResult MakeStep()
        {
            return new StepResult(null, false);
        }
    }
}
