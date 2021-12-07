using System;

// Token: 0x02000241 RID: 577
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class MediatedBy : Attribute
{
	// Token: 0x06000B69 RID: 2921 RVA: 0x000542B4 File Offset: 0x000526B4
	public MediatedBy(Type t)
	{
		this.MediatorType = t;
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000B6A RID: 2922 RVA: 0x000542C3 File Offset: 0x000526C3
	// (set) Token: 0x06000B6B RID: 2923 RVA: 0x000542CB File Offset: 0x000526CB
	public Type MediatorType { get; set; }
}
