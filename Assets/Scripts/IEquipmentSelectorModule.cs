// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public interface IEquipmentSelectorModule
{
	void LoadItems(List<EquippableItem> items);

	void Activate();

	void ForceRedraws();

	void BeginMenuFocus();

	void OnMouseModeUpdate();

	void SyncButtonModeSelection();

	void OnDrawComplete();

	void RebuildList();

	void DeselectEquipment();

	void ReleaseSelections();

	void EnterFromRight();

	void EnterFromBottom();

	bool OnCancelPressed();

	bool OnLeft();

	bool OnRight();

	void OnYButton();
}
