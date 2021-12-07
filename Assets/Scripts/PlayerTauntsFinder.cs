// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTauntsFinder : IPlayerTauntsFinder
{
	[Inject]
	public IUserTauntsModel userTauntsModel
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
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IItemLoader itemLoader
	{
		get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public IReplaySystem replaySystem
	{
		get;
		set;
	}

	public UserTaunts GetForPlayer(PlayerNum player)
	{
		if (this.enterNewGame.GamePayload != null && (this.enterNewGame.GamePayload.isOnlineGame || this.replaySystem.IsReplaying))
		{
			return this.enterNewGame.GamePayload.FindPlayerInfo(player).tauntData;
		}
		return this.userTauntsModel.GetDataObject(this.userInputManager.GetBestPortId(player));
	}

	public List<MoveData> GetPlayerEmoteMoveData(PlayerNum playerNum, CharacterID characterID)
	{
		List<MoveData> list = new List<MoveData>();
		if (characterID != CharacterID.None)
		{
			UserTaunts forPlayer = this.GetForPlayer(playerNum);
			if (forPlayer == null)
			{
				throw new UnityException("Didn't find taunts for player " + playerNum);
			}
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.LEFT), list);
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.RIGHT), list);
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.UP), list);
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.DOWN), list);
		}
		return list;
	}

	public List<HologramData> GetPlayerHologramData(PlayerNum playerNum, CharacterID characterID)
	{
		List<HologramData> list = new List<HologramData>();
		if (characterID != CharacterID.None)
		{
			UserTaunts forPlayer = this.GetForPlayer(playerNum);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.LEFT), list);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.RIGHT), list);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.UP), list);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.DOWN), list);
		}
		return list;
	}

	private void AddEmoteMoveFromEquipmentID(EquipmentID equipmentID, List<MoveData> emoteMoves)
	{
		EquippableItem item = this.equipmentModel.GetItem(equipmentID);
		if (item != null && item.type == EquipmentTypes.EMOTE)
		{
			EmoteData emoteData = this.itemLoader.LoadAsset<EmoteData>(item);
			if (emoteData != null)
			{
				if (emoteData.primaryData != null)
				{
					emoteMoves.Add(emoteData.primaryData);
				}
				if (emoteData.partnerData != null)
				{
					emoteMoves.Add(emoteData.partnerData);
				}
			}
		}
	}

	private void AddHologramDataFromEquipmentID(EquipmentID equipmentID, List<HologramData> holograms)
	{
		EquippableItem item = this.equipmentModel.GetItem(equipmentID);
		if (item != null && item.type == EquipmentTypes.HOLOGRAM)
		{
			HologramData hologramData = this.itemLoader.LoadAsset<HologramData>(item);
			if (hologramData != null)
			{
				holograms.Add(hologramData);
			}
		}
	}
}
