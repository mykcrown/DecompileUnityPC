using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public class TotemDuoLaunchComponent : MoveComponent, IMoveForceInputDirectionReader, IMoveTickMoveFrameComponent
{
	// Token: 0x06001B7C RID: 7036 RVA: 0x0008B554 File Offset: 0x00089954
	public override void RegisterPreload(PreloadContext context)
	{
		base.RegisterPreload(context);
		if (this.totemDuoComponent != null)
		{
			this.totemDuoComponent.RegisterPreload(context);
		}
		context.Register(new PreloadDef(this.PullParticlePrefab, PreloadType.EFFECT), 4);
		context.Register(new PreloadDef(this.PushParticlePrefab, PreloadType.EFFECT), 4);
		context.Register(new PreloadDef(this.PullTetherEffect.gameObject, PreloadType.EFFECT), 4);
		context.Register(new PreloadDef(this.PushTetherEffect.gameObject, PreloadType.EFFECT), 4);
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x0008B5DC File Offset: 0x000899DC
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		for (int i = 0; i < playerDelegate.Components.Count; i++)
		{
			ICharacterComponent characterComponent = playerDelegate.Components[i];
			if (characterComponent is TotemDuoComponent)
			{
				this.totemDuoComponent = (characterComponent as TotemDuoComponent);
				break;
			}
		}
		if (this.totemDuoComponent == null)
		{
			Debug.LogWarning("TotemDuoLaunchComponent failed to find totem duo component");
		}
		switch (input.facing)
		{
		case HorizontalDirection.Left:
			if (this.totemDuoComponent.MyState.partner.Transform.position.x > playerDelegate.Transform.position.x)
			{
				this.inputDirection = -1;
			}
			return;
		case HorizontalDirection.Right:
			if (this.totemDuoComponent.MyState.partner.Transform.position.x < playerDelegate.Transform.position.x)
			{
				this.inputDirection = -1;
			}
			return;
		}
		this.inputDirection = 0;
		Debug.LogWarning("Direction is invalid!");
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x0008B718 File Offset: 0x00089B18
	public bool ReadInputDirection(ref Vector2F input)
	{
		if (this.totemDuoComponent == null || this.inputDirection == 0)
		{
			return false;
		}
		input = (Vector2F)(this.totemDuoComponent.MyState.partner.Transform.position - this.playerDelegate.Transform.position).normalized;
		input *= this.inputDirection;
		return true;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x0008B7A4 File Offset: 0x00089BA4
	public void TickMoveFrame(InputButtonsData input)
	{
		if (this.moveDelegate.Model.internalFrame == this.particleCreationFrame)
		{
			ParticleData particleData = new ParticleData();
			particleData.bodyPart = BodyPart.upperTorso;
			particleData.frames = this.particleLifespanFrames;
			particleData.prefab = ((this.inputDirection != 1) ? this.PushParticlePrefab : this.PullParticlePrefab);
			particleData.attach = true;
			this.totemDuoComponent.MyState.partner.GameVFX.PlayParticle(particleData, false, TeamNum.None);
			ParticleData particleData2 = new ParticleData();
			particleData2.bodyPart = BodyPart.upperTorso;
			particleData2.frames = this.particleLifespanFrames;
			particleData2.prefab = ((this.inputDirection != 1) ? this.PushTetherEffect.gameObject : this.PullTetherEffect.gameObject);
			particleData2.attach = false;
			GeneratedEffect generatedEffect = this.totemDuoComponent.MyState.partner.GameVFX.PlayParticle(particleData2, false, TeamNum.None);
			if (generatedEffect != null)
			{
				VFXLineRenderer componentInChildren = generatedEffect.EffectObject.GetComponentInChildren<VFXLineRenderer>();
				if (componentInChildren != null)
				{
					componentInChildren.SetTargets(this.playerDelegate.Transform, this.totemDuoComponent.MyState.partner.Transform);
				}
			}
		}
	}

	// Token: 0x040014BE RID: 5310
	public TotemDuoComponent totemDuoComponent;

	// Token: 0x040014BF RID: 5311
	public GameObject PullParticlePrefab;

	// Token: 0x040014C0 RID: 5312
	public GameObject PushParticlePrefab;

	// Token: 0x040014C1 RID: 5313
	public int particleLifespanFrames;

	// Token: 0x040014C2 RID: 5314
	public int particleCreationFrame;

	// Token: 0x040014C3 RID: 5315
	public VFXLineRenderer PullTetherEffect;

	// Token: 0x040014C4 RID: 5316
	public VFXLineRenderer PushTetherEffect;

	// Token: 0x040014C5 RID: 5317
	private int inputDirection = 1;
}
