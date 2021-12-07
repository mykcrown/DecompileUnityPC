using System;
using System.Reflection;

// Token: 0x02000B2E RID: 2862
public class ProcessContext
{
	// Token: 0x06005312 RID: 21266 RVA: 0x001AD50C File Offset: 0x001AB90C
	public static void NoOp(FieldInfo field, Type type)
	{
	}

	// Token: 0x040034C1 RID: 13505
	public Action<FieldInfo, Type> IsIgnoreEqualityComparison;

	// Token: 0x040034C2 RID: 13506
	public Action<FieldInfo, Type> IsUnhandledDictionary;

	// Token: 0x040034C3 RID: 13507
	public Action<FieldInfo, Type> OnBeginProcessField;

	// Token: 0x040034C4 RID: 13508
	public Action<FieldInfo, Type> IsEnum;

	// Token: 0x040034C5 RID: 13509
	public Action<FieldInfo, Type> IsValueType;

	// Token: 0x040034C6 RID: 13510
	public Action<FieldInfo, Type> IsInterface;

	// Token: 0x040034C7 RID: 13511
	public Action<FieldInfo, Type> IsDictionary;

	// Token: 0x040034C8 RID: 13512
	public Action<FieldInfo, Type> IsEnumerable;

	// Token: 0x040034C9 RID: 13513
	public Action<FieldInfo, Type> IsFixedCapacityList;

	// Token: 0x040034CA RID: 13514
	public Action<FieldInfo, Type> IsMemberwiseEqualityObject;

	// Token: 0x040034CB RID: 13515
	public Action<FieldInfo, Type> IsGenericObject;

	// Token: 0x040034CC RID: 13516
	public Action<FieldInfo, Type> OnProcessedReferenceField;

	// Token: 0x040034CD RID: 13517
	public Action<FieldInfo, Type> OnProcessedField;
}
