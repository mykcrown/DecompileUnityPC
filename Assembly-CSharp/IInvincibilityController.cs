using System;

// Token: 0x020005D6 RID: 1494
public interface IInvincibilityController : IRollbackStateOwner, ITickable
{
	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x06002196 RID: 8598
	bool IsInvincible { get; }

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x06002197 RID: 8599
	bool IsProjectileInvincible { get; }

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06002198 RID: 8600
	bool IsFullyIntangible { get; }

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x06002199 RID: 8601
	int InvincibilityFramesRemaining { get; }

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x0600219A RID: 8602
	int ProjectileInvincibilityFramesRemaining { get; }

	// Token: 0x0600219B RID: 8603
	void BeginInvincibility(int frames);

	// Token: 0x0600219C RID: 8604
	void BeginLedgeIntangibility(int frames);

	// Token: 0x0600219D RID: 8605
	void BeginGrabIntangibility(int frames);

	// Token: 0x0600219E RID: 8606
	void BeginPlatformIntangibility();

	// Token: 0x0600219F RID: 8607
	void EndPlatformIntangibility();

	// Token: 0x060021A0 RID: 8608
	void BeginMoveIntangibility(BodyPart[] parts = null);

	// Token: 0x060021A1 RID: 8609
	void EndMoveIntangibility();

	// Token: 0x060021A2 RID: 8610
	void EndGrabInvincibility();

	// Token: 0x060021A3 RID: 8611
	void BeginMoveProjectileIntangibility(BodyPart[] parts = null);

	// Token: 0x060021A4 RID: 8612
	void EndMoveProjectileIntangibility();

	// Token: 0x060021A5 RID: 8613
	void EndAllMoveIntangibility();

	// Token: 0x060021A6 RID: 8614
	void Clear();
}
