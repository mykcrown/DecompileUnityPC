// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class ChargeThresholdMoveData
{
	public int chargeFramesNeeded;

	public MoveData moveData;

	public List<ParticleData> particles = new List<ParticleData>();

	public AudioData audio = default(AudioData);
}
