using System;
using FixedPoint;

// Token: 0x02000530 RID: 1328
[Serializable]
public struct MoveSeedData
{
	// Token: 0x06001CC3 RID: 7363 RVA: 0x00094415 File Offset: 0x00092815
	public override bool Equals(object other)
	{
		return (MoveSeedData)other == this;
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x00094428 File Offset: 0x00092828
	public override int GetHashCode()
	{
		int num = 17;
		num = num * 7 + ((!this.isActive) ? 1 : 0);
		return num * 7 + this.damage.GetHashCode();
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x00094466 File Offset: 0x00092866
	public static bool operator ==(MoveSeedData a, MoveSeedData b)
	{
		return a.isActive == b.isActive && a.damage == b.damage;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x00094491 File Offset: 0x00092891
	public static bool operator !=(MoveSeedData a, MoveSeedData b)
	{
		return !(a == b);
	}

	// Token: 0x040017B5 RID: 6069
	public bool isActive;

	// Token: 0x040017B6 RID: 6070
	public Fixed damage;
}
