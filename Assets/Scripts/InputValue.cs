// Decompile from assembly: Assembly-CSharp.dll

using System;

public class InputValue : ICloneable, IResetable
{
	public static InputValue EmptyValue = new InputValue();

	public IntegerAxis axis = new IntegerAxis();

	public bool button;

	public void Set(float axis, bool button)
	{
		this.axis.Set(axis);
		this.button = button;
	}

	public void Load(InputValue values)
	{
		this.axis.Set(values.axis);
		this.button = values.button;
	}

	public void Clear()
	{
		this.axis.Set(0);
		this.button = false;
	}

	public bool Equals(InputValue other)
	{
		return other.axis.Value == this.axis.Value && other.button == this.button;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	public void Reset()
	{
		this.Clear();
	}
}
