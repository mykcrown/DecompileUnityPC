using System;
using UnityEngine;

// Token: 0x0200038D RID: 909
[Serializable]
public class EmoteData : ScriptableObject
{
	// Token: 0x04000CE7 RID: 3303
	public CharacterID character;

	// Token: 0x04000CE8 RID: 3304
	public MoveData primaryData;

	// Token: 0x04000CE9 RID: 3305
	public MoveData partnerData;
}
