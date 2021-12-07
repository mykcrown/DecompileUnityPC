using System;
using UnityEngine;

// Token: 0x02000B07 RID: 2823
public static class InputUtil
{
	// Token: 0x06005115 RID: 20757 RVA: 0x001515DD File Offset: 0x0014F9DD
	private static bool getControl()
	{
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
	}

	// Token: 0x06005116 RID: 20758 RVA: 0x001515FB File Offset: 0x0014F9FB
	private static bool getShift()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}

	// Token: 0x06005117 RID: 20759 RVA: 0x00151619 File Offset: 0x0014FA19
	private static bool getAlt()
	{
		return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
	}

	// Token: 0x06005118 RID: 20760 RVA: 0x00151637 File Offset: 0x0014FA37
	private static bool getSuper()
	{
		return Input.GetKey(KeyCode.LeftWindows) || Input.GetKey(KeyCode.RightWindows);
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x00151658 File Offset: 0x0014FA58
	public static InputModifier GetModifiers()
	{
		InputModifier inputModifier = InputModifier.None;
		if (InputUtil.getControl())
		{
			inputModifier |= InputModifier.Ctrl;
		}
		if (InputUtil.getShift())
		{
			inputModifier |= InputModifier.Shift;
		}
		if (InputUtil.getAlt())
		{
			inputModifier |= InputModifier.Alt;
		}
		if (InputUtil.getSuper())
		{
			inputModifier |= InputModifier.Super;
		}
		return inputModifier;
	}

	// Token: 0x0600511A RID: 20762 RVA: 0x001516A0 File Offset: 0x0014FAA0
	public static bool IsModifierDown(InputModifier flag)
	{
		return BitField.HasBitFlag((uint)InputUtil.GetModifiers(), (int)flag);
	}

	// Token: 0x0600511B RID: 20763 RVA: 0x001516AD File Offset: 0x0014FAAD
	public static bool GetShortcut(KeyCode key, InputModifier modifiers)
	{
		return Input.GetKey(key) && (InputUtil.GetModifiers() & modifiers) > InputModifier.None;
	}

	// Token: 0x0600511C RID: 20764 RVA: 0x001516C7 File Offset: 0x0014FAC7
	public static bool GetShortcutDown(KeyCode key, InputModifier modifiers)
	{
		return Input.GetKeyDown(key) && (InputUtil.GetModifiers() & modifiers) > InputModifier.None;
	}

	// Token: 0x0600511D RID: 20765 RVA: 0x001516E1 File Offset: 0x0014FAE1
	public static bool GetShortcutUp(KeyCode key, InputModifier modifiers)
	{
		return Input.GetKeyUp(key) && (InputUtil.GetModifiers() & modifiers) > InputModifier.None;
	}
}
