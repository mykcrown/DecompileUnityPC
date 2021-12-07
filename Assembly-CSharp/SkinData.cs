using System;
using UnityEngine;

// Token: 0x02000606 RID: 1542
[Serializable]
public class SkinData : ScriptableObject, IDefaultableData
{
	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x060025E6 RID: 9702 RVA: 0x000BB8EE File Offset: 0x000B9CEE
	public Sprite battlePortrait
	{
		get
		{
			return this.battlePortraitFile.obj;
		}
	}

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x060025E7 RID: 9703 RVA: 0x000BB8FB File Offset: 0x000B9CFB
	public Sprite battlePortraitGrey
	{
		get
		{
			return this.battlePortraitGreyFile.obj;
		}
	}

	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x060025E8 RID: 9704 RVA: 0x000BB908 File Offset: 0x000B9D08
	public Sprite crewsHudPortrait
	{
		get
		{
			return this.crewsHudPortraitFile.obj;
		}
	}

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x060025E9 RID: 9705 RVA: 0x000BB915 File Offset: 0x000B9D15
	public GameObject characterPrefab
	{
		get
		{
			return this.characterPrefabFile.obj;
		}
	}

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x060025EA RID: 9706 RVA: 0x000BB922 File Offset: 0x000B9D22
	public GameObject combinedPrefab
	{
		get
		{
			return this.combinedPrefabFile.obj;
		}
	}

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x060025EB RID: 9707 RVA: 0x000BB92F File Offset: 0x000B9D2F
	public GameObject partnerPrefab
	{
		get
		{
			return this.partnerPrefabFile.obj;
		}
	}

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x060025EC RID: 9708 RVA: 0x000BB93C File Offset: 0x000B9D3C
	public GameObject combinedPartnerPrefab
	{
		get
		{
			return this.combinedPartnerPrefabFile.obj;
		}
	}

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x060025ED RID: 9709 RVA: 0x000BB949 File Offset: 0x000B9D49
	public GameObject lastModelImportFBX
	{
		get
		{
			return this.lastModelImportFBXFile.obj;
		}
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x060025EE RID: 9710 RVA: 0x000BB956 File Offset: 0x000B9D56
	public SkinDefinition skinDefinition
	{
		get
		{
			return this.skinDefinitionFile.obj;
		}
	}

	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x060025EF RID: 9711 RVA: 0x000BB963 File Offset: 0x000B9D63
	public bool enabled
	{
		get
		{
			return this.skinDefinition.enabled;
		}
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x060025F0 RID: 9712 RVA: 0x000BB970 File Offset: 0x000B9D70
	public bool demoEnabled
	{
		get
		{
			return this.skinDefinition.demoEnabled;
		}
	}

	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x060025F1 RID: 9713 RVA: 0x000BB97D File Offset: 0x000B9D7D
	public int priority
	{
		get
		{
			return this.skinDefinition.priority;
		}
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x060025F2 RID: 9714 RVA: 0x000BB98A File Offset: 0x000B9D8A
	public string skinName
	{
		get
		{
			return (!(this.skinDefinition == null)) ? this.skinDefinition.skinName : "??? ";
		}
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x060025F3 RID: 9715 RVA: 0x000BB9B2 File Offset: 0x000B9DB2
	public string uniqueKey
	{
		get
		{
			return this.skinDefinition.uniqueKey;
		}
	}

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x060025F4 RID: 9716 RVA: 0x000BB9BF File Offset: 0x000B9DBF
	public bool isDefault
	{
		get
		{
			return this.skinDefinition.isDefault;
		}
	}

	// Token: 0x17000960 RID: 2400
	// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000BB9CC File Offset: 0x000B9DCC
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

	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000BB9EC File Offset: 0x000B9DEC
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

	// Token: 0x17000962 RID: 2402
	// (get) Token: 0x060025F7 RID: 9719 RVA: 0x000BBA0C File Offset: 0x000B9E0C
	public bool IsDefaultData
	{
		get
		{
			return this.isDefault;
		}
	}

	// Token: 0x04001BD1 RID: 7121
	public SpriteFile battlePortraitFile = new SpriteFile();

	// Token: 0x04001BD2 RID: 7122
	public SpriteFile battlePortraitGreyFile = new SpriteFile();

	// Token: 0x04001BD3 RID: 7123
	public SpriteFile crewsHudPortraitFile = new SpriteFile();

	// Token: 0x04001BD4 RID: 7124
	public GameObjectFile characterPrefabFile = new GameObjectFile();

	// Token: 0x04001BD5 RID: 7125
	public GameObjectFile combinedPrefabFile = new GameObjectFile();

	// Token: 0x04001BD6 RID: 7126
	public GameObjectFile partnerPrefabFile = new GameObjectFile();

	// Token: 0x04001BD7 RID: 7127
	public GameObjectFile combinedPartnerPrefabFile = new GameObjectFile();

	// Token: 0x04001BD8 RID: 7128
	public GameObjectFile lastModelImportFBXFile = new GameObjectFile();

	// Token: 0x04001BD9 RID: 7129
	public SkinDefinitionFile skinDefinitionFile = new SkinDefinitionFile();

	// Token: 0x04001BDA RID: 7130
	public string skinDefinitionFileName;

	// Token: 0x04001BDB RID: 7131
	public bool overridePortraitOffset;

	// Token: 0x04001BDC RID: 7132
	public Vector2Int portraitOffset = Vector2Int.zero;

	// Token: 0x04001BDD RID: 7133
	public bool overrideVictoryPortraitOffset;

	// Token: 0x04001BDE RID: 7134
	public Vector2Int victoryPortraitOffset = Vector2Int.zero;

	// Token: 0x04001BDF RID: 7135
	public Vector2 victoryPortraitScale = Vector2.one;

	// Token: 0x04001BE0 RID: 7136
	public float lastModelImportScale = 1f;
}
