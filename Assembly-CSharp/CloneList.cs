using System;
using System.Collections.Generic;

// Token: 0x02000B0E RID: 2830
public class CloneList<T> : List<T>
{
	// Token: 0x06005146 RID: 20806 RVA: 0x00151EB6 File Offset: 0x001502B6
	public CloneList()
	{
	}

	// Token: 0x06005147 RID: 20807 RVA: 0x00151EBE File Offset: 0x001502BE
	public CloneList(List<T> other) : base(other)
	{
	}

	// Token: 0x06005148 RID: 20808 RVA: 0x00151EC7 File Offset: 0x001502C7
	public object ShallowClone()
	{
		return base.MemberwiseClone();
	}
}
