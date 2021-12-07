// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace strange.extensions.signal.impl
{
	public class Signal<T> : BaseSignal
	{
		private static Action<T> __f__am_cache0;

		private static Action<T> __f__am_cache1;

		private static Action<T> __f__am_cache2;

		public event Action<T> Listener;

		public event Action<T> OnceListener;

		public Signal()
		{
			if (Signal<T>.__f__am_cache1 == null)
			{
				Signal<T>.__f__am_cache1 = new Action<T>(Signal<T>._Listener_m__1);
			}
			this.Listener = Signal<T>.__f__am_cache1;
			if (Signal<T>.__f__am_cache2 == null)
			{
				Signal<T>.__f__am_cache2 = new Action<T>(Signal<T>._OnceListener_m__2);
			}
			this.OnceListener = Signal<T>.__f__am_cache2;
			
		}

		public void AddListener(Action<T> callback)
		{
			this.Listener = this.AddUnique(this.Listener, callback);
		}

		public void AddOnce(Action<T> callback)
		{
			this.OnceListener = this.AddUnique(this.OnceListener, callback);
		}

		public void RemoveListener(Action<T> callback)
		{
			this.Listener -= callback;
		}

		public override List<Type> GetTypes()
		{
			return new List<Type>
			{
				typeof(T)
			};
		}

		public void Dispatch(T type1)
		{
			this.Listener(type1);
			this.OnceListener(type1);
			if (Signal<T>.__f__am_cache0 == null)
			{
				Signal<T>.__f__am_cache0 = new Action<T>(Signal<T>._Dispatch_m__0);
			}
			this.OnceListener = Signal<T>.__f__am_cache0;
			object[] args = new object[]
			{
				type1
			};
			base.Dispatch(args);
		}

		private Action<T> AddUnique(Action<T> listeners, Action<T> callback)
		{
			if (!listeners.GetInvocationList().Contains(callback))
			{
				listeners = (Action<T>)Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private static void _Dispatch_m__0(T t)
		{
		}

		private static void _Listener_m__1(T t)
		{
		}

		private static void _OnceListener_m__2(T t)
		{
		}
	}
}
