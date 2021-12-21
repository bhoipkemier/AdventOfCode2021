using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day15: DayCodeBase
	{

		public override string Problem1()
		{
			var map = GetData();
			var pointWeights = GetPointWeights(map, 1);
			var destination = new Point(map.Length - 1, map[map.Length - 1].Length - 1);
			var totalWeights = new List<Tuple<Point, long>>(new[] { new Tuple<Point, long>(new Point(0, 0), 0) });
			return Solve(totalWeights, destination, pointWeights);
		}

		public override string Problem2()
		{
			var map = GetData();
			var pointWeights = GetPointWeights(map, 5);
			var destination = new Point(map.Length * 5 - 1, map[0].Length * 5 - 1);
			var totalWeights = new List<Tuple<Point, long>>(new[] { new Tuple<Point, long>(new Point(0, 0), 0) });
			return Solve(totalWeights, destination, pointWeights);
		}

		private string Solve(List<Tuple<Point, long>> totalWeights, Point destination, Dictionary<Point, long> pointWeights)
		{
			while (true)
			{
				totalWeights = totalWeights.OrderBy(t => t.Item2).ToList();
				var currentPos = totalWeights.First();
				totalWeights.Remove(currentPos);
				if (currentPos.Item1 == destination) return currentPos.Item2.ToString();
				var newPositions = new[] {
					new Point(currentPos.Item1.X, currentPos.Item1.Y-1),
					new Point(currentPos.Item1.X+1, currentPos.Item1.Y),
					new Point(currentPos.Item1.X, currentPos.Item1.Y+1),
					new Point(currentPos.Item1.X-1, currentPos.Item1.Y),
				};
				foreach (var newPos in newPositions)
				{
					if (pointWeights.ContainsKey(newPos))
					{
						totalWeights.Add(new Tuple<Point, long>(newPos, pointWeights[newPos] + currentPos.Item2));
						pointWeights.Remove(newPos);
					}
				}
			}
		}

		private Dictionary<Point, long> GetPointWeights(string[] map, int mapRepititions)
		{
			var toReturn = new Dictionary<Point, long>();

			for (var curMapRepX = 0; curMapRepX < mapRepititions; ++curMapRepX)
			{
				for (var curMapRepY = 0; curMapRepY < mapRepititions; ++curMapRepY)
				{
					for (var row = 0; row < map.Length; ++row)
					{
						for (var col = 0; col < map[row].Length; ++col)
						{
							var weight = long.Parse(map[row][col].ToString()) + curMapRepX + curMapRepY;
							if (weight > 9) weight -= 9;
							toReturn.Add(new Point(row + (curMapRepX * map.Length), col + (curMapRepY * map[row].Length)), weight);
						}
					}
				}
			}
			return toReturn;
		}
	}
}
