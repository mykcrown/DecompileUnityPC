// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class SkinData : ScriptableObject, IDefaultableData
{
	public SpriteFile battlePortraitFile = new SpriteFile();

	public SpriteFile battlePortraitGreyFile = new SpriteFile();

	public SpriteFile crewsHudPortraitFile = new SpriteFile();

	public GameObjectFile characterPrefabFile = new GameObjectFile();

	public GameObjectFile combinedPrefabFile = new GameObjectFile();

	public GameObjectFile partnerPrefabFile = new GameObjectFile();

	public GameObjectFile combinedPartnerPrefabFile = new GameObjectFile();

	public GameObjectFile lastModelImportFBXFile = new GameObjectFile();

	public SkinDefinitionFile skinDefinitionFile = new SkinDefinitionFile();

	public string skinDefinitionFileName;

	public bool overridePortraitOffset;

	public Vector2Int portraitOffset = Vector2Int.zero;

	public bool overrideVictoryPortraitOffset;

	public Vector2Int victoryPortraitOffset = Vector2Int.zero;

	public Vector2 victoryPortraitScale = Vector2.one;

	public float lastModelImportScale = 1f;

	public Sprite battlePortrait
	{
		get
		{
			return this.battlePortraitFile.obj;
		}
	}

	public Sprite battlePortraitGrey
	{
		get
		{
			return this.battlePortraitGreyFile.obj;
		}
	}

	public Sprite crewsHudPortrait
	{
		get
		{
			return this.crewsHudPortraitFile.obj;
		}
	}

	public GameObject characterPrefab
	{
		get
		{
			return this.characterPrefabFile.obj;
		}
	}

	public GameObject combinedPrefab
	{
		get
		{
			return this.combinedPrefabFile.obj;
		}
	}

	public GameObject partnerPrefab
	{
		get
		{
			return this.partnerPrefabFile.obj;
		}
	}

	public GameObject combinedPartnerPrefab
	{
		get
		{
			return this.combinedPartnerPrefabFile.obj;
		}
	}

	public GameObject lastModelImportFBX
	{
		get
		{
			return this.lastModelImportFBXFile.obj;
		}
	}

	public SkinDefinition skinDefinition
	{
		get
		{
			return this.skinDefinitionFile.obj;
		}
	}

	public bool enabled
	{
		get
		{
			return this.skinDefinition.enabled;
		}
	}

	public bool demoEnabled
	{
		get
		{
			return this.skinDefinition.demoEnabled;
		}
	}

	public int priority
	{
		get
		{
			return this.skinDefinition.priority;
		}
	}

	public string skinName
	{
		get
		{
			return (!(this.skinDefinition == null)) ? this.skinDefinition.skinName : "??? ";
		}
	}

	public string uniqueKey
	{
		get
		{
			return this.skinDefinition.uniqueKey;
		}
	}

	public bool isDefault
	{
		get
		{
			return this.skinDefinition.isDefault;
		}
	}

	public GameObject CharacterPrefab
	{
		get
		{
			if (this.combinedPrefab != null)
			{
				return this.combinedPrefab;
			}
			return this.characterPrefab;
		}
	}

	public GameObject PartnerPrefab
	{
		get
		{
			if (this.combinedPartnerPrefab != null)
			{
				return this.combinedPartnerPrefab;
			}
			return this.partnerPrefab;
		}
	}

	public bool IsDefaultData
	{
		get
		{
			return this.isDefault;
		}
	}
}
