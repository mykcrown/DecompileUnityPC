using System;

// Token: 0x02000252 RID: 594
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class Name : Attribute
{
	// Token: 0x06000BFB RID: 3067 RVA: 0x000552D4 File Offset: 0x000536D4
	public Name(object n)
	{
		this.name = n;
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06000BFC RID: 3068 RVA: 0x000552E3 File Offset: 0x000536E3
	// (set) Token: 0x06000BFD RID: 3069 RVA: 0x000552EB File Offset: 0x000536EB
	public object name { get; set; }
}
