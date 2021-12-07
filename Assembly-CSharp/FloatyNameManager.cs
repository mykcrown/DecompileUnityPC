using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008BF RID: 2239
public class FloatyNameManager : ClientBehavior
{
	// Token: 0x06003877 RID: 14455 RVA: 0x00108CC4 File Offset: 0x001070C4
	public void Init(GameObject parentNode, GameManager gameManager)
	{
		this.eventManager = gameManager.events;
		foreach (PlayerController playerController in gameManager.GetPlayers())
		{
			CharacterFloatyContainer characterFloatyContainer = UnityEngine.Object.Instantiate<CharacterFloatyContainer>(this.floatyContainerPrefab);
			characterFloatyContainer.transform.SetParent(base.transform, false);
			characterFloatyContainer.PlayerController = playerController;
			characterFloatyContainer.FrameOwner = gameManager;
			this.floatyContainerByPlayer[playerController] = characterFloatyContainer;
		}
		this.eventManager.Subscribe(typeof(CharacterSwapEvent), new Events.EventHandler(this.updateCharacterFloatyText));
		this.eventManager.Subscribe(typeof(GamePausedEvent), new Events.EventHandler(this.pauseGame));
		base.signalBus.GetSignal<EndGameSignal>().AddListener(new Action<VictoryScreenPayload>(this.endGame));
		this.eventManager.Subscribe(typeof(PlayerEngagementStateChangedEvent), new Events.EventHandler(this.onEngagementStateChanged));
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x00108DE0 File Offset: 0x001071E0
	private void updateCharacterFloatyText(GameEvent message)
	{
		CharacterSwapEvent characterSwapEvent = (CharacterSwapEvent)message;
		this.floatyContainerByPlayer[characterSwapEvent.character].UpdateDisplayState();
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x00108E0C File Offset: 0x0010720C
	private void pauseGame(GameEvent message)
	{
		GamePausedEvent gamePausedEvent = (GamePausedEvent)message;
		foreach (CharacterFloatyContainer characterFloatyContainer in this.floatyContainerByPlayer.Values)
		{
			characterFloatyContainer.OnPaused(gamePausedEvent.paused);
		}
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x00108E7C File Offset: 0x0010727C
	private void endGame(VictoryScreenPayload payload)
	{
		foreach (CharacterFloatyContainer characterFloatyContainer in this.floatyContainerByPlayer.Values)
		{
			UnityEngine.Object.Destroy(characterFloatyContainer.gameObject);
		}
		this.floatyContainerByPlayer.Clear();
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x00108EEC File Offset: 0x001072EC
	private void onEngagementStateChanged(GameEvent message)
	{
		PlayerEngagementStateChangedEvent playerEngagementStateChangedEvent = message as PlayerEngagementStateChangedEvent;
		foreach (CharacterFloatyContainer characterFloatyContainer in this.floatyContainerByPlayer.Values)
		{
			if (characterFloatyContainer.PlayerController.PlayerNum == playerEngagementStateChangedEvent.playerNum)
			{
				characterFloatyContainer.UpdateDisplayState();
			}
		}
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x00108F6C File Offset: 0x0010736C
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

	// Token: 0x040026D6 RID: 9942
	public CharacterFloatyContainer floatyContainerPrefab;

	// Token: 0x040026D7 RID: 9943
	private IEvents eventManager;

	// Token: 0x040026D8 RID: 9944
	private Dictionary<PlayerController, CharacterFloatyContainer> floatyContainerByPlayer = new Dictionary<PlayerController, CharacterFloatyContainer>();
}
