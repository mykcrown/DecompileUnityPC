using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009D4 RID: 2516
public class StageSelectScreenController : UIScreenController
{
	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x0600467C RID: 18044 RVA: 0x0013398C File Offset: 0x00131D8C
	// (set) Token: 0x0600467D RID: 18045 RVA: 0x00133994 File Offset: 0x00131D94
	[Inject]
	public SelectRandomCharacters selectRandomChars { get; set; }

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x0600467E RID: 18046 RVA: 0x0013399D File Offset: 0x00131D9D
	// (set) Token: 0x0600467F RID: 18047 RVA: 0x001339A5 File Offset: 0x00131DA5
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x06004680 RID: 18048 RVA: 0x001339AE File Offset: 0x00131DAE
	// (set) Token: 0x06004681 RID: 18049 RVA: 0x001339B6 File Offset: 0x00131DB6
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x06004682 RID: 18050 RVA: 0x001339BF File Offset: 0x00131DBF
	// (set) Token: 0x06004683 RID: 18051 RVA: 0x001339C7 File Offset: 0x00131DC7
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x06004684 RID: 18052 RVA: 0x001339D0 File Offset: 0x00131DD0
	// (set) Token: 0x06004685 RID: 18053 RVA: 0x001339D8 File Offset: 0x00131DD8
	[Inject]
	public IRichPresence richPresence { get; set; }

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x06004686 RID: 18054 RVA: 0x001339E1 File Offset: 0x00131DE1
	private GameLoadPayload gamePayload
	{
		get
		{
			return this.enterNewGame.GamePayload;
		}
	}

	// Token: 0x170010EB RID: 4331
	// (get) Token: 0x06004687 RID: 18055 RVA: 0x001339EE File Offset: 0x00131DEE
	private List<StageID> legalStageIDs
	{
		get
		{
			return this.gamePayload.stagePayloadData.legalStages;
		}
	}

	// Token: 0x170010EC RID: 4332
	// (get) Token: 0x06004688 RID: 18056 RVA: 0x00133A00 File Offset: 0x00131E00
	private Dictionary<StageID, StageState> stageStates
	{
		get
		{
			return this.gamePayload.stagePayloadData.stageStates;
		}
	}

	// Token: 0x06004689 RID: 18057 RVA: 0x00133A14 File Offset: 0x00131E14
	protected override void setupListeners()
	{
		base.subscribe(typeof(SetStageStateRequest), new Action<GameEvent>(this.onStageStateRequest));
		base.subscribe(typeof(SelectStageRequest), new Action<GameEvent>(this.onSelectStageRequest));
		base.subscribe(typeof(EnterGameRequest), new Action<GameEvent>(this.onEnterGame));
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
	}

	// Token: 0x0600468A RID: 18058 RVA: 0x00133A94 File Offset: 0x00131E94
	protected override void setup()
	{
		base.setup();
		this.gamePayload.isConfirmed = false;
		this.gamePayload.stage = StageID.None;
		foreach (StageID stageID in this.legalStageIDs)
		{
			if (!this.stageStates.ContainsKey(stageID))
			{
				this.stageStates.Add(stageID, StageState.None);
			}
			this.setStageState(stageID, StageState.None);
		}
	}

	// Token: 0x0600468B RID: 18059 RVA: 0x00133B30 File Offset: 0x00131F30
	private void onPreviousScreenRequest(GameEvent message)
	{
		this.richPresence.SetPresence("InCharacterSelect", null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Previous));
	}

	// Token: 0x0600468C RID: 18060 RVA: 0x00133B58 File Offset: 0x00131F58
	private void onSelectStageRequest(GameEvent message)
	{
		SelectStageRequest selectStageRequest = message as SelectStageRequest;
		if (this.legalStageIDs.Contains(selectStageRequest.stageID) && this.stageStates[selectStageRequest.stageID] != StageState.Struck)
		{
			this.selectStage(selectStageRequest.stageID, selectStageRequest.confirmed);
		}
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x00133BAB File Offset: 0x00131FAB
	private void selectStage(StageID stageID, bool confirmed)
	{
		if (this.gamePayload.isConfirmed)
		{
			return;
		}
		this.gamePayload.stage = stageID;
		this.gamePayload.isConfirmed = confirmed;
		this.onPayloadChanged();
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x00133BDC File Offset: 0x00131FDC
	private void onEnterGame(GameEvent message)
	{
		bool confirmed = (message as EnterGameRequest).confirmed;
		if (confirmed)
		{
			StageID stageID = (message as EnterGameRequest).stageID;
			if (base.gameDataManager.StageData.GetDataByID(stageID).stageType == StageType.Random)
			{
				StageID randomStageID = this.getRandomStageID();
				this.gamePayload.stagePayloadData.lastRandomStage = randomStageID;
				this.setStageState(randomStageID, StageState.Played);
			}
			else
			{
				this.setStageState(stageID, StageState.Played);
			}
			this.selectRandomChars.Execute(this.gamePayload);
			base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
			this.events.Broadcast(new LoadScreenCommand(ScreenType.LoadingBattle, null, ScreenUpdateType.Next));
			this.richPresence.SetPresence("Loading", null, null, null);
		}
		else
		{
			this.gamePayload.isConfirmed = false;
			this.onPayloadChanged();
		}
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x00133CB0 File Offset: 0x001320B0
	private StageID getRandomStageID()
	{
		List<StageID> list = new List<StageID>();
		for (int i = 0; i < this.legalStageIDs.Count; i++)
		{
			StageID stageID = this.legalStageIDs[i];
			StageData dataByID = base.gameDataManager.StageData.GetDataByID(stageID);
			if (dataByID.stageType != StageType.Random && this.stageStates[stageID] != StageState.Struck)
			{
				list.Add(stageID);
			}
		}
		if (list.Count == 0)
		{
			Debug.LogError("Failed to find any loadable non-random stages");
			throw new Exception("No non-random stages were found");
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x00133D5C File Offset: 0x0013215C
	private void onStageStateRequest(GameEvent message)
	{
		SetStageStateRequest setStageStateRequest = message as SetStageStateRequest;
		this.setStageState(setStageStateRequest.stageID, setStageStateRequest.state);
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x00133D84 File Offset: 0x00132184
	private void setStageState(StageID stageID, StageState stageState)
	{
		if (this.stageStates[stageID] != stageState)
		{
			if (stageState == StageState.Struck && this.playableStageCount <= 1)
			{
				return;
			}
			this.gamePayload.stagePayloadData.stageStates[stageID] = stageState;
			this.onPayloadChanged();
		}
	}

	// Token: 0x170010ED RID: 4333
	// (get) Token: 0x06004692 RID: 18066 RVA: 0x00133DD4 File Offset: 0x001321D4
	private int playableStageCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < this.legalStageIDs.Count; i++)
			{
				StageData dataByID = base.gameDataManager.StageData.GetDataByID(this.legalStageIDs[i]);
				if (dataByID.stageType != StageType.Random)
				{
					StageID stageID = dataByID.stageID;
					if (!this.gamePayload.stagePayloadData.stageStates.ContainsKey(stageID) || this.gamePayload.stagePayloadData.stageStates[stageID] != StageState.Struck)
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x00133E70 File Offset: 0x00132270
	private void onPayloadChanged()
	{
		base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
	}
}
