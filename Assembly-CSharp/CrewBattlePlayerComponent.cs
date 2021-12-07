using System;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class CrewBattlePlayerComponent : CharacterComponent, ICharacterComponent, IEngagementStateListener, IMoveRequirementValidator
{
	// Token: 0x06001A0E RID: 6670 RVA: 0x00086D39 File Offset: 0x00085139
	public override void Init(IPlayerDelegate playerDelegate)
	{
		this.effectPrefab = null;
		base.Init(playerDelegate);
		this.renderer = playerDelegate.Renderer;
		this.vfx = playerDelegate.GameVFX;
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x00086D64 File Offset: 0x00085164
	public void OnEngagementStateChanged(PlayerEngagementState state)
	{
		if (this.effectPrefab == null)
		{
			return;
		}
		if (state == PlayerEngagementState.Temporary)
		{
			this.renderer.AddMaterialEmitter(this.effectPrefab, this.vfx);
		}
		else
		{
			this.renderer.ClearMaterialEmitter(this.effectPrefab);
		}
	}

	// Token: 0x04001379 RID: 4985
	private GameObject effectPrefab;

	// Token: 0x0400137A RID: 4986
	private ICharacterRenderer renderer;

	// Token: 0x0400137B RID: 4987
	private IGameVFX vfx;
}
