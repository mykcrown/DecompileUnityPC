using System;

// Token: 0x020006A6 RID: 1702
public class InputValue : ICloneable, IResetable
{
	// Token: 0x06002A3F RID: 10815 RVA: 0x000DEE59 File Offset: 0x000DD259
	public void Set(float axis, bool button)
	{
		this.axis.Set(axis);
		this.button = button;
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x000DEE6E File Offset: 0x000DD26E
	public void Load(InputValue values)
	{
		this.axis.Set(values.axis);
		this.button = values.button;
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x000DEE8D File Offset: 0x000DD28D
	public void Clear()
	{
		this.axis.Set(0);
		this.button = false;
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x000DEEA2 File Offset: 0x000DD2A2
	public bool Equals(InputValue other)
	{
		return other.axis.Value == this.axis.Value && other.button == this.button;
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x000DEED5 File Offset: 0x000DD2D5
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000DEEDD File Offset: 0x000DD2DD
	public void Reset()
	{
		this.Clear();
	}

	// Token: 0x04001E50 RID: 7760
	public static InputValue EmptyValue = new InputValue();

	// Token: 0x04001E51 RID: 7761
	public IntegerAxis axis = new IntegerAxis();

	// Token: 0x04001E52 RID: 7762
	public bool button;
}
