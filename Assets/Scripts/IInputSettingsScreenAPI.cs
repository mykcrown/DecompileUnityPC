// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

public interface IInputSettingsScreenAPI
{
	bool IsListeningForBinding
	{
		get;
	}

	ButtonPress ListeningButtonPress
	{
		get;
	}

	int ListeningButtonIndex
	{
		get;
	}

	ButtonPress LastUnboundButtonPress
	{
		get;
		set;
	}

	BindingSource LastUnboundBinding
	{
		get;
		set;
	}

	bool isTapJump
	{
		get;
		set;
	}

	bool isTapStrike
	{
		get;
		set;
	}

	bool isRecoveryJump
	{
		get;
		set;
	}

	bool isDoubleTapToRun
	{
		get;
		set;
	}

	float LeftStickDeadZone
	{
		get;
		set;
	}

	float RightStickDeadZone
	{
		get;
		set;
	}

	float LeftTriggerDeadZone
	{
		get;
		set;
	}

	float RightTriggerDeadZone
	{
		get;
		set;
	}

	bool SettingsChanged
	{
		get;
	}

	bool IsMovementUnbound
	{
		get;
	}

	void Initialize();

	void SetBinding(ButtonPress buttonPress, BindingSource bindingSource, int buttonIndex);

	BindingSource GetBindingSource(ButtonPress buttonPress, int buttonIndex);

	void ListenForBindingSource(ButtonPress buttonPress, int buttonIndex);

	void RemoveBinding(ButtonPress buttonPress, int buttonIndex);

	void CancelListenForBinding();

	void SaveControls();

	void ResetControls();

	void OnExitScreen();
}
