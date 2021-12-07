// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace SharedCore
{
	[Serializable]
	public class WeightedValue<T>
	{
		public T Value
		{
			get;
			set;
		}

		public int Weight
		{
			get;
			set;
		}

		public WeightedValue(T self, int weight = 1)
		{
			this.Value = self;
			this.Weight = weight;
		}
	}
}
