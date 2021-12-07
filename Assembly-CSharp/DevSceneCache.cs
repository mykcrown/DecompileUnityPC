using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E9 RID: 745
public class DevSceneCache : MonoBehaviour
{
	// Token: 0x06000F75 RID: 3957 RVA: 0x0005D97D File Offset: 0x0005BD7D
	public void Add(CharacterData characterData)
	{
		if (!this.characterDataCache.Contains(characterData))
		{
			this.characterDataCache.Add(characterData);
		}
	}

	// Token: 0x04000A28 RID: 2600
	public List<CharacterData> characterDataCache;
}
