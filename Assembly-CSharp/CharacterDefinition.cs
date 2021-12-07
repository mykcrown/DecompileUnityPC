using System;
using UnityEngine;

// Token: 0x02000585 RID: 1413
[Serializable]
public class CharacterDefinition : ScriptableObject
{
	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06001FEE RID: 8174 RVA: 0x000A222B File Offset: 0x000A062B
	// (set) Token: 0x06001FEF RID: 8175 RVA: 0x000A2233 File Offset: 0x000A0633
	public int ordinal { get; set; }

	// Token: 0x04001944 RID: 6468
	public CharacterID characterID;

	// Token: 0x04001945 RID: 6469
	public bool enabled;

	// Token: 0x04001946 RID: 6470
	public bool demoEnabled = true;

	// Token: 0x04001947 RID: 6471
	public bool isRandom;

	// Token: 0x04001948 RID: 6472
	public bool isPartner;

	// Token: 0x04001949 RID: 6473
	public CharacterDefinition totemPartner;

	// Token: 0x0400194A RID: 6474
	public string characterName;

	// Token: 0x0400194B RID: 6475
	public string defaultVictoryPose;
}
