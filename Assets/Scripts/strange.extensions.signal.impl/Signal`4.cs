// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace strange.extensions.signal.impl
{
	public class Signal<T, U, V, W> : BaseSignal
	{
		private static Action<T, U, V, W> __f__am_cache0;

		private static Action<T, U, V, W> __f__am_cache1;

		private static Action<T, U, V, W> __f__am_cache2;

		public event Action<T, U, V, W> Listener;

		public event Action<T, U, V, W> OnceListener;

		public Signal()
		{
			if (Signal<T, U, V, W>.__f__am_cache1 == null)
			{
				Signal<T, U, V, W>.__f__am_cache1 = new Action<T, U, V, W>(Signal<T, U, V, W>._Listener_m__1);
			}
			this.Listener = Signal<T, U, V, W>.__f__am_cache1;
			if (Signal<T, U, V, W>.__f__am_cache2 == null)
			{
				Signal<T, U, V, W>.__f__am_cache2 = new Action<T, U, V, W>(Signal<T, U, V, W>._OnceListener_m__2);
			}
			this.OnceListener = Signal<T, U, V, W>.__f__am_cache2;
			
		}

		public void AddListener(Action<T, U, V, W> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		public void AddOnce(Action<T, U, V, W> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		public void RemoveListener(Action<T, U, V, W> callback)
		{
			this.Listener -= callback;
		}

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

		public void Dispatch(T type1, U type2, V type3, W type4)
		{
			this.Listener(type1, type2, type3, type4);
			this.OnceListener(type1, type2, type3, type4);
			if (Signal<T, U, V, W>.__f__am_cache0 == null)
			{
				Signal<T, U, V, W>.__f__am_cache0 = new Action<T, U, V, W>(Signal<T, U, V, W>._Dispatch_m__0);
			}
			this.OnceListener = Signal<T, U, V, W>.__f__am_cache0;
			object[] args = new object[]
			{
				type1,
				type2,
				type3,
				type4
			};
			base.Dispatch(args);
		}

		private Action<T, U, V, W> AddUnique(Action<T, U, V, W> listeners, Action<T, U, V, W> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T, U, V, W>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private static void _Dispatch_m__0(T t, U u, V v, W w)
		{
		}

		private static void _Listener_m__1(T t, U u, V v, W w)
		{
		}

		private static void _OnceListener_m__2(T t, U u, V v, W w)
		{
		}
	}
}
