using System;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
public class GustShieldComponent : MoveComponent, IMoveRequirementValidator, ICloneable
{
	// Token: 0x06001B3E RID: 6974 RVA: 0x0008AF71 File Offset: 0x00089371
	public object Clone()
	{
		return base.MemberwiseClone();
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x0008AF79 File Offset: 0x00089379
	public override bool ValidateRequirements(MoveData move, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.shield = playerDelegate.Shield;
		return this.shield.CanBeginGusting;
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x0008AF92 File Offset: 0x00089392
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.shield = playerDelegate.Shield;
		if (!this.shield.CanBeginGusting)
		{
			Debug.LogError("GustShieldComponent was initialized, but it is illegal to begin gusting now!");
		}
		else
		{
			this.shield.BeginGusting();
		}
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x0008AFCA File Offset: 0x000893CA
	public override void RegisterPreload(PreloadContext context)
	{
		base.RegisterPreload(context);
		if (this.data.gustParticle != null)
		{
			this.data.gustParticle.RegisterPreload(context);
		}
	}

	// Token: 0x040014AA RID: 5290
	public GustShieldData data;

	// Token: 0x040014AB RID: 5291
	private IShield shield;
}
