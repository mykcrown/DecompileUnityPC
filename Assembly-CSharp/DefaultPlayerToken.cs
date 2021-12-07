using System;
using TMPro;
using UnityEngine;

// Token: 0x020008E0 RID: 2272
public class DefaultPlayerToken : PlayerToken
{
	// Token: 0x17000E02 RID: 3586
	// (get) Token: 0x06003A32 RID: 14898 RVA: 0x00110A8D File Offset: 0x0010EE8D
	// (set) Token: 0x06003A33 RID: 14899 RVA: 0x00110A95 File Offset: 0x0010EE95
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

	// Token: 0x06003A34 RID: 14900 RVA: 0x00110AA4 File Offset: 0x0010EEA4
	private void updateTextAlpha()
	{
		this.Text.alpha = this._alpha;
		this.SmallText.alpha = this._alpha;
	}

	// Token: 0x06003A35 RID: 14901 RVA: 0x00110AC8 File Offset: 0x0010EEC8
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

	// Token: 0x06003A36 RID: 14902 RVA: 0x00110B80 File Offset: 0x0010EF80
	public override Sprite GetSpriteForColor(UIColor color)
	{
		return this.colors.GetSprite(color);
	}

	// Token: 0x040027FF RID: 10239
	public TextMeshProUGUI Text;

	// Token: 0x04002800 RID: 10240
	public TextMeshProUGUI SmallText;

	// Token: 0x04002801 RID: 10241
	public ColorSpriteContainer colors;
}
