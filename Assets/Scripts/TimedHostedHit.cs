// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

[Serializable]
public class TimedHostedHit : HostedHit, ICopyable<TimedHostedHit>, ICopyable
{
	private int durationFrames;

	public override bool IsDead
	{
		get
		{
			return base.IsDead || this.durationFrames <= 0;
		}
	}

	public TimedHostedHit()
	{
	}

	public TimedHostedHit(int frames, HitData data, IHitOwner Host, IBodyOwner HostBody, IHitOwner Owner, Dictionary<HitBoxState, CapsuleShape> capsules, bool hostIsImmune) : base(data, Host, HostBody, Owner, capsules, hostIsImmune)
	{
		this.durationFrames = frames;
	}

	public void CopyTo(TimedHostedHit target)
	{
		base.CopyTo(target);
		target.durationFrames = this.durationFrames;
	}

	public override object Clone()
	{
		TimedHostedHit timedHostedHit = new TimedHostedHit();
		this.CopyTo(timedHostedHit);
		return timedHostedHit;
	}

	public override void TickFrame()
	{
		base.TickFrame();
		this.durationFrames = Math.Max(0, this.durationFrames - 1);
	}
}
