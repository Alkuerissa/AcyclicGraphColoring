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

        public GreedyFixerColorer()
        {
        }

        
        protected override StepResult MakeStep()
        {
            return new StepResult(null, false);
        }
    }
}
