// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IControllerInputDevice : IInputDevice
{
	float LeftStickLowerDeadZone
	{
		get;
		set;
	}

	float RightStickLowerDeadZone
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

	float DefaultLeftStickLowerDeadZone
	{
		get;
	}

	float DefaultRightStickLowerDeadZone
	{
		get;
	}

	float DefaultLeftTriggerDeadZone
	{
		get;
	}

	float DefaultRightTriggerDeadZone
	{
		get;
	}
}
