using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day16: DayCodeBase
	{

		public override string Problem1()
		{
			var binary = GetBinaryData();
			var packet = new Packet(ref binary);
			return packet.VersionSum().ToString();
		}

		public override string Problem2()
		{
			var binary = GetBinaryData();
			var packet = new Packet(ref binary);
			return packet.Calculate().ToString();
		}

		private string GetBinaryData(string hex = null)
		{
			hex = hex ?? GetData().First().ToString();
			return string.Join(string.Empty,
				hex.Select(
					c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')
				)
			);
		}

		public class Packet
		{
			public int Version { get; set; }
			public int Type { get; set; }
			public long? LiteralValue { get; set; }
			public List<Packet> SubPackets { get; set; }

			public Packet(ref string binaryData)
			{
				Version = Convert.ToInt32(binaryData.Substring(0, 3), 2);
				Type = Convert.ToInt32(binaryData.Substring(3, 3), 2);
				SubPackets = new List<Packet>();
				binaryData = binaryData.Substring(6);
				if (Type == 4)
				{
					LiteralValue = ParseLiteral(ref binaryData);
				}else
				{
					SubPackets = ParseOperator(ref binaryData);
				}
			}

			private List<Packet> ParseOperator(ref string binaryData)
			{
				var toReturn = new List<Packet>();
				var lengthType = binaryData.Substring(0, 1);
				binaryData = binaryData.Substring(1);
				if (lengthType == "0")
				{
					var totalLength = Convert.ToInt32(binaryData.Substring(0,15), 2);
					binaryData = binaryData.Substring(15);
					var binarySubset = binaryData.Substring(0, totalLength);
					binaryData = binaryData.Substring(totalLength);
					while(binarySubset.Length > 6)
					{
						toReturn.Add(new Packet(ref binarySubset));
					}
				}
				else
				{
					var subsetCount = Convert.ToInt32(binaryData.Substring(0, 11), 2);
					binaryData = binaryData.Substring(11);
					for(var i =0; i< subsetCount; ++i)
					{
						toReturn.Add(new Packet(ref binaryData));
					}
				}
				return toReturn;
			}

			private long ParseLiteral(ref string binaryData)
			{
				var accumulator = "";
				for(var stop = false; !stop; binaryData = binaryData.Substring(5))
				{
					stop = binaryData.Substring(0, 1) == "0";
					accumulator += binaryData.Substring(1, 4);
				}
				return Convert.ToInt64(accumulator, 2);
			}

			public int VersionSum() => Version + SubPackets.Sum(p => p.VersionSum());

			public long Calculate()
			{
				switch(Type)
				{
					case 0: return SubPackets.Aggregate(0L, (acc, p) => acc + p.Calculate());
					case 1: return SubPackets.Aggregate(1L, (acc, p) => acc * p.Calculate());
					case 2: return SubPackets.Aggregate(long.MaxValue, (acc, p) => acc < p.Calculate() ? acc : p.Calculate());
					case 3: return SubPackets.Aggregate(long.MinValue, (acc, p) => acc > p.Calculate() ? acc : p.Calculate());
					case 4: return LiteralValue ?? 0L;
					case 5: return SubPackets[0].Calculate() > SubPackets[1].Calculate() ? 1 : 0;
					case 6: return SubPackets[0].Calculate() < SubPackets[1].Calculate() ? 1 : 0;
					case 7: return SubPackets[0].Calculate() == SubPackets[1].Calculate() ? 1 : 0;
				}
				throw new NotImplementedException();
			}
		}
	}
}
