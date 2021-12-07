using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day7 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.SelectMany(l => l.Split(','))
				.Select(int.Parse)
				.ToArray();
			var medianPosition = data.Count() / 2;
			var median = data.OrderBy(x => x).Skip(medianPosition-1).First();
			return data.Select(x => Math.Abs(median - x)).Sum().ToString();
		}


		public override string Problem2()
		{
			var data = GetData()
				.SelectMany(l => l.Split(','))
				.Select(int.Parse)
				.ToArray();
			return Enumerable.Range(data.Min(), data.Max() - data.Min())
				.Select(pos => new { pos, cost = data.Sum(d => Cost(pos, d)) })
				.Min(ans => ans.cost)
				.ToString();
		}

		private int Cost(int location, int destination) => (int)((Math.Abs(destination - location)) / 2.0 * (1.0 + Math.Abs(destination - location)));
	}
}
