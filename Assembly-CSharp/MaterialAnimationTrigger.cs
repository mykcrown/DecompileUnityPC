using System;

// Token: 0x02000442 RID: 1090
[Serializable]
public class MaterialAnimationTrigger : ISaveAsset
{
	// Token: 0x060016B0 RID: 5808 RVA: 0x0007AF06 File Offset: 0x00079306
	public bool MatchesTarget(MaterialAnimationTrigger.TargetType target)
	{
		return this.target == target || this.target == MaterialAnimationTrigger.TargetType.Both;
	}

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x060016B1 RID: 5809 RVA: 0x0007AF20 File Offset: 0x00079320
	public MaterialAnimationData Data
	{
		get
		{
			return this.data.Data;
		}
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x0007AF2D File Offset: 0x0007932D
	public void SaveAsset()
	{
		if (this.CanSave)
		{
			this.data.inlineData.SaveAsAsset();
		}
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x060016B3 RID: 5811 RVA: 0x0007AF4A File Offset: 0x0007934A
	public bool CanSave
	{
		get
		{
			return this.data.linkedData == null && this.data.inlineData != null;
		}
	}

	// Token: 0x0400115B RID: 4443
	public MaterialAnimationTrigger.TargetType target;

	// Token: 0x0400115C RID: 4444
	public int startFrame;

	// Token: 0x0400115D RID: 4445
	public MaterialAnimationDataOrAsset data = new MaterialAnimationDataOrAsset();

	// Token: 0x02000443 RID: 1091
	public enum TargetType
	{
		// Token: 0x0400115F RID: 4447
		Attacker,
		// Token: 0x04001160 RID: 4448
		Target,
		// Token: 0x04001161 RID: 4449
		Both
	}
}
