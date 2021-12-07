using System;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class MaterialAnimationDataAsset : ScriptableObject, IGameDataElement
{
	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x060016C4 RID: 5828 RVA: 0x0007B296 File Offset: 0x00079696
	public MaterialAnimationData Data
	{
		get
		{
			if (this.data == null)
			{
				this.data = new MaterialAnimationData();
			}
			return this.data;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x060016C5 RID: 5829 RVA: 0x0007B2B4 File Offset: 0x000796B4
	public bool Enabled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x060016C6 RID: 5830 RVA: 0x0007B2B7 File Offset: 0x000796B7
	public int ID
	{
		get
		{
			return (this.data != null) ? this.data.assetName.GetHashCode() : -1;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x060016C7 RID: 5831 RVA: 0x0007B2DA File Offset: 0x000796DA
	public string Key
	{
		get
		{
			return (this.data != null) ? this.data.assetName : "NULL";
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x060016C8 RID: 5832 RVA: 0x0007B2FC File Offset: 0x000796FC
	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0400118A RID: 4490
	public MaterialAnimationData data = new MaterialAnimationData();
}
