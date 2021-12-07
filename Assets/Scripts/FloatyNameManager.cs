// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class FloatyNameManager : ClientBehavior
{
	public CharacterFloatyContainer floatyContainerPrefab;

	private IEvents eventManager;

	private Dictionary<PlayerController, CharacterFloatyContainer> floatyContainerByPlayer = new Dictionary<PlayerController, CharacterFloatyContainer>();

	public void Init(GameObject parentNode, GameManager gameManager)
	{
		this.eventManager = gameManager.events;
		foreach (PlayerController current in gameManager.GetPlayers())
		{
			CharacterFloatyContainer characterFloatyContainer = UnityEngine.Object.Instantiate<CharacterFloatyContainer>(this.floatyContainerPrefab);
			characterFloatyContainer.transform.SetParent(base.transform, false);
			characterFloatyContainer.PlayerController = current;
			characterFloatyContainer.FrameOwner = gameManager;
			this.floatyContainerByPlayer[current] = characterFloatyContainer;
		}
		this.eventManager.Subscribe(typeof(CharacterSwapEvent), new Events.EventHandler(this.updateCharacterFloatyText));
		this.eventManager.Subscribe(typeof(GamePausedEvent), new Events.EventHandler(this.pauseGame));
		base.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.endGame));
		this.eventManager.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
	}

	private void updateCharacterFloatyText(GameEvent message)
	{
		CharacterSwapEvent characterSwapEvent = (CharacterSwapEvent)message;
		this.floatyContainerByPlayer[characterSwapEvent.character].UpdateDisplayState();
	}

	private void pauseGame(GameEvent message)
	{
		GamePausedEvent gamePausedEvent = (GamePausedEvent)message;
		foreach (CharacterFloatyContainer current in this.floatyContainerByPlayer.Values)
		{
			current.OnPaused(gamePausedEvent.paused);
		}
	}

	private void endGame(VictoryScreenPayload payload)
	{
		foreach (CharacterFloatyContainer current in this.floatyContainerByPlayer.Values)
		{
			UnityEngine.Object.Destroy(current.gameObject);
		}
		this.floatyContainerByPlayer.Clear();
	}

	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		foreach (CharacterFloatyContainer current in this.floatyContainerByPlayer.Values)
		{
			if (current.PlayerController.PlayerNum == playerEngagementStateChangedEvent.playerNum)
			{
				current.UpdateDisplayState();
			}
		}
	}

	public void Destroy()
	{
		if (this.eventManager == null)
		{
			return;
		}
		this.floatyContainerByPlayer.Clear();
		this.eventManager.Unsubscribe(typeof(CharacterSwapEvent), new Events.EventHandler(this.updateCharacterFloatyText));
		this.eventManager.Unsubscribe(typeof(GamePausedEvent), new Events.EventHandler(this.pauseGame));
		base.signalBus.GetSignal<EndGameSignal>().RemoveListener(new Action<VictoryScreenPayload>(this.endGame));
		this.eventManager.Unsubscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
	}
}
