using System;
using System.Collections.Generic;
using strange.framework.api;

namespace strange.extensions.injector.api
{
	// Token: 0x02000244 RID: 580
	public interface IInjectionBinder : IInstanceProvider
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000B71 RID: 2929
		// (set) Token: 0x06000B72 RID: 2930
		IInjector injector { get; set; }

		// Token: 0x06000B73 RID: 2931
		object GetInstance(Type key, object name);

		// Token: 0x06000B74 RID: 2932
		T GetInstance<T>(object name);

		// Token: 0x06000B75 RID: 2933
		int Reflect(List<Type> list);

		// Token: 0x06000B76 RID: 2934
		int ReflectAll();

		// Token: 0x06000B77 RID: 2935
		void ResolveBinding(IBinding binding, object key);

		// Token: 0x06000B78 RID: 2936
		IInjectionBinding Bind<T>();

		// Token: 0x06000B79 RID: 2937
		IInjectionBinding Bind(Type key);

		// Token: 0x06000B7A RID: 2938
		IBinding Bind(object key);

		// Token: 0x06000B7B RID: 2939
		IInjectionBinding GetBinding<T>();

		// Token: 0x06000B7C RID: 2940
		IInjectionBinding GetBinding<T>(object name);

		// Token: 0x06000B7D RID: 2941
		IInjectionBinding GetBinding(object key);

		// Token: 0x06000B7E RID: 2942
		IInjectionBinding GetBinding(object key, object name);

		// Token: 0x06000B7F RID: 2943
		void Unbind<T>();

		// Token: 0x06000B80 RID: 2944
		void Unbind<T>(object name);

		// Token: 0x06000B81 RID: 2945
		void Unbind(object key);

		// Token: 0x06000B82 RID: 2946
		void Unbind(object key, object name);

		// Token: 0x06000B83 RID: 2947
		void Unbind(IBinding binding);
	}
}
