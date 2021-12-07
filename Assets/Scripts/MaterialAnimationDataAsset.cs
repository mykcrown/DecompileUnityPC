// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class MaterialAnimationDataAsset : ScriptableObject, IGameDataElement
{
	public MaterialAnimationData data = new MaterialAnimationData();

	LocalizationData IGameDataElement.Localization
	{
		get
		{
			return null;
		}
	}

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

	public bool Enabled
	{
		get
		{
			return true;
		}
	}

	public int ID
	{
		get
		{
			return (this.data != null) ? this.data.assetName.GetHashCode() : (-1);
		}
	}

	public string Key
	{
		get
		{
			return (this.data != null) ? this.data.assetName : "NULL";
		}
	}
}
