using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
[Serializable]
public class LandingOverrideData
{
	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x06001BEB RID: 7147 RVA: 0x0008D457 File Offset: 0x0008B857
	public AnimationClip landClip
	{
		get
		{
			return this.landClipFile.obj;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x06001BEC RID: 7148 RVA: 0x0008D464 File Offset: 0x0008B864
	public AnimationClip leftLandClip
	{
		get
		{
			return this.leftLandClipFile.obj;
		}
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x0008D471 File Offset: 0x0008B871
	public string GetClipName(MoveData move, bool left = false)
	{
		return move.name + "_helplessLand" + ((!left) ? string.Empty : "_left");
	}

	// Token: 0x04001663 RID: 5731
	public MoveData landMove;

	// Token: 0x04001664 RID: 5732
	public int lightLandFrames = 15;

	// Token: 0x04001665 RID: 5733
	public int heavyLandFrames = 30;

	// Token: 0x04001666 RID: 5734
	public AnimationClipFile landClipFile = new AnimationClipFile();

	// Token: 0x04001667 RID: 5735
	public string landClipName;

	// Token: 0x04001668 RID: 5736
	public AnimationClipFile leftLandClipFile = new AnimationClipFile();

	// Token: 0x04001669 RID: 5737
	public string leftLandClipName;

	// Token: 0x0400166A RID: 5738
	public ParticleData landParticle;
}
