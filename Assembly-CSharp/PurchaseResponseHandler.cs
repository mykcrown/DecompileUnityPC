using System;
using UnityEngine;

// Token: 0x02000A16 RID: 2582
public class PurchaseResponseHandler : IPurchaseResponseHandler
{
	// Token: 0x170011E0 RID: 4576
	// (get) Token: 0x06004B10 RID: 19216 RVA: 0x00140D97 File Offset: 0x0013F197
	// (set) Token: 0x06004B11 RID: 19217 RVA: 0x00140D9F File Offset: 0x0013F19F
	[Inject]
	public IServerConnectionManager serverManager { get; set; }

	// Token: 0x170011E1 RID: 4577
	// (get) Token: 0x06004B12 RID: 19218 RVA: 0x00140DA8 File Offset: 0x0013F1A8
	// (set) Token: 0x06004B13 RID: 19219 RVA: 0x00140DB0 File Offset: 0x0013F1B0
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x170011E2 RID: 4578
	// (get) Token: 0x06004B14 RID: 19220 RVA: 0x00140DB9 File Offset: 0x0013F1B9
	// (set) Token: 0x06004B15 RID: 19221 RVA: 0x00140DC1 File Offset: 0x0013F1C1
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170011E3 RID: 4579
	// (get) Token: 0x06004B16 RID: 19222 RVA: 0x00140DCA File Offset: 0x0013F1CA
	// (set) Token: 0x06004B17 RID: 19223 RVA: 0x00140DD2 File Offset: 0x0013F1D2
	[Inject]
	public SteamManager steamManager { get; set; }

	// Token: 0x170011E4 RID: 4580
	// (get) Token: 0x06004B18 RID: 19224 RVA: 0x00140DDB File Offset: 0x0013F1DB
	// (set) Token: 0x06004B19 RID: 19225 RVA: 0x00140DE3 File Offset: 0x0013F1E3
	[Inject]
	public IOfflineModeDetector offlineMode { get; set; }

	// Token: 0x170011E5 RID: 4581
	// (get) Token: 0x06004B1A RID: 19226 RVA: 0x00140DEC File Offset: 0x0013F1EC
	// (set) Token: 0x06004B1B RID: 19227 RVA: 0x00140DF4 File Offset: 0x0013F1F4
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x06004B1C RID: 19228 RVA: 0x00140E00 File Offset: 0x0013F200
	public bool VerifySteam(IPurchaseResponseDialog dialog)
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.UnlockEverythingOnline))
		{
			return false;
		}
		Debug.Log("Overlay? " + this.steamManager.IsOverlayEnabled());
		if (!this.steamManager.IsOverlayEnabled())
		{
			dialog.ShowError(this.localization.GetText("ui.store.steamOverlayDisabled.title"), this.localization.GetText("ui.store.steamOverlayDisabled.body"));
			return false;
		}
		return true;
	}

	// Token: 0x06004B1D RID: 19229 RVA: 0x00140E7C File Offset: 0x0013F27C
	public void HandleUnlockError(UserPurchaseResult result, IPurchaseResponseDialog dialog, Action cleanup)
	{
		PurchaseResponseHandler.Result result2 = new PurchaseResponseHandler.Result();
		switch (result)
		{
		case UserPurchaseResult.DESYNC:
			result2.title = this.localization.GetText("ui.store.characters.unlockError.update.title");
			result2.body = this.localization.GetText("ui.store.characters.unlockError.update.body");
			this.events.Broadcast(new LoadScreenCommand(ScreenType.MainMenu, null, ScreenUpdateType.Next));
			this.showResult(result2, dialog, cleanup);
			break;
		case UserPurchaseResult.DESYNC_USER_CURRENCY:
			result2.title = this.localization.GetText("ui.store.characters.unlockError.moneyDesync.title");
			result2.body = this.localization.GetText("ui.store.characters.unlockError.moneyDesync.body");
			result2.onDialogClosed = delegate()
			{
				this.events.Broadcast(new LoadScreenCommand(ScreenType.LoginScreen, null, ScreenUpdateType.Next));
				this.serverManager.ErrorReconnect();
			};
			this.showResult(result2, dialog, cleanup);
			break;
		case UserPurchaseResult.NOT_LOGGED_IN:
			result2.title = this.localization.GetText("ui.store.characters.unlockError.loggedOut.title");
			result2.body = this.localization.GetText("ui.store.characters.unlockError.loggedOut.body");
			result2.onDialogClosed = delegate()
			{
				this.events.Broadcast(new LoadScreenCommand(ScreenType.LoginScreen, null, ScreenUpdateType.Next));
				this.serverManager.ErrorReconnect();
			};
			this.showResult(result2, dialog, cleanup);
			break;
		case UserPurchaseResult.USER_CANCELLED:
			result2.userCancel = true;
			this.showResult(result2, dialog, cleanup);
			break;
		default:
			result2.title = this.localization.GetText("ui.store.characters.unlockError.title");
			result2.body = this.localization.GetText("ui.store.characters.unlockError.body");
			this.showResult(result2, dialog, cleanup);
			break;
		}
	}

	// Token: 0x06004B1E RID: 19230 RVA: 0x00140FE0 File Offset: 0x0013F3E0
	private void showResult(PurchaseResponseHandler.Result handler, IPurchaseResponseDialog dialog, Action cleanup)
	{
		if (handler.userCancel)
		{
			dialog.Close();
		}
		else
		{
			if (handler.title != null && handler.body != null)
			{
				dialog.ShowError(handler.title, handler.body);
			}
			if (handler.onDialogClosed != null)
			{
				dialog.CloseCallback = delegate()
				{
					cleanup();
					handler.onDialogClosed();
				};
			}
		}
	}

	// Token: 0x02000A17 RID: 2583
	public class Result
	{
		// Token: 0x04003161 RID: 12641
		public string title;

		// Token: 0x04003162 RID: 12642
		public string body;

		// Token: 0x04003163 RID: 12643
		public bool userCancel;

		// Token: 0x04003164 RID: 12644
		public Action onDialogClosed;
	}
}
