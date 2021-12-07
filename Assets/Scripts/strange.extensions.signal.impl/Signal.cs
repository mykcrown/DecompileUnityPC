// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace strange.extensions.signal.impl
{
	public class Signal : BaseSignal
	{
		private static Action __f__am_cache0;

		private static Action __f__am_cache1;

		private static Action __f__am_cache2;

		public event Action Listener;

		public event Action OnceListener;

		public Signal()
		{
			if (Signal.__f__am_cache1 == null)
			{
				Signal.__f__am_cache1 = new Action(Signal._Listener_m__1);
			}
			this.Listener = Signal.__f__am_cache1;
			if (Signal.__f__am_cache2 == null)
			{
				Signal.__f__am_cache2 = new Action(Signal._OnceListener_m__2);
			}
			this.OnceListener = Signal.__f__am_cache2;
			
		}

		public void AddListener(Action callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		public void AddOnce(Action callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		public void RemoveListener(Action callback)
		{
			this.Listener -= callback;
		}

		public override List<Type> GetTypes()
		{
			return new List<Type>();
		}

		public void Dispatch()
		{
			this.Listener();
			this.OnceListener();
			if (Signal.__f__am_cache0 == null)
			{
				Signal.__f__am_cache0 = new Action(Signal._Dispatch_m__0);
			}
			this.OnceListener = Signal.__f__am_cache0;
			base.Dispatch(null);
		}

		private Action AddUnique(Action listeners, Action callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private static void _Dispatch_m__0()
		{
		}

		private static void _Listener_m__1()
		{
		}

		private static void _OnceListener_m__2()
		{
		}
	}
}
