using System;
using FixedPoint;
using MemberwiseEquality;

// Token: 0x0200069F RID: 1695
[Serializable]
public class InputState : MemberwiseEqualityObject, ICloneable
{
	// Token: 0x17000A58 RID: 2648
	// (get) Token: 0x06002A0A RID: 10762 RVA: 0x000DE23E File Offset: 0x000DC63E
	public Fixed axis
	{
		get
		{
			return this.value.axis.Value;
		}
	}

	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x06002A0B RID: 10763 RVA: 0x000DE250 File Offset: 0x000DC650
	public bool button
	{
		get
		{
			return this.value.button;
		}
	}

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x06002A0C RID: 10764 RVA: 0x000DE25D File Offset: 0x000DC65D
	public IntegerAxis IntAxis
	{
		get
		{
			return this.value.axis;
		}
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x000DE26C File Offset: 0x000DC66C
	public void Load(InputState state)
	{
		if (state == null)
		{
			this.Clear();
		}
		else
		{
			this.framesHeldDown = state.framesHeldDown;
			this.tapped = state.tapped;
			this.justTapped = state.justTapped;
			this.value.Load(state.value);
		}
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x000DE2BF File Offset: 0x000DC6BF
	public void Clear()
	{
		this.framesHeldDown = 0;
		this.tapped = false;
		this.justTapped = false;
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x000DE2D6 File Offset: 0x000DC6D6
	public void UpdateTapped(bool tapped, int currentFrame)
	{
		this.justTapped = (tapped && !this.tapped);
		this.tapped = tapped;
	}

	// Token: 0x06002A10 RID: 10768 RVA: 0x000DE2F8 File Offset: 0x000DC6F8
	public void LoadValue(InputValue value)
	{
		if (this.framesHeldDown > 0 && (!value.button || value.axis.RawIntegerValue == 0 || !MathUtil.SignsMatch(value.axis.Value, this.axis)))
		{
			this.framesHeldDown = 0;
		}
		this.value.Load(value);
		if (this.axis != 0 || this.button)
		{
			this.framesHeldDown++;
		}
		else
		{
			this.framesHeldDown = 0;
		}
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x000DE390 File Offset: 0x000DC790
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x04001E32 RID: 7730
	private InputValue value = new InputValue();

	// Token: 0x04001E33 RID: 7731
	public int framesHeldDown;

	// Token: 0x04001E34 RID: 7732
	public bool tapped;

	// Token: 0x04001E35 RID: 7733
	public bool justTapped;
}
