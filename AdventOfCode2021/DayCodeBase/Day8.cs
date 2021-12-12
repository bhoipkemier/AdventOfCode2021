using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day8 : DayCodeBase
	{
		public override string Problem1()
		{
			return GetData()
				.Select(line => new Display(line))
				.Sum(GetProb1Count)
				.ToString();
		}
		public override string Problem2()
		{
			return GetData()
				.Sum(line => new Display(line).GetOutputNumber())
				.ToString();
		}

		public int GetProb1Count(Display display) => display.Output.Count(c => new[] { 2, 4, 3, 7 }.Contains(c.Count()));


		public class Display
		{
			public List<string> Input { get; set; }
			public List<string> InputDigitMapping { get; set; }
			public List<string> Output { get; set; }

			public Display(string data)
			{
				var parts = data.Split('|', StringSplitOptions.RemoveEmptyEntries);
				Input = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => new string(s.ToCharArray().OrderBy(c => c).ToArray())).ToList();
				Output = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => new string(s.ToCharArray().OrderBy(c => c).ToArray())).ToList();
				InputDigitMapping = GetMapping(Input);
			}

			public int GetOutputNumber()
			{
				var lookup = InputDigitMapping.Select((pattern, indx) => new { pattern, indx }).ToDictionary(x => x.pattern, x => x.indx);
				return int.Parse(string.Join("", Output.Select(pattern => lookup[pattern].ToString())));
			}

			private List<string> GetMapping(List<string> input)
			{
				var toReturn = Enumerable.Range(0, 10).Select(_ => (string)null).ToList();
				toReturn[1] = input.FirstOrDefault(x => x.Length == 2);
				toReturn[4] = input.FirstOrDefault(x => x.Length == 4);
				toReturn[7] = input.FirstOrDefault(x => x.Length == 3);
				toReturn[8] = input.FirstOrDefault(x => x.Length == 7);

				var FourMinus1 = new string(toReturn[4].ToCharArray().Where(c => !toReturn[1].Contains(c)).ToArray());

				toReturn[3] = input.FirstOrDefault(x => x.Length == 5 && Contains(x, toReturn[1]));
				toReturn[5] = input.FirstOrDefault(x => x.Length == 5 && Contains(x, FourMinus1));
				toReturn[2] = input.FirstOrDefault(x => x.Length == 5 && x != toReturn[3] && x != toReturn[5]);

				toReturn[9] = input.FirstOrDefault(x => x.Length == 6 && Contains(x, toReturn[4]));
				toReturn[6] = input.FirstOrDefault(x => x.Length == 6 && Contains(x, toReturn[5]) && !Contains(x, toReturn[1]));
				toReturn[0] = input.FirstOrDefault(x => x.Length == 6 && x != toReturn[9] && x != toReturn[6]);

				return toReturn;
			}

			private bool Contains(string x, string v)
			{
				return v.ToCharArray().All(c => x.Contains(c));
			}

			/*
			 *  0 => 6 => 6 Segments and not 9 and not 6
			 *  1 => 2 => given
			 *  2 => 5 => 5 Segments and not 3 and not 5
			 *  3 => 5 => 5 Segments and contains 1
			 *  4 => 4 => given
			 *  5 => 5 => 5 Segments and contains (4 - 1)
			 *  6 => 6 => 6 Segments and contains 5 and does not contain 1
			 *  7 => 3 => given
			 *  8 => 7 => given
			 *  9 => 6 => 6 Segments and contains 4
			 */
		}
	}
}
