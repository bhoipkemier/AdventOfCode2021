using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day4 : DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().ToArray();
			var calledNumbers = data[0].Split(',').Select(int.Parse).ToArray();
			var boards = ParseBoards(data);
			foreach (var currentCall in calledNumbers)
			{
				foreach (var board in boards)
				{
					if (board.Called(currentCall)) return board.Score().ToString();
				}
			}
			return "No Solution";
		}
		public override string Problem2()
		{
			var data = GetData().ToArray();
			var calledNumbers = data[0].Split(',').Select(int.Parse).ToArray();
			var boards = ParseBoards(data);
			foreach (var currentCall in calledNumbers)
			{
				for (var i = boards.Count - 1; i >= 0; --i)
				{
					var board = boards[i];
					if (board.Called(currentCall))
					{
						if(boards.Count > 1)
						{
							boards.Remove(board);
						}else
						{
							return board.Score().ToString();
						}
					}
				}
			}
			return "No Solution";
		}

		private List<Board> ParseBoards(string[] data)
		{
			var toReturn = new List<Board>();
			for(var currentLine = 1; currentLine < data.Length; currentLine += 6)
			{
				toReturn.Add(new Board(data.Skip(currentLine + 1).Take(5).ToArray()));
			}
			return toReturn;
		}

		public class Board
		{
			public int[][] Cells;
			public int LastCalled;
			public bool[][] Marked;

			public Board(string[] input)
			{
				Cells = input
					.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
					.ToArray();
				Marked = Cells.Select(row => row.Select(c => false).ToArray()).ToArray();
			}

			public bool Called(int number)
			{
				LastCalled = number;
				var (row, col) = Find(number);
				if (row == null) return false;
				Marked[(int)row][(int)col] = true;
				return IsWinner();
			}

			public int Score()
			{
				var score = 0;
				for (var i = 0; i < Cells.Length; ++i)
					for (var j = 0; j < Cells[i].Length; ++j)
						if (!Marked[i][j])
							score += Cells[i][j];
				return score * LastCalled;
			}

			public bool IsWinner()
			{
				return Marked.Any(row => row.All(c => c)) ||
					Enumerable.Range(0, Marked[0].Length).Any(colIndx => Marked.All(row => row[colIndx]));
			}

			public (int?, int?) Find(int number)
			{
				for (var i = 0; i < Cells.Length; ++i)
					for (var j = 0; j < Cells[i].Length; ++j)
						if (Cells[i][j] == number)
							return (i, j);
				return (null, null);
			}
		}
	}
}
