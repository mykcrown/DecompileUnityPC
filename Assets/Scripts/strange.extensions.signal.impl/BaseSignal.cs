// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.signal.api;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace strange.extensions.signal.impl
{
	public class BaseSignal : IBaseSignal
	{
		private static Action<IBaseSignal, object[]> __f__am_cache0;

		private static Action<IBaseSignal, object[]> __f__am_cache1;

		private static Action<IBaseSignal, object[]> __f__am_cache2;

		public event Action<IBaseSignal, object[]> BaseListener;

		public event Action<IBaseSignal, object[]> OnceBaseListener;

		public BaseSignal()
		{
			if (BaseSignal.__f__am_cache1 == null)
			{
				BaseSignal.__f__am_cache1 = new Action<IBaseSignal, object[]>(BaseSignal._BaseListener_m__1);
			}
			this.BaseListener = BaseSignal.__f__am_cache1;
			if (BaseSignal.__f__am_cache2 == null)
			{
				BaseSignal.__f__am_cache2 = new Action<IBaseSignal, object[]>(BaseSignal._OnceBaseListener_m__2);
			}
			this.OnceBaseListener = BaseSignal.__f__am_cache2;
			
		}

		public void Dispatch(object[] args)
		{
			this.BaseListener(this, args);
			this.OnceBaseListener(this, args);
			if (BaseSignal.__f__am_cache0 == null)
			{
				BaseSignal.__f__am_cache0 = new Action<IBaseSignal, object[]>(BaseSignal._Dispatch_m__0);
			}
			this.OnceBaseListener = BaseSignal.__f__am_cache0;
		}

		public virtual List<Type> GetTypes()
		{
			return new List<Type>();
		}

		public void AddListener(Action<IBaseSignal, object[]> callback)
		{
			Delegate[] invocationList = this.BaseListener.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Delegate @delegate = invocationList[i];
				Action<IBaseSignal, object[]> obj = (Action<IBaseSignal, object[]>)@delegate;
				if (callback.Equals(obj))
				{
					return;
				}
			}
			this.BaseListener += callback;
		}

		public void AddOnce(Action<IBaseSignal, object[]> callback)
		{
			Delegate[] invocationList = this.OnceBaseListener.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				Delegate @delegate = invocationList[i];
				Action<IBaseSignal, object[]> obj = (Action<IBaseSignal, object[]>)@delegate;
				if (callback.Equals(obj))
				{
					return;
				}
			}
			this.OnceBaseListener += callback;
		}

		public void RemoveListener(Action<IBaseSignal, object[]> callback)
		{
			this.BaseListener -= callback;
		}

		private static void _Dispatch_m__0(IBaseSignal baseSignal, object[] array)
		{
		}

		private static void _BaseListener_m__1(IBaseSignal baseSignal, object[] array)
		{
		}

		private static void _OnceBaseListener_m__2(IBaseSignal baseSignal, object[] array)
		{
		}
	}
}
