using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020008D6 RID: 2262
public class CharacterSelectionPortrait : MonoBehaviour
{
	// Token: 0x17000DBF RID: 3519
	// (get) Token: 0x0600391C RID: 14620 RVA: 0x0010C1E1 File Offset: 0x0010A5E1
	// (set) Token: 0x0600391D RID: 14621 RVA: 0x0010C1E9 File Offset: 0x0010A5E9
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000DC0 RID: 3520
	// (get) Token: 0x0600391E RID: 14622 RVA: 0x0010C1F2 File Offset: 0x0010A5F2
	// (set) Token: 0x0600391F RID: 14623 RVA: 0x0010C1FA File Offset: 0x0010A5FA
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000DC1 RID: 3521
	// (get) Token: 0x06003920 RID: 14624 RVA: 0x0010C203 File Offset: 0x0010A603
	// (set) Token: 0x06003921 RID: 14625 RVA: 0x0010C20B File Offset: 0x0010A60B
	[Inject]
	public ITokenManager tokenManager { get; set; }

	// Token: 0x17000DC2 RID: 3522
	// (get) Token: 0x06003922 RID: 14626 RVA: 0x0010C214 File Offset: 0x0010A614
	// (set) Token: 0x06003923 RID: 14627 RVA: 0x0010C21C File Offset: 0x0010A61C
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000DC3 RID: 3523
	// (get) Token: 0x06003924 RID: 14628 RVA: 0x0010C225 File Offset: 0x0010A625
	// (set) Token: 0x06003925 RID: 14629 RVA: 0x0010C22D File Offset: 0x0010A62D
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x17000DC4 RID: 3524
	// (get) Token: 0x06003926 RID: 14630 RVA: 0x0010C236 File Offset: 0x0010A636
	// (set) Token: 0x06003927 RID: 14631 RVA: 0x0010C23E File Offset: 0x0010A63E
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x06003928 RID: 14632 RVA: 0x0010C247 File Offset: 0x0010A647
	// (set) Token: 0x06003929 RID: 14633 RVA: 0x0010C24F File Offset: 0x0010A64F
	[Inject]
	public ICharacterSelectModel model { get; set; }

	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x0600392A RID: 14634 RVA: 0x0010C258 File Offset: 0x0010A658
	// (set) Token: 0x0600392B RID: 14635 RVA: 0x0010C260 File Offset: 0x0010A660
	[Inject]
	public DeveloperConfig devConfig { get; set; }

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x0600392C RID: 14636 RVA: 0x0010C269 File Offset: 0x0010A669
	// (set) Token: 0x0600392D RID: 14637 RVA: 0x0010C271 File Offset: 0x0010A671
	[Inject]
	public IUserCharacterUnlockModel characterUnlockModel { get; set; }

	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x0600392E RID: 14638 RVA: 0x0010C27A File Offset: 0x0010A67A
	// (set) Token: 0x0600392F RID: 14639 RVA: 0x0010C282 File Offset: 0x0010A682
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x17000DC9 RID: 3529
	// (get) Token: 0x06003930 RID: 14640 RVA: 0x0010C28B File Offset: 0x0010A68B
	// (set) Token: 0x06003931 RID: 14641 RVA: 0x0010C293 File Offset: 0x0010A693
	[Inject]
	public ISoundFileManager soundFileManager { get; set; }

	// Token: 0x17000DCA RID: 3530
	// (get) Token: 0x06003932 RID: 14642 RVA: 0x0010C29C File Offset: 0x0010A69C
	// (set) Token: 0x06003933 RID: 14643 RVA: 0x0010C2A4 File Offset: 0x0010A6A4
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x17000DCB RID: 3531
	// (get) Token: 0x06003934 RID: 14644 RVA: 0x0010C2AD File Offset: 0x0010A6AD
	private GameMode currentMode
	{
		get
		{
			return this.modeData.Type;
		}
	}

	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x06003935 RID: 14645 RVA: 0x0010C2BA File Offset: 0x0010A6BA
	// (set) Token: 0x06003936 RID: 14646 RVA: 0x0010C2C2 File Offset: 0x0010A6C2
	public CharacterID CharacterId { get; set; }

	// Token: 0x06003937 RID: 14647 RVA: 0x0010C2CC File Offset: 0x0010A6CC
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

	// Token: 0x06003938 RID: 14648 RVA: 0x0010C34C File Offset: 0x0010A74C
	public void OnDestroy()
	{
		if (this.events != null)
		{
			this.events.Unsubscribe(typeof(GameModeChangedEvent), new Events.EventHandler(this.onGameModeChanged));
		}
		this.signalBus.GetSignal<PlayerCursorStatusSignal>().RemoveListener(new Action<PlayerCursor>(this.onPlayerCursorUpdate));
	}

	// Token: 0x06003939 RID: 14649 RVA: 0x0010C3A4 File Offset: 0x0010A7A4
	private void onGameModeChanged(GameEvent message)
	{
		GameModeChangedEvent gameModeChangedEvent = message as GameModeChangedEvent;
		this.modeData = gameModeChangedEvent.data;
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x0010C3C4 File Offset: 0x0010A7C4
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

	// Token: 0x0600393B RID: 14651 RVA: 0x0010C5D8 File Offset: 0x0010A9D8
	private void syncPortrait()
	{
		CharacterMenusData.UIPortraitData uiportraitData = this.character.characterSelectPortraitData;
		if (this.battleServerAPI.IsConnected)
		{
			uiportraitData = this.character.onlinePickPortraitData;
		}
		this.Portrait.transform.localPosition = uiportraitData.offset;
		this.Portrait.transform.localScale = new Vector3(uiportraitData.scale, uiportraitData.scale, uiportraitData.scale);
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x0010C64F File Offset: 0x0010AA4F
	public void UpdatePlayerList(PlayerSelectionList newPlayerList)
	{
		this.playerList = newPlayerList;
		this.updateHighlightState();
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x0010C65E File Offset: 0x0010AA5E
	private void devQuickAddCPU(CursorTargetButton target, PointerEventData eventData)
	{
		if (this.character != null)
		{
			this.signalBus.GetSignal<QuickSelectDebugCPUSignal>().Dispatch(this.character.characterID);
		}
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x0010C68C File Offset: 0x0010AA8C
	private void onClicked(CursorTargetButton target, PointerEventData eventData)
	{
		int pointerId = 0;
		if (eventData != null)
		{
			pointerId = eventData.pointerId;
		}
		this.clickCharacter(pointerId);
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x0010C6B0 File Offset: 0x0010AAB0
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

	// Token: 0x06003940 RID: 14656 RVA: 0x0010C78C File Offset: 0x0010AB8C
	private void onPlayerCursorUpdate(PlayerCursor cursor)
	{
		PlayerNum player = cursor.Player;
		if (cursor.IsHidden && this.selectingList.Contains(player))
		{
			this.selectingList.Remove(player);
		}
		this.updateHighlightState();
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x0010C7D0 File Offset: 0x0010ABD0
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

	// Token: 0x06003942 RID: 14658 RVA: 0x0010C858 File Offset: 0x0010AC58
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

	// Token: 0x06003943 RID: 14659 RVA: 0x0010C8BC File Offset: 0x0010ACBC
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

	// Token: 0x06003944 RID: 14660 RVA: 0x0010C914 File Offset: 0x0010AD14
	private bool shouldMouseover()
	{
		return this.selectingList.Count > 0;
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x0010C92C File Offset: 0x0010AD2C
	private bool shouldActive()
	{
		IEnumerator enumerator = ((IEnumerable)this.playerList).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PlayerSelectionInfo playerSelectionInfo = (PlayerSelectionInfo)obj;
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

	// Token: 0x04002787 RID: 10119
	public Image Portrait;

	// Token: 0x04002788 RID: 10120
	public Image LegacyImage;

	// Token: 0x04002789 RID: 10121
	public GameObject LockImage;

	// Token: 0x0400278A RID: 10122
	public CursorTargetButton ButtonInteract;

	// Token: 0x0400278B RID: 10123
	public GameObject MouseoverImage;

	// Token: 0x0400278C RID: 10124
	public GameObject SelectedImage;

	// Token: 0x0400278D RID: 10125
	public GameObject RandomMode;

	// Token: 0x0400278E RID: 10126
	public List<GameObject> TokenSnaps = new List<GameObject>();

	// Token: 0x0400278F RID: 10127
	private Color LockedColor = new Color(0.1f, 0.1f, 0.1f, 1f);

	// Token: 0x04002790 RID: 10128
	private CharacterMenusData character;

	// Token: 0x04002791 RID: 10129
	private PlayerSelectionList playerList;

	// Token: 0x04002792 RID: 10130
	private GameModeData modeData;

	// Token: 0x04002794 RID: 10132
	private List<PlayerNum> selectingList = new List<PlayerNum>();

	// Token: 0x04002795 RID: 10133
	public CharacterSelectionPortrait.CanInteractCallback canInteractCallback;

	// Token: 0x020008D7 RID: 2263
	// (Invoke) Token: 0x06003947 RID: 14663
	public delegate bool CanInteractCallback();
}
