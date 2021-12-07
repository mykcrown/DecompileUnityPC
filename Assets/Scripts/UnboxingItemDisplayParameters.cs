// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class UnboxingItemDisplayParameters
{
	public enum Align
	{
		CENTER,
		BOTTOM
	}

	public EquipmentTypes itemType;

	public UnboxingItemDisplayParameters.Align alignment;

	public bool autoPlay;

	public Vector3 position;

	public Vector3 rotation;

	public Vector3 scale = new Vector3(1f, 1f, 1f);
}
