using System;
using UnityEngine;

// Token: 0x02000846 RID: 2118
[AttributeUsage(AttributeTargets.Field)]
public class IsClonedManually : PropertyAttribute
{
	// Token: 0x06003512 RID: 13586 RVA: 0x000F9501 File Offset: 0x000F7901
	public IsClonedManually() : this(IsClonedManuallyType.Manual)
	{
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x000F950A File Offset: 0x000F790A
	public IsClonedManually(IsClonedManuallyType type)
	{
	}
}
