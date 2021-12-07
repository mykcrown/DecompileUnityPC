using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000508 RID: 1288
[Serializable]
public class HelplessStateData
{
	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06001BEF RID: 7151 RVA: 0x0008D4CE File Offset: 0x0008B8CE
	public AnimationClip animationClip
	{
		get
		{
			return this.animationClipFile.obj;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06001BF0 RID: 7152 RVA: 0x0008D4DB File Offset: 0x0008B8DB
	public AnimationClip leftAnimationClip
	{
		get
		{
			return this.leftAnimationClipFile.obj;
		}
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x0008D4E8 File Offset: 0x0008B8E8
	public string GetClipName(MoveData move, bool left = false)
	{
		return move.name + "_helplessState" + ((!left) ? string.Empty : "_left");
	}

	// Token: 0x0400166B RID: 5739
	public AnimationClipFile animationClipFile = new AnimationClipFile();

	// Token: 0x0400166C RID: 5740
	public AnimationClipFile leftAnimationClipFile = new AnimationClipFile();

	// Token: 0x0400166D RID: 5741
	public Fixed airMobilityMulti = 1;

	// Token: 0x0400166E RID: 5742
	public Fixed maxHAirVelocityMulti = 1;
}
