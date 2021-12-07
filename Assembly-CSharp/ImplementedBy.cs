using System;
using strange.extensions.injector.api;

// Token: 0x02000240 RID: 576
[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class ImplementedBy : Attribute
{
	// Token: 0x06000B64 RID: 2916 RVA: 0x0005427C File Offset: 0x0005267C
	public ImplementedBy(Type t, InjectionBindingScope scope = InjectionBindingScope.SINGLE_CONTEXT)
	{
		this.DefaultType = t;
		this.Scope = scope;
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00054292 File Offset: 0x00052692
	// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0005429A File Offset: 0x0005269A
	public Type DefaultType { get; set; }

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000B67 RID: 2919 RVA: 0x000542A3 File Offset: 0x000526A3
	// (set) Token: 0x06000B68 RID: 2920 RVA: 0x000542AB File Offset: 0x000526AB
	public InjectionBindingScope Scope { get; set; }
}
