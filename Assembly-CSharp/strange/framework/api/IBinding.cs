using System;

namespace strange.framework.api
{
	// Token: 0x0200028A RID: 650
	public interface IBinding
	{
		// Token: 0x06000D6F RID: 3439
		IBinding Bind<T>();

		// Token: 0x06000D70 RID: 3440
		IBinding Bind(object key);

		// Token: 0x06000D71 RID: 3441
		IBinding To<T>();

		// Token: 0x06000D72 RID: 3442
		IBinding To(object o);

		// Token: 0x06000D73 RID: 3443
		IBinding ToName<T>();

		// Token: 0x06000D74 RID: 3444
		IBinding ToName(object o);

		// Token: 0x06000D75 RID: 3445
		IBinding Named<T>();

		// Token: 0x06000D76 RID: 3446
		IBinding Named(object o);

		// Token: 0x06000D77 RID: 3447
		void RemoveKey(object o);

		// Token: 0x06000D78 RID: 3448
		void RemoveValue(object o);

		// Token: 0x06000D79 RID: 3449
		void RemoveName(object o);

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000D7A RID: 3450
		object key { get; }

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000D7B RID: 3451
		object name { get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000D7C RID: 3452
		object value { get; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000D7D RID: 3453
		// (set) Token: 0x06000D7E RID: 3454
		Enum keyConstraint { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000D7F RID: 3455
		// (set) Token: 0x06000D80 RID: 3456
		Enum valueConstraint { get; set; }

		// Token: 0x06000D81 RID: 3457
		IBinding Weak();

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000D82 RID: 3458
		bool isWeak { get; }
	}
}
