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
        
        protected override StepResult MakeStep()
        {
            return new StepResult(null, false);
        }

	    protected override void Init()
	    {
	    }
    }
}
