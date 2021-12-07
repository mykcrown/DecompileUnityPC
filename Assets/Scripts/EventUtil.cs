// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

public class EventUtil
{
	private sealed class _AddOnce_c__AnonStorey0
	{
		internal Action action;

		internal Action<Action> unsubscribe;

		internal Action myDelegate;

		internal void __m__0()
		{
			this.action();
			this.unsubscribe(this.myDelegate);
		}
	}

	public static void AddOnce(Action action, Action<Action> subscribe, Action<Action> unsubscribe)
	{
		EventUtil._AddOnce_c__AnonStorey0 _AddOnce_c__AnonStorey = new EventUtil._AddOnce_c__AnonStorey0();
		_AddOnce_c__AnonStorey.action = action;
		_AddOnce_c__AnonStorey.unsubscribe = unsubscribe;
		_AddOnce_c__AnonStorey.myDelegate = null;
		_AddOnce_c__AnonStorey.myDelegate = new Action(_AddOnce_c__AnonStorey.__m__0);
		subscribe(_AddOnce_c__AnonStorey.myDelegate);
	}
}
