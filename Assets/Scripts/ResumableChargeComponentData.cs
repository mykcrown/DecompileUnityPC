// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class ResumableChargeComponentData
{
	public int maxChargeFrames;

	[FormerlySerializedAs("onChargeCompleteMove")]
	public MoveData onChargeCompleteMoveGrounded;

	public MoveData onChargeCompleteMoveAerial;

	public List<ParticleData> fullyChargedLoopParticles;

	public List<ParticleData> chargeLevel2LoopParticles;
}
