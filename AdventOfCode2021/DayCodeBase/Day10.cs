using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day10: DayCodeBase
	{
		public override string Problem1()
		{
			return GetData().Select(GetScore1).Sum().ToString();
		}
		public override string Problem2()
		{
			var validLines = GetData().Where(x => GetScore1(x) == 0);
			var scores = validLines.Select(GetScore2).OrderBy(x => x).ToList();
			return scores.Skip((scores.Count() - 1)/2).First().ToString();
		}

		private long GetScore2(string line)
		{
			var stack = new Stack<char>();
			foreach (var c in line)
			{
				if ("([{<".Contains(c)) stack.Push(c);
				else stack.Pop();
			}
			long totalScore = 0;
			while (stack.Any())
			{
				var c = stack.Pop();
				totalScore *= 5;
				totalScore +=
					c == '(' ? 1 :
					c == '[' ? 2 :
					c == '{' ? 3 :
					c == '<' ? 4 : throw new NotImplementedException();
			}
			return totalScore;
		}

		private int GetScore1(string line)
		{
			var stack = new Stack<char>();
			foreach(var c in line)
			{
				if ("([{<".Contains(c)) stack.Push(c);
				else
				{
					bool invalid = !stack.Any();
					if (!invalid)
					{
						var stackChar = stack.Pop();
						invalid = !((stackChar == '(' && c == ')') ||
												(stackChar == '[' && c == ']') ||
												(stackChar == '{' && c == '}') ||
												(stackChar == '<' && c == '>'));
					}
					if (invalid) return
							 c == ')' ? 3 :
							 c == ']' ? 57 :
							 c == '}' ? 1197 :
							 c == '>' ? 25137 : throw new NotImplementedException();
				}
			}
			return 0;
		}
	}
}
