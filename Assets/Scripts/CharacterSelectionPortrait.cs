// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectionPortrait : MonoBehaviour
{
	public delegate bool CanInteractCallback();

	public Image Portrait;

	public Image LegacyImage;

	public GameObject LockImage;

	public CursorTargetButton ButtonInteract;

	public GameObject MouseoverImage;

	public GameObject SelectedImage;

	public GameObject RandomMode;

	public List<GameObject> TokenSnaps = new List<GameObject>();

	private Color LockedColor = new Color(0.1f, 0.1f, 0.1f, 1f);

	private CharacterMenusData character;

	private PlayerSelectionList playerList;

	private GameModeData modeData;

	private List<PlayerNum> selectingList = new List<PlayerNum>();

	public CharacterSelectionPortrait.CanInteractCallback canInteractCallback;

	[Inject]
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public ITokenManager tokenManager
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
	public IDialogController dialogController
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public ICharacterSelectModel model
	{
		get;
		set;
	}

	[Inject]
	public DeveloperConfig devConfig
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel
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
	public ISoundFileManager soundFileManager
	{
		get;
		set;
	}

	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader
	{
		private get;
		set;
	}

	private GameMode currentMode
	{
		get
		{
			return this.modeData.Type;
		}
	}

	public CharacterID CharacterId
	{
		get;
		set;
	}

	private void Awake()
	{
		if (this.events != null)
		{
			this.events.Subscribe(typeof(GameModeChangedEvent), new Events.EventHandler(this.onGameModeChanged));
		}
		this.ButtonInteract.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onClicked);
		this.ButtonInteract.SelectCallback = new Action<CursorTargetButton, PointerEventData>(this.onSelect);
		this.ButtonInteract.DeselectCallback = new Action<CursorTargetButton, PointerEventData>(this.onDeselect);
	}

	public void OnDestroy()
	{
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(GameModeChangedEvent), new Events.EventHandler(this.onGameModeChanged));
		}
		this.signalBus.GetSignal<PlayerCursorStatusSignal>().RemoveListener(new Action<PlayerCursor>(this.onPlayerCursorUpdate));
	}

	private void onGameModeChanged(GameEvent message)
	{
		GameModeChangedEvent gameModeChangedEvent = message as GameModeChangedEvent;
		this.modeData = gameModeChangedEvent.data;
	}

	public void Init(CharacterDefinition characterDef, PlayerSelectionList playerList, GameModeData modeData)
	{
		this.character = this.characterMenusDataLoader.GetData(characterDef);
		this.CharacterId = this.character.characterID;
		this.playerList = playerList;
		this.modeData = modeData;
		this.userInputManager = this.userInputManager;
		if (this.devConfig.rightClickAddCPU)
		{
			this.ButtonInteract.AltSubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.devQuickAddCPU);
		}
		if (this.character != null)
		{
			this.Portrait.overrideSprite = this.character.generalPortrait;
			this.LegacyImage.gameObject.SetActive(false);
			if (this.character.generalPortrait == null)
			{
				this.LegacyImage.gameObject.SetActive(true);
				this.LegacyImage.overrideSprite = this.character.smallPortrait;
			}
			Material material = UnityEngine.Object.Instantiate<Material>(this.Portrait.material);
			this.Portrait.material = material;
			if (!this.characterUnlockModel.IsUnlockedInGameMode(this.character.characterID, modeData.Type))
			{
				this.Portrait.material.SetFloat("_ColorWeight", 0.7f);
				this.Portrait.color = this.LockedColor;
				this.LockImage.SetActive(true);
			}
		}
		if (this.character.isRandom)
		{
			this.Portrait.gameObject.SetActive(false);
			this.LegacyImage.gameObject.SetActive(false);
			this.ButtonInteract.UseOverrideHighlightSound = true;
			this.ButtonInteract.OverrideHighlightSound = this.soundFileManager.GetSoundAsAudioData(SoundKey.characterSelect_randomHighlight);
			this.RandomMode.SetActive(true);
		}
		else
		{
			this.ButtonInteract.UseOverrideHighlightSound = true;
			this.ButtonInteract.OverrideHighlightSound = this.soundFileManager.GetSoundAsAudioData(SoundKey.characterSelect_characterHighlight);
			this.RandomMode.SetActive(false);
		}
		this.signalBus.GetSignal<PlayerCursorStatusSignal>().AddListener(new Action<PlayerCursor>(this.onPlayerCursorUpdate));
		this.syncPortrait();
	}

	private void syncPortrait()
	{
		CharacterMenusData.UIPortraitData uIPortraitData = this.character.characterSelectPortraitData;
		if (this.battleServerAPI.IsConnected)
		{
			uIPortraitData = this.character.onlinePickPortraitData;
		}
		this.Portrait.transform.localPosition = uIPortraitData.offset;
		this.Portrait.transform.localScale = new Vector3(uIPortraitData.scale, uIPortraitData.scale, uIPortraitData.scale);
	}

	public void UpdatePlayerList(PlayerSelectionList newPlayerList)
	{
		this.playerList = newPlayerList;
		this.updateHighlightState();
	}

	private void devQuickAddCPU(CursorTargetButton target, PointerEventData eventData)
	{
		if (this.character != null)
		{
			this.signalBus.GetSignal<QuickSelectDebugCPUSignal>().Dispatch(this.character.characterID);
		}
	}

	private void onClicked(CursorTargetButton target, PointerEventData eventData)
	{
		int pointerId = 0;
		if (eventData != null)
		{
			pointerId = eventData.pointerId;
		}
		this.clickCharacter(pointerId);
	}

	private void clickCharacter(int pointerId = 0)
	{
		if (this.canInteractCallback != null && !this.canInteractCallback())
		{
			return;
		}
		if (this.character == null)
		{
			return;
		}
		PlayerNum actingPlayer = this.model.GetActingPlayer(pointerId);
		if (this.playerList.GetPlayer(actingPlayer).type == PlayerType.None)
		{
			this.events.Broadcast(new SetPlayerTypeRequest(actingPlayer, PlayerType.Human, true));
		}
		if (this.character.characterID == CharacterID.None)
		{
			this.dialogController.ShowOneButtonDialog("MISSING ID", "No characterID is assigned for  " + this.character.characterID, "OK", WindowTransition.STANDARD_FADE, false, default(AudioData));
		}
		this.events.Broadcast(new SelectCharacterRequest(actingPlayer, this.character.characterID));
	}

	private void onPlayerCursorUpdate(PlayerCursor cursor)
	{
		PlayerNum player = cursor.Player;
		if (cursor.IsHidden && this.selectingList.Contains(player))
		{
			this.selectingList.Remove(player);
		}
		this.updateHighlightState();
	}

	private void onSelect(CursorTargetButton target, PointerEventData eventData)
	{
		if (this.canInteractCallback != null && !this.canInteractCallback())
		{
			return;
		}
		PlayerNum actingPlayer = this.model.GetActingPlayer(eventData.pointerId);
		PlayerNum playerNumFromPointer = PlayerUtil.GetPlayerNumFromPointer(eventData.pointerId);
		if (!this.selectingList.Contains(playerNumFromPointer))
		{
			this.selectingList.Add(playerNumFromPointer);
		}
		this.updateHighlightState();
		this.events.Broadcast(new HighlightCharacterEvent(actingPlayer, this.character.characterDefinition));
	}

	private void onDeselect(CursorTargetButton target, PointerEventData eventData)
	{
		PlayerNum actingPlayer = this.model.GetActingPlayer(eventData.pointerId);
		PlayerNum playerNumFromPointer = PlayerUtil.GetPlayerNumFromPointer(eventData.pointerId);
		if (this.selectingList.Contains(playerNumFromPointer))
		{
			this.selectingList.Remove(playerNumFromPointer);
		}
		this.updateHighlightState();
		this.events.Broadcast(new HighlightCharacterEvent(actingPlayer, null));
	}

	private void updateHighlightState()
	{
		this.MouseoverImage.SetActive(false);
		this.SelectedImage.SetActive(false);
		if (this.shouldMouseover())
		{
			this.MouseoverImage.SetActive(true);
		}
		else if (this.shouldActive())
		{
			this.SelectedImage.SetActive(true);
		}
	}

	private bool shouldMouseover()
	{
		return this.selectingList.Count > 0;
	}

	private bool shouldActive()
	{
		IEnumerator enumerator = ((IEnumerable)this.playerList).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)enumerator.Current;
				if (playerSelectionInfo.characterID == this.character.characterID)
				{
					return true;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		return false;
	}
}
