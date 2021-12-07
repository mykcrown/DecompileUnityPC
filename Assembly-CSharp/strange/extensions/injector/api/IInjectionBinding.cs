using System;
using strange.framework.api;

namespace strange.extensions.injector.api
{
	// Token: 0x02000245 RID: 581
	public interface IInjectionBinding : IBinding
	{
		// Token: 0x06000B84 RID: 2948
		IInjectionBinding ToSingleton();

		// Token: 0x06000B85 RID: 2949
		IInjectionBinding ToValue(object o);

		// Token: 0x06000B86 RID: 2950
		IInjectionBinding SetValue(object o);

		// Token: 0x06000B87 RID: 2951
		IInjectionBinding CrossContext();

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000B88 RID: 2952
		bool isCrossContext { get; }

		// Token: 0x06000B89 RID: 2953
		IInjectionBinding ToInject(bool value);

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000B8A RID: 2954
		bool toInject { get; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000B8B RID: 2955
		// (set) Token: 0x06000B8C RID: 2956
		InjectionBindingType type { get; set; }

		// Token: 0x06000B8D RID: 2957
		IInjectionBinding Bind<T>();

		// Token: 0x06000B8E RID: 2958
		IInjectionBinding Bind(object key);

		// Token: 0x06000B8F RID: 2959
		IInjectionBinding To<T>();

		// Token: 0x06000B90 RID: 2960
		IInjectionBinding To(object o);

		// Token: 0x06000B91 RID: 2961
		IInjectionBinding ToName<T>();

		// Token: 0x06000B92 RID: 2962
		IInjectionBinding ToName(object o);

		// Token: 0x06000B93 RID: 2963
		IInjectionBinding Named<T>();

		// Token: 0x06000B94 RID: 2964
		IInjectionBinding Named(object o);

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000B95 RID: 2965
		object key { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000B96 RID: 2966
		object name { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000B97 RID: 2967
		object value { get; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000B98 RID: 2968
		// (set) Token: 0x06000B99 RID: 2969
		Enum keyConstraint { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000B9A RID: 2970
		// (set) Token: 0x06000B9B RID: 2971
		Enum valueConstraint { get; set; }
	}
}
