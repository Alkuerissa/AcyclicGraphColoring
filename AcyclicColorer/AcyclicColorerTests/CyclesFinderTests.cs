using System;
using System.Collections.Generic;
using System.Linq;
using AcyclicColorer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcyclicColorerTests
{
	[TestClass]
	public class CyclesFinderTests
	{
		private string Path = @"..\..\..\TestGraphs\";

		[TestMethod]
		public void CyclesFinderStackDemo()
		{
			var g = Graph.FromFile(Path + "StackTestGraph.txt");
			var cf = new CyclesFinder(g);
			var cycles = cf.FindCycles();

			var expected = new[]
			{
				"0,1,2",
				"0,1,2,3",
				"0,1,4,3",
				"0,1,4,3,2",
				"0,2,1,4,3",
				"0,2,3",
				"1,2,3,4",
				"5,6,7"
			};

			foreach (var c in cycles)
			{
				string s = c[0].Index.ToString();
				for (int i = 1; i < c.Count; ++i)
				{
					s += "," + c[i].Index;
				}
				Assert.IsTrue(expected.Any(x => x.Equals(s)));
			}
			
		}
	}
}
