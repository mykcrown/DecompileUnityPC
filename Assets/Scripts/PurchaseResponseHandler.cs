// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PurchaseResponseHandler : IPurchaseResponseHandler
{
	public class Result
	{
		public string title;

		public string body;

		public bool userCancel;

		public Action onDialogClosed;
	}

	private sealed class _showResult_c__AnonStorey0
	{
		internal Action cleanup;

		internal PurchaseResponseHandler.Result handler;

		internal void __m__0()
		{
			this.cleanup();
			this.handler.onDialogClosed();
		}
	}

	[Inject]
	public IServerConnectionManager serverManager
	{
		get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public SteamManager steamManager
	{
		get;
		set;
	}

	[Inject]
	public IOfflineModeDetector offlineMode
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

	public bool VerifySteam(IPurchaseResponseDialog dialog)
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.UnlockEverythingOnline))
		{
			return false;
		}
		UnityEngine.Debug.Log("Overlay? " + this.steamManager.IsOverlayEnabled());
		if (!this.steamManager.IsOverlayEnabled())
		{
			dialog.ShowError(this.localization.GetText("ui.store.steamOverlayDisabled.title"), this.localization.GetText("ui.store.steamOverlayDisabled.body"));
			return false;
		}
		return true;
	}

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
			result2.onDialogClosed = new Action(this._HandleUnlockError_m__0);
			this.showResult(result2, dialog, cleanup);
			break;
		case UserPurchaseResult.NOT_LOGGED_IN:
			result2.title = this.localization.GetText("ui.store.characters.unlockError.loggedOut.title");
			result2.body = this.localization.GetText("ui.store.characters.unlockError.loggedOut.body");
			result2.onDialogClosed = new Action(this._HandleUnlockError_m__1);
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

	private void showResult(PurchaseResponseHandler.Result handler, IPurchaseResponseDialog dialog, Action cleanup)
	{
		PurchaseResponseHandler._showResult_c__AnonStorey0 _showResult_c__AnonStorey = new PurchaseResponseHandler._showResult_c__AnonStorey0();
		_showResult_c__AnonStorey.cleanup = cleanup;
		_showResult_c__AnonStorey.handler = handler;
		if (_showResult_c__AnonStorey.handler.userCancel)
		{
			dialog.Close();
		}
		else
		{
			if (_showResult_c__AnonStorey.handler.title != null && _showResult_c__AnonStorey.handler.body != null)
			{
				dialog.ShowError(_showResult_c__AnonStorey.handler.title, _showResult_c__AnonStorey.handler.body);
			}
			if (_showResult_c__AnonStorey.handler.onDialogClosed != null)
			{
				dialog.CloseCallback = new Action(_showResult_c__AnonStorey.__m__0);
			}
		}
	}

	private void _HandleUnlockError_m__0()
	{
		this.events.Broadcast(new LoadScreenCommand(ScreenType.LoginScreen, null, ScreenUpdateType.Next));
		this.serverManager.ErrorReconnect();
	}

	private void _HandleUnlockError_m__1()
	{
		this.events.Broadcast(new LoadScreenCommand(ScreenType.LoginScreen, null, ScreenUpdateType.Next));
		this.serverManager.ErrorReconnect();
	}
}
