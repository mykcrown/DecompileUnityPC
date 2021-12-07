// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class UIColorContainer
{
	public Color Red;

	public Color Yellow;

	public Color Green;

	public Color Purple;

	public Color Blue;

	public Color Pink;

	public Color Grey;

	public Color GetColor(UIColor color)
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
