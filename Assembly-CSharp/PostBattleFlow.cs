using System;

// Token: 0x020009C4 RID: 2500
public class PostBattleFlow : IPostBattleFlow
{
	// Token: 0x170010C7 RID: 4295
	// (get) Token: 0x06004607 RID: 17927 RVA: 0x0013234E File Offset: 0x0013074E
	// (set) Token: 0x06004608 RID: 17928 RVA: 0x00132356 File Offset: 0x00130756
	[Inject]
	public IDialogController dialog { get; set; }

	// Token: 0x170010C8 RID: 4296
	// (get) Token: 0x06004609 RID: 17929 RVA: 0x0013235F File Offset: 0x0013075F
	// (set) Token: 0x0600460A RID: 17930 RVA: 0x00132367 File Offset: 0x00130767
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170010C9 RID: 4297
	// (get) Token: 0x0600460B RID: 17931 RVA: 0x00132370 File Offset: 0x00130770
	// (set) Token: 0x0600460C RID: 17932 RVA: 0x00132378 File Offset: 0x00130778
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x170010CA RID: 4298
	// (get) Token: 0x0600460D RID: 17933 RVA: 0x00132381 File Offset: 0x00130781
	// (set) Token: 0x0600460E RID: 17934 RVA: 0x00132389 File Offset: 0x00130789
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170010CB RID: 4299
	// (get) Token: 0x0600460F RID: 17935 RVA: 0x00132392 File Offset: 0x00130792
	// (set) Token: 0x06004610 RID: 17936 RVA: 0x0013239A File Offset: 0x0013079A
	[Inject]
	public ICustomLobbyController lobbyController { get; set; }

	// Token: 0x170010CC RID: 4300
	// (get) Token: 0x06004611 RID: 17937 RVA: 0x001323A3 File Offset: 0x001307A3
	// (set) Token: 0x06004612 RID: 17938 RVA: 0x001323AB File Offset: 0x001307AB
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x170010CD RID: 4301
	// (get) Token: 0x06004613 RID: 17939 RVA: 0x001323B4 File Offset: 0x001307B4
	// (set) Token: 0x06004614 RID: 17940 RVA: 0x001323BC File Offset: 0x001307BC
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x06004615 RID: 17941 RVA: 0x001323C8 File Offset: 0x001307C8
	public void ExitPostGame(VictoryScreenPayload victoryPayload)
	{
		ScreenType type = ScreenType.CharacterSelect;
		if (victoryPayload.gamePayload != null)
		{
			if (this.battleServerAPI.IsConnected)
			{
				type = ScreenType.OnlineBlindPick;
				this.enterNewGame.InitPayload(GameStartType.FreePlay, victoryPayload.gamePayload);
				this.richPresence.SetPresence("InCharacterSelect", null, null, null);
			}
			else if (this.lobbyController.IsInLobby)
			{
				type = ScreenType.CustomLobbyScreen;
				this.richPresence.SetPresence("InCustomLobby", null, null, null);
			}
			else if (victoryPayload.gamePayload.isOnlineGame)
			{
				type = ScreenType.MainMenu;
				this.richPresence.SetPresence(null, null, null, null);
			}
		}
		this.events.Broadcast(new LoadScreenCommand(type, null, ScreenUpdateType.Next));
	}
}
