using System;

namespace strange.framework.api
{
	// Token: 0x02000289 RID: 649
	public interface IBinder
	{
		// Token: 0x06000D5E RID: 3422
		IBinding Bind<T>();

		// Token: 0x06000D5F RID: 3423
		IBinding Bind(object value);

		// Token: 0x06000D60 RID: 3424
		IBinding GetBinding<T>();

		// Token: 0x06000D61 RID: 3425
		IBinding GetBinding(object key);

		// Token: 0x06000D62 RID: 3426
		IBinding GetBinding<T>(object name);

		// Token: 0x06000D63 RID: 3427
		IBinding GetBinding(object key, object name);

		// Token: 0x06000D64 RID: 3428
		IBinding GetRawBinding();

		// Token: 0x06000D65 RID: 3429
		void Unbind<T>();

		// Token: 0x06000D66 RID: 3430
		void Unbind<T>(object name);

		// Token: 0x06000D67 RID: 3431
		void Unbind(object key);

		// Token: 0x06000D68 RID: 3432
		void Unbind(object key, object name);

		// Token: 0x06000D69 RID: 3433
		void Unbind(IBinding binding);

		// Token: 0x06000D6A RID: 3434
		void RemoveValue(IBinding binding, object value);

		// Token: 0x06000D6B RID: 3435
		void RemoveKey(IBinding binding, object value);

		// Token: 0x06000D6C RID: 3436
		void RemoveName(IBinding binding, object value);

		// Token: 0x06000D6D RID: 3437
		void OnRemove();

		// Token: 0x06000D6E RID: 3438
		void ResolveBinding(IBinding binding, object key);
	}
}
