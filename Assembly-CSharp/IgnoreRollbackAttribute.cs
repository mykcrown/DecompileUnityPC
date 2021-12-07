using System;
using UnityEngine;

// Token: 0x02000844 RID: 2116
[AttributeUsage(AttributeTargets.Field)]
public class IgnoreRollbackAttribute : PropertyAttribute
{
	// Token: 0x06003511 RID: 13585 RVA: 0x000F94F2 File Offset: 0x000F78F2
	public IgnoreRollbackAttribute(IgnoreRollbackType ignoreType)
	{
		this.ignoreType = ignoreType;
	}

	// Token: 0x0400248D RID: 9357
	private IgnoreRollbackType ignoreType;
}
