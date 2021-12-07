using System;
using UnityEngine;

// Token: 0x02000906 RID: 2310
[Serializable]
public class ColorSpriteContainer
{
	// Token: 0x06003C17 RID: 15383 RVA: 0x00116974 File Offset: 0x00114D74
	public Sprite GetSprite(UIColor color)
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

	// Token: 0x0400292C RID: 10540
	public Sprite Red;

	// Token: 0x0400292D RID: 10541
	public Sprite Yellow;

	// Token: 0x0400292E RID: 10542
	public Sprite Green;

	// Token: 0x0400292F RID: 10543
	public Sprite Purple;

	// Token: 0x04002930 RID: 10544
	public Sprite Blue;

	// Token: 0x04002931 RID: 10545
	public Sprite Pink;

	// Token: 0x04002932 RID: 10546
	public Sprite Grey;
}
