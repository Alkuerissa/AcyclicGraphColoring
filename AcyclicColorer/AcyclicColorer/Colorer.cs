using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AcyclicColorer
{
    public abstract class Colorer
    {
        public abstract string Name { get; }
        public Graph Graph { get; set; }
        public bool Verbose { get; set; }

        protected List<Vertex> order;    
        protected int maxColor = -1;
	    protected int stepNumber = 0;

	    public bool IsCorrect { get; private set; }
	    public bool IsAcyclic { get; private set; }

        protected Colorer()
        {
            Graph = null;
            Verbose = true;
            order = new List<Vertex>();
	        IsAcyclic = false;
	        IsCorrect = false;
        }


        protected struct StepResult
        {
            public Vertex LastColored;
            public bool ContinueAlgorithm;

            public StepResult(Vertex lastColored, bool continueAlgorithm = true)
            {
                LastColored = lastColored;
                ContinueAlgorithm = continueAlgorithm;
            }
        }

        protected abstract StepResult MakeStep();
	    protected abstract void Init();

        private bool Step()
        {
            var stepResult = MakeStep();
            if (stepResult.LastColored != null)
            {
                order.Add(stepResult.LastColored);
                if (maxColor < stepResult.LastColored.Color)
                maxColor = stepResult.LastColored.Color;
                if (Verbose)
                    Console.WriteLine($"{stepResult.LastColored.Index,5}: {stepResult.LastColored.Color,-5}");  
            }
	        ++stepNumber;
            return stepResult.ContinueAlgorithm;
        }

	    private void Validate()
	    {
			ValidateCorrect();
		    ValidateAcyclic();
	    }

	    private void ValidateCorrect()
	    {
		    IsCorrect = true;
		    foreach (var v in Graph.Vertices)
		    {
			    foreach (var e in v.Edges)
			    {
				    if (e.Color == v.Color)
				    {
					    IsCorrect = false;
					    return;
				    }
			    }
		    }
	    }

	    private void ValidateAcyclic()
	    {
		    IsAcyclic = true;
		    foreach (var c in Graph.EvenCycles)
		    {
			    if (c.Colors.Count == 2)
			    {
				    IsAcyclic = false;
				    return;
			    }
		    }
	    }

        public void Run()
        {
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine(new string('=', Name.Length));
            Console.WriteLine(Name);
            Console.WriteLine(new string('=', Name.Length));

            stopwatch.Start();
			Init();
            while (Step()){}
            stopwatch.Stop();

	        Validate();
            PrintSummary(stopwatch.Elapsed);
        }

        private void PrintSummary(TimeSpan time)
        {
            Console.WriteLine(new string('-', Name.Length));
            Console.WriteLine("Ostateczne kolorowanie:");
            foreach (var vertex in Graph.Vertices)
                Console.WriteLine($"{vertex.Index,5}: {vertex.Color,-5}");
            Console.WriteLine(new string('-', Name.Length));
            Console.WriteLine($"Czas trwania obliczeń: {time}.");
            string end = maxColor + 1 == 1 ? "" : (Enumerable.Range(2, 3).Contains(maxColor + 1) ? "y" : "ów");
            Console.WriteLine($"Zostały wykorzystane {maxColor + 1} kolor{end}.");
	        string isCorrectStr = IsCorrect ? "OK." : "błąd!";
			Console.WriteLine($"Walidacja poprawności kolorowania: {isCorrectStr}");
	        string isAcyclicStr = IsAcyclic ? "OK." : "błąd!";
			Console.WriteLine($"Walidacja acykliczności kolorowania: {isAcyclicStr}");
			Console.WriteLine();
        }

        public static List<Colorer> GetInstances()
        {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.BaseType == typeof(Colorer) && t.GetConstructor(Type.EmptyTypes) != null
                select (Colorer) Activator.CreateInstance(t)).ToList();
        }
    }
}
