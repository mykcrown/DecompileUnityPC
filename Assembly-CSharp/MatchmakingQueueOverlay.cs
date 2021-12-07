using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000930 RID: 2352
public class MatchmakingQueueOverlay : BaseGamewideOverlay
{
	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x06003DBE RID: 15806 RVA: 0x0011B041 File Offset: 0x00119441
	// (set) Token: 0x06003DBF RID: 15807 RVA: 0x0011B049 File Offset: 0x00119449
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x0011B052 File Offset: 0x00119452
	// (set) Token: 0x06003DC1 RID: 15809 RVA: 0x0011B05A File Offset: 0x0011945A
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x0011B063 File Offset: 0x00119463
	// (set) Token: 0x06003DC3 RID: 15811 RVA: 0x0011B06B File Offset: 0x0011946B
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x0011B074 File Offset: 0x00119474
	// (set) Token: 0x06003DC5 RID: 15813 RVA: 0x0011B07C File Offset: 0x0011947C
	[Inject]
	public ConfigData gameConfig { get; set; }

	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x06003DC6 RID: 15814 RVA: 0x0011B085 File Offset: 0x00119485
	// (set) Token: 0x06003DC7 RID: 15815 RVA: 0x0011B08D File Offset: 0x0011948D
	[Inject]
	public UIManager uiManager { get; set; }

	// Token: 0x06003DC8 RID: 15816 RVA: 0x0011B098 File Offset: 0x00119498
	public override void OnOpen()
	{
		this.LoadingMessageText.text = base.localization.GetText("ui.main.loadingbar.queue_started");
		this.events.Subscribe(typeof(LeaveQueuePromptCommand), new Events.EventHandler(this.onLeaveQueuePrompt));
		this.events.Subscribe(typeof(UpdateRequestMatchStatus), new Events.EventHandler(this.onUpdateRequestMatchStatus));
		WavedashUIButton button = this.Button;
		button.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(button.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton button2 = this.Button;
		button2.OnDeselectEvent = (Action<BaseEventData>)Delegate.Combine(button2.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
		if (this.BackgroundImage != null && this.FlashMaterial != null)
		{
			this.BackgroundImage.material = UnityEngine.Object.Instantiate<Material>(this.FlashMaterial);
		}
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x0011B188 File Offset: 0x00119588
	public override void Close()
	{
		this.events.Unsubscribe(typeof(LeaveQueuePromptCommand), new Events.EventHandler(this.onLeaveQueuePrompt));
		this.events.Unsubscribe(typeof(UpdateRequestMatchStatus), new Events.EventHandler(this.onUpdateRequestMatchStatus));
		WavedashUIButton button = this.Button;
		button.OnSelectEvent = (Action<BaseEventData>)Delegate.Remove(button.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton button2 = this.Button;
		button2.OnDeselectEvent = (Action<BaseEventData>)Delegate.Remove(button2.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
		if (this.BackgroundImage != null && this.FlashMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.BackgroundImage.material);
		}
		base.Close();
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x0011B25D File Offset: 0x0011965D
	private void Update()
	{
	}

	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x06003DCB RID: 15819 RVA: 0x0011B25F File Offset: 0x0011965F
	private bool showControllerInstructions
	{
		get
		{
			return this.uiManager.CurrentInputModule != null && !this.uiManager.CurrentInputModule.IsMouseMode;
		}
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x0011B289 File Offset: 0x00119689
	public override void OnXButtonPressed()
	{
		if (this.battleServerAPI.IsConnected)
		{
			return;
		}
		this.useLeaveQueuePrompt();
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x0011B2A2 File Offset: 0x001196A2
	private void onLeaveQueuePrompt(GameEvent message)
	{
		this.PromptLeaveQueue();
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x0011B2AA File Offset: 0x001196AA
	public void PromptLeaveQueue()
	{
		if (this.battleServerAPI.IsConnected)
		{
			return;
		}
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x0011B2C0 File Offset: 0x001196C0
	private void useLeaveQueuePrompt()
	{
		if (!this.isLeavePromptOpen)
		{
			this.isLeavePromptOpen = true;
			GenericDialog genericDialog = this.dialogController.ShowTwoButtonDialog(base.localization.GetText("dialog.mainMenu.promptLeaveQueue.title"), base.localization.GetText("dialog.mainMenu.promptLeaveQueue.body"), base.localization.GetText("dialog.mainMenu.promptLeaveQueue.confirm"), base.localization.GetText("dialog.mainMenu.promptLeaveQueue.cancel"));
			genericDialog.ConfirmCallback = new Action(this.confirmLeaveQueue);
			genericDialog.CloseCallback = new Action(this.closeLeaveQueue);
		}
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x0011B34F File Offset: 0x0011974F
	private void confirmLeaveQueue()
	{
		if (this.battleServerAPI.IsConnected)
		{
			return;
		}
		this.events.Broadcast(new LeaveQueuecommand());
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x0011B372 File Offset: 0x00119772
	private void closeLeaveQueue()
	{
		this.isLeavePromptOpen = false;
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x0011B37B File Offset: 0x0011977B
	private void onUpdateRequestMatchStatus(GameEvent message)
	{
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x0011B37D File Offset: 0x0011977D
	private void onButtonSelected(BaseEventData obj)
	{
		this.SetSelected(true);
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x0011B386 File Offset: 0x00119786
	private void onButtonDeselected(BaseEventData obj)
	{
		this.SetSelected(false);
	}

	// Token: 0x06003DD5 RID: 15829 RVA: 0x0011B38F File Offset: 0x0011978F
	private void killSelectTween()
	{
		if (this._selectWeen != null && this._selectWeen.IsPlaying())
		{
			this._selectWeen.Kill(false);
		}
		this._selectWeen = null;
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x0011B3C0 File Offset: 0x001197C0
	public void SetSelected(bool isSelected)
	{
		this.killSelectTween();
		this._selectWeen = DOTween.To(new DOGetter<float>(this.get_flashAmount), delegate(float x)
		{
			this.flashAmount = x;
		}, (!isSelected) ? 0f : 0.45f, 0.5f).SetEase(Ease.OutSine).OnComplete(delegate
		{
			this.killSelectTween();
		});
	}

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x0011B427 File Offset: 0x00119827
	// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x0011B42F File Offset: 0x0011982F
	private float flashAmount
	{
		get
		{
			return this._flashAmount;
		}
		set
		{
			this._flashAmount = value;
			this.BackgroundImage.material.SetFloat("_FlashAmount", this._flashAmount);
		}
	}

	// Token: 0x04002A00 RID: 10752
	public Image BackgroundImage;

	// Token: 0x04002A01 RID: 10753
	public TextMeshProUGUI LoadingMessageText;

	// Token: 0x04002A02 RID: 10754
	public TextMeshProUGUI TimerMessageText;

	// Token: 0x04002A03 RID: 10755
	public TextMeshProUGUI ControllerText;

	// Token: 0x04002A04 RID: 10756
	public Material FlashMaterial;

	// Token: 0x04002A05 RID: 10757
	public GameObject ControllerDisplay;

	// Token: 0x04002A06 RID: 10758
	public WavedashUIButton Button;

	// Token: 0x04002A07 RID: 10759
	private bool isLeavePromptOpen;

	// Token: 0x04002A08 RID: 10760
	private Tweener _selectWeen;

	// Token: 0x04002A09 RID: 10761
	private float _flashAmount;
}
