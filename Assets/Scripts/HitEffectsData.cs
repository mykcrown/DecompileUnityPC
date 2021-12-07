// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class HitEffectsData : IPreloadedGameAsset
{
	public HitParticleData[] hitParticles = new HitParticleData[0];

	public AudioData attackSound;

	public AudioData altAttackSound;

	[NonSerialized]
	public bool editorToggle;

	public void RegisterPreload(PreloadContext context)
	{
		HitParticleData[] array = this.hitParticles;
		for (int i = 0; i < array.Length; i++)
		{
			HitParticleData hitParticleData = array[i];
			if (!(hitParticleData == null))
			{
				hitParticleData.RegisterPreload(context);
			}
		}
	}
}
