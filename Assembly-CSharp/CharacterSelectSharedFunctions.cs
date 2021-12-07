using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000901 RID: 2305
public class CharacterSelectSharedFunctions : ICharacterSelectSharedFunctions
{
	// Token: 0x17000E51 RID: 3665
	// (get) Token: 0x06003BBD RID: 15293 RVA: 0x001159AA File Offset: 0x00113DAA
	// (set) Token: 0x06003BBE RID: 15294 RVA: 0x001159B2 File Offset: 0x00113DB2
	[Inject]
	public ITokenManager tokenManager { get; set; }

	// Token: 0x17000E52 RID: 3666
	// (get) Token: 0x06003BBF RID: 15295 RVA: 0x001159BB File Offset: 0x00113DBB
	// (set) Token: 0x06003BC0 RID: 15296 RVA: 0x001159C3 File Offset: 0x00113DC3
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000E53 RID: 3667
	// (get) Token: 0x06003BC1 RID: 15297 RVA: 0x001159CC File Offset: 0x00113DCC
	// (set) Token: 0x06003BC2 RID: 15298 RVA: 0x001159D4 File Offset: 0x00113DD4
	[Inject]
	public IEvents events { get; set; }

	// Token: 0x17000E54 RID: 3668
	// (get) Token: 0x06003BC3 RID: 15299 RVA: 0x001159DD File Offset: 0x00113DDD
	// (set) Token: 0x06003BC4 RID: 15300 RVA: 0x001159E5 File Offset: 0x00113DE5
	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel { get; set; }

	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x06003BC5 RID: 15301 RVA: 0x001159EE File Offset: 0x00113DEE
	// (set) Token: 0x06003BC6 RID: 15302 RVA: 0x001159F6 File Offset: 0x00113DF6
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000E56 RID: 3670
	// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x001159FF File Offset: 0x00113DFF
	// (set) Token: 0x06003BC8 RID: 15304 RVA: 0x00115A07 File Offset: 0x00113E07
	[Inject]
	public IUserGlobalEquippedModel globalEquipModel { get; set; }

	// Token: 0x17000E57 RID: 3671
	// (get) Token: 0x06003BC9 RID: 15305 RVA: 0x00115A10 File Offset: 0x00113E10
	// (set) Token: 0x06003BCA RID: 15306 RVA: 0x00115A18 File Offset: 0x00113E18
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000E58 RID: 3672
	// (get) Token: 0x06003BCB RID: 15307 RVA: 0x00115A21 File Offset: 0x00113E21
	// (set) Token: 0x06003BCC RID: 15308 RVA: 0x00115A29 File Offset: 0x00113E29
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x17000E59 RID: 3673
	// (get) Token: 0x06003BCD RID: 15309 RVA: 0x00115A32 File Offset: 0x00113E32
	// (set) Token: 0x06003BCE RID: 15310 RVA: 0x00115A3A File Offset: 0x00113E3A
	[Inject]
	public IBattleServerAPI battleServerApi { get; set; }

	// Token: 0x17000E5A RID: 3674
	// (get) Token: 0x06003BCF RID: 15311 RVA: 0x00115A43 File Offset: 0x00113E43
	// (set) Token: 0x06003BD0 RID: 15312 RVA: 0x00115A4B File Offset: 0x00113E4B
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x06003BD1 RID: 15313 RVA: 0x00115A54 File Offset: 0x00113E54
	public void Init(List<CharacterSelectionPortrait> characterSelectionPortraits, Func<PlayerNum, IPlayerCursor> findPlayerCursor, Func<PlayerNum, Vector2> getCursorDefaultPosition, Func<UnityEngine.Object, UnityEngine.Object> Instantiate, Action<UnityEngine.Object> Destroy, ShouldTokenClickCallback shouldTokenClick, GameObject TokenSpace, CursorTargetButton TokenDrop, GameObject TokenDefaultLocation, GameObject TokenGroup, GameObject PlayerTokenPrefab)
	{
		this.characterSelectionPortraits = characterSelectionPortraits;
		this.findPlayerCursor = findPlayerCursor;
		this.getCursorDefaultPosition = getCursorDefaultPosition;
		this.Instantiate = Instantiate;
		this.Destroy = Destroy;
		this.TokenSpace = TokenSpace;
		this.TokenDrop = TokenDrop;
		this.TokenDefaultLocation = TokenDefaultLocation;
		this.TokenGroup = TokenGroup;
		this.PlayerTokenPrefab = PlayerTokenPrefab;
		this.shouldTokenClick = shouldTokenClick;
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x00115AB8 File Offset: 0x00113EB8
	private bool isCursorOverCharacter(IPlayerCursor cursor, CharacterSelectionPortrait portrait)
	{
		if (cursor.RaycastCache != null)
		{
			foreach (RaycastResult raycastResult in cursor.RaycastCache)
			{
				if (raycastResult.gameObject == portrait.ButtonInteract.gameObject)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x00115B18 File Offset: 0x00113F18
	private CharacterSelectionPortrait findCharacterPortrait(CharacterID characterId)
	{
		foreach (CharacterSelectionPortrait characterSelectionPortrait in this.characterSelectionPortraits)
		{
			if (characterSelectionPortrait.CharacterId == characterId)
			{
				return characterSelectionPortrait;
			}
		}
		return null;
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x00115B84 File Offset: 0x00113F84
	private bool isAnyTokenNearby(Vector3 target)
	{
		foreach (PlayerToken playerToken in this.tokenManager.GetAll())
		{
			if ((target - playerToken.transform.position).magnitude <= 1f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x00115BDC File Offset: 0x00113FDC
	private Vector3 getCharacterPortraitCenter(CharacterSelectionPortrait portrait)
	{
		foreach (GameObject gameObject in portrait.TokenSnaps)
		{
			Vector3 position = gameObject.transform.position;
			if (!this.isAnyTokenNearby(position))
			{
				return position;
			}
		}
		return portrait.TokenSnaps[0].transform.position;
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x00115C68 File Offset: 0x00114068
	private void attachTokenToCharacter(PlayerToken token, IPlayerCursor cursor, CharacterID characterID)
	{
		bool flag = false;
		CharacterSelectionPortrait portrait = this.findCharacterPortrait(characterID);
		if (cursor != null && this.isCursorOverCharacter(cursor, portrait))
		{
			flag = true;
		}
		if (token.AttachedToCharacter != characterID)
		{
			if (flag)
			{
				token.SnapToCursor(cursor, false);
			}
			else
			{
				Vector3 characterPortraitCenter = this.getCharacterPortraitCenter(portrait);
				token.SnapToPoint(characterPortraitCenter, token.Alpha == 0f);
			}
		}
		this.tokenManager.ReleaseAnyGrabbers(token);
		token.AttachedToCharacter = characterID;
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x00115CE4 File Offset: 0x001140E4
	public void syncTokenPosition(PlayerSelectionInfo newInfo)
	{
		IPlayerCursor cursor = this.findPlayerCursor(newInfo.playerNum);
		PlayerToken playerToken = this.tokenManager.GetPlayerToken(newInfo.playerNum);
		if (playerToken != null)
		{
			PlayerNum playerNum = this.tokenManager.IsBeingGrabbedByPlayer(playerToken);
			if ((newInfo.type != PlayerType.Human || playerNum == PlayerNum.All) && playerNum != PlayerNum.None)
			{
				cursor = this.findPlayerCursor(playerNum);
			}
			if (newInfo.characterID != CharacterID.None)
			{
				this.attachTokenToCharacter(playerToken, cursor, newInfo.characterID);
			}
		}
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x00115D70 File Offset: 0x00114170
	private void initializeTokenPosition(PlayerNum player, PlayerToken token)
	{
		this.tokenManager.GrabToken(player, token, 0f);
		IPlayerCursor playerCursor = this.findPlayerCursor(player);
		if (playerCursor == null)
		{
			token.SnapToPoint(this.getCursorDefaultPosition(player), true);
		}
		else
		{
			token.SnapToCursor(playerCursor, true);
			token.Attach(playerCursor, false);
		}
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x00115DD0 File Offset: 0x001141D0
	private bool isOverExcludeButton(IPlayerCursor cursor)
	{
		if (cursor.RaycastCache != null)
		{
			foreach (RaycastResult raycastResult in cursor.RaycastCache)
			{
				if (raycastResult.gameObject != null)
				{
					CursorTargetButton component = raycastResult.gameObject.GetComponent<CursorTargetButton>();
					if (component != null && component != this.TokenDrop && !this.isCharacterSelectButton(component) && !this.isAToken(component) && component.IsAuthorized(PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false)))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x00115E80 File Offset: 0x00114280
	private bool isAToken(CursorTargetButton button)
	{
		Transform parent = button.transform.parent;
		return !(parent == null) && parent.GetComponent<PlayerToken>() != null;
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x00115EB3 File Offset: 0x001142B3
	public bool shouldDisplayToken(IPlayerCursor cursor)
	{
		return !this.isOverExcludeButton(cursor) && this.isCursorOverTokenSpace(cursor);
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x00115ED4 File Offset: 0x001142D4
	private bool isCursorOverTokenSpace(IPlayerCursor cursor)
	{
		if (cursor.RaycastCache != null)
		{
			foreach (RaycastResult raycastResult in cursor.RaycastCache)
			{
				if (raycastResult.gameObject == this.TokenSpace.gameObject)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x00115F34 File Offset: 0x00114334
	private void onTokenClick(PlayerToken token, PointerEventData eventData)
	{
		if (this.shouldTokenClick != null && !this.shouldTokenClick())
		{
			return;
		}
		PlayerNum pointerEventOwner = PlayerUtil.GetPointerEventOwner(eventData);
		PlayerToken currentlyGrabbing = this.tokenManager.GetCurrentlyGrabbing(pointerEventOwner);
		if (currentlyGrabbing != null)
		{
			IPlayerCursor attachToCursor = currentlyGrabbing.AttachToCursor;
			if (this.isOverCharacterButton(attachToCursor))
			{
				currentlyGrabbing.SnapToPoint(this.TokenDefaultLocation.transform.position, false);
			}
		}
		this.tokenManager.GrabToken(pointerEventOwner, token, eventData.clickTime);
		this.events.Broadcast(new SelectCharacterRequest(token.PlayerNum, CharacterID.None));
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x00115FD4 File Offset: 0x001143D4
	public void OnClickTokenDrop(CursorTargetButton target, PointerEventData eventData, ShouldDisplayToken displayCallback, GameLoadPayload gamePayload)
	{
		PlayerNum pointerEventOwner = PlayerUtil.GetPointerEventOwner(eventData);
		PlayerToken currentlyGrabbing = this.tokenManager.GetCurrentlyGrabbing(pointerEventOwner);
		if (currentlyGrabbing != null && currentlyGrabbing.GrabbedByTime != eventData.clickTime && displayCallback(currentlyGrabbing))
		{
			this.tokenManager.ReleaseToken(pointerEventOwner);
		}
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x0011602C File Offset: 0x0011442C
	private bool isOverCharacterButton(IPlayerCursor cursor)
	{
		if (cursor.RaycastCache != null)
		{
			foreach (RaycastResult raycastResult in cursor.RaycastCache)
			{
				CursorTargetButton component = raycastResult.gameObject.GetComponent<CursorTargetButton>();
				if (component != null && this.isCharacterSelectButton(component))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x00116094 File Offset: 0x00114494
	private bool isCharacterSelectButton(CursorTargetButton button)
	{
		Transform parent = button.transform.parent;
		return !(parent == null) && parent.GetComponent<CharacterSelectionPortrait>() != null;
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x001160C8 File Offset: 0x001144C8
	public void updateTokenVisuals(PlayerSelectionInfo info, PlayerToken token)
	{
		UIColor color;
		if (info.type == PlayerType.Human)
		{
			token.ButtonInteract.RequireAuthorization(info.playerNum);
			token.ButtonInteract.RequireAuthorization(PlayerNum.All);
			color = PlayerUtil.GetUIColorFromPlayerNum(info.playerNum);
		}
		else
		{
			token.ButtonInteract.DisableAuthorization();
			color = UIColor.Grey;
		}
		token.Image.sprite = token.GetSpriteForColor(color);
		token.UpdateText(info, color);
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x00116138 File Offset: 0x00114538
	public PlayerToken addToken(PlayerSelectionInfo info)
	{
		GameObject arg = this.PlayerTokenPrefab;
		if (info.type == PlayerType.Human)
		{
			EquipmentID equippedByType = this.globalEquipModel.GetEquippedByType(EquipmentTypes.TOKEN, this.userInputManager.GetBestPortId(info.playerNum));
			if (!equippedByType.IsNull())
			{
				EquippableItem item = this.equipmentModel.GetItem(equippedByType);
				if (item != null && item.localAssetId != null)
				{
					PlayerToken playerToken = this.itemLoader.LoadPrefab<PlayerToken>(item);
					if (playerToken != null)
					{
						arg = playerToken.gameObject;
					}
				}
			}
		}
		PlayerToken component = (this.Instantiate(arg) as GameObject).GetComponent<PlayerToken>();
		this.injector.Inject(component);
		component.PlayerNum = info.playerNum;
		component.transform.SetParent(this.TokenGroup.transform, false);
		this.tokenManager.AddToken(info.playerNum, component);
		component.OnClick = new Action<PlayerToken, PointerEventData>(this.onTokenClick);
		this.initializeTokenPosition(info.playerNum, component);
		this.syncTokenPosition(info);
		if (info.characterID != CharacterID.None)
		{
			CharacterSelectionPortrait portrait = this.findCharacterPortrait(info.characterID);
			Vector3 characterPortraitCenter = this.getCharacterPortraitCenter(portrait);
			component.SnapToPoint(characterPortraitCenter, true);
		}
		return component;
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x00116273 File Offset: 0x00114673
	private void destroyToken(PlayerToken token)
	{
		this.tokenManager.RemoveToken(token.PlayerNum, token);
		this.Destroy(token.gameObject);
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x00116298 File Offset: 0x00114698
	public void updateTokenState(PlayerSelectionInfo info, bool isActive)
	{
		PlayerNum playerNum = info.playerNum;
		PlayerToken playerToken = this.tokenManager.GetPlayerToken(playerNum);
		if (isActive)
		{
			if (playerToken == null)
			{
				playerToken = this.addToken(info);
			}
			PlayerNum playerNum2 = this.tokenManager.IsBeingGrabbedByPlayer(playerToken);
			if (playerNum2 == PlayerNum.None)
			{
			}
			this.updateTokenVisuals(info, playerToken);
			this.syncTokenPosition(info);
		}
		else if (playerToken != null)
		{
			this.destroyToken(playerToken);
		}
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x00116314 File Offset: 0x00114714
	public List<CharacterDefinition> SortCharacters(CharacterDefinition[] data, GameMode gameMode)
	{
		List<CharacterDefinition> list = new List<CharacterDefinition>();
		for (int i = 0; i < data.Length; i++)
		{
			data[i].ordinal = i;
			list.Add(data[i]);
		}
		list.Sort(delegate(CharacterDefinition a, CharacterDefinition b)
		{
			if (a.isRandom != b.isRandom)
			{
				if (a.isRandom)
				{
					return 1000000;
				}
				return -1000000;
			}
			else
			{
				bool flag = this.userCharacterUnlockModel.IsUnlockedInGameMode(a.characterID, gameMode);
				bool flag2 = this.userCharacterUnlockModel.IsUnlockedInGameMode(b.characterID, gameMode);
				if (flag == flag2)
				{
					return a.ordinal - b.ordinal;
				}
				if (flag)
				{
					return -10000;
				}
				return 10000;
			}
		});
		return list;
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x00116374 File Offset: 0x00114774
	public void onAltSubmit(PlayerSelectionInfo playerInfo, ShouldDisplayToken displayCallback)
	{
		PlayerToken playerToken = this.tokenManager.GetPlayerToken(playerInfo.playerNum);
		if (playerInfo.characterID != CharacterID.None)
		{
			this.events.Broadcast(new SelectCharacterRequest(playerInfo.playerNum, CharacterID.None));
		}
		else if (playerInfo.type == PlayerType.CPU && !displayCallback(playerToken))
		{
			this.events.Broadcast(new SelectCharacterRequest(playerInfo.playerNum));
		}
		if (playerToken != null)
		{
			this.tokenManager.GrabToken(playerInfo.playerNum, playerToken, 0f);
		}
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x0011640C File Offset: 0x0011480C
	public void PlayPlayerSelectionSounds(PlayerSelectionInfo oldInfo, PlayerSelectionInfo newInfo)
	{
		if (oldInfo == null || newInfo == null)
		{
			return;
		}
		bool flag = newInfo.characterID != CharacterID.None && newInfo.characterID != oldInfo.characterID;
		bool flag2 = newInfo.type != PlayerType.None && oldInfo.type == PlayerType.None && newInfo.characterID != CharacterID.None;
		bool flag3 = newInfo.characterID == CharacterID.None && oldInfo.characterID != CharacterID.None;
		bool flag4 = newInfo.type == PlayerType.None && oldInfo.type != PlayerType.None;
		if (flag || flag2)
		{
			if (newInfo.characterID == CharacterID.Random)
			{
				this.audioManager.PlayMenuSound(SoundKey.characterSelect_randomSelect, 0f);
			}
			else
			{
				this.audioManager.PlayMenuSound(SoundKey.characterSelect_characterSelect, 0f);
			}
		}
		else if (flag3 || flag4)
		{
			if (oldInfo.characterID == CharacterID.Random)
			{
				this.audioManager.PlayMenuSound(SoundKey.characterSelect_randomDeselect, 0f);
			}
			else
			{
				this.audioManager.PlayMenuSound(SoundKey.characterSelect_characterDeselect, 0f);
			}
		}
	}

	// Token: 0x0400291A RID: 10522
	private List<CharacterSelectionPortrait> characterSelectionPortraits;

	// Token: 0x0400291B RID: 10523
	private Func<PlayerNum, IPlayerCursor> findPlayerCursor;

	// Token: 0x0400291C RID: 10524
	private Func<PlayerNum, Vector2> getCursorDefaultPosition;

	// Token: 0x0400291D RID: 10525
	private Func<UnityEngine.Object, UnityEngine.Object> Instantiate;

	// Token: 0x0400291E RID: 10526
	private Action<UnityEngine.Object> Destroy;

	// Token: 0x0400291F RID: 10527
	private ShouldTokenClickCallback shouldTokenClick;

	// Token: 0x04002920 RID: 10528
	private GameObject TokenSpace;

	// Token: 0x04002921 RID: 10529
	private CursorTargetButton TokenDrop;

	// Token: 0x04002922 RID: 10530
	private GameObject TokenGroup;

	// Token: 0x04002923 RID: 10531
	private GameObject TokenDefaultLocation;

	// Token: 0x04002924 RID: 10532
	private GameObject PlayerTokenPrefab;
}
