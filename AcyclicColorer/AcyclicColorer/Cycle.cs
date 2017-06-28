using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AcyclicColorer
{
	public class Cycle
	{
		public HashSet<Vertex> Vertices;
		private Dictionary<int, int> _colors;

		public Dictionary<int, int> Colors
		{
			get
			{
				if (!Valid)
				{
					_colors = (from v in Vertices select v.Color).Where(c => c != Vertex.None).GroupBy(v => v).Select(group => new
					{
						Color = group.Key,
						Count = group.Count()
					}).ToDictionary(c => c.Color, c => c.Count);
					Valid = true;
				}
				return _colors;
			}
			protected set { _colors = value; }
		}

		protected bool Valid = false;

		public Cycle(List<Vertex> vertices)
		{
			Vertices = new HashSet<Vertex>(vertices);
		}

		public void InvalidateColors()
		{
			Valid = false;
		}
	}
}
