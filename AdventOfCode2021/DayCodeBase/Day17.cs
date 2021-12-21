using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day17: DayCodeBase
	{

		public override string Problem1()
		{
			var target = ParseTarget(GetData().First());
			return Enumerable
				.Range(1, Math.Abs(target.Item1.Y) - 1)
				.Sum(x => x)
				.ToString();
		}

		public override string Problem2()
		{
			var target = ParseTarget(GetData().First());
			//var target = ParseTarget("target area: x=20..30, y=-10..-5");
			var toReturn = 0;
			for (var x = 1; x <= target.Item2.X; ++x)
			{
				for (var y = target.Item1.Y; y <= (Math.Abs(target.Item1.Y) - 1); ++y)
				{
					if (TestHit(target, x, y)) ++toReturn;
				}
			}
			return toReturn.ToString();
		}

		private bool TestHit(Tuple<Point, Point> target, int velocityX, int velocityY)
		{
			var curLocation = new Point(0, 0);
			while (curLocation.X <= target.Item2.X && curLocation.Y >= target.Item1.Y) {
				//move
				curLocation = new Point(curLocation.X + velocityX, curLocation.Y + velocityY);

				//test
				if (curLocation.X >= target.Item1.X && curLocation.X <= target.Item2.X &&
					curLocation.Y >= target.Item1.Y && curLocation.Y <= target.Item2.Y) return true;

				//adjust velocity
				velocityX = Math.Max(0, velocityX - 1);
				velocityY--;
			}
			return false;
		}

		private Tuple<Point, Point> ParseTarget(string data)
		{
			var x_yParts = data.Split(",");
			var xRange = x_yParts[0].Substring(x_yParts[0].IndexOf('=') + 1).Split("..").Select(int.Parse).OrderBy(x => x).ToList();
			var yRange = x_yParts[1].Substring(x_yParts[1].IndexOf('=') + 1).Split("..").Select(int.Parse).OrderBy(x => x).ToList();
			return new Tuple<Point,Point>(new Point(xRange[0], yRange[0]), new Point(xRange[1], yRange[1]));
		}
	}
}
