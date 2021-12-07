using System;
using System.Collections.Generic;

// Token: 0x020003C1 RID: 961
[Serializable]
public class TimedHostedHit : HostedHit, ICopyable<TimedHostedHit>, ICopyable
{
	// Token: 0x060014DD RID: 5341 RVA: 0x000736CE File Offset: 0x00071ACE
	public TimedHostedHit()
	{
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x000736D6 File Offset: 0x00071AD6
	public TimedHostedHit(int frames, HitData data, IHitOwner Host, IBodyOwner HostBody, IHitOwner Owner, Dictionary<HitBoxState, CapsuleShape> capsules, bool hostIsImmune) : base(data, Host, HostBody, Owner, capsules, hostIsImmune)
	{
		this.durationFrames = frames;
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x000736EF File Offset: 0x00071AEF
	public void CopyTo(TimedHostedHit target)
	{
		base.CopyTo(target);
		target.durationFrames = this.durationFrames;
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x00073704 File Offset: 0x00071B04
	public override object Clone()
	{
		TimedHostedHit timedHostedHit = new TimedHostedHit();
		this.CopyTo(timedHostedHit);
		return timedHostedHit;
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x0007371F File Offset: 0x00071B1F
	public override void TickFrame()
	{
		base.TickFrame();
		this.durationFrames = Math.Max(0, this.durationFrames - 1);
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0007373B File Offset: 0x00071B3B
	public override bool IsDead
	{
		get
		{
			return base.IsDead || this.durationFrames <= 0;
		}
	}

	// Token: 0x04000DCB RID: 3531
	private int durationFrames;
}
