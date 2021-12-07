// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class MaterialChangeData
{
	public int startFrame;

	public int lerpFrames = 10;

	public Material targetMaterial;

	public bool restoreDefaultMaterial;
}
