// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;

[Serializable]
public class DefaultInputBinding : ICloneable
{
	public ButtonPress button;

	public InputControlType controlType;

	public Key defaultP1Key;

	public Key defaultP2Key;

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
