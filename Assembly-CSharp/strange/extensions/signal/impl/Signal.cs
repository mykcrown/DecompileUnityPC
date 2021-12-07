using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace strange.extensions.signal.impl
{
	// Token: 0x02000280 RID: 640
	public class Signal : BaseSignal
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000D15 RID: 3349 RVA: 0x00056A84 File Offset: 0x00054E84
		// (remove) Token: 0x06000D16 RID: 3350 RVA: 0x00056ABC File Offset: 0x00054EBC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Listener = delegate()
		{
		};

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000D17 RID: 3351 RVA: 0x00056AF4 File Offset: 0x00054EF4
		// (remove) Token: 0x06000D18 RID: 3352 RVA: 0x00056B2C File Offset: 0x00054F2C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnceListener = delegate()
		{
		};

		// Token: 0x06000D19 RID: 3353 RVA: 0x00056B62 File Offset: 0x00054F62
		public void AddListener(Action callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00056B77 File Offset: 0x00054F77
		public void AddOnce(Action callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00056B8C File Offset: 0x00054F8C
		public void RemoveListener(Action callback)
		{
			this.Listener -= callback;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00056B95 File Offset: 0x00054F95
		public override List<Type> GetTypes()
		{
			return new List<Type>();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00056B9C File Offset: 0x00054F9C
		public void Dispatch()
		{
			this.Listener();
			this.OnceListener();
			this.OnceListener = delegate()
			{
			};
			base.Dispatch(null);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00056BE9 File Offset: 0x00054FE9
		private Action AddUnique(Action listeners, Action callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}
	}
}
