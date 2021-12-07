using System;
using System.Collections.Generic;

// Token: 0x02000AB3 RID: 2739
public class EqualityUtil
{
	// Token: 0x06005060 RID: 20576 RVA: 0x0014F5DC File Offset: 0x0014D9DC
	public static int ComparePress(ButtonPress a, ButtonPress b)
	{
		if (a == b)
		{
			return 0;
		}
		return a - b;
	}

	// Token: 0x02000AB4 RID: 2740
	public class ButtonPressEqualityComparer : EqualityComparer<ButtonPress>
	{
		// Token: 0x06005062 RID: 20578 RVA: 0x0014F604 File Offset: 0x0014DA04
		public override bool Equals(ButtonPress press1, ButtonPress press2)
		{
			if (press1 == ButtonPress.Side)
			{
				ButtonPress untapped = InputUtils.GetUntapped(press2);
				return untapped == ButtonPress.Forward || untapped == ButtonPress.Backward || untapped == ButtonPress.Side;
			}
			if (press2 == ButtonPress.Side)
			{
				return press1 == ButtonPress.Forward || press1 == ButtonPress.Backward || press1 == ButtonPress.Side;
			}
			if (press1 == ButtonPress.SideTap)
			{
				return press2 == ButtonPress.ForwardTap || press2 == ButtonPress.BackwardTap || press2 == ButtonPress.SideTap;
			}
			if (press2 == ButtonPress.SideTap)
			{
				return press1 == ButtonPress.ForwardTap || press1 == ButtonPress.BackwardTap || press1 == ButtonPress.SideTap;
			}
			if (press1 == ButtonPress.Taunt)
			{
				return press2 == ButtonPress.Taunt || press2 == ButtonPress.TauntUp || press2 == ButtonPress.TauntDown || press2 == ButtonPress.TauntLeft || press2 == ButtonPress.TauntRight;
			}
			if (press2 == ButtonPress.Taunt)
			{
				return press1 == ButtonPress.Taunt || press1 == ButtonPress.TauntUp || press1 == ButtonPress.TauntDown || press1 == ButtonPress.TauntLeft || press1 == ButtonPress.TauntRight;
			}
			return press1 == press2 || press1 == InputUtils.GetUntapped(press2);
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x0014F70B File Offset: 0x0014DB0B
		public override int GetHashCode(ButtonPress press)
		{
			return press.GetHashCode();
		}
	}
}
