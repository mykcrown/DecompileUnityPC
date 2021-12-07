using System;
using UnityEngine;

// Token: 0x020003FB RID: 1019
[Serializable]
public class UnboxingItemDisplayParameters
{
	// Token: 0x04001020 RID: 4128
	public EquipmentTypes itemType;

	// Token: 0x04001021 RID: 4129
	public UnboxingItemDisplayParameters.Align alignment;

	// Token: 0x04001022 RID: 4130
	public bool autoPlay;

	// Token: 0x04001023 RID: 4131
	public Vector3 position;

	// Token: 0x04001024 RID: 4132
	public Vector3 rotation;

	// Token: 0x04001025 RID: 4133
	public Vector3 scale = new Vector3(1f, 1f, 1f);

	// Token: 0x020003FC RID: 1020
	public enum Align
	{
		// Token: 0x04001027 RID: 4135
		CENTER,
		// Token: 0x04001028 RID: 4136
		BOTTOM
	}
}
