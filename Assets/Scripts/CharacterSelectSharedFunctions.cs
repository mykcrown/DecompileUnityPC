// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelectSharedFunctions : ICharacterSelectSharedFunctions
{
	private sealed class _SortCharacters_c__AnonStorey0
	{
		internal GameMode gameMode;

		internal CharacterSelectSharedFunctions _this;

		internal int __m__0(CharacterDefinition a, CharacterDefinition b)
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
				bool flag = this._this.userCharacterUnlockModel.IsUnlockedInGameMode(a.characterID, this.gameMode);
				bool flag2 = this._this.userCharacterUnlockModel.IsUnlockedInGameMode(b.characterID, this.gameMode);
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
		}
	}

	private List<CharacterSelectionPortrait> characterSelectionPortraits;

	private Func<PlayerNum, IPlayerCursor> findPlayerCursor;

	private Func<PlayerNum, Vector2> getCursorDefaultPosition;

	private Func<UnityEngine.Object, UnityEngine.Object> Instantiate;

	private Action<UnityEngine.Object> Destroy;

	private ShouldTokenClickCallback shouldTokenClick;

	private GameObject TokenSpace;

	private CursorTargetButton TokenDrop;

	private GameObject TokenGroup;

	private GameObject TokenDefaultLocation;

	private GameObject PlayerTokenPrefab;

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
	public IEvents events
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	[Inject]
	public IUserGlobalEquippedModel globalEquipModel
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
	public IBattleServerAPI battleServerApi
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

	private bool isCursorOverCharacter(IPlayerCursor cursor, CharacterSelectionPortrait portrait)
	{
		if (cursor.RaycastCache != null)
		{
			RaycastResult[] raycastCache = cursor.RaycastCache;
			for (int i = 0; i < raycastCache.Length; i++)
			{
				RaycastResult raycastResult = raycastCache[i];
				if (raycastResult.gameObject == portrait.ButtonInteract.gameObject)
				{
					return true;
				}
			}
		}
		return false;
	}

	private CharacterSelectionPortrait findCharacterPortrait(CharacterID characterId)
	{
		foreach (CharacterSelectionPortrait current in this.characterSelectionPortraits)
		{
			if (current.CharacterId == characterId)
			{
				return current;
			}
		}
		return null;
	}

	private bool isAnyTokenNearby(Vector3 target)
	{
		PlayerToken[] all = this.tokenManager.GetAll();
		for (int i = 0; i < all.Length; i++)
		{
			PlayerToken playerToken = all[i];
			if ((target - playerToken.transform.position).magnitude <= 1f)
			{
				return true;
			}
		}
		return false;
	}

	private Vector3 getCharacterPortraitCenter(CharacterSelectionPortrait portrait)
	{
		foreach (GameObject current in portrait.TokenSnaps)
		{
			Vector3 position = current.transform.position;
			if (!this.isAnyTokenNearby(position))
			{
				return position;
			}
		}
		return portrait.TokenSnaps[0].transform.position;
	}

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

	private bool isOverExcludeButton(IPlayerCursor cursor)
	{
		if (cursor.RaycastCache != null)
		{
			RaycastResult[] raycastCache = cursor.RaycastCache;
			for (int i = 0; i < raycastCache.Length; i++)
			{
				RaycastResult raycastResult = raycastCache[i];
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

	private bool isAToken(CursorTargetButton button)
	{
		Transform parent = button.transform.parent;
		return !(parent == null) && parent.GetComponent<PlayerToken>() != null;
	}

	public bool shouldDisplayToken(IPlayerCursor cursor)
	{
		return !this.isOverExcludeButton(cursor) && this.isCursorOverTokenSpace(cursor);
	}

	private bool isCursorOverTokenSpace(IPlayerCursor cursor)
	{
		if (cursor.RaycastCache != null)
		{
			RaycastResult[] raycastCache = cursor.RaycastCache;
			for (int i = 0; i < raycastCache.Length; i++)
			{
				RaycastResult raycastResult = raycastCache[i];
				if (raycastResult.gameObject == this.TokenSpace.gameObject)
				{
					return true;
				}
			}
		}
		return false;
	}

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

	public void OnClickTokenDrop(CursorTargetButton target, PointerEventData eventData, ShouldDisplayToken displayCallback, GameLoadPayload gamePayload)
	{
		PlayerNum pointerEventOwner = PlayerUtil.GetPointerEventOwner(eventData);
		PlayerToken currentlyGrabbing = this.tokenManager.GetCurrentlyGrabbing(pointerEventOwner);
		if (currentlyGrabbing != null && currentlyGrabbing.GrabbedByTime != eventData.clickTime && displayCallback(currentlyGrabbing))
		{
			this.tokenManager.ReleaseToken(pointerEventOwner);
		}
	}

	private bool isOverCharacterButton(IPlayerCursor cursor)
	{
		if (cursor.RaycastCache != null)
		{
			RaycastResult[] raycastCache = cursor.RaycastCache;
			for (int i = 0; i < raycastCache.Length; i++)
			{
				RaycastResult raycastResult = raycastCache[i];
				CursorTargetButton component = raycastResult.gameObject.GetComponent<CursorTargetButton>();
				if (component != null && this.isCharacterSelectButton(component))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool isCharacterSelectButton(CursorTargetButton button)
	{
		Transform parent = button.transform.parent;
		return !(parent == null) && parent.GetComponent<CharacterSelectionPortrait>() != null;
	}

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

	private void destroyToken(PlayerToken token)
	{
		this.tokenManager.RemoveToken(token.PlayerNum, token);
		this.Destroy(token.gameObject);
	}

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

	public List<CharacterDefinition> SortCharacters(CharacterDefinition[] data, GameMode gameMode)
	{
		CharacterSelectSharedFunctions._SortCharacters_c__AnonStorey0 _SortCharacters_c__AnonStorey = new CharacterSelectSharedFunctions._SortCharacters_c__AnonStorey0();
		_SortCharacters_c__AnonStorey.gameMode = gameMode;
		_SortCharacters_c__AnonStorey._this = this;
		List<CharacterDefinition> list = new List<CharacterDefinition>();
		for (int i = 0; i < data.Length; i++)
		{
			data[i].ordinal = i;
			list.Add(data[i]);
		}
		list.Sort(new Comparison<CharacterDefinition>(_SortCharacters_c__AnonStorey.__m__0));
		return list;
	}

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
}
