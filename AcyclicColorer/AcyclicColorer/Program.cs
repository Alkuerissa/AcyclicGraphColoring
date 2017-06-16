using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcyclicColorer
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new Runner(args);
            runner.Run();
        }
    }
}
