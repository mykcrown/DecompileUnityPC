// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class TauntSettings
{
	public int emoteCooldownFrames = 300;

	public AudioData emoteCooldownSound;

	public bool useEmotesPerTime = true;

	public int emotesPerTimeFrames = 300;

	public int emotesPerTimeMax = 5;

	public Fixed holoOffsetX = 1;

	public Fixed holoOffsetY = (Fixed)2.5;
}
