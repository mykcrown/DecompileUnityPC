using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace strange.extensions.signal.impl
{
	// Token: 0x02000282 RID: 642
	public class Signal<T, U> : BaseSignal
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000D31 RID: 3377 RVA: 0x00056E8C File Offset: 0x0005528C
		// (remove) Token: 0x06000D32 RID: 3378 RVA: 0x00056EC4 File Offset: 0x000552C4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T, U> Listener = delegate(T A_0, U A_1)
		{
		};

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06000D33 RID: 3379 RVA: 0x00056EFC File Offset: 0x000552FC
		// (remove) Token: 0x06000D34 RID: 3380 RVA: 0x00056F34 File Offset: 0x00055334
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T, U> OnceListener = delegate(T A_0, U A_1)
		{
		};

		// Token: 0x06000D35 RID: 3381 RVA: 0x00056F6A File Offset: 0x0005536A
		public void AddListener(Action<T, U> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00056F7F File Offset: 0x0005537F
		public void AddOnce(Action<T, U> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00056F94 File Offset: 0x00055394
		public void RemoveListener(Action<T, U> callback)
		{
			this.Listener -= callback;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00056FA0 File Offset: 0x000553A0
		public override List<Type> GetTypes()
		{
			return new List<Type>
			{
				typeof(T),
				typeof(U)
			};
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00056FD4 File Offset: 0x000553D4
		public void Dispatch(T type1, U type2)
		{
			this.Listener(type1, type2);
			this.OnceListener(type1, type2);
			this.OnceListener = delegate(T A_0, U A_1)
			{
			};
			object[] args = new object[]
			{
				type1,
				type2
			};
			base.Dispatch(args);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0005703E File Offset: 0x0005543E
		private Action<T, U> AddUnique(Action<T, U> listeners, Action<T, U> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T, U>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}
	}
}
