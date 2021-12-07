// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

[Serializable]
public class WeaponTrailData
{
	public int startFrame;

	public int endFrame = 1;

	public BodyPart bodyPart = BodyPart.leftHand;

	public BodyPart bodyPart2;

	public int frameLength = 10;

	public int granularity = 60;

	public int fadeFrames = 6;

	public Material overrideMaterial;

	public Color color = Color.white;

	public bool useOffsets;

	public Fixed bodyPartOffset1 = 0;

	public Fixed bodyPartOffset2 = 0;
}
