using System;
using System.Collections.Generic;
using System.Diagnostics;
using strange.extensions.signal.api;

namespace strange.extensions.signal.impl
{
	// Token: 0x0200027F RID: 639
	public class BaseSignal : IBaseSignal
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000D08 RID: 3336 RVA: 0x0005685C File Offset: 0x00054C5C
		// (remove) Token: 0x06000D09 RID: 3337 RVA: 0x00056894 File Offset: 0x00054C94
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IBaseSignal, object[]> BaseListener = delegate(IBaseSignal A_0, object[] A_1)
		{
		};

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000D0A RID: 3338 RVA: 0x000568CC File Offset: 0x00054CCC
		// (remove) Token: 0x06000D0B RID: 3339 RVA: 0x00056904 File Offset: 0x00054D04
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<IBaseSignal, object[]> OnceBaseListener = delegate(IBaseSignal A_0, object[] A_1)
		{
		};

		// Token: 0x06000D0C RID: 3340 RVA: 0x0005693A File Offset: 0x00054D3A
		public void Dispatch(object[] args)
		{
			this.BaseListener(this, args);
			this.OnceBaseListener(this, args);
			this.OnceBaseListener = delegate(IBaseSignal A_0, object[] A_1)
			{
			};
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00056979 File Offset: 0x00054D79
		public virtual List<Type> GetTypes()
		{
			return new List<Type>();
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00056980 File Offset: 0x00054D80
		public void AddListener(Action<IBaseSignal, object[]> callback)
		{
			foreach (Delegate @delegate in this.BaseListener.GetInvocationList())
			{
				Action<IBaseSignal, object[]> obj = (Action<IBaseSignal, object[]>)@delegate;
				if (callback.Equals(obj))
				{
					return;
				}
			}
			this.BaseListener += callback;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000569CC File Offset: 0x00054DCC
		public void AddOnce(Action<IBaseSignal, object[]> callback)
		{
			foreach (Delegate @delegate in this.OnceBaseListener.GetInvocationList())
			{
				Action<IBaseSignal, object[]> obj = (Action<IBaseSignal, object[]>)@delegate;
				if (callback.Equals(obj))
				{
					return;
				}
			}
			this.OnceBaseListener += callback;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00056A18 File Offset: 0x00054E18
		public void RemoveListener(Action<IBaseSignal, object[]> callback)
		{
			this.BaseListener -= callback;
		}
	}
}
