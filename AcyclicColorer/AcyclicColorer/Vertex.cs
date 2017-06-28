using System;
using System.Collections.Generic;
using System.Linq;

namespace AcyclicColorer
{
    public class Vertex
    {
        public HashSet<Vertex> Edges { get; protected set; }
        public int Index;

	    public const int None = -1;
	    private int _color = None; //-1 - brak koloru
	    public int Color
	    {
		    get
		    {
			    return _color;
		    }
		    set
		    {
			    _color = value;
			    if (EvenCycles != null)
			    {
				    foreach (var c in EvenCycles)
				    {
					    c.InvalidateColors();
				    }
			    }
		    }
	    }  
	    public List<Cycle> EvenCycles = null;

        public Vertex()
        {
            Edges = new HashSet<Vertex>();
        }

        public Vertex(Vertex vertex)
        {
            Edges = new HashSet<Vertex>();
            Index = vertex.Index;
            Color = vertex.Color;
        }

        public void AddEdge(Vertex to)
        {
            to.Edges.Add(this);
        }

        public void RemoveEdge(Vertex to)
        {
            to.Edges.Remove(this);
        }

		public bool HasEnteringEdges()
		{
			return Edges.Count > 0;
		}

	    public int FindMinColor(IEnumerable<int> bannedColors = null)
	    {
			if (bannedColors == null)
				bannedColors = new int[] { };
			var usedColors = (from v in Edges select v.Color).Concat(bannedColors).ToList(); //Znajdujemy zajęte kolory
			usedColors.Add(Color);	//Nie chcemy tego koloru, który już mamy
		    usedColors = usedColors.Distinct().ToList();
			var max = Math.Max(usedColors.Max(i => (int?)i) ?? -1, -1); //Najwiekszy zajety kolor
			return Enumerable.Range(0, max + 2).Except(usedColors).Min(); //Spośród kolorów od 0 do największego zajętego + 1 wywalamy użyte kolory i bierzemy najmniejszy możliwy
		}

	    public int FindMinNonBreakingColor(IEnumerable<int> bannedColors = null)
	    {
			if (bannedColors == null)
				bannedColors = new int[] { };
			var acyclicBans = new List<int>();
			var t = new List<Vertex> {this};
		    foreach (var c in EvenCycles)
		    {
			    var s = c.Colors;
				if (s.Count == 3 && s[Color] == 1)		//jeśli zmiana koloru obecnego wierzchołka sprawiłaby, że zepsujemy dobry cykl, zakazujemy kolorów z tego cyklu, żeby otrzymać nowy 3-kolorowy cykl
					acyclicBans.AddRange(s.Keys);
		    }

			return FindMinColor(bannedColors.Concat(acyclicBans));
	    }

        public virtual Vertex Copy()
	    {
		    return new Vertex(this);
	    }
    }
}
