using System;
using MemberwiseEquality;

// Token: 0x02000880 RID: 2176
[Serializable]
public abstract class CloneableObject : MemberwiseEqualityObject, ICloneable
{
	// Token: 0x060036A7 RID: 13991 RVA: 0x0000AFF6 File Offset: 0x000093F6
	public virtual object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x0000B000 File Offset: 0x00009400
	public virtual object DeepClone()
	{
		object obj = base.MemberwiseClone();
		((CloneableObject)obj).Assign(this);
		return obj;
	}
}
