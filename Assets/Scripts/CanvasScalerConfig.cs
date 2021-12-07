// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CanvasScalerConfig
{
	public float defaultSpriteDPI = 96f;

	public float fallbackScreenDPI = 96f;

	public float matchWidthOrHeight;

	public CanvasScaler.Unit physicalUnit = CanvasScaler.Unit.Points;

	public float referencePixelsPerUnit = 100f;

	public Vector2 referenceResolution = new Vector2(1920f, 1080f);

	public float scaleFactor = 1f;

	public CanvasScaler.ScreenMatchMode screenMatchMode;

	public CanvasScaler.ScaleMode scaleMode;
}
