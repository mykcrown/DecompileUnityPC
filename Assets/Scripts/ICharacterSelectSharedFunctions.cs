// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICharacterSelectSharedFunctions
{
	void Init(List<CharacterSelectionPortrait> characterSelectionPortraits, Func<PlayerNum, IPlayerCursor> findPlayerCursor, Func<PlayerNum, Vector2> getCursorDefaultPosition, Func<UnityEngine.Object, UnityEngine.Object> Instantiate, Action<UnityEngine.Object> Destroy, ShouldTokenClickCallback ShouldTokenClick, GameObject TokenSpace, CursorTargetButton TokenDrop, GameObject TokenDefaultLocation, GameObject TokenGroup, GameObject PlayerTokenPrefab);

	void syncTokenPosition(PlayerSelectionInfo newInfo);

	bool shouldDisplayToken(IPlayerCursor cursor);

	void updateTokenVisuals(PlayerSelectionInfo info, PlayerToken token);

	void updateTokenState(PlayerSelectionInfo info, bool isActive);

	void onAltSubmit(PlayerSelectionInfo playerInfo, ShouldDisplayToken displayCallback);

	void OnClickTokenDrop(CursorTargetButton target, PointerEventData eventData, ShouldDisplayToken displayCallback, GameLoadPayload gamePayload);

	List<CharacterDefinition> SortCharacters(CharacterDefinition[] data, GameMode gameMode);

	void PlayPlayerSelectionSounds(PlayerSelectionInfo oldInfo, PlayerSelectionInfo newInfo);
}
