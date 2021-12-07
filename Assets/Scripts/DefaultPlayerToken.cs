// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class DefaultPlayerToken : PlayerToken
{
	public TextMeshProUGUI Text;

	public TextMeshProUGUI SmallText;

	public ColorSpriteContainer colors;

	public override float Alpha
	{
		get
		{
			return base.Alpha;
		}
		set
		{
			base.Alpha = value;
			this.updateTextAlpha();
		}
	}

	private void updateTextAlpha()
	{
		this.Text.alpha = this._alpha;
		this.SmallText.alpha = this._alpha;
	}

	public override void UpdateText(PlayerSelectionInfo info, UIColor color)
	{
		if (info.type == PlayerType.CPU)
		{
			this.Text.gameObject.SetActive(false);
			this.SmallText.gameObject.SetActive(true);
			this.SmallText.text = PlayerUtil.GetPlayerNumText(base.localization, info);
			this.SmallText.color = PlayerUtil.GetColorFromUIColor(color);
		}
		else
		{
			this.SmallText.gameObject.SetActive(false);
			this.Text.gameObject.SetActive(true);
			this.Text.text = PlayerUtil.GetPlayerNumText(base.localization, info);
			this.Text.color = PlayerUtil.GetColorFromUIColor(color);
		}
		this.updateTextAlpha();
	}

	public override Sprite GetSpriteForColor(UIColor color)
	{
		return this.colors.GetSprite(color);
	}
}
