// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class MaterialAnimationTrigger : ISaveAsset
{
	public enum TargetType
	{
		Attacker,
		Target,
		Both
	}

	public MaterialAnimationTrigger.TargetType target;

	public int startFrame;

	public MaterialAnimationDataOrAsset data = new MaterialAnimationDataOrAsset();

	public MaterialAnimationData Data
	{
		get
		{
			return this.data.Data;
		}
	}

	public bool CanSave
	{
		get
		{
			return this.data.linkedData == null && this.data.inlineData != null;
		}
	}

	public bool MatchesTarget(MaterialAnimationTrigger.TargetType target)
	{
		return this.target == target || this.target == MaterialAnimationTrigger.TargetType.Both;
	}

	public void SaveAsset()
	{
		if (this.CanSave)
		{
			this.data.inlineData.SaveAsAsset();
		}
	}
}
