using System;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day2: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.Select(x => x.Split(' '))
				.ToArray();
			long depth = 0;
			long dist = 0;
			foreach(var line in data)
			{
				switch (line[0])
				{
					case "forward": dist += int.Parse(line[1]); break;
					case "up": depth -= int.Parse(line[1]); break;
					case "down": depth += int.Parse(line[1]); break;
				}
			}
			return (depth * dist)
				.ToString();
		}


		public override string Problem2()
		{
			var data = GetData()
				.Select(x => x.Split(' '))
				.ToArray();
			long aim = 0;
			long depth = 0;
			long dist = 0;
			foreach (var line in data)
			{
				switch (line[0])
				{
					case "forward": 
						dist += int.Parse(line[1]);
						depth += (aim * int.Parse(line[1]));
						break;
					case "up": aim -= int.Parse(line[1]); break;
					case "down": aim += int.Parse(line[1]); break;
				}
			}
			return (depth * dist)
				.ToString();
		}
	}
}
