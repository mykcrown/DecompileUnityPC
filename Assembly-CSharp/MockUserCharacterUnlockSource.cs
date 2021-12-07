using System;
using System.Collections.Generic;

// Token: 0x020006D8 RID: 1752
public class MockUserCharacterUnlockSource : IUserCharacterUnlockSource
{
	// Token: 0x17000ACE RID: 2766
	// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000E4D4D File Offset: 0x000E314D
	// (set) Token: 0x06002C0C RID: 11276 RVA: 0x000E4D55 File Offset: 0x000E3155
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000ACF RID: 2767
	// (get) Token: 0x06002C0D RID: 11277 RVA: 0x000E4D5E File Offset: 0x000E315E
	// (set) Token: 0x06002C0E RID: 11278 RVA: 0x000E4D66 File Offset: 0x000E3166
	[Inject]
	public IServerConnectionManager serverConnectionManager { get; set; }

	// Token: 0x17000AD0 RID: 2768
	// (get) Token: 0x06002C0F RID: 11279 RVA: 0x000E4D6F File Offset: 0x000E316F
	// (set) Token: 0x06002C10 RID: 11280 RVA: 0x000E4D77 File Offset: 0x000E3177
	[Inject]
	public GameEnvironmentData environmentData { get; set; }

	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x06002C11 RID: 11281 RVA: 0x000E4D80 File Offset: 0x000E3180
	// (set) Token: 0x06002C12 RID: 11282 RVA: 0x000E4D88 File Offset: 0x000E3188
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x06002C13 RID: 11283 RVA: 0x000E4D91 File Offset: 0x000E3191
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(ServerConnectionManager.UPDATED, new Action(this.onServerConnectionUpdate));
		this.updateCharacterSource();
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x000E4DB8 File Offset: 0x000E31B8
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

	// Token: 0x06002C15 RID: 11285 RVA: 0x000E4E08 File Offset: 0x000E3208
	private void updateCharacterSource()
	{
		if (this.environmentData.toggles.GetToggle(FeatureID.UnlockEverything))
		{
			List<CharacterID> list = new List<CharacterID>();
			foreach (CharacterDefinition characterDefinition in this.characterLists.GetNonRandomCharacters())
			{
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

	// Token: 0x06002C16 RID: 11286 RVA: 0x000E4E92 File Offset: 0x000E3292
	public CharacterID[] GetOwned()
	{
		return this.ownedCharacters;
	}

	// Token: 0x04001F5D RID: 8029
	private CharacterID[] ownedCharacters;

	// Token: 0x04001F5E RID: 8030
	private bool isConnected;
}
