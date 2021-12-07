// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace strange.extensions.signal.impl
{
	public class Signal<T, U> : BaseSignal
	{
		private static Action<T, U> __f__am_cache0;

		private static Action<T, U> __f__am_cache1;

		private static Action<T, U> __f__am_cache2;

		public event Action<T, U> Listener;

		public event Action<T, U> OnceListener;

		public Signal()
		{
			if (Signal<T, U>.__f__am_cache1 == null)
			{
				Signal<T, U>.__f__am_cache1 = new Action<T, U>(Signal<T, U>._Listener_m__1);
			}
			this.Listener = Signal<T, U>.__f__am_cache1;
			if (Signal<T, U>.__f__am_cache2 == null)
			{
				Signal<T, U>.__f__am_cache2 = new Action<T, U>(Signal<T, U>._OnceListener_m__2);
			}
			this.OnceListener = Signal<T, U>.__f__am_cache2;
			
		}

		public new void AddListener(Action<T, U> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		public new void AddOnce(Action<T, U> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		public new void RemoveListener(Action<T, U> callback)
		{
			this.Listener -= callback;
		}

		public override List<Type> GetTypes()
		{
			return new List<Type>
			{
				typeof(T),
				typeof(U)
			};
		}

		public void Dispatch(T type1, U type2)
		{
			this.Listener(type1, type2);
			this.OnceListener(type1, type2);
			if (Signal<T, U>.__f__am_cache0 == null)
			{
				Signal<T, U>.__f__am_cache0 = new Action<T, U>(Signal<T, U>._Dispatch_m__0);
			}
			this.OnceListener = Signal<T, U>.__f__am_cache0;
			object[] args = new object[]
			{
				type1,
				type2
			};
			base.Dispatch(args);
		}

		private Action<T, U> AddUnique(Action<T, U> listeners, Action<T, U> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T, U>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private static void _Dispatch_m__0(T t, U u)
		{
		}

		private static void _Listener_m__1(T t, U u)
		{
		}

		private static void _OnceListener_m__2(T t, U u)
		{
		}
	}
}
