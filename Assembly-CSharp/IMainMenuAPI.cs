using System;

// Token: 0x020009A9 RID: 2473
public interface IMainMenuAPI : IStartupLoader
{
	// Token: 0x060043DF RID: 17375
	void RandomizeCharacter();

	// Token: 0x060043E0 RID: 17376
	string GetCurrentStage();

	// Token: 0x1700100B RID: 4107
	// (get) Token: 0x060043E1 RID: 17377
	// (set) Token: 0x060043E2 RID: 17378
	CharacterID characterHighlight { get; set; }

	// Token: 0x060043E3 RID: 17379
	SkinDefinition GetCharacterSkin();

	// Token: 0x1700100C RID: 4108
	// (get) Token: 0x060043E4 RID: 17380
	// (set) Token: 0x060043E5 RID: 17381
	bool ShowedLoginBonus { get; set; }

	// Token: 0x060043E6 RID: 17382
	SoundKey GetCurrentMusic();
}
