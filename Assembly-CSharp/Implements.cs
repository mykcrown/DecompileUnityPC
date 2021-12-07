using System;
using strange.extensions.injector.api;

// Token: 0x0200023F RID: 575
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class Implements : Attribute
{
	// Token: 0x06000B59 RID: 2905 RVA: 0x000541E9 File Offset: 0x000525E9
	public Implements()
	{
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x000541F1 File Offset: 0x000525F1
	public Implements(InjectionBindingScope scope)
	{
		this.Scope = scope;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00054200 File Offset: 0x00052600
	public Implements(Type t, InjectionBindingScope scope = InjectionBindingScope.SINGLE_CONTEXT)
	{
		this.DefaultInterface = t;
		this.Scope = scope;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00054216 File Offset: 0x00052616
	public Implements(InjectionBindingScope scope, object name)
	{
		this.Scope = scope;
		this.Name = name;
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0005422C File Offset: 0x0005262C
	public Implements(Type t, InjectionBindingScope scope, object name)
	{
		this.DefaultInterface = t;
		this.Name = name;
		this.Scope = scope;
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00054249 File Offset: 0x00052649
	// (set) Token: 0x06000B5F RID: 2911 RVA: 0x00054251 File Offset: 0x00052651
	public object Name { get; set; }

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0005425A File Offset: 0x0005265A
	// (set) Token: 0x06000B61 RID: 2913 RVA: 0x00054262 File Offset: 0x00052662
	public Type DefaultInterface { get; set; }

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0005426B File Offset: 0x0005266B
	// (set) Token: 0x06000B63 RID: 2915 RVA: 0x00054273 File Offset: 0x00052673
	public InjectionBindingScope Scope { get; set; }
}
