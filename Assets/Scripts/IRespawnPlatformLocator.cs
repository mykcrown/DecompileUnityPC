// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public interface IRespawnPlatformLocator
{
	GameObject GetPrefab(PlayerSelectionInfo playerInfo);

	EquippableItem GetCustomItem(PlayerSelectionInfo playerInfo);
}
