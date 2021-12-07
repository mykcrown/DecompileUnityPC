using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000935 RID: 2357
public class DialogController : IDialogController
{
	// Token: 0x17000ED1 RID: 3793
	// (get) Token: 0x06003E21 RID: 15905 RVA: 0x0011C088 File Offset: 0x0011A488
	// (set) Token: 0x06003E22 RID: 15906 RVA: 0x0011C090 File Offset: 0x0011A490
	[Inject]
	public IWindowDisplay windowDisplay { get; set; }

	// Token: 0x06003E23 RID: 15907 RVA: 0x0011C09C File Offset: 0x0011A49C
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

	// Token: 0x06003E24 RID: 15908 RVA: 0x0011C19C File Offset: 0x0011A59C
	public GenericDialog ShowOneButtonDialog(string title, string body, string buttonText, WindowTransition transition = WindowTransition.STANDARD_FADE, bool useOverrideOpenSound = false, AudioData overrideOpenSound = default(AudioData))
	{
		IWindowDisplay windowDisplay = this.windowDisplay;
		GameObject prefab = this.oneButtonDialogPrefab;
		this.currentDialog = windowDisplay.Add<GenericDialog>(prefab, transition, false, false, useOverrideOpenSound, overrideOpenSound);
		this.currentDialog.SetBody(body);
		this.currentDialog.SetTitle(title);
		this.currentDialog.SetConfirm(buttonText);
		return this.currentDialog;
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x0011C1FC File Offset: 0x0011A5FC
	public GenericDialog ShowTwoButtonDialog(string title, string body, string confirmText, string cancelText)
	{
		this.currentDialog = this.windowDisplay.Add<GenericDialog>(this.twoButtonDialogPrefab, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		this.currentDialog.SetBody(body);
		this.currentDialog.SetTitle(title);
		this.currentDialog.SetConfirm(confirmText);
		this.currentDialog.SetCancel(cancelText);
		return this.currentDialog;
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x0011C264 File Offset: 0x0011A664
	public GenericDialog ShowOneButtonSpinnyDialog(string title, string buttonText)
	{
		this.currentDialog = this.windowDisplay.Add<GenericDialog>(this.oneButtonSpinnyDialogPrefab, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		this.currentDialog.SetTitle(title);
		this.currentDialog.SetConfirm(buttonText);
		return this.currentDialog;
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x0011C2B4 File Offset: 0x0011A6B4
	public GenericDialog ShowSpinnyDialog(string title, WindowTransition transition = WindowTransition.STANDARD_FADE)
	{
		this.currentDialog = this.windowDisplay.Add<GenericDialog>(this.spinnyDialogPrefab, transition, false, false, false, default(AudioData));
		this.currentDialog.SetTitle(title);
		return this.currentDialog;
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x0011C2F8 File Offset: 0x0011A6F8
	public UnlockFlowDialog ShowUnlockFlowDialog()
	{
		return this.windowDisplay.Add<UnlockFlowDialog>(this.unlockFlowDialogPrefab, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x0011C324 File Offset: 0x0011A724
	public DetailedUnlockFlowDialog ShowDetailedUnlockFlowDialog()
	{
		return this.windowDisplay.Add<DetailedUnlockFlowDialog>(this.detailedUnlockFlowDialogPrefab, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x0011C350 File Offset: 0x0011A750
	public LootBoxBuyPopup ShowLootBoxBuyDialog()
	{
		return this.windowDisplay.Add<LootBoxBuyPopup>(this.lootBoxBuyDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x0011C37C File Offset: 0x0011A77C
	public RankedFriendlyPopup ShowNuxRankChoiceDialog()
	{
		return this.windowDisplay.Add<RankedFriendlyPopup>(this.nuxRankChoiceDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x0011C3A8 File Offset: 0x0011A7A8
	public void QueueNuxRankChoiceDialog(List<Func<BaseWindow>> windowQueue)
	{
		windowQueue.Add(delegate
		{
			RankedFriendlyPopup rankedFriendlyPopup = this.ShowNuxRankChoiceDialog();
			rankedFriendlyPopup.SetQueueWindowCreator(windowQueue);
			return rankedFriendlyPopup;
		});
	}

	// Token: 0x06003E2D RID: 15917 RVA: 0x0011C3E0 File Offset: 0x0011A7E0
	public NuxPopup ShowNuxRankConfirmedDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxRankConfirmedDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x0011C40C File Offset: 0x0011A80C
	public NuxPopup ShowNuxNewPlayerDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxNewUserDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x0011C438 File Offset: 0x0011A838
	public NuxPopup ShowNuxFirstTokenDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxFirstTokenDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x0011C464 File Offset: 0x0011A864
	public NuxPopup ShowNuxSecondTokenDialog()
	{
		return this.windowDisplay.Add<NuxPopup>(this.nuxSecondTokenDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x0011C490 File Offset: 0x0011A890
	public EquipTauntDialog ShowEquipDialog()
	{
		return this.windowDisplay.Add<EquipTauntDialog>(this.equipDialogPrefab, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x0011C4BC File Offset: 0x0011A8BC
	public AdvancedControllerDialog ShowAdvancedControllerDialog()
	{
		return this.windowDisplay.Add<AdvancedControllerDialog>(this.advancedControllerControlsDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E33 RID: 15923 RVA: 0x0011C4E8 File Offset: 0x0011A8E8
	public AdvancedKeyboardDialog ShowAdvancedKeyboardDialog()
	{
		return this.windowDisplay.Add<AdvancedKeyboardDialog>(this.advancedKeyboardControlsDialog, WindowTransition.STANDARD_FADE, false, true, false, default(AudioData));
	}

	// Token: 0x06003E34 RID: 15924 RVA: 0x0011C514 File Offset: 0x0011A914
	public FeedbackDialog ShowFeedbackDialog()
	{
		return this.windowDisplay.Add<FeedbackDialog>(this.feedbackDialog, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
	}

	// Token: 0x06003E35 RID: 15925 RVA: 0x0011C540 File Offset: 0x0011A940
	public CreateLobbyDialog ShowCreateLobbyDialog(bool isCreate)
	{
		CreateLobbyDialog createLobbyDialog = this.windowDisplay.Add<CreateLobbyDialog>(this.createLobbyDialog, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		createLobbyDialog.Initialize(isCreate);
		return createLobbyDialog;
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x0011C574 File Offset: 0x0011A974
	public JoinLobbyDialog ShowJoinLobbyDialog()
	{
		JoinLobbyDialog joinLobbyDialog = this.windowDisplay.Add<JoinLobbyDialog>(this.joinLobbyDialog, WindowTransition.STANDARD_FADE, false, false, false, default(AudioData));
		joinLobbyDialog.Initialize();
		return joinLobbyDialog;
	}

	// Token: 0x04002A39 RID: 10809
	private GameObject twoButtonDialogPrefab;

	// Token: 0x04002A3A RID: 10810
	private GameObject oneButtonDialogPrefab;

	// Token: 0x04002A3B RID: 10811
	private GameObject oneButtonSpinnyDialogPrefab;

	// Token: 0x04002A3C RID: 10812
	private GameObject spinnyDialogPrefab;

	// Token: 0x04002A3D RID: 10813
	private GameObject unlockFlowDialogPrefab;

	// Token: 0x04002A3E RID: 10814
	private GameObject detailedUnlockFlowDialogPrefab;

	// Token: 0x04002A3F RID: 10815
	private GameObject equipDialogPrefab;

	// Token: 0x04002A40 RID: 10816
	private GameObject advancedControllerControlsDialog;

	// Token: 0x04002A41 RID: 10817
	private GameObject advancedKeyboardControlsDialog;

	// Token: 0x04002A42 RID: 10818
	private GameObject feedbackDialog;

	// Token: 0x04002A43 RID: 10819
	private GameObject createLobbyDialog;

	// Token: 0x04002A44 RID: 10820
	private GameObject joinLobbyDialog;

	// Token: 0x04002A45 RID: 10821
	private GameObject lootBoxBuyDialog;

	// Token: 0x04002A46 RID: 10822
	private GameObject nuxNewUserDialog;

	// Token: 0x04002A47 RID: 10823
	private GameObject nuxFirstTokenDialog;

	// Token: 0x04002A48 RID: 10824
	private GameObject nuxSecondTokenDialog;

	// Token: 0x04002A49 RID: 10825
	private GameObject nuxRankChoiceDialog;

	// Token: 0x04002A4A RID: 10826
	private GameObject nuxRankConfirmedDialog;

	// Token: 0x04002A4B RID: 10827
	private GenericDialog currentDialog;
}
