// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CursorManager : ICursorManager
{
	private bool state = true;

	private ProxyMono proxyObject;

	[PostConstruct]
	public void Init()
	{
		this.proxyObject = ProxyMono.CreateNew("CursorManager");
		ProxyMono expr_16 = this.proxyObject;
		expr_16.OnApplicationFocusFn = (Action<bool>)Delegate.Combine(expr_16.OnApplicationFocusFn, new Action<bool>(this.OnApplicationFocus));
	}

	public void SetDisplay(bool value)
	{
		this.state = value;
		this.syncCursor();
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			this.syncCursor();
		}
	}

	private void syncCursor()
	{
		if (Application.isEditor)
		{
			Cursor.visible = true;
		}
		else
		{
			Cursor.visible = this.state;
		}
	}
}
