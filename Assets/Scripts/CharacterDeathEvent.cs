// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class CharacterDeathEvent : CharacterEvent
{
	public bool wasEliminated;

	public Vector3F velocity;

	public CharacterDeathEvent(PlayerController pCharacter, bool wasEliminated, Vector3F velocity) : base(pCharacter)
	{
		this.wasEliminated = wasEliminated;
		this.velocity = velocity;
	}
}
