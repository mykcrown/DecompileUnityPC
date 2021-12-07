// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;

public class UITextHelper : IUITextHelper
{
	private ProxyMono proxyObject;

	private List<TextRedrawTracker> list = new List<TextRedrawTracker>();

	private List<Action> callbackList = new List<Action>();

	[PostConstruct]
	public void Init()
	{
		this.proxyObject = ProxyMono.CreateNew("UITextHelper");
		ProxyMono expr_16 = this.proxyObject;
		expr_16.OnUpdate = (Action)Delegate.Combine(expr_16.OnUpdate, new Action(this.Update));
	}

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

	public void UpdateText(TextMeshProUGUI textField, string text)
	{
		if (textField == null || textField.text == text)
		{
			return;
		}
		textField.text = text;
		foreach (TextRedrawTracker current in this.list)
		{
			if (current.textField == textField)
			{
				current.BeginMonitoring();
				break;
			}
		}
	}

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
		foreach (Action current in this.callbackList)
		{
			current();
		}
	}
}
