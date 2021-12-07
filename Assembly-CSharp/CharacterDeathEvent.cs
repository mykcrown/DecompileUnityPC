using System;
using FixedPoint;

// Token: 0x02000ADC RID: 2780
public class CharacterDeathEvent : CharacterEvent
{
	// Token: 0x060050C1 RID: 20673 RVA: 0x0015071F File Offset: 0x0014EB1F
	public CharacterDeathEvent(PlayerController pCharacter, bool wasEliminated, Vector3F velocity) : base(pCharacter)
	{
		this.wasEliminated = wasEliminated;
		this.velocity = velocity;
	}

	// Token: 0x0400340C RID: 13324
	public bool wasEliminated;

	// Token: 0x0400340D RID: 13325
	public Vector3F velocity;
}
