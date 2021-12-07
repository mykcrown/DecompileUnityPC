using System;
using strange.extensions.reflector.api;

namespace strange.extensions.injector.api
{
	// Token: 0x02000246 RID: 582
	public interface IInjector
	{
		// Token: 0x06000B9C RID: 2972
		object Instantiate(IInjectionBinding binding);

		// Token: 0x06000B9D RID: 2973
		object Inject(object target);

		// Token: 0x06000B9E RID: 2974
		object Inject(object target, bool attemptConstructorInjection);

		// Token: 0x06000B9F RID: 2975
		void Uninject(object target);

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000BA0 RID: 2976
		// (set) Token: 0x06000BA1 RID: 2977
		IInjectorFactory factory { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000BA2 RID: 2978
		// (set) Token: 0x06000BA3 RID: 2979
		IInjectionBinder binder { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000BA4 RID: 2980
		// (set) Token: 0x06000BA5 RID: 2981
		IReflectionBinder reflector { get; set; }
	}
}
