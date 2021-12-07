// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public interface IMoveInput
{
	Fixed GetHorizontalAxis();

	Fixed GetVerticalAxis();

	bool GetButton(ButtonPress button);

	bool GetButtonDown(ButtonPress button);

	Vector2F GetAxisValue();

	string GenerateDebugString();
}
