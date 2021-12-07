// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class ColorSpriteContainer
{
	public Sprite Red;

	public Sprite Yellow;

	public Sprite Green;

	public Sprite Purple;

	public Sprite Blue;

	public Sprite Pink;

	public Sprite Grey;

	public Sprite GetSprite(UIColor color)
	{
		switch (color)
		{
		case UIColor.Blue:
			IL_27:
			return this.Blue;
		case UIColor.Red:
			return this.Red;
		case UIColor.Yellow:
			return this.Yellow;
		case UIColor.Green:
			return this.Green;
		case UIColor.Purple:
			return this.Purple;
		case UIColor.Pink:
			return this.Pink;
		case UIColor.Grey:
			return this.Grey;
		}
		goto IL_27;
	}
}
