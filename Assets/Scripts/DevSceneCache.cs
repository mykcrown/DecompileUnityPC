// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DevSceneCache : MonoBehaviour
{
	public List<CharacterData> characterDataCache;

	public void Add(CharacterData characterData)
	{
		if (!this.characterDataCache.Contains(characterData))
		{
			this.characterDataCache.Add(characterData);
		}
	}
}
