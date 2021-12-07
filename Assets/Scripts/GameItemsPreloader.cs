// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class GameItemsPreloader : IGameItemsPreloader
{
	private sealed class _recursiveLoadPlayers_c__AnonStorey1
	{
		internal PlayerSelectionList list;

		internal int i;

		internal Action callback;

		internal GameItemsPreloader _this;
	}

	private sealed class _recursiveLoadPlayers_c__AnonStorey0
	{
		internal PlayerSelectionInfo info;

		internal GameItemsPreloader._recursiveLoadPlayers_c__AnonStorey1 __f__ref_1;

		internal void __m__0()
		{
			this.__f__ref_1._this.loadPlayerVictoryPose(this.info, new Action(this.__m__1));
		}

		internal void __m__1()
		{
			this.__f__ref_1._this.loadPlayerEmotes(this.info, new Action(this.__m__2));
		}

		internal void __m__2()
		{
			this.__f__ref_1._this.recursiveLoadPlayers(this.__f__ref_1.list, this.__f__ref_1.i + 1, this.__f__ref_1.callback);
		}
	}

	private sealed class _preloadEmotes_c__AnonStorey3
	{
		internal Action callback;
	}

	private sealed class _preloadEmotes_c__AnonStorey2
	{
		internal ReferenceValue<int> remaining;

		internal GameItemsPreloader._preloadEmotes_c__AnonStorey3 __f__ref_3;

		internal void __m__0()
		{
			this.remaining.Value--;
			if (this.remaining.Value == 0)
			{
				this.__f__ref_3.callback();
			}
		}
	}

	private static Func<EquippableItem, bool> __f__am_cache0;

	[Inject]
	public IRespawnPlatformLocator respawnPlatformLocator
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel
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
	public IPlayerTauntsFinder playerTaunts
	{
		get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
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
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	public void Preload(GameLoadPayload payload, Action callback)
	{
		this.recursiveLoadPlayers(payload.players, 0, callback);
	}

	private void recursiveLoadPlayers(PlayerSelectionList list, int i, Action callback)
	{
		GameItemsPreloader._recursiveLoadPlayers_c__AnonStorey1 _recursiveLoadPlayers_c__AnonStorey = new GameItemsPreloader._recursiveLoadPlayers_c__AnonStorey1();
		_recursiveLoadPlayers_c__AnonStorey.list = list;
		_recursiveLoadPlayers_c__AnonStorey.i = i;
		_recursiveLoadPlayers_c__AnonStorey.callback = callback;
		_recursiveLoadPlayers_c__AnonStorey._this = this;
		if (_recursiveLoadPlayers_c__AnonStorey.i > _recursiveLoadPlayers_c__AnonStorey.list.Length - 1)
		{
			_recursiveLoadPlayers_c__AnonStorey.callback();
		}
		else
		{
			GameItemsPreloader._recursiveLoadPlayers_c__AnonStorey0 _recursiveLoadPlayers_c__AnonStorey2 = new GameItemsPreloader._recursiveLoadPlayers_c__AnonStorey0();
			_recursiveLoadPlayers_c__AnonStorey2.__f__ref_1 = _recursiveLoadPlayers_c__AnonStorey;
			_recursiveLoadPlayers_c__AnonStorey2.info = _recursiveLoadPlayers_c__AnonStorey.list[_recursiveLoadPlayers_c__AnonStorey.i];
			if (_recursiveLoadPlayers_c__AnonStorey2.info.type == PlayerType.None || _recursiveLoadPlayers_c__AnonStorey2.info.isSpectator)
			{
				this.recursiveLoadPlayers(_recursiveLoadPlayers_c__AnonStorey.list, _recursiveLoadPlayers_c__AnonStorey.i + 1, _recursiveLoadPlayers_c__AnonStorey.callback);
			}
			else
			{
				this.loadPlayerPlatform(_recursiveLoadPlayers_c__AnonStorey2.info, new Action(_recursiveLoadPlayers_c__AnonStorey2.__m__0));
			}
		}
	}

	private void loadPlayerPlatform(PlayerSelectionInfo info, Action callback)
	{
		EquippableItem customItem = this.respawnPlatformLocator.GetCustomItem(info);
		if (customItem == null)
		{
			callback();
		}
		else
		{
			this.itemLoader.Preload<CustomPlatform>(customItem, callback);
		}
	}

	private void loadPlayerVictoryPose(PlayerSelectionInfo info, Action callback)
	{
		int bestPortId = this.userInputManager.GetBestPortId(info.playerNum);
		EquippableItem equippedItem = this.userCharacterEquippedModel.GetEquippedItem(EquipmentTypes.VICTORY_POSE, info.characterID, bestPortId);
		if (equippedItem == null)
		{
			callback();
		}
		else
		{
			this.itemLoader.PreloadAsset(equippedItem, callback);
		}
	}

	private void loadPlayerEmotes(PlayerSelectionInfo info, Action callback)
	{
		UserTaunts forPlayer = this.playerTaunts.GetForPlayer(info.playerNum);
		List<EquippableItem> loadList = null;
		if (forPlayer != null)
		{
			IEnumerable<EquippableItem> arg_54_0 = forPlayer.GetSlotsForCharacter(info.characterID).Select(new Func<KeyValuePair<TauntSlot, EquipmentID>, EquippableItem>(this._loadPlayerEmotes_m__0));
			if (GameItemsPreloader.__f__am_cache0 == null)
			{
				GameItemsPreloader.__f__am_cache0 = new Func<EquippableItem, bool>(GameItemsPreloader._loadPlayerEmotes_m__1);
			}
			loadList = arg_54_0.Where(GameItemsPreloader.__f__am_cache0).ToList<EquippableItem>();
		}
		this.preloadEmotes(loadList, callback);
	}

	private void preloadEmotes(List<EquippableItem> loadList, Action callback)
	{
		GameItemsPreloader._preloadEmotes_c__AnonStorey3 _preloadEmotes_c__AnonStorey = new GameItemsPreloader._preloadEmotes_c__AnonStorey3();
		_preloadEmotes_c__AnonStorey.callback = callback;
		if (loadList == null || loadList.Count == 0)
		{
			_preloadEmotes_c__AnonStorey.callback();
		}
		else
		{
			GameItemsPreloader._preloadEmotes_c__AnonStorey2 _preloadEmotes_c__AnonStorey2 = new GameItemsPreloader._preloadEmotes_c__AnonStorey2();
			_preloadEmotes_c__AnonStorey2.__f__ref_3 = _preloadEmotes_c__AnonStorey;
			_preloadEmotes_c__AnonStorey2.remaining = new ReferenceValue<int>(loadList.Count);
			foreach (EquippableItem current in loadList)
			{
				this.itemLoader.PreloadAsset(current, new Action(_preloadEmotes_c__AnonStorey2.__m__0));
			}
		}
	}

	private EquippableItem _loadPlayerEmotes_m__0(KeyValuePair<TauntSlot, EquipmentID> kv)
	{
		return this.equipmentModel.GetItem(kv.Value);
	}

	private static bool _loadPlayerEmotes_m__1(EquippableItem item)
	{
		return item != null && item.type == EquipmentTypes.EMOTE;
	}
}
