using System;
using System.Collections.Generic;
using System.Linq;

namespace AcyclicColorer
{
    public class Vertex
    {
        public HashSet<Vertex> Edges { get; protected set; }
        public int Index;
        public int Color = -1;  //-1 - brak koloru
	    public List<List<Vertex>> EvenCycles = null;

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
			var usedColors = (from v in Edges select v.Color).Union(bannedColors).ToList(); //Znajdujemy zajęte kolory
			var max = Math.Max(usedColors.Max(i => (int?)i) ?? 0, 0); //Najwiekszy zajety kolor
			return Enumerable.Range(0, max + 1).Except(usedColors).Min(); //Spośród kolorów od 0 do największego zajętego + 1 wywalamy użyte kolory i bierzemy najmniejszy możliwy
		}

        public virtual Vertex Copy()
	    {
		    return new Vertex(this);
	    }
    }
}
