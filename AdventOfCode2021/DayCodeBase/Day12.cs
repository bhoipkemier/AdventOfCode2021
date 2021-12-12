using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day12: DayCodeBase
	{
		public override string Problem1()
		{
			var map = GetDataDict();
			return Visit(map, new HashSet<string>(), "start").Count().ToString();
		}

		public override string Problem2()
		{
			var map = GetDataDict();
			var paths = new List<string>();
			foreach (var bonusRoom in map.Keys.Where(k => k != "start" && k != "end"))
				paths.AddRange(Visit(map, new HashSet<string>(), "start", string.Empty, bonusRoom));
			return paths.Distinct().Count().ToString();
		}

		private List<string> Visit(Dictionary<string, List<string>> map, HashSet<string> visited, string cave, string path = "", string bonusVisitCave = null, bool visitedBonus = false)
		{
			if (cave == "end") return new[] { path + $",{cave}" }.ToList();
			if (cave == bonusVisitCave && !visitedBonus) visitedBonus = true;
			else if (cave.ToLower() == cave) visited.Add(cave);
			var toReturn = new List<string>();
			foreach(var nextRoom in map[cave])
			{
				if (!visited.Contains(nextRoom))
					toReturn.AddRange(Visit(map, visited, nextRoom, path + $",{cave}", bonusVisitCave, visitedBonus));
			}
			visited.Remove(cave);
			return toReturn;
		}

		private Dictionary<string, List<string>> GetDataDict()
		{
			return GetData().Aggregate(new Dictionary<string, List<string>>(), (dict, line) =>
			{
				var parts = line.Split('-');
				if (!dict.ContainsKey(parts[0])) dict.Add(parts[0], new List<string>());
				if (!dict.ContainsKey(parts[1])) dict.Add(parts[1], new List<string>());
				dict[parts[0]].Add(parts[1]);
				dict[parts[1]].Add(parts[0]);
				return dict;
			});
		}
	}
}
