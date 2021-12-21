using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day14: DayCodeBase
	{

		public override string Problem1() => Calculate(10);
		public override string Problem2() => Calculate(40);

		public string Calculate(int rounds)
		{
			var data = GetData().ToList();
			var polymer = data.First();
			var pairs = Enumerable.Range(0, polymer.Length-1)
				.Select(i => polymer.Substring(i,2))
				.Aggregate(new Dictionary<string, long>(), (acc, item) => {
					acc[item] = acc.GetValueOrDefault(item, 0) + 1;
					return acc;
				});
			var mappings = GetMappings(data);

			for(var i = 0; i < rounds; ++i)
			{
				pairs = DoRound(pairs, mappings);
			}

			var pairOccurances = pairs.Aggregate(new Dictionary<char, long>(), (acc, pair) =>
			{
				acc[pair.Key[0]] = acc.GetValueOrDefault(pair.Key[0]) + pair.Value;
				acc[pair.Key[1]] = acc.GetValueOrDefault(pair.Key[1]) + pair.Value;
				return acc;
			});
			pairOccurances[polymer.First()] += 1;
			pairOccurances[polymer.Last()] += 1;
			return ((pairOccurances.Values.Max() / 2l) - (pairOccurances.Values.Min() / 2l)).ToString();

		}

		private Dictionary<string, long> DoRound(Dictionary<string, long> pairs, Dictionary<string, char> mappings)
		{
			var toReturn = new Dictionary<string, long>();
			foreach(var pair in pairs)
			{
				var newChar = mappings[pair.Key];
				toReturn[$"{pair.Key[0]}{newChar}"] = toReturn.GetValueOrDefault($"{pair.Key[0]}{newChar}", 0) + pair.Value;
				toReturn[$"{newChar}{pair.Key[1]}"] = toReturn.GetValueOrDefault($"{newChar}{pair.Key[1]}", 0) + pair.Value;
			}
			return toReturn;
		}

		private Dictionary<string, char> GetMappings(List<string> data)
		{
			return data.Where(l => l.Contains("->"))
				.Select(l => l.Split(" -> "))
				.ToDictionary(p => p[0], p => p[1][0]);
		}
	}
}
