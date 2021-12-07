// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HyperlinkText : ClientBehavior, IPointerDownHandler, IEventSystemHandler
{
	public TMP_Text text;

	[Inject]
	public IHyperlinkHandler hyperlinkHandler
	{
		get;
		set;
	}

	public override void Awake()
	{
		base.Awake();
		if (!this.text)
		{
			this.text = base.GetComponent<TMP_Text>();
		}
		if (this.text)
		{
			this.text.raycastTarget = true;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		this.hyperlinkHandler.TryClickLink(this.text, Input.mousePosition);
	}
}
