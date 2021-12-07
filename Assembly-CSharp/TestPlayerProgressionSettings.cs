using System;

// Token: 0x02000407 RID: 1031
[Serializable]
public class TestPlayerProgressionSettings
{
	// Token: 0x04001053 RID: 4179
	public CharacterID characterID = CharacterID.Ashani;

	// Token: 0x04001054 RID: 4180
	public int playerXpStart;

	// Token: 0x04001055 RID: 4181
	public int playerLevelStart;

	// Token: 0x04001056 RID: 4182
	public int playerXpEarned = 50;

	// Token: 0x04001057 RID: 4183
	public int playerXpPerLevel = 100;

	// Token: 0x04001058 RID: 4184
	public int playerLevelCount = 5;

	// Token: 0x04001059 RID: 4185
	public int characterXpStart;

	// Token: 0x0400105A RID: 4186
	public int characterLevelStart;

	// Token: 0x0400105B RID: 4187
	public int characterXpEarned = 50;

	// Token: 0x0400105C RID: 4188
	public int characterXpPerLevel = 100;

	// Token: 0x0400105D RID: 4189
	public int characterLevelCount = 5;

	// Token: 0x0400105E RID: 4190
	public bool fwotd;
}
