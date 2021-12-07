using System;

// Token: 0x0200072D RID: 1837
[Serializable]
public struct EquipmentID
{
	// Token: 0x06002D51 RID: 11601 RVA: 0x000E88E2 File Offset: 0x000E6CE2
	public EquipmentID(long id)
	{
		this.id = id;
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x000E88EB File Offset: 0x000E6CEB
	public EquipmentID(ulong id)
	{
		this.id = (long)id;
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x000E88F4 File Offset: 0x000E6CF4
	public bool IsNull()
	{
		return this.id == 0L;
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x000E8900 File Offset: 0x000E6D00
	public override bool Equals(object other)
	{
		return other is EquipmentID && (EquipmentID)other == this;
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x000E8920 File Offset: 0x000E6D20
	public override int GetHashCode()
	{
		int num = 17;
		return num * 7 + ((this.id != 0L) ? this.id.GetHashCode() : 0);
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x000E895A File Offset: 0x000E6D5A
	public static bool operator ==(EquipmentID a, EquipmentID b)
	{
		return a.id == b.id;
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x000E896C File Offset: 0x000E6D6C
	public static bool operator !=(EquipmentID a, EquipmentID b)
	{
		return !(a == b);
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x000E8978 File Offset: 0x000E6D78
	public override string ToString()
	{
		return this.id.ToString();
	}

	// Token: 0x04002030 RID: 8240
	public long id;
}
