using System;
using strange.framework.api;

namespace strange.extensions.pool.api
{
	// Token: 0x02000264 RID: 612
	public interface IPool : IManagedList
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000C48 RID: 3144
		// (set) Token: 0x06000C49 RID: 3145
		IInstanceProvider instanceProvider { get; set; }

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000C4A RID: 3146
		// (set) Token: 0x06000C4B RID: 3147
		Type poolType { get; set; }

		// Token: 0x06000C4C RID: 3148
		object GetInstance();

		// Token: 0x06000C4D RID: 3149
		void ReturnInstance(object value);

		// Token: 0x06000C4E RID: 3150
		void Clean();

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000C4F RID: 3151
		int available { get; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000C50 RID: 3152
		// (set) Token: 0x06000C51 RID: 3153
		int size { get; set; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000C52 RID: 3154
		int instanceCount { get; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000C53 RID: 3155
		// (set) Token: 0x06000C54 RID: 3156
		PoolOverflowBehavior overflowBehavior { get; set; }

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000C55 RID: 3157
		// (set) Token: 0x06000C56 RID: 3158
		PoolInflationType inflationType { get; set; }
	}
}
