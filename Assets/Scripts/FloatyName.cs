// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class FloatyName : GameBehavior
{
	private PlayerController currentPlayer;

	private TextMeshProUGUI nameText;

	private Color displayColorBase;

	private Color displayColorInactive;

	private CanvasGroup canvasGroup;

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

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
}
