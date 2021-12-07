using System;
using System.Collections.Generic;
using FixedPoint;
using InControl;
using UnityEngine;

// Token: 0x020006A1 RID: 1697
public class InputUtils
{
	// Token: 0x06002A1F RID: 10783 RVA: 0x000DE623 File Offset: 0x000DCA23
	public static bool IsTap(ButtonPress press)
	{
		return InputUtils.tapMap.ContainsKey((int)press);
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x000DE630 File Offset: 0x000DCA30
	public static bool IsTappedDirection(ButtonPress press)
	{
		return InputUtils.IsTap(press) && InputUtils.IsDirection(press);
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x000DE646 File Offset: 0x000DCA46
	public static bool IsUntappedDirection(ButtonPress press)
	{
		return !InputUtils.IsTap(press) && InputUtils.IsDirection(press);
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x000DE65C File Offset: 0x000DCA5C
	public static ButtonPress GetOppositeHorizontalButton(ButtonPress input)
	{
		switch (input)
		{
		case ButtonPress.BackwardStrike:
			return ButtonPress.ForwardStrike;
		case ButtonPress.ForwardStrike:
			return ButtonPress.BackwardStrike;
		default:
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
				Debug.LogWarning("No opposite found for " + input);
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
	}

	// Token: 0x06002A23 RID: 10787 RVA: 0x000DE6F2 File Offset: 0x000DCAF2
	public static ButtonPress GetTapped(ButtonPress press)
	{
		if (InputUtils.IsTap(press))
		{
			return press;
		}
		return InputUtils.untapMap[(int)press];
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x000DE70C File Offset: 0x000DCB0C
	public static ButtonPress GetUntapped(ButtonPress press)
	{
		if (InputUtils.IsTap(press))
		{
			return InputUtils.tapMap[(int)press];
		}
		return press;
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x000DE726 File Offset: 0x000DCB26
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

	// Token: 0x06002A26 RID: 10790 RVA: 0x000DE746 File Offset: 0x000DCB46
	public static bool IsVertical(ButtonPress buttonPress)
	{
		return buttonPress == ButtonPress.Up || buttonPress == ButtonPress.Down || buttonPress == ButtonPress.UpTap || buttonPress == ButtonPress.DownTap;
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x000DE770 File Offset: 0x000DCB70
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
			break;
		default:
			switch (buttonPress)
			{
			case ButtonPress.Side:
			case ButtonPress.ForwardTap:
			case ButtonPress.BackwardTap:
			case ButtonPress.SideTap:
				break;
			default:
				if (buttonPress != ButtonPress.Forward && buttonPress != ButtonPress.Backward)
				{
					return false;
				}
				break;
			}
			break;
		}
		return true;
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x000DE7EC File Offset: 0x000DCBEC
	public static bool IsHorizontalLeftStick(ButtonPress buttonPress)
	{
		switch (buttonPress)
		{
		case ButtonPress.Side:
		case ButtonPress.ForwardTap:
		case ButtonPress.BackwardTap:
		case ButtonPress.SideTap:
			break;
		default:
			if (buttonPress != ButtonPress.Forward && buttonPress != ButtonPress.Backward)
			{
				return false;
			}
			break;
		}
		return true;
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x000DE828 File Offset: 0x000DCC28
	public static bool IsVerticalLeftStick(ButtonPress buttonPress)
	{
		return buttonPress == ButtonPress.Up || buttonPress == ButtonPress.Down || buttonPress == ButtonPress.UpTap || buttonPress == ButtonPress.DownTap;
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x000DE850 File Offset: 0x000DCC50
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

	// Token: 0x06002A2B RID: 10795 RVA: 0x000DE88F File Offset: 0x000DCC8F
	public static bool IsVerticalRightStick(ButtonPress buttonPress)
	{
		return buttonPress == ButtonPress.UpStrike || buttonPress == ButtonPress.DownStrike;
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x000DE8A9 File Offset: 0x000DCCA9
	public static bool IsDirection(ButtonPress buttonPress)
	{
		return InputUtils.IsVertical(buttonPress) || InputUtils.IsHorizontal(buttonPress);
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x000DE8BF File Offset: 0x000DCCBF
	public static bool IsFacing(HorizontalDirection facing, Vector3F position1, Vector3F position2)
	{
		if (facing != HorizontalDirection.Left)
		{
			return facing == HorizontalDirection.Right && position1.x < position2.x;
		}
		return position1.x > position2.x;
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x000DE8FD File Offset: 0x000DCCFD
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

	// Token: 0x06002A2F RID: 10799 RVA: 0x000DE917 File Offset: 0x000DCD17
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

	// Token: 0x06002A30 RID: 10800 RVA: 0x000DE940 File Offset: 0x000DCD40
	public static Fixed GetDirectionAngle(HorizontalDirection facing)
	{
		Vector3F directionVector = InputUtils.GetDirectionVector(facing);
		return MathUtil.VectorToAngle(ref directionVector);
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x000DE95C File Offset: 0x000DCD5C
	public static HorizontalDirection GetDirection(Fixed horizontalValue)
	{
		HorizontalDirection result = HorizontalDirection.None;
		if (horizontalValue != 0)
		{
			result = ((!(horizontalValue < 0)) ? HorizontalDirection.Right : HorizontalDirection.Left);
		}
		return result;
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x000DE98C File Offset: 0x000DCD8C
	public static HorizontalDirection GetOppositeDirection(HorizontalDirection direction)
	{
		return InputUtils.oppositeDirectionTable[(int)direction];
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x000DE998 File Offset: 0x000DCD98
	public static ButtonPress GetButtonFromHorizontalValue(HorizontalDirection facing, Fixed horizontalValue)
	{
		HorizontalDirection direction = InputUtils.GetDirection(horizontalValue);
		if (direction == HorizontalDirection.None)
		{
			return ButtonPress.None;
		}
		return (direction != facing) ? ButtonPress.Backward : ButtonPress.Forward;
	}

	// Token: 0x06002A34 RID: 10804 RVA: 0x000DE9C4 File Offset: 0x000DCDC4
	public static HorizontalDirection GetDirectionFromButton(HorizontalDirection facing, ButtonPress side)
	{
		ButtonPress untapped = InputUtils.GetUntapped(side);
		switch (untapped)
		{
		case ButtonPress.BackwardStrike:
		case ButtonPress.BackwardTilt:
		case ButtonPress.BackwardSpecial:
			break;
		case ButtonPress.ForwardStrike:
		case ButtonPress.ForwardTilt:
		case ButtonPress.ForwardSpecial:
			return facing;
		default:
			if (untapped == ButtonPress.Forward)
			{
				return facing;
			}
			if (untapped != ButtonPress.Backward)
			{
				return HorizontalDirection.None;
			}
			break;
		}
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

	// Token: 0x06002A35 RID: 10805 RVA: 0x000DEA3C File Offset: 0x000DCE3C
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

	// Token: 0x06002A36 RID: 10806 RVA: 0x000DEAAC File Offset: 0x000DCEAC
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

	// Token: 0x06002A37 RID: 10807 RVA: 0x000DEB1C File Offset: 0x000DCF1C
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

	// Token: 0x06002A38 RID: 10808 RVA: 0x000DEB48 File Offset: 0x000DCF48
	public static bool IsCrouchInput(Fixed yInput, Fixed xInput, InputConfigData inputConfig)
	{
		Fixed one = FixedMath.Atan2(yInput, xInput) / FixedMath.PI * 180;
		return one < -(Fixed)((double)inputConfig.walkOptions.crouchAngleThreshold) && one > -180 + (Fixed)((double)inputConfig.walkOptions.crouchAngleThreshold) && yInput <= -(Fixed)((double)inputConfig.walkOptions.crouchThreshold);
	}

	// Token: 0x06002A39 RID: 10809 RVA: 0x000DEBDC File Offset: 0x000DCFDC
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

	// Token: 0x06002A3A RID: 10810 RVA: 0x000DEC18 File Offset: 0x000DD018
	public static bool IsDeviceActivity(IInputDevice device)
	{
		InputDevice inputDevice = device as InputDevice;
		return (inputDevice != null && (inputDevice.Command.WasPressed || inputDevice.Action1.WasPressed || inputDevice.LeftStick.IsPressed)) || (device is KeyboardInputDevice && (device as KeyboardInputDevice).WasAnyKeyPressed);
	}

	// Token: 0x06002A3B RID: 10811 RVA: 0x000DEC82 File Offset: 0x000DD082
	public static bool IsTappedInputValue(InputConfigData inputConfig, Fixed value, int framesHeld)
	{
		return FixedMath.Abs(value) >= (Fixed)((double)inputConfig.tapThreshold) && framesHeld <= inputConfig.tapFrames;
	}

	// Token: 0x06002A3C RID: 10812 RVA: 0x000DECB0 File Offset: 0x000DD0B0
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

	// Token: 0x04001E38 RID: 7736
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

	// Token: 0x04001E39 RID: 7737
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

	// Token: 0x04001E3A RID: 7738
	private static HorizontalDirection[] oppositeDirectionTable = new HorizontalDirection[]
	{
		HorizontalDirection.None,
		HorizontalDirection.Right,
		HorizontalDirection.Left,
		HorizontalDirection.Any
	};

	// Token: 0x04001E3B RID: 7739
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
}
