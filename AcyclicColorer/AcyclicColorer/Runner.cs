using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcyclicColorer
{
    public class Runner
    {
        protected List<Colorer> colorers;
        protected bool usage = false;
        protected int method = -1;
        protected Graph graph = null;

        private void PrintUsage()
        {
            Console.WriteLine($"{AppDomain.CurrentDomain.FriendlyName} path [method] [silent]");
            Console.WriteLine("path - ścieżka do pliku zawierającego graf,");
            Console.WriteLine("method - metoda kolorowania, spośród:");
            for (int i = 0; i<colorers.Count; ++i)
            {
                Console.WriteLine($"  {i+1, -3} - {colorers[i].Name.ToLower()},");
            }
            Console.WriteLine("  all - wszystkie z powyższych (domyślnie),");
            Console.WriteLine("silent - jeśli to słowo zostanie uwzględnione, program nie będzie wypisywać pośrednich wyników w trakcie kolorowania.");
            Console.WriteLine($"Na przykład: \"{AppDomain.CurrentDomain.FriendlyName} input.txt all silent\" pokoloruje graf wszystkimi dostępnymi metodami, nie wypisując pośrednich wyników.");
        }

        private void CheckUsageNeeded(string[] args)
        {
            if (args.Length < 1 || args.Length > 3 || args[0] == "--usage" || args[0] == "--help" ||
               (args.Length == 3 && (args[2] != "silent" || args[1] == "silent")))
            {
                usage = true;
            }
        }

        private void SetSilent(string[] args)
        {
            if (args.Length == 3 || (args.Length == 2 && args[1] == "silent"))
            {
                foreach (var colorer in colorers)
                {
                    colorer.Verbose = false;
                }
            }
        }

        private void SetMethod(string[] args)
        {
            if (args.Length >= 2 && args[1] != "all" && args[1] != "silent")
            {
                try
                {
                    method = int.Parse(args[1]);
                }
                catch (Exception)
                {
                    method = 0;
                }
            }
        }

        private void LoadGraph(string[] args)
        {
            try
            {
                graph = Graph.FromFile(args[0]);
                foreach (var colorer in colorers)
                    colorer.Graph = new Graph(graph);
            }
            catch (Exception)
            {
                graph = null;
            }
        }

        public Runner(string[] args)
        {
            colorers = Colorer.GetInstances();
            CheckUsageNeeded(args);
            if (!usage)
            {
                SetSilent(args);
                SetMethod(args);
                LoadGraph(args);
            }
        }

        public void Run()
        {
            if (usage)
                PrintUsage();
            else if (graph == null)
                Console.WriteLine("Nie udało się odczytać grafu z podanej ścieżki.");
            else if (method == 0)
                Console.WriteLine($"Zła podana metoda - wybierz spośród wartości od 1 do {colorers.Count} lub all");
            else
            {
                if (method == -1)
                    foreach (var colorer in colorers)
                        colorer.Run();
                else
                    colorers[method - 1].Run();
            }
        }
    }
}
