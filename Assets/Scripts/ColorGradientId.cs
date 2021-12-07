// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class ColorGradientId
{
	public string Id;

	public Gradient Gradient = new Gradient();

	public AnimationCurve Multiplier = AnimationCurve.Constant(0f, 1f, 1f);

	public Color Evaluate(float percent)
	{
		return this.Gradient.Evaluate(percent) * this.Multiplier.Evaluate(percent);
	}
}
