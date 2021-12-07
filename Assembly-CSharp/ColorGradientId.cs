using System;
using UnityEngine;

// Token: 0x0200044F RID: 1103
[Serializable]
public class ColorGradientId
{
	// Token: 0x060016E2 RID: 5858 RVA: 0x0007BC1D File Offset: 0x0007A01D
	public Color Evaluate(float percent)
	{
		return this.Gradient.Evaluate(percent) * this.Multiplier.Evaluate(percent);
	}

	// Token: 0x04001194 RID: 4500
	public string Id;

	// Token: 0x04001195 RID: 4501
	public Gradient Gradient = new Gradient();

	// Token: 0x04001196 RID: 4502
	public AnimationCurve Multiplier = AnimationCurve.Constant(0f, 1f, 1f);
}
