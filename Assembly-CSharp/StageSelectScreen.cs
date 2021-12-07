using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009D3 RID: 2515
public class StageSelectScreen : GameScreen
{
	// Token: 0x170010E1 RID: 4321
	// (get) Token: 0x06004662 RID: 18018 RVA: 0x0013302F File Offset: 0x0013142F
	// (set) Token: 0x06004663 RID: 18019 RVA: 0x00133037 File Offset: 0x00131437
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x170010E2 RID: 4322
	// (get) Token: 0x06004664 RID: 18020 RVA: 0x00133040 File Offset: 0x00131440
	// (set) Token: 0x06004665 RID: 18021 RVA: 0x00133048 File Offset: 0x00131448
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x170010E3 RID: 4323
	// (get) Token: 0x06004666 RID: 18022 RVA: 0x00133051 File Offset: 0x00131451
	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	// Token: 0x170010E4 RID: 4324
	// (get) Token: 0x06004667 RID: 18023 RVA: 0x0013305E File Offset: 0x0013145E
	protected override bool SupportsAutoJoin
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06004668 RID: 18024 RVA: 0x00133064 File Offset: 0x00131464
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

	// Token: 0x06004669 RID: 18025 RVA: 0x00133125 File Offset: 0x00131525
	protected override void createScreenCursors()
	{
		base.createScreenCursors();
		this.createCursor(PlayerNum.All);
	}

	// Token: 0x0600466A RID: 18026 RVA: 0x00133138 File Offset: 0x00131538
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

	// Token: 0x0600466B RID: 18027 RVA: 0x001332CD File Offset: 0x001316CD
	private void tweenIn()
	{
		this.tweenInItem(this.RightTweenIn, 1, null);
		this.tweenInItem(this.LeftTweenIn, -1, delegate()
		{
			this.onAnimationsComplete();
		});
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x001332F8 File Offset: 0x001316F8
	private void tweenInItem(GameObject theList, int direction, Action callback = null)
	{
		Vector3 localPosition = theList.transform.localPosition;
		Vector3 localPosition2 = theList.transform.localPosition;
		localPosition2.x += base.gameDataManager.ConfigData.uiConfig.canvasScaler.referenceResolution.x * (float)direction;
		theList.transform.localPosition = localPosition2;
		DOTween.To(() => theList.transform.localPosition, delegate(Vector3 valueIn)
		{
			theList.transform.localPosition = valueIn;
		}, localPosition, 0.25f).SetEase(Ease.OutSine).OnComplete(delegate
		{
			if (callback != null)
			{
				callback();
			}
		});
	}

	// Token: 0x0600466D RID: 18029 RVA: 0x001333B8 File Offset: 0x001317B8
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

	// Token: 0x0600466E RID: 18030 RVA: 0x001333F8 File Offset: 0x001317F8
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

	// Token: 0x0600466F RID: 18031 RVA: 0x00133440 File Offset: 0x00131840
	public void ResetStageStateData()
	{
		for (int i = 0; i < this.stages.Count; i++)
		{
			StageData stageData = this.stages[i];
			base.events.Broadcast(new SetStageStateRequest(stageData.stageID, StageState.None));
		}
		base.audioManager.PlayMenuSound(SoundKey.stageSelect_stageStikesReset, 0f);
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x0013349F File Offset: 0x0013189F
	public override void OnCancelPressed(IPlayerCursor cursor)
	{
		this.GoToPreviousScreen();
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x001334A7 File Offset: 0x001318A7
	public override void GoToNextScreen()
	{
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x001334A9 File Offset: 0x001318A9
	public override void OnStartPressed(IPlayerCursor cursor)
	{
		base.events.Broadcast(new SelectStageRequest(this.selectedStageID, true));
	}

	// Token: 0x06004673 RID: 18035 RVA: 0x001334C4 File Offset: 0x001318C4
	public override void UpdatePayload(Payload payload)
	{
		base.UpdatePayload(payload);
		GameLoadPayload newPayload = this.gamePayload;
		this.selectStage(newPayload.stage);
		bool flag = false;
		if (this.currentPayload != null)
		{
			flag = this.currentPayload.isConfirmed;
		}
		if (newPayload.isConfirmed != flag)
		{
			if (newPayload.isConfirmed)
			{
				this.showEnteringGame(delegate
				{
					this.enterGame(newPayload.stage);
				});
			}
			else if (this.dialog != null)
			{
				this.dialog.Close();
			}
		}
		foreach (StageID key in newPayload.stagePayloadData.stageStates.Keys)
		{
			this.stageItemByID[key].SetStageState(newPayload.stagePayloadData.stageStates[key], false);
		}
		this.currentPayload = newPayload.Clone();
	}

	// Token: 0x06004674 RID: 18036 RVA: 0x00133600 File Offset: 0x00131A00
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

	// Token: 0x06004675 RID: 18037 RVA: 0x001336F0 File Offset: 0x00131AF0
	private void fadeOutAndDestroyPreview(StageSelectPreview preview)
	{
		if (preview != null)
		{
			preview.TweenAlpha(0f, 0.08f, delegate()
			{
				preview.gameObject.SetActive(false);
				this.releaseStageSelectPreview(preview);
			});
		}
	}

	// Token: 0x06004676 RID: 18038 RVA: 0x00133744 File Offset: 0x00131B44
	private void showEnteringGame(Action callback)
	{
		this.dialog = base.dialogController.ShowOneButtonSpinnyDialog(base.localization.GetText("dialog.enteringGame.title"), base.localization.GetText("dialog.cancel"));
		Action onLoadComplete = delegate()
		{
			Action callback2 = callback;
			this.dialog.Close();
			if (callback2 != null)
			{
				callback2();
			}
		};
		Action closeCallback = delegate()
		{
			this.timer.CancelTimeout(onLoadComplete);
			this.dialog = null;
			callback = null;
		};
		Action action = delegate()
		{
			this.events.Broadcast(new EnterGameRequest(StageID.None, false));
		};
		this.dialog.CloseCallback = closeCallback;
		this.dialog.ConfirmCallback = action;
		this.dialog.CancelCallback = action;
		int time = 2000;
		this.timer.SetTimeout(time, onLoadComplete);
	}

	// Token: 0x06004677 RID: 18039 RVA: 0x001337FE File Offset: 0x00131BFE
	private void enterGame(StageID stageID)
	{
		base.events.Broadcast(new EnterGameRequest(stageID, true));
	}

	// Token: 0x06004678 RID: 18040 RVA: 0x00133814 File Offset: 0x00131C14
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

	// Token: 0x06004679 RID: 18041 RVA: 0x0013385F File Offset: 0x00131C5F
	private void releaseStageSelectPreview(StageSelectPreview obj)
	{
		this.previewList.Add(obj);
	}

	// Token: 0x04002EA4 RID: 11940
	private const float ENTER_GAME_DISPLAY_MIN_TIME = 2f;

	// Token: 0x04002EA5 RID: 11941
	private const float ENTER_NETWORK_GAME_DISPLAY_MIN_TIME = 5f;

	// Token: 0x04002EA6 RID: 11942
	private StageID selectedStageID;

	// Token: 0x04002EA7 RID: 11943
	private List<StageData> stages;

	// Token: 0x04002EA8 RID: 11944
	private Dictionary<StageID, StageData> stagesByID = new Dictionary<StageID, StageData>();

	// Token: 0x04002EA9 RID: 11945
	private Dictionary<StageID, StageSelectItem> stageItemByID = new Dictionary<StageID, StageSelectItem>();

	// Token: 0x04002EAA RID: 11946
	public TextMeshProUGUI SelectedStageName;

	// Token: 0x04002EAB RID: 11947
	public TextMeshProUGUI SelectedStageDesc;

	// Token: 0x04002EAC RID: 11948
	public GridLayoutGroup StageListTop;

	// Token: 0x04002EAD RID: 11949
	public GridLayoutGroup StageListBottom;

	// Token: 0x04002EAE RID: 11950
	public GameObject LeftTweenIn;

	// Token: 0x04002EAF RID: 11951
	public GameObject RightTweenIn;

	// Token: 0x04002EB0 RID: 11952
	public GameObject StageItemPrefab;

	// Token: 0x04002EB1 RID: 11953
	public GameObject StagePreviewPrefab;

	// Token: 0x04002EB2 RID: 11954
	public GameObject StagePreviewSlot;

	// Token: 0x04002EB3 RID: 11955
	public GameObject BackButtonPrefab;

	// Token: 0x04002EB4 RID: 11956
	public Transform BackButtonAnchor;

	// Token: 0x04002EB5 RID: 11957
	public GameObject StrikeInstructionsPrefab;

	// Token: 0x04002EB6 RID: 11958
	public Transform StrikeInstructionsAnchor;

	// Token: 0x04002EB7 RID: 11959
	private List<StageSelectPreview> previewList = new List<StageSelectPreview>();

	// Token: 0x04002EB8 RID: 11960
	public Vector2 StageBuffer = default(Vector2);

	// Token: 0x04002EB9 RID: 11961
	public ScreenType PreviousScreen;

	// Token: 0x04002EBA RID: 11962
	public ScreenType NextScreen;

	// Token: 0x04002EBB RID: 11963
	private GameLoadPayload currentPayload;

	// Token: 0x04002EBC RID: 11964
	private GenericDialog dialog;

	// Token: 0x04002EBD RID: 11965
	private StageSelectPreview currentPreview;
}
