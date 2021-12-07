using System;

// Token: 0x02000254 RID: 596
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class PostConstruct : Attribute
{
	// Token: 0x06000BFF RID: 3071 RVA: 0x000552FC File Offset: 0x000536FC
	public PostConstruct()
	{
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00055304 File Offset: 0x00053704
	public PostConstruct(int p)
	{
		this.priority = p;
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00055313 File Offset: 0x00053713
	// (set) Token: 0x06000C02 RID: 3074 RVA: 0x0005531B File Offset: 0x0005371B
	public int priority { get; set; }
}
