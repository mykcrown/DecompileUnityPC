using System;
using UnityEngine;

// Token: 0x02000449 RID: 1097
[Serializable]
public class MaterialAnimationDataOrAsset : ISaveAsset, ISerializationCallbackReceiver
{
	// Token: 0x060016BD RID: 5821 RVA: 0x0007B12C File Offset: 0x0007952C
	public MaterialAnimationDataOrAsset Clone()
	{
		MaterialAnimationDataOrAsset materialAnimationDataOrAsset = new MaterialAnimationDataOrAsset();
		if (this.linkedData != null)
		{
			materialAnimationDataOrAsset.linkedData = this.linkedData;
			materialAnimationDataOrAsset.inlineData = null;
		}
		else if (this.inlineData != null)
		{
			materialAnimationDataOrAsset.inlineData = this.inlineData.Clone();
			materialAnimationDataOrAsset.linkedData = null;
		}
		return materialAnimationDataOrAsset;
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x060016BE RID: 5822 RVA: 0x0007B18C File Offset: 0x0007958C
	public MaterialAnimationData Data
	{
		get
		{
			if (this.linkedData != null)
			{
				this.inlineData = null;
				return this.linkedData.Data;
			}
			if (this.inlineData != null)
			{
				this.linkedData = null;
				return this.inlineData;
			}
			this.inlineData = new MaterialAnimationData();
			return this.inlineData;
		}
	}

	// Token: 0x060016BF RID: 5823 RVA: 0x0007B1E7 File Offset: 0x000795E7
	public void SaveAsset()
	{
		if (this.CanSave)
		{
			this.inlineData.SaveAsAsset();
		}
	}

	// Token: 0x060016C0 RID: 5824 RVA: 0x0007B1FF File Offset: 0x000795FF
	public void OnBeforeSerialize()
	{
		if (this.linkedData == null)
		{
			this.inlineDataString = JsonUtility.ToJson(this.inlineData);
		}
		else
		{
			this.inlineDataString = string.Empty;
		}
	}

	// Token: 0x060016C1 RID: 5825 RVA: 0x0007B233 File Offset: 0x00079633
	public void OnAfterDeserialize()
	{
		if (!string.IsNullOrEmpty(this.inlineDataString))
		{
			this.inlineData = JsonUtility.FromJson<MaterialAnimationData>(this.inlineDataString);
			this.inlineDataString = string.Empty;
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x060016C2 RID: 5826 RVA: 0x0007B261 File Offset: 0x00079661
	public bool CanSave
	{
		get
		{
			return this.linkedData == null && this.inlineData != null;
		}
	}

	// Token: 0x04001187 RID: 4487
	public MaterialAnimationDataAsset linkedData;

	// Token: 0x04001188 RID: 4488
	[NonSerialized]
	public MaterialAnimationData inlineData;

	// Token: 0x04001189 RID: 4489
	[SerializeField]
	private string inlineDataString = string.Empty;
}
