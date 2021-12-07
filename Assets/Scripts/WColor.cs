// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class WColor
{
	public delegate float Interpolator(float a, float b, float frac);

	public static Color WDOrange = new Color(1f, 0.258823544f, 0f);

	public static Color WDBlack = new Color(0.113725491f, 0.113725491f, 0.113725491f);

	public static Color WDWhite = new Color(0.8392157f, 0.8392157f, 0.8392157f);

	public static Color WDGreen = new Color(0.03137255f, 0.6117647f, 0.168627456f);

	public static Color WDBlue = new Color(0.129411772f, 0.384313732f, 0.894117653f);

	public static Color UIBlue = new Color(0.211764708f, 0.5411765f, 0.807843149f);

	public static Color UIRed = new Color(0.7411765f, 0.152941182f, 0.13333334f);

	public static Color UIYellow = new Color(0.9019608f, 0.6f, 0.168627456f);

	public static Color UIGreen = new Color(0.0196078438f, 0.5803922f, 0.08627451f);

	public static Color UIPurple = new Color(0.58431375f, 0.09803922f, 0.5254902f);

	public static Color UIPink = new Color(0.917647064f, 0.101960786f, 0.607843161f);

	public static Color UIGrey = new Color(0.6784314f, 0.6666667f, 0.6666667f);

	public static Color UILightBlue = new Color(0.572549045f, 0.7921569f, 0.9764706f);

	public static Color UILightRed = new Color(0.9647059f, 0.478431374f, 0.466666669f);

	public static Color UILightYellow = new Color(0.9764706f, 0.854901969f, 0.5294118f);

	public static Color UILightGreen = new Color(0.6f, 0.8901961f, 0.6f);

	public static Color DebugHitboxColor_Grab = new Color(0f, 0.392156869f, 1f, 0.5f);

	public static Color DebugHitboxColor_Hit = new Color(1f, 0f, 0f, 0.5f);

	public static Color DebugHitboxColor_Projectile = new Color(0.5882353f, 0.5882353f, 0f, 0.5f);

	public static Color DebugHitboxColor_Counter = new Color(0f, 0.5882353f, 0.5882353f, 0.5f);

	public static Color DebugHitboxColor_SelfHit = new Color(0.5882353f, 0.294117659f, 0f, 0.5f);

	public static Color DebugHurtboxColor = new Color(0f, 1f, 0f, 0.5f);

	public static Color DebugHurtboxProjectileImmuneColor = new Color(0.58431375f, 0.09803922f, 0.5254902f, 0.5f);

	public static Color DebugShieldColor = new Color(0.392156869f, 0f, 0.784313738f, 0.5f);

	public static Color DebugHitboxOrange = new Color(1f, 0.258823544f, 0f, 0.5f);

	public static Color DebugHitboxYellow = new Color(0.8352941f, 0.7490196f, 0.0117647061f, 0.5f);

	public static Color DebugHitboxRed = new Color(1f, 0f, 0f, 0.5f);

	public static Color DebugAnimationBoundsOrange = new Color(1f, 0.258823544f, 0f, 0.5f);

	public static float VisibleAlpha = 1f;

	public static float BlockerAlpha = 0.4f;

	public static float HiddenAlpha = 0f;

	public static Color RawTrajectoryColor = Color.yellow;

	public static Color RotatedTrajectoryColor = Color.green;

	public static Color NullColor = new Color(0f, 0f, 0f, 0f);

	public static Color Interpolate(Color start, Color end, float frac, WColor.Interpolator interpolator = null)
	{
		return Color.Lerp(start, end, frac);
	}

	public static Color FromBytes(byte r, byte g, byte b, byte a = 255)
	{
		return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
	}
}
