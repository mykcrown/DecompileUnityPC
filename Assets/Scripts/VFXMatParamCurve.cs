// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXMatParamCurve : VFXBehavior
{
	[Serializable]
	public class MatParamData
	{
		public string paramID;

		public AnimationCurve panTimeCurve;

		public bool onlyTargetsOneMaterial;

		public int materialIndex;
	}

	public float duration;

	public List<VFXMatParamCurve.MatParamData> matParams = new List<VFXMatParamCurve.MatParamData>();

	private Renderer theRenderer;

	private float time;

	public override void OnVFXInit()
	{
		this.theRenderer = base.GetComponent<Renderer>();
	}

	public override void OnVFXStart()
	{
		this.time = 0f;
	}

	private void Update()
	{
		this.time += WTime.deltaTime;
		float num = (this.duration <= 0f) ? 0f : (this.time / this.duration);
		foreach (VFXMatParamCurve.MatParamData current in this.matParams)
		{
			float value = current.panTimeCurve.Evaluate(num);
			for (int i = 0; i < this.theRenderer.materials.Length; i++)
			{
				Material material = this.theRenderer.materials[i];
				if (current.onlyTargetsOneMaterial || current.materialIndex == i)
				{
					material.SetFloat(current.paramID, value);
				}
			}
		}
	}
}
