using System;
using UnityEngine;

// Token: 0x020009FB RID: 2555
public class EquipTypeDefinition
{
	// Token: 0x06004961 RID: 18785 RVA: 0x0013BC75 File Offset: 0x0013A075
	public EquipTypeDefinition(EquipmentTypes id)
	{
		this.type = id;
	}

	// Token: 0x04003062 RID: 12386
	public EquipmentTypes type;

	// Token: 0x04003063 RID: 12387
	public Sprite icon;
}
