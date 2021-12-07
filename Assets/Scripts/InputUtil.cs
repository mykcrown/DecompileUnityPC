// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public static class InputUtil
{
	private static bool getControl()
	{
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
	}

	private static bool getShift()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}

	private static bool getAlt()
	{
		return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
	}

	private static bool getSuper()
	{
		return Input.GetKey(KeyCode.LeftWindows) || Input.GetKey(KeyCode.RightWindows);
	}

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

	public static bool IsModifierDown(InputModifier flag)
	{
		return BitField.HasBitFlag((uint)InputUtil.GetModifiers(), (int)flag);
	}

	public static bool GetShortcut(KeyCode key, InputModifier modifiers)
	{
		return Input.GetKey(key) && (InputUtil.GetModifiers() & modifiers) > InputModifier.None;
	}

	public static bool GetShortcutDown(KeyCode key, InputModifier modifiers)
	{
		return Input.GetKeyDown(key) && (InputUtil.GetModifiers() & modifiers) > InputModifier.None;
	}

	public static bool GetShortcutUp(KeyCode key, InputModifier modifiers)
	{
		return Input.GetKeyUp(key) && (InputUtil.GetModifiers() & modifiers) > InputModifier.None;
	}
}
