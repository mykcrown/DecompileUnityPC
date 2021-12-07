using System;
using System.Collections.Generic;

// Token: 0x020004F1 RID: 1265
public static class ButtonPressUtil
{
	// Token: 0x06001B9C RID: 7068 RVA: 0x0008BDC8 File Offset: 0x0008A1C8
	public static bool ListContains(List<ButtonPress> list, ButtonPress obj)
	{
		foreach (ButtonPress buttonPress in list)
		{
			if (buttonPress == obj)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x0008BE2C File Offset: 0x0008A22C
	public static void ListRemove(List<ButtonPress> list, ButtonPress obj)
	{
		for (int num = list.IndexOf(obj); num != -1; num = list.IndexOf(obj))
		{
			list.RemoveAt(num);
		}
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x0008BE5C File Offset: 0x0008A25C
	public static bool isTauntButton(ButtonPress button)
	{
		return button == ButtonPress.TauntUp || button == ButtonPress.TauntDown || button == ButtonPress.TauntRight || button == ButtonPress.TauntLeft;
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x0008BE7E File Offset: 0x0008A27E
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

	// Token: 0x06001BA0 RID: 7072 RVA: 0x0008BEB0 File Offset: 0x0008A2B0
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
