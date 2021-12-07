// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public static class ButtonPressUtil
{
	public static bool ListContains(List<ButtonPress> list, ButtonPress obj)
	{
		foreach (ButtonPress current in list)
		{
			if (current == obj)
			{
				return true;
			}
		}
		return false;
	}

	public static void ListRemove(List<ButtonPress> list, ButtonPress obj)
	{
		for (int num = list.IndexOf(obj); num != -1; num = list.IndexOf(obj))
		{
			list.RemoveAt(num);
		}
	}

	public static bool isTauntButton(ButtonPress button)
	{
		return button == ButtonPress.TauntUp || button == ButtonPress.TauntDown || button == ButtonPress.TauntRight || button == ButtonPress.TauntLeft;
	}

	public static TauntSlot getTauntSlotForButton(ButtonPress button)
	{
		switch (button)
		{
		case ButtonPress.TauntLeft:
			return TauntSlot.LEFT;
		case ButtonPress.TauntRight:
			return TauntSlot.RIGHT;
		case ButtonPress.TauntUp:
			return TauntSlot.UP;
		case ButtonPress.TauntDown:
			return TauntSlot.DOWN;
		default:
			throw new Exception("Not a taunt button");
		}
	}

	public static ButtonPress getButtonForTauntSlot(TauntSlot button)
	{
		switch (button)
		{
		case TauntSlot.UP:
			return ButtonPress.TauntUp;
		case TauntSlot.DOWN:
			return ButtonPress.TauntDown;
		case TauntSlot.LEFT:
			return ButtonPress.TauntLeft;
		case TauntSlot.RIGHT:
			return ButtonPress.TauntRight;
		default:
			throw new Exception("Not a taunt button");
		}
	}
}
