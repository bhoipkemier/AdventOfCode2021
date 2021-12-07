using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day6 : DayCodeBase
	{
		public override string Problem1() => Solve(80);
		public override string Problem2() => Solve(256);

		private string Solve(int days)
		{
			var data = GetData()
				.SelectMany(l => l.Split(','))
				.Select(int.Parse)
				.ToArray();
			var cache = new Dictionary<Tuple<int, int>, long>();
			return data.Select(x => Calculate(cache, x, days)).Sum().ToString();
		}

		private long Calculate(Dictionary<Tuple<int, int>, long> cache, int curAge, int days)
		{
			var cacheKey = new Tuple<int, int>(curAge, days);
			if (cache.ContainsKey(cacheKey)) return cache[cacheKey];
			var toReturn = days == 0 ? 1 :
				curAge > 0 ? Calculate(cache, curAge - 1, days - 1) : 
				             Calculate(cache, 6, days - 1) + Calculate(cache, 8, days - 1);
			return cache[cacheKey] = toReturn;
		}
	}
}
