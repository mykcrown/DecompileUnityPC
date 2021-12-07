// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class PlayerCardIconDisplay : BaseItem3DPreviewDisplay
{
	public SpriteRenderer spriteRenderer;

	public void SetIcon(Sprite sprite)
	{
		this.spriteRenderer.sprite = sprite;
	}
}
