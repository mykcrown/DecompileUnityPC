using System;
using TMPro;
using UnityEngine;

// Token: 0x02000909 RID: 2313
public class CreditHeading : MonoBehaviour
{
	// Token: 0x06003C21 RID: 15393 RVA: 0x00116C15 File Offset: 0x00115015
	public void Set(string text, bool useUnderline = true, float fontSize = 40f)
	{
		this.headingText.text = text;
		if (!useUnderline)
		{
			this.headingText.fontStyle = FontStyles.Normal;
		}
		this.headingText.fontSize = fontSize;
	}

	// Token: 0x04002937 RID: 10551
	public TextMeshPro headingText;
}
