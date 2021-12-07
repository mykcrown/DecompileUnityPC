using System;
using UnityEngine;

// Token: 0x02000A40 RID: 2624
[Serializable]
public class UIColorContainer
{
	// Token: 0x06004CD3 RID: 19667 RVA: 0x00145454 File Offset: 0x00143854
	public Color GetColor(UIColor color)
	{
		switch (color)
		{
		default:
			return this.Blue;
		case UIColor.Red:
			return this.Red;
		case UIColor.Yellow:
			return this.Yellow;
		case UIColor.Green:
			return this.Green;
		case UIColor.Purple:
			return this.Purple;
		case UIColor.Pink:
			return this.Pink;
		case UIColor.Grey:
			return this.Grey;
		}
	}

	// Token: 0x0400325E RID: 12894
	public Color Red;

	// Token: 0x0400325F RID: 12895
	public Color Yellow;

	// Token: 0x04003260 RID: 12896
	public Color Green;

	// Token: 0x04003261 RID: 12897
	public Color Purple;

	// Token: 0x04003262 RID: 12898
	public Color Blue;

	// Token: 0x04003263 RID: 12899
	public Color Pink;

	// Token: 0x04003264 RID: 12900
	public Color Grey;
}
