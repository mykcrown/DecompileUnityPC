// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class UserCharacterUnlockModel : IUserCharacterUnlockModel
{
	public static string UPDATED = "UserCharacterUnlockModel.UPDATED";

	public static string SOURCE_UPDATED = "UserCharacterUnlockModel.SOURCE_UPDATED";

	private Dictionary<CharacterID, bool> isUnlocked = new Dictionary<CharacterID, bool>();

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IUserProAccountUnlockedModel userProAccountUnlockedModel
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockSource characterSource
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserCharacterUnlockModel.SOURCE_UPDATED, new Action(this.updateUnlockModel));
		this.updateUnlockModel();
	}

	private void updateUnlockModel()
	{
		CharacterID[] owned = this.characterSource.GetOwned();
		for (int i = 0; i < owned.Length; i++)
		{
			CharacterID characterId = owned[i];
			this.SetUnlocked(characterId, false);
		}
		this.signalBus.Dispatch(UserCharacterUnlockModel.UPDATED);
	}

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

	public void SetUnlocked(CharacterID characterId, bool dispatchUpdate = true)
	{
		this.isUnlocked[characterId] = true;
		if (dispatchUpdate)
		{
			this.signalBus.Dispatch(UserCharacterUnlockModel.UPDATED);
		}
	}

	public bool IsUnlockedInGameMode(CharacterID characterId, GameMode gameMode)
	{
		return gameMode == GameMode.Training || this.IsUnlocked(characterId);
	}
}
