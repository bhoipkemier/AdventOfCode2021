using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace AdventOfCode2021.DayCodeBase
{
	public class Day19: DayCodeBase
	{
		private static List<Matrix4x4> _rotations = null;
		public static List<Matrix4x4> Rotations { get { return _rotations ?? (_rotations = BuildRotationMaticies()); } }
		public static List<Scanner> Scanners = null;

		public override string Problem1()
		{
			Scanners = LoadScanners(GetData());
			var beacons = Solve(Scanners);
			return beacons.Count().ToString();
		}
		public override string Problem2()
		{
			var distance = 0;
			foreach(var s1 in Scanners)
			{
				foreach(var s2 in Scanners)
				{
					var x = (int)Math.Abs(s1.Location.X - s2.Location.X);
					var y = (int)Math.Abs(s1.Location.Y - s2.Location.Y);
					var z = (int)Math.Abs(s1.Location.Z - s2.Location.Z);
					distance = Math.Max(distance, x + y + z);
				}
			}
			return distance.ToString();
		}

		private List<Vector3> Solve(List<Scanner> scanners)
		{
			var context = new HashSet<Vector3>(scanners[0].Beacons);
			return Solve(context, scanners.Skip(1).ToList());

		}

		private List<Vector3>  Solve(HashSet<Vector3> context, List<Scanner> scanners)
		{
			if (!scanners.Any()) return context.ToList();
			foreach(var scanner in scanners.ToList())
			{
				foreach(var rotation in Rotations)
				{
					foreach(var location in context)
					{
						var proposed = scanner.GetBeaconLocations(context, rotation, location);
						if(proposed != null)
						{
							var newContext = new HashSet<Vector3>(context.Union(proposed));
							return Solve(newContext, scanners.Where(s => scanner != s).ToList());
						}
					}
				}
			}
			throw new NotImplementedException();
		}

		private List<Scanner> LoadScanners(string[] data)
		{
			var toReturn = new List<Scanner>();
			Scanner scanner = null;
			foreach(var line in data)
			{
				if (line.StartsWith("---"))
				{
					scanner = new Scanner() { Beacons = new List<Vector3>() };
					toReturn.Add(scanner);
				}else if(line.Length > 0)
				{
					var cords = line.Split(',').Select(float.Parse).ToList();
					scanner.Beacons.Add(new Vector3(cords[0], cords[1], cords[2]));
				}
			}
			return toReturn;
		}

		public class Scanner
		{
			public List<Vector3> Beacons { get; set; }
			public Vector3 Location = new Vector3(0, 0, 0);

			internal List<Vector3> GetBeaconLocations(HashSet<Vector3> context, Matrix4x4 rotation, Vector3 adjustment)
			{
				var rotatedLocations = Beacons.Select(b => TransRound(b, rotation));
				foreach(var beacon in rotatedLocations)
				{
					//Console.WriteLine($"{newVec0}{newVec0 + (adj - newVec0)}  :::  {newVec}{newVec + (adj - newVec0)}");
					var offset = adjustment - beacon;
					var translated = rotatedLocations.Select(b => b + offset).ToList();
					var proposed = context.Intersect(translated).ToList();
					if (proposed.Count() >= 12)
					{
						Location = offset;
						return translated;
					}
				}
				return null;
			}
		}

		public static List<Matrix4x4> BuildRotationMaticies()
		{
			var toCheck = new HashSet<Vector3>();
			var toReturn = new List<Matrix4x4>();
			var srcVec = new Vector3(1, 2, 3);
			for(var x = 0; x < 4; ++x)
			{
				for (var y = 0; y < 4; ++y)
				{
					for (var z = 0; z < 4; ++z)
					{
						var matrix = Matrix4x4.CreateRotationX(x * (float)Math.PI / 2f) *
							Matrix4x4.CreateRotationY(y * (float)Math.PI / 2f) * 
							Matrix4x4.CreateRotationZ(z * (float)Math.PI / 2f);
						var result = TransRound(srcVec, matrix);
						if (!toCheck.Contains(result))
						{
							toCheck.Add(result);
							toReturn.Add(matrix);
						}
					}
				}
			}
			return toReturn;
		}

		public static Vector3 TransRound(Vector3 vec, Matrix4x4 matrix) {
			var toRound = Vector3.Transform(vec, matrix);
			return new Vector3((float)Math.Round((decimal)toRound.X), (float)Math.Round((decimal)toRound.Y), (float)Math.Round((decimal)toRound.Z));
		}
	}
}
