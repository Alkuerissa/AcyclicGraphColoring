using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AcyclicColorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcyclicColorerTests
{
	[TestClass]
	public class GraphTests
	{
		private string Path = @"..\..\..\TestGraphs\";

		[TestMethod]
		public void LoadingFromFileTest()
		{
			var expectedEdges = new List<Tuple<int, int>>
			{
				new Tuple<int, int>(0, 1),
				new Tuple<int, int>(0, 2),
				new Tuple<int, int>(0, 3),
				new Tuple<int, int>(1, 2),
				new Tuple<int, int>(2, 3),
				new Tuple<int, int>(1, 4),
				new Tuple<int, int>(3, 4),
				new Tuple<int, int>(4, 5),
				new Tuple<int, int>(5, 6),
				new Tuple<int, int>(6, 7),
				new Tuple<int, int>(5, 7)
			};

			var g = Graph.FromFile(Path + "StackTestGraph.txt");
			Assert.AreEqual(8, g.Vertices.Count);
			var edgeIndices = (from e in g.GetAllEdges() select new Tuple<int, int>(e.Item1.Index, e.Item2.Index)).ToList();
			Assert.AreEqual(expectedEdges.Count, edgeIndices.Count);
			foreach (var e in expectedEdges)
			{
				Assert.IsTrue(edgeIndices.Any(v => (v.Item1 == e.Item1 && v.Item2 == e.Item2) || (v.Item1 == e.Item2 && v.Item2 == e.Item1)));
			}
		}
	}
}
