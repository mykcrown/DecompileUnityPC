using System;

// Token: 0x0200020C RID: 524
public static class StaticInject
{
	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00050DD8 File Offset: 0x0004F1D8
	// (set) Token: 0x060009D4 RID: 2516 RVA: 0x00050DDF File Offset: 0x0004F1DF
	public static IDependencyInjection staticInjector { private get; set; }

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x060009D5 RID: 2517 RVA: 0x00050DE7 File Offset: 0x0004F1E7
	public static bool readyToInject
	{
		get
		{
			return StaticInject.staticInjector != null;
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00050DF4 File Offset: 0x0004F1F4
	public static void Inject(object target)
	{
		if (StaticInject.readyToInject && target != null)
		{
			StaticInject.staticInjector.Inject(target);
		}
	}
}
