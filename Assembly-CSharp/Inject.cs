using System;

// Token: 0x02000251 RID: 593
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class Inject : Attribute
{
	// Token: 0x06000BF7 RID: 3063 RVA: 0x000552AC File Offset: 0x000536AC
	public Inject()
	{
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x000552B4 File Offset: 0x000536B4
	public Inject(object n)
	{
		this.name = n;
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x000552C3 File Offset: 0x000536C3
	// (set) Token: 0x06000BFA RID: 3066 RVA: 0x000552CB File Offset: 0x000536CB
	public object name { get; set; }
}
