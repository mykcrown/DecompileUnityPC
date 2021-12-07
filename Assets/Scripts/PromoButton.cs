// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public class PromoButton : MenuItemButton
{
	public Image MainImage;

	public void SetImage(Sprite sprite)
	{
		this.MainImage.sprite = sprite;
	}
}
