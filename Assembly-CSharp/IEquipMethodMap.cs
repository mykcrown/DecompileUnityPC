using System;
using System.Collections.Generic;

// Token: 0x02000730 RID: 1840
public interface IEquipMethodMap
{
	// Token: 0x06002D5D RID: 11613
	EquipMethod GetMethod(EquipmentTypes type);

	// Token: 0x06002D5E RID: 11614
	List<EquipmentTypes> GetTypesWithMethod(EquipMethod method);
}
