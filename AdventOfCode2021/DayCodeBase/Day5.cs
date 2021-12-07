using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day5 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.Select(GetPoints)
				.Where(seg => seg[0].X == seg[1].X || seg[0].Y == seg[1].Y)
				.ToArray();
			var locations = new Dictionary<Point, int>();
			foreach (var vent in data)
			{
				AddLocation(locations, vent);
			}
			return locations.Where(l => l.Value > 1).Count().ToString();
		}
		public override string Problem2()
		{
			var data = GetData()
				.Select(GetPoints)
				.ToArray();
			var locations = new Dictionary<Point, int>();
			foreach (var vent in data)
			{
				AddLocation(locations, vent);
			}
			return locations.Where(l => l.Value > 1).Count().ToString();
		}

		private void AddLocation(Dictionary<Point, int> locations, Point[] vent)
		{
			var offsetX = vent[0].X == vent[1].X ? 0 :
				vent[0].X > vent[1].X ? -1 : 1;
			var offsetY = vent[0].Y == vent[1].Y ? 0 :
				vent[0].Y > vent[1].Y ? -1 : 1;
			var curPoint = new Point(vent[0].X, vent[0].Y);
			if (locations.ContainsKey(curPoint)) locations[curPoint] += 1; else locations[curPoint] = 1;
			while(curPoint != vent[1])
			{
				curPoint = new Point(curPoint.X + offsetX, curPoint.Y + offsetY);
				if (locations.ContainsKey(curPoint)) locations[curPoint] += 1; else locations[curPoint] = 1;
			}
		}

		private Point[] GetPoints(string input)
		{
			var parts = input.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
			return new[] { GetPoint(parts[0]), GetPoint(parts[1]) };
		}

		private Point GetPoint(string input)
		{
			var parts = input.Split(',').Select(int.Parse).ToArray();
			return new Point(parts[0], parts[1]);
		}
	}
}
