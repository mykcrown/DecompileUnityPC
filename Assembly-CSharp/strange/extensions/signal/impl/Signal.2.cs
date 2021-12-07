using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace strange.extensions.signal.impl
{
	// Token: 0x02000281 RID: 641
	public class Signal<T> : BaseSignal
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000D23 RID: 3363 RVA: 0x00056C70 File Offset: 0x00055070
		// (remove) Token: 0x06000D24 RID: 3364 RVA: 0x00056CA8 File Offset: 0x000550A8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T> Listener = delegate(T A_0)
		{
		};

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000D25 RID: 3365 RVA: 0x00056CE0 File Offset: 0x000550E0
		// (remove) Token: 0x06000D26 RID: 3366 RVA: 0x00056D18 File Offset: 0x00055118
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<T> OnceListener = delegate(T A_0)
		{
		};

		// Token: 0x06000D27 RID: 3367 RVA: 0x00056D4E File Offset: 0x0005514E
		public void AddListener(Action<T> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00056D63 File Offset: 0x00055163
		public void AddOnce(Action<T> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00056D78 File Offset: 0x00055178
		public void RemoveListener(Action<T> callback)
		{
			this.Listener -= callback;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00056D84 File Offset: 0x00055184
		public override List<Type> GetTypes()
		{
			return new List<Type>
			{
				typeof(T)
			};
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00056DA8 File Offset: 0x000551A8
		public void Dispatch(T type1)
		{
			this.Listener(type1);
			this.OnceListener(type1);
			this.OnceListener = delegate(T A_0)
			{
			};
			object[] args = new object[]
			{
				type1
			};
			base.Dispatch(args);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00056E07 File Offset: 0x00055207
		private Action<T> AddUnique(Action<T> listeners, Action<T> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}
	}
}
