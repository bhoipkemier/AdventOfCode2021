using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day11: DayCodeBase
	{
		public override string Problem1()
		{
			var grid = GetData().Select(l => l.Select(c => int.Parse(c.ToString())).ToList()).ToList();
			var flashCount = 0;
			for (var i = 0; i < 100; ++i)
			{
				flashCount += DoRound(grid);
			}
			return flashCount.ToString();
		}
		public override string Problem2()
		{
			var grid = GetData().Select(l => l.Select(c => int.Parse(c.ToString())).ToList()).ToList();
			var flashCount = 0;
			for (var i = 0; true; ++i)
			{
				flashCount += DoRound(grid);
				if (grid.All(r => r.All(c => c == 0))) return (i + 1).ToString();
			}
		}

		private int DoRound(List<List<int>> grid)
		{
			for(var i = 0; i < grid.Count(); ++i)
			{
				for(var j = 0; j<grid[i].Count(); ++j)
				{
					grid[i][j]++;
				}
			}
			var toReturn = CheckFlash(grid);

			return toReturn;
		}

		private int CheckFlash(List<List<int>> grid)
		{
			var toReturn = 0;
			var flashSeen = true;
			while(flashSeen)
			{
				flashSeen = false;
				for (var i = 0; i < grid.Count(); ++i)
				{
					for (var j = 0; j < grid[i].Count(); ++j)
					{
						if (grid[i][j] > 9)
						{
							flashSeen = true;
							toReturn++;
							grid[i][j] = 0;
							foreach(var x in new[] { -1, 0, 1 })
							{
								foreach (var y in new[] { -1, 0, 1 })
								{
									if((x + i >= 0) && 
										(x + i <= grid.Count() - 1) && 
										(y + j >= 0) && 
										(y + j <= grid[x+i].Count() - 1) &&
										grid[x+i][y+j] != 0)
									{
										grid[x + i][y + j]++;
									}
								}
							}
						}
					}
				}
			}
			return toReturn;
		}
	}
}
