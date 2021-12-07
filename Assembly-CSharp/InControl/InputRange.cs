using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200006F RID: 111
	public struct InputRange
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x000189A0 File Offset: 0x00016DA0
		private InputRange(float value0, float value1, InputRangeType type)
		{
			this.Value0 = value0;
			this.Value1 = value1;
			this.Type = type;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000189B7 File Offset: 0x00016DB7
		public InputRange(InputRangeType type)
		{
			this.Value0 = InputRange.TypeToRange[(int)type].Value0;
			this.Value1 = InputRange.TypeToRange[(int)type].Value1;
			this.Type = type;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000189EC File Offset: 0x00016DEC
		public bool Includes(float value)
		{
			return !this.Excludes(value);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000189F8 File Offset: 0x00016DF8
		public bool Excludes(float value)
		{
			return this.Type == InputRangeType.None || value < Mathf.Min(this.Value0, this.Value1) || value > Mathf.Max(this.Value0, this.Value1);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00018A38 File Offset: 0x00016E38
		public static float Remap(float value, InputRange sourceRange, InputRange targetRange)
		{
			if (sourceRange.Excludes(value))
			{
				return 0f;
			}
			float t = Mathf.InverseLerp(sourceRange.Value0, sourceRange.Value1, value);
			return Mathf.Lerp(targetRange.Value0, targetRange.Value1, t);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00018A84 File Offset: 0x00016E84
		internal static float Remap(float value, InputRangeType sourceRangeType, InputRangeType targetRangeType)
		{
			InputRange sourceRange = InputRange.TypeToRange[(int)sourceRangeType];
			InputRange targetRange = InputRange.TypeToRange[(int)targetRangeType];
			return InputRange.Remap(value, sourceRange, targetRange);
		}

		// Token: 0x04000380 RID: 896
		public static readonly InputRange None = new InputRange(0f, 0f, InputRangeType.None);

		// Token: 0x04000381 RID: 897
		public static readonly InputRange MinusOneToOne = new InputRange(-1f, 1f, InputRangeType.MinusOneToOne);

		// Token: 0x04000382 RID: 898
		public static readonly InputRange OneToMinusOne = new InputRange(1f, -1f, InputRangeType.OneToMinusOne);

		// Token: 0x04000383 RID: 899
		public static readonly InputRange ZeroToOne = new InputRange(0f, 1f, InputRangeType.ZeroToOne);

		// Token: 0x04000384 RID: 900
		public static readonly InputRange ZeroToMinusOne = new InputRange(0f, -1f, InputRangeType.ZeroToMinusOne);

		// Token: 0x04000385 RID: 901
		public static readonly InputRange OneToZero = new InputRange(1f, 0f, InputRangeType.OneToZero);

		// Token: 0x04000386 RID: 902
		public static readonly InputRange MinusOneToZero = new InputRange(-1f, 0f, InputRangeType.MinusOneToZero);

		// Token: 0x04000387 RID: 903
		private static readonly InputRange[] TypeToRange = new InputRange[]
		{
			InputRange.None,
			InputRange.MinusOneToOne,
			InputRange.OneToMinusOne,
			InputRange.ZeroToOne,
			InputRange.ZeroToMinusOne,
			InputRange.OneToZero,
			InputRange.MinusOneToZero
		};

		// Token: 0x04000388 RID: 904
		public readonly float Value0;

		// Token: 0x04000389 RID: 905
		public readonly float Value1;

		// Token: 0x0400038A RID: 906
		public readonly InputRangeType Type;
	}
}
