using System;
using UnityEngine;

// Token: 0x02000614 RID: 1556
public interface IRespawnPlatformLocator
{
	// Token: 0x06002661 RID: 9825
	GameObject GetPrefab(PlayerSelectionInfo playerInfo);

	// Token: 0x06002662 RID: 9826
	EquippableItem GetCustomItem(PlayerSelectionInfo playerInfo);
}
