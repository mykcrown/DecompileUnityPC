// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectScreen : GameScreen
{
	private sealed class _tweenInItem_c__AnonStorey0
	{
		internal GameObject theList;

		internal Action callback;

		internal Vector3 __m__0()
		{
			return this.theList.transform.localPosition;
		}

		internal void __m__1(Vector3 valueIn)
		{
			this.theList.transform.localPosition = valueIn;
		}

		internal void __m__2()
		{
			if (this.callback != null)
			{
				this.callback();
			}
		}
	}

	private sealed class _UpdatePayload_c__AnonStorey1
	{
		internal GameLoadPayload newPayload;

		internal StageSelectScreen _this;

		internal void __m__0()
		{
			this._this.enterGame(this.newPayload.stage);
		}
	}

	private sealed class _fadeOutAndDestroyPreview_c__AnonStorey2
	{
		internal StageSelectPreview preview;

		internal StageSelectScreen _this;

		internal void __m__0()
		{
			this.preview.gameObject.SetActive(false);
			this._this.releaseStageSelectPreview(this.preview);
		}
	}

	private sealed class _showEnteringGame_c__AnonStorey3
	{
		internal Action callback;

		internal Action onLoadComplete;

		internal StageSelectScreen _this;

		internal void __m__0()
		{
			Action action = this.callback;
			this._this.dialog.Close();
			if (action != null)
			{
				action();
			}
		}

		internal void __m__1()
		{
			this._this.timer.CancelTimeout(this.onLoadComplete);
			this._this.dialog = null;
			this.callback = null;
		}

		internal void __m__2()
		{
			this._this.events.Broadcast(new EnterGameRequest(StageID.None, false));
		}
	}

	private const float ENTER_GAME_DISPLAY_MIN_TIME = 2f;

	private const float ENTER_NETWORK_GAME_DISPLAY_MIN_TIME = 5f;

	private StageID selectedStageID;

	private List<StageData> stages;

	private Dictionary<StageID, StageData> stagesByID = new Dictionary<StageID, StageData>();

	private Dictionary<StageID, StageSelectItem> stageItemByID = new Dictionary<StageID, StageSelectItem>();

	public TextMeshProUGUI SelectedStageName;

	public TextMeshProUGUI SelectedStageDesc;

	public GridLayoutGroup StageListTop;

	public GridLayoutGroup StageListBottom;

	public GameObject LeftTweenIn;

	public GameObject RightTweenIn;

	public GameObject StageItemPrefab;

	public GameObject StagePreviewPrefab;

	public GameObject StagePreviewSlot;

	public GameObject BackButtonPrefab;

	public Transform BackButtonAnchor;

	public GameObject StrikeInstructionsPrefab;

	public Transform StrikeInstructionsAnchor;

	private List<StageSelectPreview> previewList = new List<StageSelectPreview>();

	public Vector2 StageBuffer = default(Vector2);

	public ScreenType PreviousScreen;

	public ScreenType NextScreen;

	private GameLoadPayload currentPayload;

	private GenericDialog dialog;

	private StageSelectPreview currentPreview;

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	public override void LoadPayload(Payload payload)
	{
		base.LoadPayload(payload);
		this.stages = base.gameDataManager.StageData.GetDataByIDs(this.gamePayload.stagePayloadData.legalStages);
		this.gamePayload.isConfirmed = false;
		for (int i = 0; i < this.stages.Count; i++)
		{
			this.stagesByID[this.stages[i].stageID] = this.stages[i];
		}
		if (this.gamePayload.stagePayloadData.stageStates == null)
		{
			this.gamePayload.stagePayloadData.stageStates = new Dictionary<StageID, StageState>();
		}
		this.populateScreen();
		this.UpdatePayload(payload);
	}

	protected override void createScreenCursors()
	{
		base.createScreenCursors();
		this.createCursor(PlayerNum.All);
	}

	private void populateScreen()
	{
		base.lockInput();
		base.addBackButtonForCursorScreen(this.BackButtonAnchor, this.BackButtonPrefab);
		base.addInputInstuctionsForMenuScreen(this.StrikeInstructionsAnchor, this.StrikeInstructionsPrefab.gameObject);
		this.SelectedStageName.text = string.Empty;
		this.SelectedStageDesc.text = string.Empty;
		int num = 5;
		bool flag = this.stages.Count <= num;
		int num2 = (int)Mathf.Ceil((float)(this.stages.Count / 2));
		if (flag)
		{
			Vector3 position = this.StageListBottom.transform.position;
			position.y = (position.y + this.StageListTop.transform.position.y) / 2f;
			this.StageListBottom.transform.position = position;
		}
		for (int i = 0; i < this.stages.Count; i++)
		{
			StageData stageData = this.stages[i];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.StageItemPrefab);
			if (i < num2 && !flag)
			{
				gameObject.transform.SetParent(this.StageListTop.transform, false);
			}
			else
			{
				gameObject.transform.SetParent(this.StageListBottom.transform, false);
			}
			StageSelectItem componentInChildren = gameObject.GetComponentInChildren<StageSelectItem>();
			base.injector.Inject(componentInChildren);
			componentInChildren.Init(stageData);
			this.stageItemByID[stageData.stageID] = componentInChildren;
		}
		this.tweenIn();
	}

	private void tweenIn()
	{
		this.tweenInItem(this.RightTweenIn, 1, null);
		this.tweenInItem(this.LeftTweenIn, -1, new Action(this._tweenIn_m__0));
	}

	private void tweenInItem(GameObject theList, int direction, Action callback = null)
	{
		StageSelectScreen._tweenInItem_c__AnonStorey0 _tweenInItem_c__AnonStorey = new StageSelectScreen._tweenInItem_c__AnonStorey0();
		_tweenInItem_c__AnonStorey.theList = theList;
		_tweenInItem_c__AnonStorey.callback = callback;
		Vector3 localPosition = _tweenInItem_c__AnonStorey.theList.transform.localPosition;
		Vector3 localPosition2 = _tweenInItem_c__AnonStorey.theList.transform.localPosition;
		localPosition2.x += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x * (float)direction;
		_tweenInItem_c__AnonStorey.theList.transform.localPosition = localPosition2;
		DOTween.To(new DOGetter<Vector3>(_tweenInItem_c__AnonStorey.__m__0), new DOSetter<Vector3>(_tweenInItem_c__AnonStorey.__m__1), localPosition, 0.25f).SetEase(Ease.OutSine).OnComplete(new TweenCallback(_tweenInItem_c__AnonStorey.__m__2));
	}

	private void onAnimationsComplete()
	{
		StageData stageData = this.findRandom();
		if (stageData == null)
		{
			stageData = this.stages[0];
		}
		this.selectStage(stageData.stageID);
		base.unlockInput();
	}

	private StageData findRandom()
	{
		for (int i = 0; i < this.stages.Count; i++)
		{
			StageData stageData = this.stages[i];
			if (stageData.stageType == StageType.Random)
			{
				return stageData;
			}
		}
		return null;
	}

	public void ResetStageStateData()
	{
		for (int i = 0; i < this.stages.Count; i++)
		{
			StageData stageData = this.stages[i];
			base.events.Broadcast(new SetStageStateRequest(stageData.stageID, StageState.None));
		}
		base.audioManager.PlayMenuSound(SoundKey.stageSelect_stageStikesReset, 0f);
	}

	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		this.GoToPreviousScreen();
	}

	public override void GoToNextScreen()
	{
	}

	public override void OnStartPressed(IPlayerCursor cursor)
	{
		base.events.Broadcast(new SelectStageRequest(this.selectedStageID, true));
	}

	public override void UpdatePayload(Payload payload)
	{
		StageSelectScreen._UpdatePayload_c__AnonStorey1 _UpdatePayload_c__AnonStorey = new StageSelectScreen._UpdatePayload_c__AnonStorey1();
		_UpdatePayload_c__AnonStorey._this = this;
		base.UpdatePayload(payload);
		_UpdatePayload_c__AnonStorey.newPayload = this.gamePayload;
		this.selectStage(_UpdatePayload_c__AnonStorey.newPayload.stage);
		bool flag = false;
		if (this.currentPayload != null)
		{
			flag = this.currentPayload.isConfirmed;
		}
		if (_UpdatePayload_c__AnonStorey.newPayload.isConfirmed != flag)
		{
			if (_UpdatePayload_c__AnonStorey.newPayload.isConfirmed)
			{
				this.showEnteringGame(new Action(_UpdatePayload_c__AnonStorey.__m__0));
			}
			else if (this.dialog != null)
			{
				this.dialog.Close();
			}
		}
		foreach (StageID current in _UpdatePayload_c__AnonStorey.newPayload.stagePayloadData.stageStates.Keys)
		{
			this.stageItemByID[current].SetStageState(_UpdatePayload_c__AnonStorey.newPayload.stagePayloadData.stageStates[current], false);
		}
		this.currentPayload = _UpdatePayload_c__AnonStorey.newPayload.Clone();
	}

	private void selectStage(StageID stageID)
	{
		if (this.selectedStageID == stageID || stageID == StageID.None)
		{
			return;
		}
		this.selectedStageID = stageID;
		StageData stageData = this.stagesByID[stageID];
		this.SelectedStageName.text = base.localization.GetText("gameData.stageSelect." + stageData.stageName);
		string text = "gameData.stagePreview." + stageData.stageName;
		string text2 = base.localization.GetText(text);
		if (text2 == null)
		{
			text2 = text;
		}
		this.SelectedStageDesc.text = text2.ToUpper();
		this.fadeOutAndDestroyPreview(this.currentPreview);
		StageSelectPreview stageSelectPreview = this.requestStageSelectPreview();
		stageSelectPreview.Load(stageData);
		stageSelectPreview.Alpha = 0f;
		stageSelectPreview.TweenAlpha(1f, 0.08f);
		stageSelectPreview.gameObject.SetActive(true);
		stageSelectPreview.transform.SetParent(this.StagePreviewSlot.transform, false);
		this.currentPreview = stageSelectPreview;
	}

	private void fadeOutAndDestroyPreview(StageSelectPreview preview)
	{
		StageSelectScreen._fadeOutAndDestroyPreview_c__AnonStorey2 _fadeOutAndDestroyPreview_c__AnonStorey = new StageSelectScreen._fadeOutAndDestroyPreview_c__AnonStorey2();
		_fadeOutAndDestroyPreview_c__AnonStorey.preview = preview;
		_fadeOutAndDestroyPreview_c__AnonStorey._this = this;
		if (_fadeOutAndDestroyPreview_c__AnonStorey.preview != null)
		{
			_fadeOutAndDestroyPreview_c__AnonStorey.preview.TweenAlpha(0f, 0.08f, new Action(_fadeOutAndDestroyPreview_c__AnonStorey.__m__0));
		}
	}

	private void showEnteringGame(Action callback)
	{
		StageSelectScreen._showEnteringGame_c__AnonStorey3 _showEnteringGame_c__AnonStorey = new StageSelectScreen._showEnteringGame_c__AnonStorey3();
		_showEnteringGame_c__AnonStorey.callback = callback;
		_showEnteringGame_c__AnonStorey._this = this;
		this.dialog = base.dialogController.ShowOneButtonSpinnyDialog(base.localization.GetText("dialog.enteringGame.title"), base.localization.GetText("dialog.cancel"));
		_showEnteringGame_c__AnonStorey.onLoadComplete = new Action(_showEnteringGame_c__AnonStorey.__m__0);
		Action closeCallback = new Action(_showEnteringGame_c__AnonStorey.__m__1);
		Action action = new Action(_showEnteringGame_c__AnonStorey.__m__2);
		this.dialog.CloseCallback = closeCallback;
		this.dialog.ConfirmCallback = action;
		this.dialog.CancelCallback = action;
		int time = 2000;
		this.timer.SetTimeout(time, _showEnteringGame_c__AnonStorey.onLoadComplete);
	}

	private void enterGame(StageID stageID)
	{
		base.events.Broadcast(new EnterGameRequest(stageID, true));
	}

	private StageSelectPreview requestStageSelectPreview()
	{
		if (this.previewList.Count > 0)
		{
			StageSelectPreview result = this.previewList[0];
			this.previewList.RemoveAt(0);
			return result;
		}
		return UnityEngine.Object.Instantiate<GameObject>(this.StagePreviewPrefab).GetComponent<StageSelectPreview>();
	}

	private void releaseStageSelectPreview(StageSelectPreview obj)
	{
		this.previewList.Add(obj);
	}

	private void _tweenIn_m__0()
	{
		this.onAnimationsComplete();
	}
}
