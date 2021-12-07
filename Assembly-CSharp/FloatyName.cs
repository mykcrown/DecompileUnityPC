using System;
using TMPro;
using UnityEngine;

// Token: 0x020008BE RID: 2238
public class FloatyName : GameBehavior
{
	// Token: 0x17000DA6 RID: 3494
	// (get) Token: 0x06003870 RID: 14448 RVA: 0x00108BAE File Offset: 0x00106FAE
	// (set) Token: 0x06003871 RID: 14449 RVA: 0x00108BB6 File Offset: 0x00106FB6
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x06003872 RID: 14450 RVA: 0x00108BC0 File Offset: 0x00106FC0
	public void Init(PlayerController attachedPlayer)
	{
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.currentPlayer = attachedPlayer;
		this.nameText = base.GetComponentInChildren<TextMeshProUGUI>();
		this.nameText.text = PlayerUtil.GetPlayerNametag(this.localization, this.currentPlayer);
		Color iconColor = attachedPlayer.iconColor;
		this.nameText.color = iconColor;
		this.displayColorBase = iconColor;
		this.displayColorInactive = iconColor;
		this.displayColorInactive.a = 0.33f;
		this.UpdateDisplayState();
	}

	// Token: 0x06003873 RID: 14451 RVA: 0x00108C40 File Offset: 0x00107040
	public void UpdateDisplayState()
	{
		if (this.currentPlayer != null)
		{
			if (this.currentPlayer.IsActive)
			{
				this.nameText.color = this.displayColorBase;
			}
			else
			{
				this.nameText.color = this.displayColorInactive;
			}
		}
	}

	// Token: 0x17000DA7 RID: 3495
	// (get) Token: 0x06003875 RID: 14453 RVA: 0x00108CA3 File Offset: 0x001070A3
	// (set) Token: 0x06003874 RID: 14452 RVA: 0x00108C95 File Offset: 0x00107095
	public float Alpha
	{
		get
		{
			return this.canvasGroup.alpha;
		}
		set
		{
			this.canvasGroup.alpha = value;
		}
	}

	// Token: 0x040026D1 RID: 9937
	private PlayerController currentPlayer;

	// Token: 0x040026D2 RID: 9938
	private TextMeshProUGUI nameText;

	// Token: 0x040026D3 RID: 9939
	private Color displayColorBase;

	// Token: 0x040026D4 RID: 9940
	private Color displayColorInactive;

	// Token: 0x040026D5 RID: 9941
	private CanvasGroup canvasGroup;
}
