using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day20: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().ToList();
			var mapping = data.First().Select(c => c == '#').ToArray();
			var lit = GetLights(data.Skip(2).ToList());
			lit = DoRound(lit, mapping, false, new Point(0, 0), new Point(99, 99));
			lit = DoRound(lit, mapping, true, new Point(-1, -1), new Point(100, 100));
			return lit.Count().ToString();
		}
		public override string Problem2()
		{
			var data = GetData().ToList();
			var mapping = data.First().Select(c => c == '#').ToArray();
			var lit = GetLights(data.Skip(2).ToList());
			var edges = false;
			for(var i = 0; i < 50; ++i)
			{
				lit = DoRound(lit, mapping, edges, new Point(0-i, 0-i), new Point(99+i, 99+i));
				edges = !edges;
			}
			return lit.Count().ToString();
		}

		private void Print(HashSet<Point> lit)
		{
			for(var y = -2; y <= 102; ++y)
			{
				Console.WriteLine("");
				for (var x = -2; x <= 102; ++x)
				{
					Console.Write(lit.Contains(new Point(x,y)) ? "#" : ".");
				}
			}
			Console.WriteLine("");
			Console.WriteLine("");
		}

		private HashSet<Point> DoRound(HashSet<Point> lit, bool[] mapping, bool edgesValue, Point topLeft, Point bottomRight)
		{
			var toReturn = new HashSet<Point>();
			for(var x = topLeft.X - 1; x <= bottomRight.X + 1; ++x)
			{
				for (var y = topLeft.Y - 1; y <= bottomRight.Y + 1; ++y)
				{
					var val = 0;
					for(var testY = -1; testY <= 1; ++testY)
					{
						for (var testX = -1; testX <= 1; ++testX)
						{
							var checkX = x + testX;
							var checkY = y + testY;
							var lightOn = checkX < topLeft.X || checkX > bottomRight.X || checkY < topLeft.Y || checkY > bottomRight.Y ? edgesValue : lit.Contains(new Point(checkX, checkY));
							val <<= 1;
							if (lightOn) val += 1;
						}
					}
					if (mapping[val]) toReturn.Add(new Point(x, y));
				}
			}
			return toReturn;
		}

		private HashSet<Point> GetLights(List<string> data)
		{
			var toReturn = new HashSet<Point>();
			for (var y = 0; y < data.Count(); ++y)
			{
				for (var x = 0; x < data[0].Length; ++x)
				{
					if (data[y][x] == '#') toReturn.Add(new Point(x, y));
				}
			}
			return toReturn;
		}
	}
}
