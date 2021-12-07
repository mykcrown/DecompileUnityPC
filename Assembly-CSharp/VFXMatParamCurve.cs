using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B8B RID: 2955
public class VFXMatParamCurve : VFXBehavior
{
	// Token: 0x0600554D RID: 21837 RVA: 0x001B5250 File Offset: 0x001B3650
	public override void OnVFXInit()
	{
		this.theRenderer = base.GetComponent<Renderer>();
	}

	// Token: 0x0600554E RID: 21838 RVA: 0x001B525E File Offset: 0x001B365E
	public override void OnVFXStart()
	{
		this.time = 0f;
	}

	// Token: 0x0600554F RID: 21839 RVA: 0x001B526C File Offset: 0x001B366C
	private void Update()
	{
		this.time += WTime.deltaTime;
		float num = (this.duration <= 0f) ? 0f : (this.time / this.duration);
		foreach (VFXMatParamCurve.MatParamData matParamData in this.matParams)
		{
			float value = matParamData.panTimeCurve.Evaluate(num);
			for (int i = 0; i < this.theRenderer.materials.Length; i++)
			{
				Material material = this.theRenderer.materials[i];
				if (matParamData.onlyTargetsOneMaterial || matParamData.materialIndex == i)
				{
					material.SetFloat(matParamData.paramID, value);
				}
			}
		}
	}

	// Token: 0x04003633 RID: 13875
	public float duration;

	// Token: 0x04003634 RID: 13876
	public List<VFXMatParamCurve.MatParamData> matParams = new List<VFXMatParamCurve.MatParamData>();

	// Token: 0x04003635 RID: 13877
	private Renderer theRenderer;

	// Token: 0x04003636 RID: 13878
	private float time;

	// Token: 0x02000B8C RID: 2956
	[Serializable]
	public class MatParamData
	{
		// Token: 0x04003637 RID: 13879
		public string paramID;

		// Token: 0x04003638 RID: 13880
		public AnimationCurve panTimeCurve;

		// Token: 0x04003639 RID: 13881
		public bool onlyTargetsOneMaterial;

		// Token: 0x0400363A RID: 13882
		public int materialIndex;
	}
}
