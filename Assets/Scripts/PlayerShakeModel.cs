// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class PlayerShakeModel : CloneableObject
{
	public int framesUntilReduction;

	public float linearReduction;

	public int frameCount;

	public float amplitude;

	public float startingAmplitude;

	public float wavelength = 1f;

	public float xFactor;

	public float yFactor;

	public void CopyTo(PlayerShakeModel targetIn)
	{
		targetIn.framesUntilReduction = this.framesUntilReduction;
		targetIn.linearReduction = this.linearReduction;
		targetIn.frameCount = this.frameCount;
		targetIn.amplitude = this.amplitude;
		targetIn.startingAmplitude = this.startingAmplitude;
		targetIn.wavelength = this.wavelength;
		targetIn.xFactor = this.xFactor;
		targetIn.yFactor = this.yFactor;
	}
}
