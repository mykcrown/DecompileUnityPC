// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class GenericWindowFlow
{
	private sealed class _openNextWindowOnClose_c__AnonStorey0
	{
		internal int currentIndex;

		internal GenericWindowFlow _this;

		internal void __m__0()
		{
			this._this.openNextWindowOnClose(this.currentIndex + 1);
		}
	}

	public Action AllClosedCallback;

	private List<Func<BaseWindow>> createWindows;

	public void StartFlow(List<Func<BaseWindow>> createWindows)
	{
		this.createWindows = createWindows;
		this.openNextWindowOnClose(0);
	}

	private void openNextWindowOnClose(int currentIndex)
	{
		GenericWindowFlow._openNextWindowOnClose_c__AnonStorey0 _openNextWindowOnClose_c__AnonStorey = new GenericWindowFlow._openNextWindowOnClose_c__AnonStorey0();
		_openNextWindowOnClose_c__AnonStorey.currentIndex = currentIndex;
		_openNextWindowOnClose_c__AnonStorey._this = this;
		if (_openNextWindowOnClose_c__AnonStorey.currentIndex >= this.createWindows.Count)
		{
			if (this.AllClosedCallback != null)
			{
				this.AllClosedCallback();
			}
		}
		else
		{
			BaseWindow baseWindow = this.createWindows[_openNextWindowOnClose_c__AnonStorey.currentIndex]();
			BaseWindow expr_5D = baseWindow;
			expr_5D.CloseCallback = (Action)Delegate.Combine(expr_5D.CloseCallback, new Action(_openNextWindowOnClose_c__AnonStorey.__m__0));
		}
	}
}
