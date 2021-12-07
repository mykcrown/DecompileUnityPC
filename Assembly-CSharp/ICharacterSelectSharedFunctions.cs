using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000902 RID: 2306
public interface ICharacterSelectSharedFunctions
{
	// Token: 0x06003BE8 RID: 15336
	void Init(List<CharacterSelectionPortrait> characterSelectionPortraits, Func<PlayerNum, IPlayerCursor> findPlayerCursor, Func<PlayerNum, Vector2> getCursorDefaultPosition, Func<UnityEngine.Object, UnityEngine.Object> Instantiate, Action<UnityEngine.Object> Destroy, ShouldTokenClickCallback ShouldTokenClick, GameObject TokenSpace, CursorTargetButton TokenDrop, GameObject TokenDefaultLocation, GameObject TokenGroup, GameObject PlayerTokenPrefab);

	// Token: 0x06003BE9 RID: 15337
	void syncTokenPosition(PlayerSelectionInfo newInfo);

	// Token: 0x06003BEA RID: 15338
	bool shouldDisplayToken(IPlayerCursor cursor);

	// Token: 0x06003BEB RID: 15339
	void updateTokenVisuals(PlayerSelectionInfo info, PlayerToken token);

	// Token: 0x06003BEC RID: 15340
	void updateTokenState(PlayerSelectionInfo info, bool isActive);

	// Token: 0x06003BED RID: 15341
	void onAltSubmit(PlayerSelectionInfo playerInfo, ShouldDisplayToken displayCallback);

	// Token: 0x06003BEE RID: 15342
	void OnClickTokenDrop(CursorTargetButton target, PointerEventData eventData, ShouldDisplayToken displayCallback, GameLoadPayload gamePayload);

	// Token: 0x06003BEF RID: 15343
	List<CharacterDefinition> SortCharacters(CharacterDefinition[] data, GameMode gameMode);

	// Token: 0x06003BF0 RID: 15344
	void PlayPlayerSelectionSounds(PlayerSelectionInfo oldInfo, PlayerSelectionInfo newInfo);
}
