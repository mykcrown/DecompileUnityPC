// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace strange.extensions.signal.impl
{
	public class Signal<T, U, V> : BaseSignal
	{
		private static Action<T, U, V> __f__am_cache0;

		private static Action<T, U, V> __f__am_cache1;

		private static Action<T, U, V> __f__am_cache2;

		public event Action<T, U, V> Listener;

		public event Action<T, U, V> OnceListener;

		public Signal()
		{
			if (Signal<T, U, V>.__f__am_cache1 == null)
			{
				Signal<T, U, V>.__f__am_cache1 = new Action<T, U, V>(Signal<T, U, V>._Listener_m__1);
			}
			this.Listener = Signal<T, U, V>.__f__am_cache1;
			if (Signal<T, U, V>.__f__am_cache2 == null)
			{
				Signal<T, U, V>.__f__am_cache2 = new Action<T, U, V>(Signal<T, U, V>._OnceListener_m__2);
			}
			this.OnceListener = Signal<T, U, V>.__f__am_cache2;
			
		}

		public void AddListener(Action<T, U, V> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		public void AddOnce(Action<T, U, V> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		public void RemoveListener(Action<T, U, V> callback)
		{
			this.Listener -= callback;
		}

		public override List<Type> GetTypes()
		{
			return new List<Type>
			{
				typeof(T),
				typeof(U),
				typeof(V)
			};
		}

		public void Dispatch(T type1, U type2, V type3)
		{
			this.Listener(type1, type2, type3);
			this.OnceListener(type1, type2, type3);
			if (Signal<T, U, V>.__f__am_cache0 == null)
			{
				Signal<T, U, V>.__f__am_cache0 = new Action<T, U, V>(Signal<T, U, V>._Dispatch_m__0);
			}
			this.OnceListener = Signal<T, U, V>.__f__am_cache0;
			object[] args = new object[]
			{
				type1,
				type2,
				type3
			};
			base.Dispatch(args);
		}

		private Action<T, U, V> AddUnique(Action<T, U, V> listeners, Action<T, U, V> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T, U, V>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private static void _Dispatch_m__0(T t, U u, V v)
		{
		}

		private static void _Listener_m__1(T t, U u, V v)
		{
		}

		private static void _OnceListener_m__2(T t, U u, V v)
		{
		}
	}
}
