// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class ChargeMoveComponent : CharacterComponent, IMoveControllerInitializer, IMoveTickListener, ITaggedParticleListener, IDeathListener, IMoveKillListener, IRollbackStateOwner, IDebugStringComponent
{
	[SerializeField]
	private ChargeConfig chargeData;

	public MoveData chargeMove;

	public MoveData fireMoveGround;

	public MoveData fireMoveAir;

	public ParticleTag chargeParticleTag;

	private ChargeMoveComponentState state = new ChargeMoveComponentState();

	public Fixed ChargeFraction
	{
		get
		{
			return this.state.storedChargeFrames / Mathf.Max(1, this.ChargeData.maxChargeFrames);
		}
	}

	public ChargeConfig ChargeData
	{
		get
		{
			return this.chargeData;
		}
	}

	public string DebugString
	{
		get
		{
			return "Charged Value: " + this.ChargeFraction.ToString("N2");
		}
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ChargeMoveComponentState>(ref this.state);
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<ChargeMoveComponentState>(this.state));
		return true;
	}

	public void OnDeath()
	{
		if (this.state.chargeParticle != null)
		{
			this.state.chargeParticle.DestroySafe();
			this.state.chargeParticle = null;
		}
		this.state.storedChargeFrames = 0;
	}

	public void OnMoveKilled(MoveData move)
	{
		if (move.Equals(this.chargeMove))
		{
			if (this.state.chargeParticle != null && this.state.chargeParticle.activeInHierarchy)
			{
				this.state.chargeParticle.DestroySafe();
			}
			this.state.chargeParticle = null;
		}
	}

	public bool InitializeMoveController(MoveController controller, MoveModel moveModel)
	{
		if (moveModel.data.Equals(this.chargeMove) || moveModel.data.Equals(this.fireMoveGround) || moveModel.data.Equals(this.fireMoveAir))
		{
			moveModel.chargeFrames = this.state.storedChargeFrames;
			moveModel.ChargeData = this.ChargeData;
		}
		if (moveModel.data.Equals(this.chargeMove) && this.state.storedChargeFrames >= this.ChargeData.maxChargeFrames)
		{
			if (this.playerDelegate.State.IsJumpingState)
			{
				moveModel.data = this.fireMoveAir;
			}
			else
			{
				moveModel.data = this.fireMoveGround;
			}
		}
		if (moveModel.data.Equals(this.fireMoveGround) || moveModel.data.Equals(this.fireMoveAir))
		{
			this.state.storedChargeFrames = 0;
			if (this.state.chargeParticle != null)
			{
				this.state.chargeParticle.DestroySafe();
			}
		}
		controller.Initialize(moveModel, InputButtonsData.EmptyInput, false, false, null, true);
		return true;
	}

	public void OnCreateTaggedParticle(ParticleTag tag, GameObject particle)
	{
		if (tag == this.chargeParticleTag)
		{
			this.state.chargeParticle = particle;
		}
	}

	public void OnTickMove(MoveController move)
	{
		if (move == null || move.Data == null)
		{
			return;
		}
		if (move.Data.Equals(this.chargeMove))
		{
			if (this.state.chargeParticle != null)
			{
				Fixed scaledValue = this.ChargeData.GetScaledValue(this.ChargeData.maxChargeProjectileScale, this.ChargeFraction);
				this.state.chargeParticle.transform.localScale = new Vector3((float)scaledValue, (float)scaledValue, (float)scaledValue);
			}
			this.state.storedChargeFrames++;
			move.Model.chargeFrames = this.state.storedChargeFrames;
			if (this.state.storedChargeFrames >= this.ChargeData.maxChargeFrames)
			{
				this.state.storedChargeFrames = this.ChargeData.maxChargeFrames;
				this.playerDelegate.EndActiveMove(MoveEndType.Finished, true, false);
			}
		}
	}
}
