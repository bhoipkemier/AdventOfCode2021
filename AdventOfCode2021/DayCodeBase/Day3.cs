using System;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day3: DayCodeBase
	{
		public override string Problem1()
		{
			var data = GetData().ToArray();
			var freqs = GetFreqs(data);
			var gamaStr = freqs.Select(f => f > data.Length / 2 ? '1' : '0').ToArray();
			var epsStr = freqs.Select(f => f > data.Length / 2 ? '0' : '1').ToArray();
			return (Convert.ToInt32(new string(gamaStr), 2) * Convert.ToInt32(new string(epsStr), 2)).ToString();
		}

		public override string Problem2()
		{
			var data = GetData().ToArray();
			var oxRating = GetRating(data, GetHighestFreqBit);
			var co2Rating = GetRating(data, GetLowestFreqBit);
			return (Convert.ToInt32(new string(oxRating), 2) * Convert.ToInt32(new string(co2Rating), 2)).ToString();
		}

		private string GetRating(string[] data, Func<int[], int, int, char> getFreqBit, int bitToConsider = 0)
		{
			if (data.Length == 1) return data[0];
			var bitToMatch = getFreqBit(GetFreqs(data), bitToConsider, data.Length);
			var newData = data.Where(d => d[bitToConsider] == bitToMatch).ToArray();
			return GetRating(newData, getFreqBit, bitToConsider + 1);
		}

		private char GetHighestFreqBit(int[] freqs, int bitToConsider, int size) => freqs[bitToConsider] >= (size - freqs[bitToConsider]) ? '1' : '0';
		private char GetLowestFreqBit(int[] freqs, int bitToConsider, int size) => freqs[bitToConsider] >= (size - freqs[bitToConsider]) ? '0' : '1';

		private int[] GetFreqs(string[] data) => data[0].Select((_, indx) => data.Count(line => line[indx] == '1')).ToArray();
	}
}
