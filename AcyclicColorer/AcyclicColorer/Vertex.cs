using System.Collections.Generic;

namespace AcyclicColorer
{
    public class Vertex
    {
        public HashSet<Vertex> ExitingEdges { get; protected set; }
        public HashSet<Vertex> EnteringEdges { get; protected set; }
        public int Index;
        public int Color = -1;  //-1 - brak koloru

        public Vertex()
        {
            ExitingEdges = new HashSet<Vertex>();
            EnteringEdges = new HashSet<Vertex>();
        }

        public Vertex(Vertex vertex)
        {
            ExitingEdges = new HashSet<Vertex>();
            EnteringEdges = new HashSet<Vertex>();
            Index = vertex.Index;
            Color = vertex.Color;
        }

        public void AddEdge(Vertex to)
        {
            ExitingEdges.Add(to);
            to.EnteringEdges.Add(this);
        }

        public void RemoveEdge(Vertex to)
        {
            ExitingEdges.Remove(to);
            to.EnteringEdges.Remove(this);
        }

        public bool HasEnteringEdges()
        {
            return EnteringEdges.Count > 0;
        }

        public virtual Vertex Copy()
	    {
		    return new Vertex(this);
	    }
    }
}
