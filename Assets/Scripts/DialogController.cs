// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DialogController : IDialogController
{
	private sealed class _QueueNuxRankChoiceDialog_c__AnonStorey0
	{
		internal List<Func<BaseWindow>> windowQueue;

		internal DialogController _this;

		internal BaseWindow __m__0()
		{
			RankedFriendlyPopup rankedFriendlyPopup = this._this.ShowNuxRankChoiceDialog();
			rankedFriendlyPopup.SetQueueWindowCreator(this.windowQueue);
			return rankedFriendlyPopup;
		}
	}

	private GameObject twoButtonDialogPrefab;

	private GameObject oneButtonDialogPrefab;

	private GameObject oneButtonSpinnyDialogPrefab;

	private GameObject spinnyDialogPrefab;

	private GameObject unlockFlowDialogPrefab;

	private GameObject detailedUnlockFlowDialogPrefab;

	private GameObject equipDialogPrefab;

	private GameObject advancedControllerControlsDialog;

	private GameObject advancedKeyboardControlsDialog;

	private GameObject feedbackDialog;

	private GameObject createLobbyDialog;

	private GameObject joinLobbyDialog;

	private GameObject lootBoxBuyDialog;

	private GameObject nuxNewUserDialog;

	private GameObject nuxFirstTokenDialog;

	private GameObject nuxSecondTokenDialog;

	private GameObject nuxRankChoiceDialog;

	private GameObject nuxRankConfirmedDialog;

	private GenericDialog currentDialog;

	[Inject]
	public IWindowDisplay windowDisplay
	{
		get;
		set;
	}

	public void Init(GameClient client)
	{
		Canvas componentInChildren = client.gameObject.GetComponentInChildren<Canvas>();
		CanvasContainer component = componentInChildren.gameObject.GetComponent<CanvasContainer>();
		this.twoButtonDialogPrefab = component.TwoButtonDialogPrefab;
		this.oneButtonDialogPrefab = component.OneButtonDialogPrefab;
		this.oneButtonSpinnyDialogPrefab = component.OneButtonSpinnyDialogPrefab;
		this.spinnyDialogPrefab = component.SpinnyDialogPrefab;
		this.unlockFlowDialogPrefab = component.UnlockFlowDialogPrefab;
		this.detailedUnlockFlowDialogPrefab = component.DetailedUnlockFlowDialogPrefab;
		this.lootBoxBuyDialog = component.LootBoxBuyDialog;
		this.equipDialogPrefab = component.EquipDialogPrefab;
		this.nuxNewUserDialog = component.NuxNewUserDialog;
		this.nuxFirstTokenDialog = component.NuxFirstTokenDialog;
		this.nuxSecondTokenDialog = component.NuxSecondTokenDialog;
		this.nuxRankChoiceDialog = component.NuxRankChoiceDialog;
		this.nuxRankConfirmedDialog = component.NuxRankConfirmedDialog;
		this.advancedControllerControlsDialog = component.AdvancedControllerControlsDialog;
		this.advancedKeyboardControlsDialog = component.AdvancedKeyboardControlsDialog;
		this.feedbackDialog = component.FeedbackDialog;
		this.createLobbyDialog = component.CreateLobbyDialog;
		this.joinLobbyDialog = component.JoinLobbyDialog;
	}

	public GenericDialog ShowOneButtonDialog(string title, string body, string buttonText, WindowTransition transition = WindowTransition.STANDARD_FADE, bool useOverrideOpenSound = false, AudioData overrideOpenSound = default(AudioData))
	{
		IWindowDisplay arg_1D_0 = this.windowDisplay;
		GameObject prefab = this.oneButtonDialogPrefab;
		this.currentDialog = arg_1D_0.Add<GenericDialog>(prefab, transition, false, false, useOverrideOpenSound, overrideOpenSound);
		this.currentDialog.SetBody(body);
		this.currentDialog.SetTitle(title);
		this.currentDialog.SetConfirm(buttonText);
		return this.currentDialog;
	}

	public GenericDialog ShowTwoButtonDialog(string title, string body, string confirmText, string cancelText)
	{
		this.currentDialog = this.windowDisplay.Add<GenericDialog>(this.twoButtonDialogPrefab, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		this.currentDialog.SetBody(body);
		this.currentDialog.SetTitle(title);
		this.currentDialog.SetConfirm(confirmText);
		this.currentDialog.SetCancel(cancelText);
		return this.currentDialog;
	}

	public GenericDialog ShowOneButtonSpinnyDialog(string title, string buttonText)
	{
		this.currentDialog = this.windowDisplay.Add<GenericDialog>(this.oneButtonSpinnyDialogPrefab, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		this.currentDialog.SetTitle(title);
		this.currentDialog.SetConfirm(buttonText);
		return this.currentDialog;
	}

	public GenericDialog ShowSpinnyDialog(string title, WindowTransition transition = WindowTransition.STANDARD_FADE)
	{
		this.currentDialog = this.windowDisplay.Add<GenericDialog>(this.spinnyDialogPrefab, transition, false, false, false, default(AudioData));
		this.currentDialog.SetTitle(title);
		return this.currentDialog;
	}

	public UnlockFlowDialog ShowUnlockFlowDialog()
	{
		return this.windowDisplay.Add<UnlockFlowDialog>(this.unlockFlowDialogPrefab, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public DetailedUnlockFlowDialog ShowDetailedUnlockFlowDialog()
	{
		return this.windowDisplay.Add<DetailedUnlockFlowDialog>(this.detailedUnlockFlowDialogPrefab, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public LootBoxBuyPopup ShowLootBoxBuyDialog()
	{
		return this.windowDisplay.Add<LootBoxBuyPopup>(this.lootBoxBuyDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public RankedFriendlyPopup ShowNuxRankChoiceDialog()
	{
		return this.windowDisplay.Add<RankedFriendlyPopup>(this.nuxRankChoiceDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public void QueueNuxRankChoiceDialog(List<Func<BaseWindow>> windowQueue)
	{
		DialogController._QueueNuxRankChoiceDialog_c__AnonStorey0 _QueueNuxRankChoiceDialog_c__AnonStorey = new DialogController._QueueNuxRankChoiceDialog_c__AnonStorey0();
		_QueueNuxRankChoiceDialog_c__AnonStorey.windowQueue = windowQueue;
		_QueueNuxRankChoiceDialog_c__AnonStorey._this = this;
		_QueueNuxRankChoiceDialog_c__AnonStorey.windowQueue.Add(new Func<BaseWindow>(_QueueNuxRankChoiceDialog_c__AnonStorey.__m__0));
	}

	public NuxPopup ShowNuxRankConfirmedDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxRankConfirmedDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public NuxPopup ShowNuxNewPlayerDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxNewUserDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public NuxPopup ShowNuxFirstTokenDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxFirstTokenDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public NuxPopup ShowNuxSecondTokenDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxSecondTokenDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public EquipTauntDialog ShowEquipDialog()
	{
		return this.windowDisplay.Add<EquipTauntDialog>(this.equipDialogPrefab, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public AdvancedControllerDialog ShowAdvancedControllerDialog()
	{
		return this.windowDisplay.Add<AdvancedControllerDialog>(this.advancedControllerControlsDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public AdvancedKeyboardDialog ShowAdvancedKeyboardDialog()
	{
		return this.windowDisplay.Add<AdvancedKeyboardDialog>(this.advancedKeyboardControlsDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	public FeedbackDialog ShowFeedbackDialog()
	{
		return this.windowDisplay.Add<FeedbackDialog>(this.feedbackDialog, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
	}

	public CreateLobbyDialog ShowCreateLobbyDialog(bool isCreate)
	{
		CreateLobbyDialog createLobbyDialog = this.windowDisplay.Add<CreateLobbyDialog>(this.createLobbyDialog, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		createLobbyDialog.Initialize(isCreate);
		return createLobbyDialog;
	}

	public JoinLobbyDialog ShowJoinLobbyDialog()
	{
		JoinLobbyDialog joinLobbyDialog = this.windowDisplay.Add<JoinLobbyDialog>(this.joinLobbyDialog, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		joinLobbyDialog.Initialize();
		return joinLobbyDialog;
	}
}
