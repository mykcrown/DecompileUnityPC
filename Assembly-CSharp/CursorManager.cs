using System;
using UnityEngine;

// Token: 0x02000919 RID: 2329
public class CursorManager : ICursorManager
{
	// Token: 0x06003C79 RID: 15481 RVA: 0x00118C98 File Offset: 0x00117098
	[PostConstruct]
	public void Init()
	{
		this.proxyObject = ProxyMono.CreateNew("CursorManager");
		ProxyMono proxyMono = this.proxyObject;
		proxyMono.OnApplicationFocusFn = (Action<bool>)Delegate.Combine(proxyMono.OnApplicationFocusFn, new Action<bool>(this.OnApplicationFocus));
	}

	// Token: 0x06003C7A RID: 15482 RVA: 0x00118CD1 File Offset: 0x001170D1
	public void SetDisplay(bool value)
	{
		this.state = value;
		this.syncCursor();
	}

	// Token: 0x06003C7B RID: 15483 RVA: 0x00118CE0 File Offset: 0x001170E0
	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			this.syncCursor();
		}
	}

	// Token: 0x06003C7C RID: 15484 RVA: 0x00118CEE File Offset: 0x001170EE
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

	// Token: 0x0400296C RID: 10604
	private bool state = true;

	// Token: 0x0400296D RID: 10605
	private ProxyMono proxyObject;
}
