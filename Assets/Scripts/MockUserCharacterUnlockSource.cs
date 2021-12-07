// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class MockUserCharacterUnlockSource : IUserCharacterUnlockSource
{
	private CharacterID[] ownedCharacters;

	private bool isConnected;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IServerConnectionManager serverConnectionManager
	{
		get;
		set;
	}

	[Inject]
	public GameEnvironmentData environmentData
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerConnectionUpdate));
		this.updateCharacterSource();
	}

	private void onServerConnectionUpdate()
	{
		if (!this.isConnected || this.serverConnectionManager.IsConnectedToNexus != this.isConnected)
		{
			this.isConnected = this.serverConnectionManager.IsConnectedToNexus;
			if (this.isConnected)
			{
				this.updateCharacterSource();
			}
		}
	}

	private void updateCharacterSource()
	{
		if (this.environmentData.toggles.GetToggle(FeatureID.UnlockEverything))
		{
			List<CharacterID> list = new List<CharacterID>();
			CharacterDefinition[] nonRandomCharacters = this.characterLists.GetNonRandomCharacters();
			for (int i = 0; i < nonRandomCharacters.Length; i++)
			{
				CharacterDefinition characterDefinition = nonRandomCharacters[i];
				list.Add(characterDefinition.characterID);
			}
			this.ownedCharacters = list.ToArray();
		}
		else
		{
			this.ownedCharacters = new CharacterID[]
			{
				CharacterID.Ashani
			};
		}
		this.signalBus.Dispatch(UserCharacterUnlockModel.SOURCE_UPDATED);
	}

	public CharacterID[] GetOwned()
	{
		return this.ownedCharacters;
	}
}
