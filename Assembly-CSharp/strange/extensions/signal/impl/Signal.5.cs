using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace strange.extensions.signal.impl
{
	// Token: 0x02000284 RID: 644
	public class Signal<T, U, V, W> : BaseSignal
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000D4D RID: 3405 RVA: 0x00057318 File Offset: 0x00055718
		// (remove) Token: 0x06000D4E RID: 3406 RVA: 0x00057350 File Offset: 0x00055750
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T, U, V, W> Listener = delegate(T A_0, U A_1, V A_2, W A_3)
		{
		};

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000D4F RID: 3407 RVA: 0x00057388 File Offset: 0x00055788
		// (remove) Token: 0x06000D50 RID: 3408 RVA: 0x000573C0 File Offset: 0x000557C0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T, U, V, W> OnceListener = delegate(T A_0, U A_1, V A_2, W A_3)
		{
		};

		// Token: 0x06000D51 RID: 3409 RVA: 0x000573F6 File Offset: 0x000557F6
		public void AddListener(Action<T, U, V, W> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0005740B File Offset: 0x0005580B
		public void AddOnce(Action<T, U, V, W> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00057420 File Offset: 0x00055820
		public void RemoveListener(Action<T, U, V, W> callback)
		{
			this.Listener -= callback;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0005742C File Offset: 0x0005582C
		public override List<Type> GetTypes()
		{
			return new List<Type>
			{
				typeof(T),
				typeof(U),
				typeof(V),
				typeof(W)
			};
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00057480 File Offset: 0x00055880
		public void Dispatch(T type1, U type2, V type3, W type4)
		{
			this.Listener(type1, type2, type3, type4);
			this.OnceListener(type1, type2, type3, type4);
			this.OnceListener = delegate(T A_0, U A_1, V A_2, W A_3)
			{
			};
			object[] args = new object[]
			{
				type1,
				type2,
				type3,
				type4
			};
			base.Dispatch(args);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00057503 File Offset: 0x00055903
		private Action<T, U, V, W> AddUnique(Action<T, U, V, W> listeners, Action<T, U, V, W> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T, U, V, W>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}
	}
}
