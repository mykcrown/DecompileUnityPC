// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using InControl;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputUtils
{
	private static Dictionary<int, ButtonPress> tapMap = new Dictionary<int, ButtonPress>
	{
		{
			14,
			ButtonPress.Backward
		},
		{
			13,
			ButtonPress.Forward
		},
		{
			17,
			ButtonPress.Side
		},
		{
			16,
			ButtonPress.Down
		},
		{
			15,
			ButtonPress.Up
		}
	};

	private static Dictionary<int, ButtonPress> untapMap = new Dictionary<int, ButtonPress>
	{
		{
			1,
			ButtonPress.BackwardTap
		},
		{
			0,
			ButtonPress.ForwardTap
		},
		{
			11,
			ButtonPress.SideTap
		},
		{
			3,
			ButtonPress.DownTap
		},
		{
			2,
			ButtonPress.UpTap
		}
	};

	private static HorizontalDirection[] oppositeDirectionTable = new HorizontalDirection[]
	{
		HorizontalDirection.None,
		HorizontalDirection.Right,
		HorizontalDirection.Left,
		HorizontalDirection.Any
	};

	private static Vector2F[] eightWayDirections = new Vector2F[]
	{
		Vector2F.right,
		Vector2F.upRight,
		Vector2F.up,
		Vector2F.upLeft,
		Vector2F.left,
		Vector2F.downLeft,
		Vector2F.down,
		Vector2F.downRight
	};

	public static bool IsTap(ButtonPress press)
	{
		return InputUtils.tapMap.ContainsKey((int)press);
	}

	public static bool IsTappedDirection(ButtonPress press)
	{
		return InputUtils.IsTap(press) && InputUtils.IsDirection(press);
	}

	public static bool IsUntappedDirection(ButtonPress press)
	{
		return !InputUtils.IsTap(press) && InputUtils.IsDirection(press);
	}

	public static ButtonPress GetOppositeHorizontalButton(ButtonPress input)
	{
		switch (input)
		{
		case ButtonPress.BackwardStrike:
			return ButtonPress.ForwardStrike;
		case ButtonPress.ForwardStrike:
			return ButtonPress.BackwardStrike;
		case ButtonPress.Strike:
		case ButtonPress.UpTilt:
		case ButtonPress.DownTilt:
		case ButtonPress.UpSpecial:
		case ButtonPress.DownSpecial:
			IL_35:
			if (input == ButtonPress.Forward)
			{
				return ButtonPress.Backward;
			}
			if (input == ButtonPress.Backward)
			{
				return ButtonPress.Forward;
			}
			if (input == ButtonPress.ForwardTap)
			{
				return ButtonPress.BackwardTap;
			}
			if (input != ButtonPress.BackwardTap)
			{
				UnityEngine.Debug.LogWarning("No opposite found for " + input);
				return input;
			}
			return ButtonPress.ForwardTap;
		case ButtonPress.BackwardTilt:
			return ButtonPress.ForwardTilt;
		case ButtonPress.ForwardTilt:
			return ButtonPress.BackwardTilt;
		case ButtonPress.BackwardSpecial:
			return ButtonPress.ForwardSpecial;
		case ButtonPress.ForwardSpecial:
			return ButtonPress.BackwardSpecial;
		}
		goto IL_35;
	}

	public static ButtonPress GetTapped(ButtonPress press)
	{
		if (InputUtils.IsTap(press))
		{
			return press;
		}
		return InputUtils.untapMap[(int)press];
	}

	public static ButtonPress GetUntapped(ButtonPress press)
	{
		if (InputUtils.IsTap(press))
		{
			return InputUtils.tapMap[(int)press];
		}
		return press;
	}

	public static bool IsAxis(InputType inputType)
	{
		switch (inputType)
		{
		case InputType.HorizontalAxis:
		case InputType.VerticalAxis:
		case InputType.TriggerAxis:
			return true;
		}
		return false;
	}

	public static bool IsVertical(ButtonPress buttonPress)
	{
		return buttonPress == ButtonPress.Up || buttonPress == ButtonPress.Down || buttonPress == ButtonPress.UpTap || buttonPress == ButtonPress.DownTap;
	}

	public static bool IsHorizontal(ButtonPress buttonPress)
	{
		switch (buttonPress)
		{
		case ButtonPress.BackwardStrike:
		case ButtonPress.ForwardStrike:
		case ButtonPress.BackwardTilt:
		case ButtonPress.ForwardTilt:
		case ButtonPress.BackwardSpecial:
		case ButtonPress.ForwardSpecial:
			return true;
		case ButtonPress.Strike:
		case ButtonPress.UpTilt:
		case ButtonPress.DownTilt:
		case ButtonPress.UpSpecial:
		case ButtonPress.DownSpecial:
			IL_35:
			switch (buttonPress)
			{
			case ButtonPress.Side:
			case ButtonPress.ForwardTap:
			case ButtonPress.BackwardTap:
			case ButtonPress.SideTap:
				return true;
			case ButtonPress.Tilt:
			case ButtonPress.UpTap:
			case ButtonPress.DownTap:
				IL_5A:
				if (buttonPress != ButtonPress.Forward && buttonPress != ButtonPress.Backward)
				{
					return false;
				}
				return true;
			}
			goto IL_5A;
		}
		goto IL_35;
	}

	public static bool IsHorizontalLeftStick(ButtonPress buttonPress)
	{
		switch (buttonPress)
		{
		case ButtonPress.Side:
		case ButtonPress.ForwardTap:
		case ButtonPress.BackwardTap:
		case ButtonPress.SideTap:
			return true;
		case ButtonPress.Tilt:
		case ButtonPress.UpTap:
		case ButtonPress.DownTap:
			IL_25:
			if (buttonPress != ButtonPress.Forward && buttonPress != ButtonPress.Backward)
			{
				return false;
			}
			return true;
		}
		goto IL_25;
	}

	public static bool IsVerticalLeftStick(ButtonPress buttonPress)
	{
		return buttonPress == ButtonPress.Up || buttonPress == ButtonPress.Down || buttonPress == ButtonPress.UpTap || buttonPress == ButtonPress.DownTap;
	}

	public static bool IsHorizontalRightStick(ButtonPress buttonPress)
	{
		switch (buttonPress)
		{
		case ButtonPress.BackwardStrike:
		case ButtonPress.ForwardStrike:
		case ButtonPress.BackwardTilt:
		case ButtonPress.ForwardTilt:
		case ButtonPress.BackwardSpecial:
		case ButtonPress.ForwardSpecial:
			return true;
		}
		return false;
	}

	public static bool IsVerticalRightStick(ButtonPress buttonPress)
	{
		return buttonPress == ButtonPress.UpStrike || buttonPress == ButtonPress.DownStrike;
	}

	public static bool IsDirection(ButtonPress buttonPress)
	{
		return InputUtils.IsVertical(buttonPress) || InputUtils.IsHorizontal(buttonPress);
	}

	public static bool IsFacing(HorizontalDirection facing, Vector3F position1, Vector3F position2)
	{
		if (facing != HorizontalDirection.Left)
		{
			return facing == HorizontalDirection.Right && position1.x < position2.x;
		}
		return position1.x > position2.x;
	}

	public static int GetDirectionMultiplier(HorizontalDirection facing)
	{
		if (facing == HorizontalDirection.Left)
		{
			return -1;
		}
		if (facing != HorizontalDirection.Right)
		{
			return 0;
		}
		return 1;
	}

	public static Vector3F GetDirectionVector(HorizontalDirection facing)
	{
		if (facing == HorizontalDirection.Left)
		{
			return Vector3F.left;
		}
		if (facing != HorizontalDirection.Right)
		{
			return Vector3F.zero;
		}
		return Vector3F.right;
	}

	public static Fixed GetDirectionAngle(HorizontalDirection facing)
	{
		Vector3F directionVector = InputUtils.GetDirectionVector(facing);
		return MathUtil.VectorToAngle(ref directionVector);
	}

	public static HorizontalDirection GetDirection(Fixed horizontalValue)
	{
		HorizontalDirection result = HorizontalDirection.None;
		if (horizontalValue != 0)
		{
			result = ((!(horizontalValue < 0)) ? HorizontalDirection.Right : HorizontalDirection.Left);
		}
		return result;
	}

	public static HorizontalDirection GetOppositeDirection(HorizontalDirection direction)
	{
		return InputUtils.oppositeDirectionTable[(int)direction];
	}

	public static ButtonPress GetButtonFromHorizontalValue(HorizontalDirection facing, Fixed horizontalValue)
	{
		HorizontalDirection direction = InputUtils.GetDirection(horizontalValue);
		if (direction == HorizontalDirection.None)
		{
			return ButtonPress.None;
		}
		return (direction != facing) ? ButtonPress.Backward : ButtonPress.Forward;
	}

	public static HorizontalDirection GetDirectionFromButton(HorizontalDirection facing, ButtonPress side)
	{
		ButtonPress untapped = InputUtils.GetUntapped(side);
		switch (untapped)
		{
		case ButtonPress.BackwardStrike:
		case ButtonPress.BackwardTilt:
		case ButtonPress.BackwardSpecial:
			goto IL_4E;
		case ButtonPress.ForwardStrike:
		case ButtonPress.ForwardTilt:
		case ButtonPress.ForwardSpecial:
			return facing;
		case ButtonPress.Strike:
		case ButtonPress.UpTilt:
		case ButtonPress.DownTilt:
		case ButtonPress.UpSpecial:
		case ButtonPress.DownSpecial:
			IL_3C:
			if (untapped == ButtonPress.Forward)
			{
				return facing;
			}
			if (untapped != ButtonPress.Backward)
			{
				return HorizontalDirection.None;
			}
			goto IL_4E;
		}
		goto IL_3C;
		IL_4E:
		if (facing == HorizontalDirection.Left)
		{
			return HorizontalDirection.Right;
		}
		if (facing != HorizontalDirection.Right)
		{
			return HorizontalDirection.None;
		}
		return HorizontalDirection.Left;
	}

	public static bool ContainsVertical(List<ButtonPress> presses, out ButtonPress vertical)
	{
		for (int i = 0; i < presses.Count; i++)
		{
			ButtonPress buttonPress = presses[i];
			if (InputUtils.IsVerticalRightStick(buttonPress))
			{
				vertical = buttonPress;
				return true;
			}
		}
		for (int j = 0; j < presses.Count; j++)
		{
			ButtonPress buttonPress2 = presses[j];
			if (InputUtils.IsVerticalLeftStick(buttonPress2))
			{
				vertical = buttonPress2;
				return true;
			}
		}
		vertical = ButtonPress.None;
		return false;
	}

	public static bool ContainsHorizontal(List<ButtonPress> presses, out ButtonPress horizontal)
	{
		for (int i = 0; i < presses.Count; i++)
		{
			ButtonPress buttonPress = presses[i];
			if (InputUtils.IsHorizontalRightStick(buttonPress))
			{
				horizontal = buttonPress;
				return true;
			}
		}
		for (int j = 0; j < presses.Count; j++)
		{
			ButtonPress buttonPress2 = presses[j];
			if (InputUtils.IsHorizontalLeftStick(buttonPress2))
			{
				horizontal = buttonPress2;
				return true;
			}
		}
		horizontal = ButtonPress.None;
		return false;
	}

	public static bool IsTaunt(ButtonPress button)
	{
		switch (button)
		{
		case ButtonPress.TauntLeft:
		case ButtonPress.TauntRight:
		case ButtonPress.TauntUp:
		case ButtonPress.TauntDown:
			break;
		default:
			if (button != ButtonPress.Taunt)
			{
				return false;
			}
			break;
		}
		return true;
	}

	public static bool IsCrouchInput(Fixed yInput, Fixed xInput, InputConfigData inputConfig)
	{
		Fixed one = FixedMath.Atan2(yInput, xInput) / FixedMath.PI * 180;
		return one < -(Fixed)((double)inputConfig.walkOptions.crouchAngleThreshold) && one > -180 + (Fixed)((double)inputConfig.walkOptions.crouchAngleThreshold) && yInput <= -(Fixed)((double)inputConfig.walkOptions.crouchThreshold);
	}

	public static UniqueDeviceType GetDeviceType(IInputDevice device)
	{
		InputDevice inputDevice = device as InputDevice;
		if (inputDevice != null)
		{
			if (inputDevice.DeviceStyle == InputDeviceStyle.NintendoGameCube)
			{
				return UniqueDeviceType.GC;
			}
			return UniqueDeviceType.Other;
		}
		else
		{
			if (device is KeyboardInputDevice)
			{
				return UniqueDeviceType.Keyboard;
			}
			return UniqueDeviceType.Unkown;
		}
	}

	public static bool IsDeviceActivity(IInputDevice device)
	{
		InputDevice inputDevice = device as InputDevice;
		return (inputDevice != null && (inputDevice.Command.WasPressed || inputDevice.Action1.WasPressed || inputDevice.LeftStick.IsPressed)) || (device is KeyboardInputDevice && (device as KeyboardInputDevice).WasAnyKeyPressed);
	}

	public static bool IsTappedInputValue(InputConfigData inputConfig, Fixed value, int framesHeld)
	{
		return FixedMath.Abs(value) >= (Fixed)((double)inputConfig.tapThreshold) && framesHeld <= inputConfig.tapFrames;
	}

	public static Vector2F ClampVectorToEightWayInput(Vector2F vector)
	{
		if (vector.magnitude == 0)
		{
			return Vector2F.zero;
		}
		Vector2F normalized = vector.normalized;
		Fixed one = MathUtil.VectorToAngle(ref normalized);
		int num = (int)(FixedMath.WrapDegrees(one + (Fixed)22.5) / 45);
		return InputUtils.eightWayDirections[num];
	}
}
