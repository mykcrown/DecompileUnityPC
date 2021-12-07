// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputConfigData : ScriptableObject
{
	public int tapFrames = 3;

	public int doubleTapFrameThreshold = 30;

	public float tapThreshold = 0.9f;

	public Fixed tiltModifierInputMagnitude = (Fixed)0.60000002384185791;

	public Fixed specialReverseThreshold = (Fixed)0.40000000596046448;

	public int cardinalSnapAngle;

	public int diagonalSnapAngle;

	public WalkConfig walkOptions = new WalkConfig();

	public int inputBufferFrames = 5;

	public int inputBufferCollationFrames = 7;

	public int inputBufferReverseFrames = 7;

	public int inputShorthopFrames = 5;

	public Fixed fallThroughPlatformsThreshold = (Fixed)0.800000011920929;

	public int fallThroughPlatformsMaxAngle = 30;

	public int pivotJumpFrames = 4;

	public bool enableXInput;

	public bool enableVibrate;

	public InputProfileMap inputProfileMap;

	public List<DefaultInputBinding> defaultBindings = new List<DefaultInputBinding>();

	public List<DefaultInputBinding> defaultGCBindings = new List<DefaultInputBinding>();
}
