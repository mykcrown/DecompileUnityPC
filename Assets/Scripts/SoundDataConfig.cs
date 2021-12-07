// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class SoundDataConfig : ScriptableObject
{
	[Serializable]
	public class WavedashArena
	{
		public AudioData music;
	}

	[Serializable]
	public class Cryostation
	{
		public AudioData music;
	}

	[Serializable]
	public class CombatLab
	{
		public AudioData music;
	}

	[Serializable]
	public class CombatLabAlt
	{
		public AudioData music;
	}

	[Serializable]
	public class Malumalu
	{
		public AudioData music;
	}

	[Serializable]
	public class MalumaluAlt
	{
		public AudioData music;
	}

	[Serializable]
	public class ForbiddenShrine
	{
		public AudioData music;
	}

	[Serializable]
	public class ForbiddenShrineAlt
	{
		public AudioData music;
	}

	[Serializable]
	public class Shadowbriar
	{
		public AudioData music;
	}

	[Serializable]
	public class ShadowbriarAlt
	{
		public AudioData music;
	}

	[Serializable]
	public class Cargo
	{
		public AudioData music;
	}

	[Serializable]
	public class AshaniPostGame
	{
		public AudioData levelup;
	}

	[Serializable]
	public class KiddPostGame
	{
		public AudioData levelup;
	}

	[Serializable]
	public class XanaPostGame
	{
		public AudioData levelup;
	}

	[Serializable]
	public class RaymerPostGame
	{
		public AudioData levelup;
	}

	[Serializable]
	public class ZhurongPostGame
	{
		public AudioData levelup;
	}

	[Serializable]
	public class AfiPostGame
	{
		public AudioData levelup;
	}

	[Serializable]
	public class WeishanPostGame
	{
		public AudioData levelup;
	}

	[Serializable]
	public class AshaniStore
	{
		public AudioData purchaseItem;
	}

	[Serializable]
	public class KiddStore
	{
		public AudioData purchaseItem;
	}

	[Serializable]
	public class XanaStore
	{
		public AudioData purchaseItem;
	}

	[Serializable]
	public class RaymerStore
	{
		public AudioData purchaseItem;
	}

	[Serializable]
	public class ZhurongStore
	{
		public AudioData purchaseItem;
	}

	[Serializable]
	public class AfiStore
	{
		public AudioData purchaseItem;
	}

	[Serializable]
	public class WeishanStore
	{
		public AudioData purchaseItem;
	}

	[Serializable]
	public class GenericSounds
	{
		public AudioData buttonHighlight;

		public AudioData dialogOpen;

		public AudioData dialogClose;

		public AudioData arrowClickLeft;

		public AudioData arrowClickRight;

		public AudioData dropdownOpen;

		public AudioData dropdownClose;

		public AudioData toggleOn;

		public AudioData toggleOff;

		public AudioData typingCharacter;

		public AudioData typingBackspace;

		public AudioData screenForwardTransition;

		public AudioData screenBackTransition;

		public AudioData dynamicMoveActivate;

		public AudioData powerMoveActivate;

		public AudioData dynamicMoveDenied;

		public AudioData powerMoveDenied;
	}

	[Serializable]
	public class LoginSounds
	{
		public AudioData accountCreationConfirmed;

		public AudioData accountCreationError;
	}

	[Serializable]
	public class MainMenuSounds
	{
		public AudioData music;

		public AudioData mainMenuFoldoutOpen;

		public AudioData mainMenuFoldoutClose;

		public AudioData queueStart;

		public AudioData queueStop;
	}

	[Serializable]
	public class CharacterSelectSounds
	{
		public AudioData characterSelectScreenOpen;

		public AudioData onlineBlindPickScreenOpen;

		public AudioData characterHighlight;

		public AudioData characterSelect;

		public AudioData characterDeselect;

		public AudioData randomHighlight;

		public AudioData randomSelect;

		public AudioData randomDeselect;

		public AudioData skinCycle;

		public AudioData teamCycle;

		public AudioData totemPartnerCycle;

		public AudioData ruleSetDelete;

		public AudioData ruleSetAdd;

		public AudioData ruleSetSelect;
	}

	[Serializable]
	public class StageSelectSounds
	{
		public AudioData stageSelectScreenOpen;

		public AudioData stageStrike;

		public AudioData stageUnstrike;

		public AudioData stageStikesReset;
	}

	[Serializable]
	public class LoadingBattleSounds
	{
		public AudioData loadingBattleScreenOpen;
	}

	[Serializable]
	public class InGameSounds
	{
		public AudioData exitGame;

		public AudioData endGame;

		public AudioData pause;

		public AudioData unpause;

		public AudioData flyout;

		public AudioData flyout_small;

		public AudioData tumbleHitGround;
	}

	[Serializable]
	public class VictoryScreenSounds
	{
		public AudioData victoryScreenOpen;
	}

	[Serializable]
	public class PlayerProgressionSounds
	{
		public AudioData playerProgressionScreenOpen;

		public AudioData experienceTick;

		public AudioData levelUp;
	}

	[Serializable]
	public class CustomLobbySounds
	{
		public AudioData customLobbyScreenOpen;

		public AudioData customLobbyPlayerJoined;

		public AudioData customLobbyPlayerLeft;
	}

	[Serializable]
	public class SettingsSounds
	{
		public AudioData settingsScreenOpen;

		public AudioData controlBindingSelect;

		public AudioData controlBindingSet;

		public AudioData settingsReset;

		public AudioData settingsTabChange;

		public AudioData soundEffectSliderSet;
	}

	[Serializable]
	public class StoreSounds
	{
		public AudioData music;

		public AudioData storeScreenOpen;

		public AudioData characterEquipViewOpen;

		public AudioData characterEquipViewClosed;

		public AudioData collectiblesEquipViewOpen;

		public AudioData collectiblesEquipViewClosed;

		public AudioData miniCharacterSelected;

		public AudioData categorySelected;

		public AudioData equipmentSelected;

		public AudioData itemEquip;

		public AudioData storeTabChange;

		public AudioData purchaseDialogOpened;

		public AudioData purchaseConfirmed;
	}

	[Serializable]
	public class UnboxingSounds
	{
		public AudioData music;

		public AudioData openUnboxingScene;

		public AudioData exitUnboxingScene;

		public AudioData clickOpenBox;

		public AudioData clickOpenBoxButton;

		public AudioData crystalDrop;

		public AudioData portalAppear;

		public AudioData blueFlashSide;

		public AudioData blueFlashCenter;

		public AudioData highlight;

		public AudioData currency;

		public AudioData itemSideCommon;

		public AudioData itemSideUncommon;

		public AudioData itemSideRare;

		public AudioData itemSideLegendary;

		public AudioData itemCenterCommon;

		public AudioData itemCenterUncommon;

		public AudioData itemCenterRare;

		public AudioData itemCenterLegendary;
	}

	[Serializable]
	public class CreditsScreenSounds
	{
		public AudioData music;
	}

	public bool editorTestingMode;

	public SoundDataConfig.GenericSounds generic = new SoundDataConfig.GenericSounds();

	public SoundDataConfig.WavedashArena wavedashArena = new SoundDataConfig.WavedashArena();

	public SoundDataConfig.Cryostation cryoStation = new SoundDataConfig.Cryostation();

	public SoundDataConfig.CombatLab combatLab = new SoundDataConfig.CombatLab();

	public SoundDataConfig.CombatLabAlt combatLabAlt = new SoundDataConfig.CombatLabAlt();

	public SoundDataConfig.Malumalu maluMalu = new SoundDataConfig.Malumalu();

	public SoundDataConfig.MalumaluAlt maluMaluAlt = new SoundDataConfig.MalumaluAlt();

	public SoundDataConfig.ForbiddenShrine forbiddenShrine = new SoundDataConfig.ForbiddenShrine();

	public SoundDataConfig.ForbiddenShrineAlt forbiddenShrineAlt = new SoundDataConfig.ForbiddenShrineAlt();

	public SoundDataConfig.Shadowbriar shadowbriar = new SoundDataConfig.Shadowbriar();

	public SoundDataConfig.ShadowbriarAlt shadowbriarAlt = new SoundDataConfig.ShadowbriarAlt();

	public SoundDataConfig.Cargo cargo = new SoundDataConfig.Cargo();

	public SoundDataConfig.AshaniPostGame ashaniPostGame = new SoundDataConfig.AshaniPostGame();

	public SoundDataConfig.KiddPostGame kiddPostGame = new SoundDataConfig.KiddPostGame();

	public SoundDataConfig.XanaPostGame xanaPostGame = new SoundDataConfig.XanaPostGame();

	public SoundDataConfig.RaymerPostGame raymerPostGame = new SoundDataConfig.RaymerPostGame();

	public SoundDataConfig.ZhurongPostGame zhurongPostGame = new SoundDataConfig.ZhurongPostGame();

	public SoundDataConfig.AfiPostGame afiPostGame = new SoundDataConfig.AfiPostGame();

	public SoundDataConfig.WeishanPostGame weishanPostGame = new SoundDataConfig.WeishanPostGame();

	public SoundDataConfig.AshaniStore ashaniStore = new SoundDataConfig.AshaniStore();

	public SoundDataConfig.KiddStore kiddStore = new SoundDataConfig.KiddStore();

	public SoundDataConfig.XanaStore xanaStore = new SoundDataConfig.XanaStore();

	public SoundDataConfig.RaymerStore raymerStore = new SoundDataConfig.RaymerStore();

	public SoundDataConfig.ZhurongStore zhurongStore = new SoundDataConfig.ZhurongStore();

	public SoundDataConfig.AfiStore afiStore = new SoundDataConfig.AfiStore();

	public SoundDataConfig.WeishanStore weishanStore = new SoundDataConfig.WeishanStore();

	public SoundDataConfig.LoginSounds login = new SoundDataConfig.LoginSounds();

	public SoundDataConfig.MainMenuSounds mainMenu = new SoundDataConfig.MainMenuSounds();

	public SoundDataConfig.CharacterSelectSounds characterSelect = new SoundDataConfig.CharacterSelectSounds();

	public SoundDataConfig.StageSelectSounds stageSelect = new SoundDataConfig.StageSelectSounds();

	public SoundDataConfig.LoadingBattleSounds loadingBattle = new SoundDataConfig.LoadingBattleSounds();

	public SoundDataConfig.InGameSounds inGame = new SoundDataConfig.InGameSounds();

	public SoundDataConfig.VictoryScreenSounds victoryScreen = new SoundDataConfig.VictoryScreenSounds();

	public SoundDataConfig.PlayerProgressionSounds playerProgression = new SoundDataConfig.PlayerProgressionSounds();

	public SoundDataConfig.CustomLobbySounds customLobby = new SoundDataConfig.CustomLobbySounds();

	public SoundDataConfig.SettingsSounds settings = new SoundDataConfig.SettingsSounds();

	public SoundDataConfig.StoreSounds store = new SoundDataConfig.StoreSounds();

	public SoundDataConfig.UnboxingSounds unboxing = new SoundDataConfig.UnboxingSounds();

	public SoundDataConfig.CreditsScreenSounds credits = new SoundDataConfig.CreditsScreenSounds();
}
