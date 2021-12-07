// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

public static class LayoutGroupExtensions
{
	public static void Redraw(this LayoutGroup target)
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(target.GetComponent<RectTransform>());
	}
}
