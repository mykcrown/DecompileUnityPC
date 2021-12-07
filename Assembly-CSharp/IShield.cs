using System;
using System.Collections.Generic;
using FixedPoint;

// Token: 0x02000604 RID: 1540
public interface IShield : IRollbackStateOwner, IDestroyable
{
	// Token: 0x060025C9 RID: 9673
	void Initialize(IPlayerDelegate player, ShieldConfig data, MoveData[] gustShieldMoves, IFrameOwner frameOwner);

	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x060025CA RID: 9674
	// (set) Token: 0x060025CB RID: 9675
	Fixed ShieldHealth { get; set; }

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x060025CC RID: 9676
	bool IsActive { get; }

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x060025CD RID: 9677
	bool IsBroken { get; }

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x060025CE RID: 9678
	bool IsGusting { get; }

	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x060025CF RID: 9679
	bool GustSuccess { get; }

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x060025D0 RID: 9680
	int ShieldBeginFrame { get; }

	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x060025D1 RID: 9681
	bool WasRunning { get; }

	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x060025D2 RID: 9682
	bool CanBeginGusting { get; }

	// Token: 0x060025D3 RID: 9683
	void BeginGusting();

	// Token: 0x060025D4 RID: 9684
	void BreakShield();

	// Token: 0x060025D5 RID: 9685
	void OnShieldBegin(bool wasRunning);

	// Token: 0x060025D6 RID: 9686
	void OnShieldReleased();

	// Token: 0x060025D7 RID: 9687
	void ResetHealth();

	// Token: 0x060025D8 RID: 9688
	void OnEndGust(InputButtonsData input);

	// Token: 0x060025D9 RID: 9689
	void TickFrame(InputButtonsData input);

	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x060025DA RID: 9690
	ShieldConfig Data { get; }

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x060025DB RID: 9691
	GustShieldData GustData { get; }

	// Token: 0x060025DC RID: 9692
	bool TryToGustObject(IHitOwner other);

	// Token: 0x060025DD RID: 9693
	void OnHit(HitData data, IHitOwner other);

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x060025DE RID: 9694
	List<Hit> ShieldHits { get; }

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x060025DF RID: 9695
	Vector3F ShieldPosition { get; }

	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x060025E0 RID: 9696
	Fixed ShieldPercentage { get; }
}
