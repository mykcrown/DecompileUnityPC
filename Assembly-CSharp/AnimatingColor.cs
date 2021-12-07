using System;
using UnityEngine;

// Token: 0x02000598 RID: 1432
[Serializable]
public class AnimatingColor
{
	// Token: 0x040019C2 RID: 6594
	public Color colorStart;

	// Token: 0x040019C3 RID: 6595
	public bool animateColor;

	// Token: 0x040019C4 RID: 6596
	public AnimatingColor.Method method;

	// Token: 0x040019C5 RID: 6597
	public Color colorEnd;

	// Token: 0x040019C6 RID: 6598
	public int frames;

	// Token: 0x02000599 RID: 1433
	public enum Method
	{
		// Token: 0x040019C8 RID: 6600
		LOOP,
		// Token: 0x040019C9 RID: 6601
		YOYO
	}
}
