using System;
using FixedPoint;
using UnityEngine;

// Token: 0x020005C4 RID: 1476
public class ChargeMoveComponent : CharacterComponent, IMoveControllerInitializer, IMoveTickListener, ITaggedParticleListener, IDeathListener, IMoveKillListener, IRollbackStateOwner, IDebugStringComponent
{
	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x060020DB RID: 8411 RVA: 0x000A4AB2 File Offset: 0x000A2EB2
	public Fixed ChargeFraction
	{
		get
		{
			return this.state.storedChargeFrames / Mathf.Max(1, this.ChargeData.maxChargeFrames);
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x060020DC RID: 8412 RVA: 0x000A4ADA File Offset: 0x000A2EDA
	public ChargeConfig ChargeData
	{
		get
		{
			return this.chargeData;
		}
	}

	// Token: 0x060020DD RID: 8413 RVA: 0x000A4AE2 File Offset: 0x000A2EE2
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ChargeMoveComponentState>(ref this.state);
		return true;
	}

	// Token: 0x060020DE RID: 8414 RVA: 0x000A4AF2 File Offset: 0x000A2EF2
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<ChargeMoveComponentState>(this.state));
		return true;
	}

	// Token: 0x060020DF RID: 8415 RVA: 0x000A4B0E File Offset: 0x000A2F0E
	public void OnDeath()
	{
		if (this.state.chargeParticle != null)
		{
			this.state.chargeParticle.DestroySafe();
			this.state.chargeParticle = null;
		}
		this.state.storedChargeFrames = 0;
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x000A4B50 File Offset: 0x000A2F50
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

	// Token: 0x060020E1 RID: 8417 RVA: 0x000A4BB8 File Offset: 0x000A2FB8
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

	// Token: 0x060020E2 RID: 8418 RVA: 0x000A4CF7 File Offset: 0x000A30F7
	public void OnCreateTaggedParticle(ParticleTag tag, GameObject particle)
	{
		if (tag == this.chargeParticleTag)
		{
			this.state.chargeParticle = particle;
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x000A4D14 File Offset: 0x000A3114
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

	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x060020E4 RID: 8420 RVA: 0x000A4E18 File Offset: 0x000A3218
	public string DebugString
	{
		get
		{
			return "Charged Value: " + this.ChargeFraction.ToString("N2");
		}
	}

	// Token: 0x04001A06 RID: 6662
	[SerializeField]
	private ChargeConfig chargeData;

	// Token: 0x04001A07 RID: 6663
	public MoveData chargeMove;

	// Token: 0x04001A08 RID: 6664
	public MoveData fireMoveGround;

	// Token: 0x04001A09 RID: 6665
	public MoveData fireMoveAir;

	// Token: 0x04001A0A RID: 6666
	public ParticleTag chargeParticleTag;

	// Token: 0x04001A0B RID: 6667
	private ChargeMoveComponentState state = new ChargeMoveComponentState();
}
