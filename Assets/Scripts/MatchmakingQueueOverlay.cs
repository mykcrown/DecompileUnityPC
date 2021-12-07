// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchmakingQueueOverlay : BaseGamewideOverlay
{
	public Image BackgroundImage;

	public TextMeshProUGUI LoadingMessageText;

	public TextMeshProUGUI TimerMessageText;

	public TextMeshProUGUI ControllerText;

	public Material FlashMaterial;

	public GameObject ControllerDisplay;

	public WavedashUIButton Button;

	private bool isLeavePromptOpen;

	private Tweener _selectWeen;

	private float _flashAmount;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public ConfigData gameConfig
	{
		get;
		set;
	}

	[Inject]
	public UIManager uiManager
	{
		get;
		set;
	}

	private bool showControllerInstructions
	{
		get
		{
			return this.uiManager.CurrentInputModule != null && !this.uiManager.CurrentInputModule.IsMouseMode;
		}
	}

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

	public override void OnOpen()
	{
		this.LoadingMessageText.text = base.localization.GetText("ui.main.loadingbar.queue_started");
		this.events.Subscribe(typeof(LeaveQueuePromptCommand), new Events.EventHandler(this.onLeaveQueuePrompt));
		this.events.Subscribe(typeof(UpdateRequestMatchStatus), new Events.EventHandler(this.onUpdateRequestMatchStatus));
		WavedashUIButton expr_63 = this.Button;
		expr_63.OnSelectEvent = (Action<BaseEventData>)Delegate.Combine(expr_63.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton expr_8A = this.Button;
		expr_8A.OnDeselectEvent = (Action<BaseEventData>)Delegate.Combine(expr_8A.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
		if (this.BackgroundImage != null && this.FlashMaterial != null)
		{
			this.BackgroundImage.material = UnityEngine.Object.Instantiate<Material>(this.FlashMaterial);
		}
	}

	public override void Close()
	{
		this.events.Unsubscribe(typeof(LeaveQueuePromptCommand), new Events.EventHandler(this.onLeaveQueuePrompt));
		this.events.Unsubscribe(typeof(UpdateRequestMatchStatus), new Events.EventHandler(this.onUpdateRequestMatchStatus));
		WavedashUIButton expr_48 = this.Button;
		expr_48.OnSelectEvent = (Action<BaseEventData>)Delegate.Remove(expr_48.OnSelectEvent, new Action<BaseEventData>(this.onButtonSelected));
		WavedashUIButton expr_6F = this.Button;
		expr_6F.OnDeselectEvent = (Action<BaseEventData>)Delegate.Remove(expr_6F.OnDeselectEvent, new Action<BaseEventData>(this.onButtonDeselected));
		if (this.BackgroundImage != null && this.FlashMaterial != null)
		{
			UnityEngine.Object.DestroyImmediate(this.BackgroundImage.material);
		}
		base.Close();
	}

	private void Update()
	{
	}

	public override void OnXButtonPressed()
	{
		if (this.battleServerAPI.IsConnected)
		{
			return;
		}
		this.useLeaveQueuePrompt();
	}

	private void onLeaveQueuePrompt(GameEvent message)
	{
		this.PromptLeaveQueue();
	}

	public void PromptLeaveQueue()
	{
		if (this.battleServerAPI.IsConnected)
		{
			return;
		}
	}

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

	private void confirmLeaveQueue()
	{
		if (this.battleServerAPI.IsConnected)
		{
			return;
		}
		this.events.Broadcast(new LeaveQueuecommand());
	}

	private void closeLeaveQueue()
	{
		this.isLeavePromptOpen = false;
	}

	private void onUpdateRequestMatchStatus(GameEvent message)
	{
	}

	private void onButtonSelected(BaseEventData obj)
	{
		this.SetSelected(true);
	}

	private void onButtonDeselected(BaseEventData obj)
	{
		this.SetSelected(false);
	}

	private void killSelectTween()
	{
		if (this._selectWeen != null && this._selectWeen.IsPlaying())
		{
			this._selectWeen.Kill(false);
		}
		this._selectWeen = null;
	}

	public void SetSelected(bool isSelected)
	{
		this.killSelectTween();
		this._selectWeen = DOTween.To(new DOGetter<float>(this.get_flashAmount), new DOSetter<float>(this._SetSelected_m__0), (!isSelected) ? 0f : 0.45f, 0.5f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(this._SetSelected_m__1));
	}

	private void _SetSelected_m__0(float x)
	{
		this.flashAmount = x;
	}

	private void _SetSelected_m__1()
	{
		this.killSelectTween();
	}
}
