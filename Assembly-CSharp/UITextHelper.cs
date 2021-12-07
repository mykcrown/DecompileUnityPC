using System;
using System.Collections.Generic;
using TMPro;

// Token: 0x02000B70 RID: 2928
public class UITextHelper : IUITextHelper
{
	// Token: 0x060054AE RID: 21678 RVA: 0x001B2C50 File Offset: 0x001B1050
	[PostConstruct]
	public void Init()
	{
		this.proxyObject = ProxyMono.CreateNew("UITextHelper");
		ProxyMono proxyMono = this.proxyObject;
		proxyMono.OnUpdate = (Action)Delegate.Combine(proxyMono.OnUpdate, new Action(this.Update));
	}

	// Token: 0x060054AF RID: 21679 RVA: 0x001B2C8C File Offset: 0x001B108C
	public void TrackText(TextMeshProUGUI textField, Action sizeChangedCallback)
	{
		if (textField != null)
		{
			TextRedrawTracker textRedrawTracker = new TextRedrawTracker();
			textRedrawTracker.textField = textField;
			textRedrawTracker.prevWidth = textField.renderedWidth;
			textRedrawTracker.onResizeCallback = sizeChangedCallback;
			this.list.Add(textRedrawTracker);
		}
	}

	// Token: 0x060054B0 RID: 21680 RVA: 0x001B2CD4 File Offset: 0x001B10D4
	public void UpdateText(TextMeshProUGUI textField, string text)
	{
		if (textField == null || textField.text == text)
		{
			return;
		}
		textField.text = text;
		foreach (TextRedrawTracker textRedrawTracker in this.list)
		{
			if (textRedrawTracker.textField == textField)
			{
				textRedrawTracker.BeginMonitoring();
				break;
			}
		}
	}

	// Token: 0x060054B1 RID: 21681 RVA: 0x001B2D6C File Offset: 0x001B116C
	public void UntrackText(TextMeshProUGUI textField)
	{
		for (int i = this.list.Count - 1; i >= 0; i--)
		{
			if (this.list[i].textField == textField)
			{
				this.list[i].Release();
				this.list.RemoveAt(i);
				break;
			}
		}
	}

	// Token: 0x060054B2 RID: 21682 RVA: 0x001B2DD8 File Offset: 0x001B11D8
	private void Update()
	{
		this.callbackList.Clear();
		for (int i = this.list.Count - 1; i >= 0; i--)
		{
			if (this.list[i].monitor)
			{
				TextRedrawTracker textRedrawTracker = this.list[i];
				textRedrawTracker.TickMonitoring();
				float renderedWidth = textRedrawTracker.textField.renderedWidth;
				if (renderedWidth != textRedrawTracker.prevWidth)
				{
					textRedrawTracker.prevWidth = renderedWidth;
					this.callbackList.Add(textRedrawTracker.onResizeCallback);
				}
			}
			if (this.list[i].textField.IsDestroyed())
			{
				this.list[i].Release();
				this.list.RemoveAt(i);
			}
		}
		foreach (Action action in this.callbackList)
		{
			action();
		}
	}

	// Token: 0x04003590 RID: 13712
	private ProxyMono proxyObject;

	// Token: 0x04003591 RID: 13713
	private List<TextRedrawTracker> list = new List<TextRedrawTracker>();

	// Token: 0x04003592 RID: 13714
	private List<Action> callbackList = new List<Action>();
}
