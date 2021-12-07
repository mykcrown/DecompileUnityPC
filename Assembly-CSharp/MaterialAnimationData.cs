using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000444 RID: 1092
[Serializable]
public class MaterialAnimationData
{
	// Token: 0x060016B5 RID: 5813 RVA: 0x0007AFB4 File Offset: 0x000793B4
	public MaterialAnimationData Clone()
	{
		MaterialAnimationData materialAnimationData = new MaterialAnimationData();
		materialAnimationData.assetName = this.assetName;
		materialAnimationData.totalFrames = this.totalFrames;
		materialAnimationData.wrap = this.wrap;
		materialAnimationData.affectAllMaterialsOnModel = this.affectAllMaterialsOnModel;
		materialAnimationData.materialTargetId = this.materialTargetId;
		materialAnimationData.shaderVariableName = this.shaderVariableName;
		materialAnimationData.endCondition = this.endCondition;
		materialAnimationData.endFrames = (from endFrame in this.endFrames
		select new MaterialAnimationData.EndFrameData
		{
			frame = endFrame.frame,
			move = endFrame.move
		}).ToList<MaterialAnimationData.EndFrameData>();
		materialAnimationData.valueType = this.valueType;
		materialAnimationData.curve = new AnimationCurve(this.curve.keys);
		materialAnimationData.stringValue = this.stringValue;
		materialAnimationData.stringValueSecondary = this.stringValueSecondary;
		materialAnimationData.spawnOnCompletion = new MaterialAnimationDataOrAsset[this.spawnOnCompletion.Length];
		for (int i = 0; i < this.spawnOnCompletion.Length; i++)
		{
			materialAnimationData.spawnOnCompletion[i] = this.spawnOnCompletion[i].Clone();
		}
		return materialAnimationData;
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x0007B0CA File Offset: 0x000794CA
	public void SaveAsAsset()
	{
	}

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x060016B7 RID: 5815 RVA: 0x0007B0CC File Offset: 0x000794CC
	public bool Enabled
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x060016B8 RID: 5816 RVA: 0x0007B0CF File Offset: 0x000794CF
	public int ID
	{
		get
		{
			return this.assetName.GetHashCode();
		}
	}

	// Token: 0x1700046D RID: 1133
	// (get) Token: 0x060016B9 RID: 5817 RVA: 0x0007B0DC File Offset: 0x000794DC
	public string Key
	{
		get
		{
			return this.assetName;
		}
	}

	// Token: 0x04001162 RID: 4450
	public string assetName = "New Material Animation";

	// Token: 0x04001163 RID: 4451
	public List<MaterialAnimationData.EndFrameData> endFrames;

	// Token: 0x04001164 RID: 4452
	public int totalFrames;

	// Token: 0x04001165 RID: 4453
	public MaterialAnimationData.WrapMode wrap;

	// Token: 0x04001166 RID: 4454
	public bool affectAllMaterialsOnModel;

	// Token: 0x04001167 RID: 4455
	public string materialTargetId;

	// Token: 0x04001168 RID: 4456
	public string shaderVariableName;

	// Token: 0x04001169 RID: 4457
	public MaterialAnimationData.TriggerType endCondition;

	// Token: 0x0400116A RID: 4458
	public MaterialAnimationData.Type valueType;

	// Token: 0x0400116B RID: 4459
	public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400116C RID: 4460
	public string stringValue;

	// Token: 0x0400116D RID: 4461
	public string stringValueSecondary;

	// Token: 0x0400116E RID: 4462
	public MaterialAnimationDataOrAsset[] spawnOnCompletion = new MaterialAnimationDataOrAsset[0];

	// Token: 0x02000445 RID: 1093
	public enum TriggerType
	{
		// Token: 0x04001171 RID: 4465
		MoveEnd = 1,
		// Token: 0x04001172 RID: 4466
		Land,
		// Token: 0x04001173 RID: 4467
		TakeDamage = 4,
		// Token: 0x04001174 RID: 4468
		DealDamage = 8,
		// Token: 0x04001175 RID: 4469
		MoveFrame = 16,
		// Token: 0x04001176 RID: 4470
		Grabbed = 32,
		// Token: 0x04001177 RID: 4471
		Flinched = 64,
		// Token: 0x04001178 RID: 4472
		Died = 128
	}

	// Token: 0x02000446 RID: 1094
	public enum Type
	{
		// Token: 0x0400117A RID: 4474
		Float,
		// Token: 0x0400117B RID: 4475
		Texture,
		// Token: 0x0400117C RID: 4476
		Color,
		// Token: 0x0400117D RID: 4477
		ColorTween,
		// Token: 0x0400117E RID: 4478
		ColorGradient,
		// Token: 0x0400117F RID: 4479
		Material
	}

	// Token: 0x02000447 RID: 1095
	public enum WrapMode
	{
		// Token: 0x04001181 RID: 4481
		None,
		// Token: 0x04001182 RID: 4482
		Loop,
		// Token: 0x04001183 RID: 4483
		PingPong,
		// Token: 0x04001184 RID: 4484
		Clamp
	}

	// Token: 0x02000448 RID: 1096
	[Serializable]
	public class EndFrameData
	{
		// Token: 0x04001185 RID: 4485
		public MoveData move;

		// Token: 0x04001186 RID: 4486
		public int frame;
	}
}
