// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IGameInput
{
	Fixed HorizontalAxisValue
	{
		get;
	}

	Fixed VerticalAxisValue
	{
		get;
	}

	bool IsTapJumpInputPressed
	{
		get;
	}

	bool IsJumpInputPressed
	{
		get;
	}

	bool IsCrouchingInputPressed
	{
		get;
	}

	bool IsShieldInputPressed
	{
		get;
	}

	bool IsHorizontalDirectionHeld(HorizontalDirection dir);

	bool GetButton(ButtonPress press);

	void Vibrate(float leftMotor, float rightMotor, float duration);
}
