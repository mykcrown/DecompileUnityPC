// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

internal class PortraitDefinition
{
	public PortraitMode mode;

	public PortraitBgMode bgMode;

	public Texture2D texture;

	public PortraitDefinition(PortraitMode mode, PortraitBgMode bgMode)
	{
		this.mode = mode;
		this.bgMode = bgMode;
	}
}
