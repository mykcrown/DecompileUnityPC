// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class InputValuesSnapshot
{
	public enum InputFlag
	{
		TapJump,
		TapStrike,
		RecoveryJumping,
		RequireDoubleTapToRun,
		COUNT
	}

	public ulong buttons;

	public ushort axes;

	public uint inputFlags;

	public void GetValue(InputData data, InputValue value)
	{
		value.Clear();
		if (data.inputType == InputType.Button)
		{
			value.button = this.GetButton(data.button);
		}
		else
		{
			value.axis.Set(this.GetAxis(data.inputType));
			value.button = (value.axis.RawIntegerValue != 0);
		}
	}

	public void SetValue(InputData data, InputValue value)
	{
		if (data.inputType == InputType.Button)
		{
			this.SetButton(data.button, value.button);
		}
		else
		{
			this.SetAxis(data.inputType, value.axis);
		}
	}

	public void SetButton(ButtonPress button, bool pressed)
	{
		if (pressed)
		{
			this.buttons |= 1uL << (int)button;
		}
		else
		{
			this.buttons &= ~(1uL << (int)button);
		}
	}

	public bool GetButton(ButtonPress button)
	{
		return (this.buttons & 1uL << (int)button) != 0uL;
	}

	public void SetAxis(InputType axisType, IntegerAxis value)
	{
		int index = (int)(axisType * (InputType)IntegerAxis.BITS_PER_AXIS);
		int rawIntegerValue = value.RawIntegerValue;
		byte b = (byte)(rawIntegerValue & IntegerAxis.VALUE_MASK);
		if (rawIntegerValue < 0)
		{
			b = (byte)(-rawIntegerValue & IntegerAxis.VALUE_MASK);
			b = BitField.AddBitFlag(b, IntegerAxis.NEGATIVE_FLAG);
		}
		this.axes = BitField.WriteByte(this.axes, index, b, IntegerAxis.BITS_PER_AXIS);
	}

	public int GetAxis(InputType axisType)
	{
		int index = (int)(axisType * (InputType)IntegerAxis.BITS_PER_AXIS);
		byte b = BitField.ReadByte(this.axes, index, IntegerAxis.BITS_PER_AXIS);
		int result = (int)b;
		if (BitField.HasBitFlag(b, IntegerAxis.NEGATIVE_FLAG))
		{
			result = (int)(-(int)BitField.RemoveBitFlag(b, IntegerAxis.NEGATIVE_FLAG));
		}
		return result;
	}

	public void SetFlag(InputValuesSnapshot.InputFlag flag, bool flagValue)
	{
		if (flagValue)
		{
			this.inputFlags |= 1u << (int)flag;
		}
		else
		{
			this.inputFlags &= ~(1u << (int)flag);
		}
	}

	public bool GetFlag(InputValuesSnapshot.InputFlag flag)
	{
		return (this.inputFlags & 1u << (int)flag) != 0u;
	}

	public bool Equals(InputValuesSnapshot other)
	{
		return other.buttons == this.buttons && other.axes == this.axes && other.inputFlags == this.inputFlags;
	}

	public void CopyFrom(InputValuesSnapshot other)
	{
		this.buttons = other.buttons;
		this.axes = other.axes;
		this.inputFlags = other.inputFlags;
	}

	public void Clear()
	{
		this.buttons = 0uL;
		this.axes = 0;
		this.inputFlags = 0u;
	}

	public override string ToString()
	{
		return string.Format("{0} - {1} - {2}", BitField.ToString(this.buttons), BitField.ToString(this.axes), BitField.ToString(this.inputFlags));
	}
}
