using System;
using UnityEngine;

// Token: 0x02000390 RID: 912
[Serializable]
public class VictoryPoseData : ScriptableObject, IDefaultableData
{
	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06001388 RID: 5000 RVA: 0x000701FB File Offset: 0x0006E5FB
	public bool IsDefaultData
	{
		get
		{
			return this.isDefault;
		}
	}

	// Token: 0x04000CEA RID: 3306
	public CharacterID character;

	// Token: 0x04000CEB RID: 3307
	public bool isDefault;

	// Token: 0x04000CEC RID: 3308
	public string localizedNameWhenIsDefault;

	// Token: 0x04000CED RID: 3309
	public float minDuration = 3f;

	// Token: 0x04000CEE RID: 3310
	public float maxDuration = 3f;

	// Token: 0x04000CEF RID: 3311
	public MoveData moveData;

	// Token: 0x04000CF0 RID: 3312
	public MoveData loopingMoveData;

	// Token: 0x04000CF1 RID: 3313
	public MoveData partnerMoveData;

	// Token: 0x04000CF2 RID: 3314
	public MoveData partnerLoopingMoveData;
}
