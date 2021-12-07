using System;
using System.Collections.Generic;

// Token: 0x0200074F RID: 1871
public interface IUserTauntsSource
{
	// Token: 0x06002E74 RID: 11892
	Dictionary<CharacterID, Dictionary<TauntSlot, EquipmentID>> GetSourceData();
}
