using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace strange.extensions.signal.impl
{
	// Token: 0x02000283 RID: 643
	public class Signal<T, U, V> : BaseSignal
	{
		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06000D3F RID: 3391 RVA: 0x000570C4 File Offset: 0x000554C4
		// (remove) Token: 0x06000D40 RID: 3392 RVA: 0x000570FC File Offset: 0x000554FC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T, U, V> Listener = delegate(T A_0, U A_1, V A_2)
		{
		};

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06000D41 RID: 3393 RVA: 0x00057134 File Offset: 0x00055534
		// (remove) Token: 0x06000D42 RID: 3394 RVA: 0x0005716C File Offset: 0x0005556C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T, U, V> OnceListener = delegate(T A_0, U A_1, V A_2)
		{
		};

		// Token: 0x06000D43 RID: 3395 RVA: 0x000571A2 File Offset: 0x000555A2
		public void AddListener(Action<T, U, V> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000571B7 File Offset: 0x000555B7
		public void AddOnce(Action<T, U, V> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000571CC File Offset: 0x000555CC
		public void RemoveListener(Action<T, U, V> callback)
		{
			this.Listener -= callback;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x000571D8 File Offset: 0x000555D8
		public override List<Type> GetTypes()
		{
			return new List<Type>
			{
				typeof(T),
				typeof(U),
				typeof(V)
			};
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0005721C File Offset: 0x0005561C
		public void Dispatch(T type1, U type2, V type3)
		{
			this.Listener(type1, type2, type3);
			this.OnceListener(type1, type2, type3);
			this.OnceListener = delegate(T A_0, U A_1, V A_2)
			{
			};
			object[] args = new object[]
			{
				type1,
				type2,
				type3
			};
			base.Dispatch(args);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00057291 File Offset: 0x00055691
		private Action<T, U, V> AddUnique(Action<T, U, V> listeners, Action<T, U, V> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T, U, V>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}
	}
}
