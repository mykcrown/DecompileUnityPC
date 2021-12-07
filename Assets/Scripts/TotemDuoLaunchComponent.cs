// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class TotemDuoLaunchComponent : MoveComponent, IMoveForceInputDirectionReader, IMoveTickMoveFrameComponent
{
	public TotemDuoComponent totemDuoComponent;

	public GameObject PullParticlePrefab;

	public GameObject PushParticlePrefab;

	public int particleLifespanFrames;

	public int particleCreationFrame;

	public VFXLineRenderer PullTetherEffect;

	public VFXLineRenderer PushTetherEffect;

	private int inputDirection = 1;

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
			UnityEngine.Debug.LogWarning("TotemDuoLaunchComponent failed to find totem duo component");
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
		UnityEngine.Debug.LogWarning("Direction is invalid!");
	}

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
}
