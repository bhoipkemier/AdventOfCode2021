using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day9 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData()
				.Select(line => line.ToCharArray().Select(c => int.Parse(c.ToString())).ToList())
				.ToList();
			var toReturn = 0;
			for(var row = 0; row < data.Count(); ++row)
			{
				for (var col = 0; col < data[row].Count(); ++col)
				{
					if((row == 0 || data[row-1][col] > data[row][col]) &&
						(col == (data[row].Count() - 1) || data[row][col + 1] > data[row][col]) &&
						(row == (data.Count() - 1) || data[row+1][col] > data[row][col]) &&
						(col == 0 || data[row][col - 1] > data[row][col]))
					{
						toReturn += data[row][col] + 1;
					}
				}
			}
			return toReturn.ToString();
		}

		public override string Problem2()
		{
			var data = GetData()
				.SelectMany((line, row) => 
					line
						.ToCharArray()
						.Select((c, col) =>
							new Cell() {
								Location = new Point(row, col),
								Height = int.Parse(c.ToString())
							})
					)
				.Where(c => c.Height < 9)
				.ToDictionary(c => c.Location, c => c);
			var groups = new List<List<Cell>>();
			while(data.Count > 0)
			{
				var group = new List<Cell>();
				FloodFill(data, group, data.First().Value);
				groups.Add(group);
			}

			return groups.OrderByDescending(g => g.Count)
				.Take(3)
				.Aggregate(1, (acc,g) => acc * g.Count())
				.ToString();
		}

		private void FloodFill(Dictionary<Point, Cell> data, List<Cell> group, Cell cell)
		{
			data.Remove(cell.Location);
			group.Add(cell);
			if (data.ContainsKey(new Point(cell.Location.X + 1, cell.Location.Y))) FloodFill(data, group, data[new Point(cell.Location.X + 1, cell.Location.Y)]);
			if (data.ContainsKey(new Point(cell.Location.X - 1, cell.Location.Y))) FloodFill(data, group, data[new Point(cell.Location.X - 1, cell.Location.Y)]);
			if (data.ContainsKey(new Point(cell.Location.X, cell.Location.Y + 1))) FloodFill(data, group, data[new Point(cell.Location.X, cell.Location.Y + 1)]);
			if (data.ContainsKey(new Point(cell.Location.X, cell.Location.Y - 1))) FloodFill(data, group, data[new Point(cell.Location.X, cell.Location.Y - 1)]);
		}

		public class Cell
		{
			public Point Location { get; set; }
			public int Height { get; set; }
		}
	}
}
