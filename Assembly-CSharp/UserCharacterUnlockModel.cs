using System;
using System.Collections.Generic;

// Token: 0x020006DE RID: 1758
public class UserCharacterUnlockModel : IUserCharacterUnlockModel
{
	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x06002C35 RID: 11317 RVA: 0x000E522F File Offset: 0x000E362F
	// (set) Token: 0x06002C36 RID: 11318 RVA: 0x000E5237 File Offset: 0x000E3637
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x06002C37 RID: 11319 RVA: 0x000E5240 File Offset: 0x000E3640
	// (set) Token: 0x06002C38 RID: 11320 RVA: 0x000E5248 File Offset: 0x000E3648
	[Inject]
	public IUserProAccountUnlockedModel userProAccountUnlockedModel { get; set; }

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x06002C39 RID: 11321 RVA: 0x000E5251 File Offset: 0x000E3651
	// (set) Token: 0x06002C3A RID: 11322 RVA: 0x000E5259 File Offset: 0x000E3659
	[Inject]
	public IUserCharacterUnlockSource characterSource { get; set; }

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000E5262 File Offset: 0x000E3662
	// (set) Token: 0x06002C3C RID: 11324 RVA: 0x000E526A File Offset: 0x000E366A
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06002C3D RID: 11325 RVA: 0x000E5273 File Offset: 0x000E3673
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserCharacterUnlockModel.SOURCE_UPDATED, new Action(this.updateUnlockModel));
		this.updateUnlockModel();
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x000E5298 File Offset: 0x000E3698
	private void updateUnlockModel()
	{
		foreach (CharacterID characterId in this.characterSource.GetOwned())
		{
			this.SetUnlocked(characterId, false);
		}
		this.signalBus.Dispatch(UserCharacterUnlockModel.UPDATED);
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x000E52E4 File Offset: 0x000E36E4
	public bool IsUnlocked(CharacterID characterId)
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.UnlockEverythingOnline))
		{
			return true;
		}
		if (characterId == CharacterID.Random || characterId == CharacterID.None || characterId == CharacterID.Any)
		{
			return true;
		}
		if (this.userProAccountUnlockedModel.IsUnlocked())
		{
			return true;
		}
		bool result = false;
		this.isUnlocked.TryGetValue(characterId, out result);
		return result;
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x000E5340 File Offset: 0x000E3740
	public void SetUnlocked(CharacterID characterId, bool dispatchUpdate = true)
	{
		this.isUnlocked[characterId] = true;
		if (dispatchUpdate)
		{
			this.signalBus.Dispatch(UserCharacterUnlockModel.UPDATED);
		}
	}

	// Token: 0x06002C41 RID: 11329 RVA: 0x000E5365 File Offset: 0x000E3765
	public bool IsUnlockedInGameMode(CharacterID characterId, GameMode gameMode)
	{
		return gameMode == GameMode.Training || this.IsUnlocked(characterId);
	}

	// Token: 0x04001F6B RID: 8043
	public static string UPDATED = "UserCharacterUnlockModel.UPDATED";

	// Token: 0x04001F6C RID: 8044
	public static string SOURCE_UPDATED = "UserCharacterUnlockModel.SOURCE_UPDATED";

	// Token: 0x04001F71 RID: 8049
	private Dictionary<CharacterID, bool> isUnlocked = new Dictionary<CharacterID, bool>();
}
