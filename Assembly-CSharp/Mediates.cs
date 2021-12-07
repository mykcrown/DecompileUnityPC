using System;

// Token: 0x02000242 RID: 578
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class Mediates : Attribute
{
	// Token: 0x06000B6C RID: 2924 RVA: 0x000542D4 File Offset: 0x000526D4
	public Mediates(Type t)
	{
		this.ViewType = t;
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000B6D RID: 2925 RVA: 0x000542E3 File Offset: 0x000526E3
	// (set) Token: 0x06000B6E RID: 2926 RVA: 0x000542EB File Offset: 0x000526EB
	public Type ViewType { get; set; }
}
