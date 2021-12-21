using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day18: DayCodeBase
	{

		public override string Problem1()
		{
			var snailNumbers = GetData()
				.Select(line => SnailNumber.Parse(ref line, 0))
				.ToList();
			var summation = snailNumbers.Aggregate(null, (SnailNumber acc, SnailNumber sn) => acc == null ? sn : acc + sn);
			return summation.Magnitude().ToString();
		}

		public override string Problem2()
		{
			var snailNumbers = GetData()
				.Select(line => SnailNumber.Parse(ref line, 0))
				.ToList();
			var toReturn = 0l;
			foreach(var n1 in snailNumbers)
			{
				foreach (var n2 in snailNumbers)
				{
					if(n1 != n2)
					{
						var newMax = (n1 + n2).Magnitude();
						if(newMax > toReturn)
						{
							toReturn = newMax;
						}
					}
				}
			}
			return toReturn.ToString();
		}

		public abstract class SnailNumber {
			public int Depth { get; set; }

			public static SnailNumber operator +(SnailNumber left, SnailNumber right)
			{
				var toReturn = new SnailNumberPair(left.Copy(), right.Copy());
				toReturn.Reduce();
				return toReturn;
			}

			private SnailNumber Copy()
			{
				if(this is SnailNumberPair pair)
				{
					return new SnailNumberPair(pair.Left.Copy(), pair.Right.Copy());
				}
				return new SnailNumberRegular { Value = (this as SnailNumberRegular).Value };
			}

			public static SnailNumber Parse(ref string data, int depth)
			{
				SnailNumber toReturn = data[0] == '[' ? new SnailNumberPair(ref data, depth) : new SnailNumberRegular(ref data);
				toReturn.Depth = depth;
				return toReturn;
			}

			internal abstract long Magnitude();

			internal void Reduce()
			{
				SetDepth(0);
				while (NeedsExploding() || NeedsSplitting())
				{
					if(NeedsExploding())
					{
						var nextNumberList = new LinkedList<SnailNumberRegular>();
						SetNextItem(nextNumberList);
						Explode(nextNumberList, null);
						continue;
					}else
					{
						Split(null);
					}
					SetDepth(0);
				}
			}

			private void SetDepth(int depth)
			{
				Depth = depth;
				if(this is SnailNumberPair pair)
				{
					pair.Left.SetDepth(depth + 1);
					pair.Right.SetDepth(depth + 1);
				}
			}

			internal abstract void Explode(LinkedList<SnailNumberRegular> nextNumberList, SnailNumberPair parent);
			internal abstract void Split(SnailNumber parent);

			internal abstract bool NeedsExploding();
			internal abstract bool NeedsSplitting();
			internal abstract void SetNextItem(LinkedList<SnailNumberRegular> items);

			internal void Print()
			{
				if(this is SnailNumberPair pair)
				{
					Console.Write("[");
					pair.Left.Print();
					Console.Write(",");
					pair.Right.Print();
					Console.Write("]");
				}
				else
				{
					Console.Write((this as SnailNumberRegular).Value.ToString());
				}
			}
		}

		public class SnailNumberPair: SnailNumber
		{
			public SnailNumber Left { get; set; }
			public SnailNumber Right { get; set; }
			public SnailNumberPair(ref string data, int depth) {
				data = data.Substring(1); //consume [
				Left = SnailNumber.Parse(ref data, depth + 1);
				data = data.Substring(1); //consume ,
				Right = SnailNumber.Parse(ref data, depth + 1);
				data = data.Substring(1); //consume ]
			}

			public SnailNumberPair(SnailNumber left, SnailNumber right)
			{
				Left = left;
				Right = right;
			}

			internal override long Magnitude() => 3L * Left.Magnitude() + 2L * Right.Magnitude();

			internal bool SelfNeedsExploding() => (Left is SnailNumberRegular && Right is SnailNumberRegular && Depth >= 4);

			internal override bool NeedsExploding()
			{
				return SelfNeedsExploding() ||
					Left.NeedsExploding() ||
					Right.NeedsExploding();
			}

			internal override bool NeedsSplitting() => Left.NeedsSplitting() || Right.NeedsSplitting();

			internal override void Explode(LinkedList<SnailNumberRegular> nextNumberList, SnailNumberPair parent)
			{
				if (SelfNeedsExploding())
				{
					var left = nextNumberList.Find(Left as SnailNumberRegular);
					var prev = left.Previous;
					if (prev != null) prev.Value.Value += (Left as SnailNumberRegular).Value;
					var right = nextNumberList.Find(Right as SnailNumberRegular);
					var next = right.Next;
					if (next != null) next.Value.Value += (Right as SnailNumberRegular).Value;
					if (parent.Left == this) parent.Left = new SnailNumberRegular { Value = 0 };
					if (parent.Right == this) parent.Right = new SnailNumberRegular { Value = 0 };
				}
				else if (Left.NeedsExploding()) Left.Explode(nextNumberList, this);
				else if (Right.NeedsExploding()) Right.Explode(nextNumberList, this);
			}

			internal override void Split(SnailNumber parent)
			{
				if (Left.NeedsSplitting())
				{
					Left.Split(this);
				}
				else
				{
					Right.Split(this);
				}
			}

			internal override void SetNextItem(LinkedList<SnailNumberRegular> nextNumberList)
			{
				Left.SetNextItem(nextNumberList);
				Right.SetNextItem(nextNumberList);
			}
		}

		public class SnailNumberRegular : SnailNumber {
			public long Value { get; set; }
			public SnailNumberRegular() { }
			public SnailNumberRegular(ref string data)
			{
				Value = long.Parse(data[0].ToString());
				data = data.Substring(1);
			}

			internal override long Magnitude() => Value;

			internal override bool NeedsExploding() => false;

			internal override bool NeedsSplitting() => Value > 9;

			internal override void Explode(LinkedList<SnailNumberRegular> nextNumberList, SnailNumberPair parent)
			{
				throw new NotImplementedException();
			}

			internal override void Split(SnailNumber parent)
			{
				var newPair = new SnailNumberPair(
					new SnailNumberRegular() { Value = (int)((decimal)Value / 2m) },
					new SnailNumberRegular() { Value = (int)Math.Ceiling((decimal)Value / 2m) }
				);

				if ((parent as SnailNumberPair).Left == this) (parent as SnailNumberPair).Left = newPair;
				if ((parent as SnailNumberPair).Right == this) (parent as SnailNumberPair).Right = newPair;
			}

			internal override void SetNextItem(LinkedList<SnailNumberRegular> items)
			{
				items.AddLast(this);
			}
		}
	}
}
