// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CrewBattlePlayerComponent : CharacterComponent, ICharacterComponent, IEngagementStateListener, IMoveRequirementValidator
{
	private GameObject effectPrefab;

	private ICharacterRenderer renderer;

	private IGameVFX vfx;

	public override void Init(IPlayerDelegate playerDelegate)
	{
		this.effectPrefab = null;
		base.Init(playerDelegate);
		this.renderer = playerDelegate.Renderer;
		this.vfx = playerDelegate.GameVFX;
	}

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
}
