using System;
using UnityEngine;
using Xft;

// Token: 0x02000022 RID: 34
public class XWeaponTrailDemo : MonoBehaviour
{
	// Token: 0x06000141 RID: 321 RVA: 0x0000C19A File Offset: 0x0000A59A
	public void Start()
	{
		this.ProTrailDistort.Init();
		this.ProTrailShort.Init();
		this.ProTraillong.Init();
		this.SimpleTrail.Init();
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000C1C8 File Offset: 0x0000A5C8
	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 150f, 30f), "Activate Trail1"))
		{
			this.ProTrailDistort.Deactivate();
			this.ProTrailShort.Deactivate();
			this.ProTraillong.Deactivate();
			this.SwordAnimation.Play();
			this.SimpleTrail.Activate();
		}
		if (GUI.Button(new Rect(0f, 30f, 150f, 30f), "Stop Trail1"))
		{
			this.SimpleTrail.Deactivate();
		}
		if (GUI.Button(new Rect(0f, 60f, 150f, 30f), "Stop Trail1 Smoothly"))
		{
			this.SimpleTrail.StopSmoothly(20);
		}
		if (GUI.Button(new Rect(0f, 120f, 150f, 30f), "Activate Trail2"))
		{
			this.SimpleTrail.Deactivate();
			this.SwordAnimation.Play();
			this.ProTrailDistort.Activate();
			this.ProTrailShort.Activate();
			this.ProTraillong.Activate();
		}
		if (GUI.Button(new Rect(0f, 150f, 150f, 30f), "Stop Trail2"))
		{
			this.ProTrailDistort.Deactivate();
			this.ProTrailShort.Deactivate();
			this.ProTraillong.Deactivate();
		}
		if (GUI.Button(new Rect(0f, 180f, 150f, 30f), "Stop Trail2 Smoothly"))
		{
			this.ProTrailDistort.StopSmoothly(20);
			this.ProTrailShort.StopSmoothly(20);
			this.ProTraillong.StopSmoothly(20);
		}
	}

	// Token: 0x04000107 RID: 263
	public Animation SwordAnimation;

	// Token: 0x04000108 RID: 264
	public XWeaponTrail ProTrailDistort;

	// Token: 0x04000109 RID: 265
	public XWeaponTrail ProTrailShort;

	// Token: 0x0400010A RID: 266
	public XWeaponTrail ProTraillong;

	// Token: 0x0400010B RID: 267
	public XWeaponTrail SimpleTrail;
}
