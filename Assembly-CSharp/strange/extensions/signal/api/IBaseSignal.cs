using System;
using System.Collections.Generic;

namespace strange.extensions.signal.api
{
	// Token: 0x0200027D RID: 637
	public interface IBaseSignal
	{
		// Token: 0x06000D02 RID: 3330
		void Dispatch(object[] args);

		// Token: 0x06000D03 RID: 3331
		void AddListener(Action<IBaseSignal, object[]> callback);

		// Token: 0x06000D04 RID: 3332
		void AddOnce(Action<IBaseSignal, object[]> callback);

		// Token: 0x06000D05 RID: 3333
		void RemoveListener(Action<IBaseSignal, object[]> callback);

		// Token: 0x06000D06 RID: 3334
		List<Type> GetTypes();
	}
}
