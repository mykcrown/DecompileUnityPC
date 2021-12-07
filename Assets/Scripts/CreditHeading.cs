// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class CreditHeading : MonoBehaviour
{
	public TextMeshPro headingText;

	public void Set(string text, bool useUnderline = true, float fontSize = 40f)
	{
		this.headingText.text = text;
		if (!useUnderline)
		{
			this.headingText.fontStyle = FontStyles.Normal;
		}
		this.headingText.fontSize = fontSize;
	}
}
