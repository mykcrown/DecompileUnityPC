using System;

// Token: 0x020006A7 RID: 1703
[Serializable]
public class InputValuesSnapshot
{
	// Token: 0x06002A47 RID: 10823 RVA: 0x000DEEFC File Offset: 0x000DD2FC
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

	// Token: 0x06002A48 RID: 10824 RVA: 0x000DEF60 File Offset: 0x000DD360
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

	// Token: 0x06002A49 RID: 10825 RVA: 0x000DEF97 File Offset: 0x000DD397
	public void SetButton(ButtonPress button, bool pressed)
	{
		if (pressed)
		{
			this.buttons |= 1UL << (int)button;
		}
		else
		{
			this.buttons &= ~(1UL << (int)button);
		}
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x000DEFCD File Offset: 0x000DD3CD
	public bool GetButton(ButtonPress button)
	{
		return (this.buttons & 1UL << (int)button) != 0UL;
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000DEFE4 File Offset: 0x000DD3E4
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

	// Token: 0x06002A4C RID: 10828 RVA: 0x000DF040 File Offset: 0x000DD440
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

	// Token: 0x06002A4D RID: 10829 RVA: 0x000DF087 File Offset: 0x000DD487
	public void SetFlag(InputValuesSnapshot.InputFlag flag, bool flagValue)
	{
		if (flagValue)
		{
			this.inputFlags |= 1U << (int)flag;
		}
		else
		{
			this.inputFlags &= ~(1U << (int)flag);
		}
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000DF0BB File Offset: 0x000DD4BB
	public bool GetFlag(InputValuesSnapshot.InputFlag flag)
	{
		return (this.inputFlags & 1U << (int)flag) != 0U;
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000DF0D0 File Offset: 0x000DD4D0
	public bool Equals(InputValuesSnapshot other)
	{
		return other.buttons == this.buttons && other.axes == this.axes && other.inputFlags == this.inputFlags;
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x000DF105 File Offset: 0x000DD505
	public void CopyFrom(InputValuesSnapshot other)
	{
		this.buttons = other.buttons;
		this.axes = other.axes;
		this.inputFlags = other.inputFlags;
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x000DF12B File Offset: 0x000DD52B
	public void Clear()
	{
		this.buttons = 0UL;
		this.axes = 0;
		this.inputFlags = 0U;
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x000DF143 File Offset: 0x000DD543
	public override string ToString()
	{
		return string.Format("{0} - {1} - {2}", BitField.ToString(this.buttons), BitField.ToString(this.axes), BitField.ToString(this.inputFlags));
	}

	// Token: 0x04001E53 RID: 7763
	public ulong buttons;

	// Token: 0x04001E54 RID: 7764
	public ushort axes;

	// Token: 0x04001E55 RID: 7765
	public uint inputFlags;

	// Token: 0x020006A8 RID: 1704
	public enum InputFlag
	{
		// Token: 0x04001E57 RID: 7767
		TapJump,
		// Token: 0x04001E58 RID: 7768
		TapStrike,
		// Token: 0x04001E59 RID: 7769
		RecoveryJumping,
		// Token: 0x04001E5A RID: 7770
		RequireDoubleTapToRun,
		// Token: 0x04001E5B RID: 7771
		COUNT
	}
}
