using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000953 RID: 2387
public class HyperlinkText : ClientBehavior, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x17000F05 RID: 3845
	// (get) Token: 0x06003F66 RID: 16230 RVA: 0x00120624 File Offset: 0x0011EA24
	// (set) Token: 0x06003F67 RID: 16231 RVA: 0x0012062C File Offset: 0x0011EA2C
	[Inject]
	public IHyperlinkHandler hyperlinkHandler { get; set; }

	// Token: 0x06003F68 RID: 16232 RVA: 0x00120635 File Offset: 0x0011EA35
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

	// Token: 0x06003F69 RID: 16233 RVA: 0x00120675 File Offset: 0x0011EA75
	public void OnPointerDown(PointerEventData eventData)
	{
		this.hyperlinkHandler.TryClickLink(this.text, Input.mousePosition);
	}

	// Token: 0x04002B04 RID: 11012
	public TMP_Text text;
}
