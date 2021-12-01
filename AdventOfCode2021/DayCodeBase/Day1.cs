using System;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day1: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.Select(int.Parse)
				.ToArray();
			return Enumerable.Range(1, data.Length - 1)
				.Where(i => data[i-1] < data[i])
				.Count()
				.ToString();
		}


		public override string Problem2()
		{
			var data = GetData()
				.Select(int.Parse)
				.ToArray();
			var windows = Enumerable.Range(2, data.Length - 2)
				.Select(i => data[i - 2] + data[i - 1] + data[i])
				.ToArray();

			return Enumerable.Range(1, windows.Length - 1)
				.Where(i => windows[i - 1] < windows[i])
				.Count()
				.ToString();
		}
	}
}
