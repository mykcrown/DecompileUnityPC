// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;

[Serializable]
public class FloatCharacterComponentData
{
	public CharacterPhysicsOverride physicsData = new CharacterPhysicsOverride();

	public MoveLabel[] moveWhitelist = new MoveLabel[0];

	public CharacterAnimation floatForwardAnim = new CharacterAnimation();

	public CharacterAnimation floatBackAnim = new CharacterAnimation();

	public CharacterAnimation floatForwardToBackAnim = new CharacterAnimation();

	public CharacterAnimation floatBackToForwardAnim = new CharacterAnimation();

	public Fixed verticalAcceleration;

	public Fixed verticalFriction;

	public Fixed verticalMaxSpeed;

	public Fixed maxRollAngle;

	public bool deferEndUntilAerialMoveComplete;

	public bool allowSpecialInputCancel;

	public bool allowJumpCancel;

	public bool allowFastFallCancel;

	public bool allowFallThroughPlatform;

	public List<ParticleData> loopParticles = new List<ParticleData>();

	public AudioData loopAudio = default(AudioData);
}
