// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IButtonInputDelegate
{
	void OnSubmitPressed();

	void OnCancelPressed();

	void OnRightTriggerPressed();

	void OnLeftTriggerPressed();

	void OnLeftBumperPressed();

	void OnZPressed();

	void OnRightStickRight();

	void OnRightStickLeft();

	void OnRightStickUp();

	void OnRightStickDown();

	void UpdateRightStick(float x, float y);

	void OnLeft();

	void OnRight();

	void OnUp();

	void OnDown();

	void OnDPadLeft();

	void OnDPadRight();

	void OnDPadUp();

	void OnDPadDown();

	void OnYButtonPressed();

	void OnXButtonPressed();

	void OnAnythingPressed();

	void OnAnyMouseEvent();

	void OnAnyNavigationButtonPressed();
}
