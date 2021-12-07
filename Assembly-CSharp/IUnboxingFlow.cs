using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A12 RID: 2578
public interface IUnboxingFlow
{
	// Token: 0x06004AD9 RID: 19161
	void SetCamera(Camera theCamera);

	// Token: 0x06004ADA RID: 19162
	void SetShowRevealedItemCallback(Action<int> callback);

	// Token: 0x06004ADB RID: 19163
	void ResetScene();

	// Token: 0x06004ADC RID: 19164
	void OnEnterScreen();

	// Token: 0x06004ADD RID: 19165
	void StartDisplay(List<GameObject> dynamicPrefabs, List<EquipmentRarity> rarityList);

	// Token: 0x06004ADE RID: 19166
	void EndDisplay();

	// Token: 0x06004ADF RID: 19167
	void HighlightPortal(int index);

	// Token: 0x06004AE0 RID: 19168
	void UnhighlightPortal(int index);
}
