// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public interface IUnboxingFlow
{
	void SetCamera(Camera theCamera);

	void SetShowRevealedItemCallback(Action<int> callback);

	void ResetScene();

	void OnEnterScreen();

	void StartDisplay(List<GameObject> dynamicPrefabs, List<EquipmentRarity> rarityList);

	void EndDisplay();

	void HighlightPortal(int index);

	void UnhighlightPortal(int index);
}
