// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICustomInputModule
{
	UIManager uiManager
	{
		set;
	}

	bool enabled
	{
		get;
		set;
	}

	ControlMode CurrentMode
	{
		get;
	}

	bool IsMouseMode
	{
		get;
	}

	WavedashTMProInput CurrentInputField
	{
		get;
	}

	bool ShouldActivateModule();

	void UpdateModule();

	void InitControlMode(ControlMode controlMode);

	void SetSelectedInputField(WavedashTMProInput inputField);

	void SetPauseMode(bool isPause);
}
