using System;
using UnityEngine;

// Token: 0x02000B7C RID: 2940
public class WColor
{
	// Token: 0x060054EB RID: 21739 RVA: 0x001B3E89 File Offset: 0x001B2289
	public static Color Interpolate(Color start, Color end, float frac, WColor.Interpolator interpolator = null)
	{
		return Color.Lerp(start, end, frac);
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x001B3E93 File Offset: 0x001B2293
	public static Color FromBytes(byte r, byte g, byte b, byte a = 255)
	{
		return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
	}

	// Token: 0x040035D5 RID: 13781
	public static Color WDOrange = new Color(1f, 0.25882354f, 0f);

	// Token: 0x040035D6 RID: 13782
	public static Color WDBlack = new Color(0.11372549f, 0.11372549f, 0.11372549f);

	// Token: 0x040035D7 RID: 13783
	public static Color WDWhite = new Color(0.8392157f, 0.8392157f, 0.8392157f);

	// Token: 0x040035D8 RID: 13784
	public static Color WDGreen = new Color(0.03137255f, 0.6117647f, 0.16862746f);

	// Token: 0x040035D9 RID: 13785
	public static Color WDBlue = new Color(0.12941177f, 0.38431373f, 0.89411765f);

	// Token: 0x040035DA RID: 13786
	public static Color UIBlue = new Color(0.21176471f, 0.5411765f, 0.80784315f);

	// Token: 0x040035DB RID: 13787
	public static Color UIRed = new Color(0.7411765f, 0.15294118f, 0.13333334f);

	// Token: 0x040035DC RID: 13788
	public static Color UIYellow = new Color(0.9019608f, 0.6f, 0.16862746f);

	// Token: 0x040035DD RID: 13789
	public static Color UIGreen = new Color(0.019607844f, 0.5803922f, 0.08627451f);

	// Token: 0x040035DE RID: 13790
	public static Color UIPurple = new Color(0.58431375f, 0.09803922f, 0.5254902f);

	// Token: 0x040035DF RID: 13791
	public static Color UIPink = new Color(0.91764706f, 0.101960786f, 0.60784316f);

	// Token: 0x040035E0 RID: 13792
	public static Color UIGrey = new Color(0.6784314f, 0.6666667f, 0.6666667f);

	// Token: 0x040035E1 RID: 13793
	public static Color UILightBlue = new Color(0.57254905f, 0.7921569f, 0.9764706f);

	// Token: 0x040035E2 RID: 13794
	public static Color UILightRed = new Color(0.9647059f, 0.47843137f, 0.46666667f);

	// Token: 0x040035E3 RID: 13795
	public static Color UILightYellow = new Color(0.9764706f, 0.85490197f, 0.5294118f);

	// Token: 0x040035E4 RID: 13796
	public static Color UILightGreen = new Color(0.6f, 0.8901961f, 0.6f);

	// Token: 0x040035E5 RID: 13797
	public static Color DebugHitboxColor_Grab = new Color(0f, 0.39215687f, 1f, 0.5f);

	// Token: 0x040035E6 RID: 13798
	public static Color DebugHitboxColor_Hit = new Color(1f, 0f, 0f, 0.5f);

	// Token: 0x040035E7 RID: 13799
	public static Color DebugHitboxColor_Projectile = new Color(0.5882353f, 0.5882353f, 0f, 0.5f);

	// Token: 0x040035E8 RID: 13800
	public static Color DebugHitboxColor_Counter = new Color(0f, 0.5882353f, 0.5882353f, 0.5f);

	// Token: 0x040035E9 RID: 13801
	public static Color DebugHitboxColor_SelfHit = new Color(0.5882353f, 0.29411766f, 0f, 0.5f);

	// Token: 0x040035EA RID: 13802
	public static Color DebugHurtboxColor = new Color(0f, 1f, 0f, 0.5f);

	// Token: 0x040035EB RID: 13803
	public static Color DebugHurtboxProjectileImmuneColor = new Color(0.58431375f, 0.09803922f, 0.5254902f, 0.5f);

	// Token: 0x040035EC RID: 13804
	public static Color DebugShieldColor = new Color(0.39215687f, 0f, 0.78431374f, 0.5f);

	// Token: 0x040035ED RID: 13805
	public static Color DebugHitboxOrange = new Color(1f, 0.25882354f, 0f, 0.5f);

	// Token: 0x040035EE RID: 13806
	public static Color DebugHitboxYellow = new Color(0.8352941f, 0.7490196f, 0.011764706f, 0.5f);

	// Token: 0x040035EF RID: 13807
	public static Color DebugHitboxRed = new Color(1f, 0f, 0f, 0.5f);

	// Token: 0x040035F0 RID: 13808
	public static Color DebugAnimationBoundsOrange = new Color(1f, 0.25882354f, 0f, 0.5f);

	// Token: 0x040035F1 RID: 13809
	public static float VisibleAlpha = 1f;

	// Token: 0x040035F2 RID: 13810
	public static float BlockerAlpha = 0.4f;

	// Token: 0x040035F3 RID: 13811
	public static float HiddenAlpha = 0f;

	// Token: 0x040035F4 RID: 13812
	public static Color RawTrajectoryColor = Color.yellow;

	// Token: 0x040035F5 RID: 13813
	public static Color RotatedTrajectoryColor = Color.green;

	// Token: 0x040035F6 RID: 13814
	public static Color NullColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x02000B7D RID: 2941
	// (Invoke) Token: 0x060054EF RID: 21743
	public delegate float Interpolator(float a, float b, float frac);
}
