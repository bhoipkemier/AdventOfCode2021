using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day13: DayCodeBase
	{
		public override string Problem1()
		{
			var points = GetPoints();
			var folds = GetFolds();
			DoFold(folds.First(), points);
			return points.Count().ToString();
		}
		public override string Problem2()
		{
			var points = GetPoints();
			var folds = GetFolds();
			foreach(var fold in folds)
			{
				DoFold(fold, points);
			}
			return PointsToString(points);
		}

		private string PointsToString(HashSet<Point> points)
		{
			var toReturn = "\n";
			for(var y = points.Min(p => p.Y); y <= points.Max(p => p.Y); ++y)
			{
				for (var x = points.Min(p => p.X); x <= points.Max(p => p.X); ++x)
				{
					toReturn += points.Contains(new Point(x, y)) ? "#": " ";
				}
				toReturn += "\n";
			}
			return toReturn;
		}

		private void DoFold(Tuple<string, int> fold, HashSet<Point> points)
		{
			if (fold.Item1.Contains('x'))
			{
				var pointsToRemove = points.Where(p => p.X > fold.Item2).ToList();
				foreach (var toRemove in pointsToRemove)
				{
					points.Remove(toRemove);
					points.Add(new Point(fold.Item2 * 2 - toRemove.X, toRemove.Y));
				}
			}
			else
			{
				var pointsToRemove = points.Where(p => p.Y > fold.Item2).ToList();
				foreach (var toRemove in pointsToRemove)
				{
					points.Remove(toRemove);
					points.Add(new Point(toRemove.X, fold.Item2 * 2 - toRemove.Y));
				}
			}
		}

		private List<Tuple<string, int>> GetFolds()
		{
			return GetData()
				.Where(l => l.Contains('='))
				.Select(l =>
				{
					var parts = l.Split('=');
					return new Tuple<string,int>(parts[0], int.Parse(parts[1]));
				})
				.ToList();
		}

		private HashSet<Point> GetPoints()
		{
			return new HashSet<Point>(
				GetData()
					.Where(l => l.Contains(','))
					.Select(l =>
					{
						var parts = l.Split(',');
						return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
					}));
		}
	}
}
