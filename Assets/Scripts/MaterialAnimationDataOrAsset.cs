// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class MaterialAnimationDataOrAsset : ISaveAsset, ISerializationCallbackReceiver
{
	public MaterialAnimationDataAsset linkedData;

	[NonSerialized]
	public MaterialAnimationData inlineData;

	[SerializeField]
	private string inlineDataString = string.Empty;

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

	public bool CanSave
	{
		get
		{
			return this.linkedData == null && this.inlineData != null;
		}
	}

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

	public void SaveAsset()
	{
		if (this.CanSave)
		{
			this.inlineData.SaveAsAsset();
		}
	}

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

	public void OnAfterDeserialize()
	{
		if (!string.IsNullOrEmpty(this.inlineDataString))
		{
			this.inlineData = JsonUtility.FromJson<MaterialAnimationData>(this.inlineDataString);
			this.inlineDataString = string.Empty;
		}
	}
}
