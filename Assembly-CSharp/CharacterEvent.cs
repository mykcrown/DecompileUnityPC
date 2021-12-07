using System;

// Token: 0x02000AD8 RID: 2776
public class CharacterEvent : GameEvent
{
	// Token: 0x060050BB RID: 20667 RVA: 0x001506B3 File Offset: 0x0014EAB3
	public CharacterEvent(PlayerController pCharacter)
	{
		this.character = pCharacter;
	}

	// Token: 0x170012F7 RID: 4855
	// (get) Token: 0x060050BC RID: 20668 RVA: 0x001506C2 File Offset: 0x0014EAC2
	// (set) Token: 0x060050BD RID: 20669 RVA: 0x001506CA File Offset: 0x0014EACA
	public PlayerController character { get; private set; }
}
