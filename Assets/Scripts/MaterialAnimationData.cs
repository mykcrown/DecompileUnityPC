// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class MaterialAnimationData
{
	public enum TriggerType
	{
		MoveEnd = 1,
		Land,
		TakeDamage = 4,
		DealDamage = 8,
		MoveFrame = 16,
		Grabbed = 32,
		Flinched = 64,
		Died = 128
	}

	public enum Type
	{
		Float,
		Texture,
		Color,
		ColorTween,
		ColorGradient,
		Material
	}

	public enum WrapMode
	{
		None,
		Loop,
		PingPong,
		Clamp
	}

	[Serializable]
	public class EndFrameData
	{
		public MoveData move;

		public int frame;
	}

	public string assetName = "New Material Animation";

	public List<MaterialAnimationData.EndFrameData> endFrames;

	public int totalFrames;

	public MaterialAnimationData.WrapMode wrap;

	public bool affectAllMaterialsOnModel;

	public string materialTargetId;

	public string shaderVariableName;

	public MaterialAnimationData.TriggerType endCondition;

	public MaterialAnimationData.Type valueType;

	public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	public string stringValue;

	public string stringValueSecondary;

	public MaterialAnimationDataOrAsset[] spawnOnCompletion = new MaterialAnimationDataOrAsset[0];

	private static Func<MaterialAnimationData.EndFrameData, MaterialAnimationData.EndFrameData> __f__am_cache0;

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
			return this.assetName.GetHashCode();
		}
	}

	public string Key
	{
		get
		{
			return this.assetName;
		}
	}

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
		MaterialAnimationData arg_88_0 = materialAnimationData;
		IEnumerable<MaterialAnimationData.EndFrameData> arg_7E_0 = this.endFrames;
		if (MaterialAnimationData.__f__am_cache0 == null)
		{
			MaterialAnimationData.__f__am_cache0 = new Func<MaterialAnimationData.EndFrameData, MaterialAnimationData.EndFrameData>(MaterialAnimationData._Clone_m__0);
		}
		arg_88_0.endFrames = arg_7E_0.Select(MaterialAnimationData.__f__am_cache0).ToList<MaterialAnimationData.EndFrameData>();
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

	public void SaveAsAsset()
	{
	}

	private static MaterialAnimationData.EndFrameData _Clone_m__0(MaterialAnimationData.EndFrameData endFrame)
	{
		return new MaterialAnimationData.EndFrameData
		{
			frame = endFrame.frame,
			move = endFrame.move
		};
	}
}
