// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectScreenController : UIScreenController
{
	[Inject]
	public SelectRandomCharacters selectRandomChars
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
	public IEnterNewGame enterNewGame
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
	public IRichPresence richPresence
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

	private List<StageID> legalStageIDs
	{
		get
		{
			return this.gamePayload.stagePayloadData.legalStages;
		}
	}

	private Dictionary<StageID, StageState> stageStates
	{
		get
		{
			return this.gamePayload.stagePayloadData.stageStates;
		}
	}

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

	protected override void setupListeners()
	{
		base.subscribe(typeof(SetStageStateRequest), new Action<GameEvent>(this.onStageStateRequest));
		base.subscribe(typeof(SelectStageRequest), new Action<GameEvent>(this.onSelectStageRequest));
		base.subscribe(typeof(EnterGameRequest), new Action<GameEvent>(this.onEnterGame));
		base.subscribe(typeof(PreviousScreenRequest), new Action<GameEvent>(this.onPreviousScreenRequest));
	}

	protected override void setup()
	{
		base.setup();
		this.gamePayload.isConfirmed = false;
		this.gamePayload.stage = StageID.None;
		foreach (StageID current in this.legalStageIDs)
		{
			if (!this.stageStates.ContainsKey(current))
			{
				this.stageStates.Add(current, StageState.None);
			}
			this.setStageState(current, StageState.None);
		}
	}

	private void onPreviousScreenRequest(GameEvent message)
	{
		this.richPresence.SetPresence("InCharacterSelect", null, null, null);
		this.events.Broadcast(new LoadScreenCommand(ScreenType.CharacterSelect, null, ScreenUpdateType.Previous));
	}

	private void onSelectStageRequest(GameEvent message)
	{
		SelectStageRequest selectStageRequest = message as SelectStageRequest;
		if (this.legalStageIDs.Contains(selectStageRequest.stageID) && this.stageStates[selectStageRequest.stageID] != StageState.Struck)
		{
			this.selectStage(selectStageRequest.stageID, selectStageRequest.confirmed);
		}
	}

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
			UnityEngine.Debug.LogError("Failed to find any loadable non-random stages");
			throw new Exception("No non-random stages were found");
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}

	private void onStageStateRequest(GameEvent message)
	{
		SetStageStateRequest setStageStateRequest = message as SetStageStateRequest;
		this.setStageState(setStageStateRequest.stageID, setStageStateRequest.state);
	}

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

	private void onPayloadChanged()
	{
		base.sendUpdate(new UIPayloadUpdate(this.gamePayload));
	}
}
