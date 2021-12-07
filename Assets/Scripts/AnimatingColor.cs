// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class AnimatingColor
{
	public enum Method
	{
		LOOP,
		YOYO
	}

	public Color colorStart;

	public bool animateColor;

	public AnimatingColor.Method method;

	public Color colorEnd;

	public int frames;
}
