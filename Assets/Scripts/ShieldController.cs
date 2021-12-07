// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : GameBehavior, IShield, IRollbackStateOwner, IDestroyable
{
	private ShieldConfig data;

	private GustShieldData gustData;

	private ShieldModel model;

	private Renderer shieldRenderer;

	private Material[] shieldMaterials;

	private IPlayerDelegate player;

	private IFrameOwner frameOwner;

	private List<Hit> shieldHits = new List<Hit>();

	private MoveData gustMove;

	private float displayOffset;

	private CapsuleShape capsuleHurtBox;

	ShieldConfig IShield.Data
	{
		get
		{
			return this.data;
		}
	}

	GustShieldData IShield.GustData
	{
		get
		{
			return this.gustData;
		}
	}

	[Inject]
	public ICombatCalculator combatCalculator
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	public Fixed ShieldHealth
	{
		get
		{
			return this.model.shieldHealth;
		}
		set
		{
			this.model.shieldHealth = value;
		}
	}

	private GameObject shield
	{
		get
		{
			return this.model.shield;
		}
		set
		{
			this.model.shield = value;
		}
	}

	public bool IsActive
	{
		get
		{
			return this.model.shieldActive;
		}
	}

	public bool IsBroken
	{
		get
		{
			return this.ShieldHealth <= 0;
		}
	}

	public int ShieldBeginFrame
	{
		get
		{
			return this.model.shieldBeginFrame;
		}
	}

	public bool WasRunning
	{
		get
		{
			return this.model.wasRunningBeforeShield;
		}
	}

	public List<Hit> ShieldHits
	{
		get
		{
			return this.shieldHits;
		}
	}

	public Vector3F ShieldPosition
	{
		get
		{
			return this.model.position;
		}
	}

	private Fixed maxGustShieldSize
	{
		get
		{
			if (this.gustData.useFixedGustShieldSize)
			{
				return this.gustData.fixedGustShieldRadius;
			}
			return this.gustData.maxSizeMultiplier * this.calculateShieldRadius((!this.gustData.gustSizeDependentOnHealth) ? this.data.maxShieldHealth : this.model.shieldHealth, this.gustData.expandFrames);
		}
	}

	private Fixed currentGustShieldCost
	{
		get
		{
			return this.gustData.shieldHealthCostFlatValue + this.gustData.shieldHealthCostOfMaxPercent * this.data.maxShieldHealth + this.gustData.shieldHealthCostOfCurrentPercent * this.model.shieldHealth;
		}
	}

	private Fixed gustShieldUseThreshold
	{
		get
		{
			return this.gustData.gustUsableThresholdValue + this.gustData.gustUsableThresholdPercent * this.data.maxShieldHealth;
		}
	}

	private Fixed shieldHealthAfterGusting
	{
		get
		{
			if (this.gustData.subtractHealthOnUse)
			{
				return this.model.shieldHealth - this.currentGustShieldCost;
			}
			return this.data.maxShieldHealth * this.gustData.shieldHealthAfterUsePercentOfMax + this.gustData.shieldHealthAfterUseFlatValue;
		}
	}

	public bool IsGusting
	{
		get
		{
			return this.model.isGusting;
		}
	}

	public bool GustSuccess
	{
		get
		{
			return this.model.isGustSuccessful;
		}
	}

	public Fixed ShieldPercentage
	{
		get
		{
			return this.model.shieldHealth / base.config.shieldConfig.maxShieldHealth;
		}
	}

	public Fixed Radius
	{
		get
		{
			Fixed result = 0;
			if (this.model.shieldActive)
			{
				if (this.IsGusting)
				{
					int shieldExpandFrames = this.data.shieldExpandFrames;
					Fixed startRadius = this.calculateShieldRadius(this.model.shieldHealth, shieldExpandFrames);
					Fixed maxGustShieldSize = this.maxGustShieldSize;
					Fixed endRadius = this.calculateShieldRadius(this.shieldHealthAfterGusting, shieldExpandFrames);
					result = this.getGustShieldRadius(this.frameOwner.Frame - this.model.gustBeginFrame, startRadius, maxGustShieldSize, endRadius);
				}
				else
				{
					int framesIntoExpand = this.frameOwner.Frame - this.model.shieldBeginFrame;
					result = this.calculateShieldRadius(this.model.shieldHealth, framesIntoExpand);
				}
			}
			return result;
		}
	}

	public bool CanBeginGusting
	{
		get
		{
			if (this.gustData.subtractHealthOnUse)
			{
				return !this.IsGusting && (this.gustData.allowGustShieldBreak || this.model.shieldHealth > this.currentGustShieldCost) && this.model.shieldHealth > this.gustShieldUseThreshold;
			}
			return !this.IsGusting && this.model.shieldHealth > this.shieldHealthAfterGusting && this.model.shieldHealth > this.gustShieldUseThreshold;
		}
	}

	public void Initialize(IPlayerDelegate player, ShieldConfig data, MoveData[] gustShieldMoves, IFrameOwner frameOwner)
	{
		GustShieldComponent gustShieldComponent = null;
		for (int i = 0; i < gustShieldMoves.Length; i++)
		{
			MoveData moveData = gustShieldMoves[i];
			gustShieldComponent = moveData.GetComponent<GustShieldComponent>();
			if (gustShieldComponent != null)
			{
				this.gustMove = moveData;
				break;
			}
		}
		if (gustShieldComponent == null)
		{
			UnityEngine.Debug.LogError("No GustShield labeled move contains a GustShieldComponent");
		}
		this.data = data;
		this.gustData = gustShieldComponent.data;
		this.player = player;
		this.frameOwner = frameOwner;
		this.model = new ShieldModel();
		this.model.shieldHealth = data.maxShieldHealth;
		this.shield = UnityEngine.Object.Instantiate<GameObject>(data.shieldPrefab);
		this.shield.SetActive(false);
		this.shield.transform.localScale = Vector3.one;
		this.displayOffset = this.shield.transform.GetChild(0).localPosition.z;
		if (this.shield.transform.childCount != 1)
		{
			UnityEngine.Debug.LogError("Unhandled number of shield children");
		}
		this.shieldRenderer = this.shield.GetComponentInChildren<Renderer>();
		this.shieldRenderer.material.color = data.normalShieldColor;
		this.shieldMaterials = this.shieldRenderer.materials;
		Vector3F shieldOffset = player.CharacterData.shield.shieldOffset;
		shieldOffset.x *= InputUtils.GetDirectionMultiplier(player.Facing);
		this.model.position = player.Physics.State.position + shieldOffset;
		HitBox hitBox = new HitBox();
		hitBox.radius = (float)this.Radius;
		hitBox.bodyPart = BodyPart.shield;
		HitData hitData = new HitData();
		hitData.hitBoxes = new List<HitBox>
		{
			hitBox
		};
		this.model.normalHit = new Hit(hitData);
		this.model.normalHit.hitBoxes[0].position = this.model.position;
		HitData hitData2 = new HitData();
		hitData2.hitBoxes = new List<HitBox>
		{
			hitBox
		};
		this.model.gustHit_ground = new Hit(hitData2);
		this.model.gustHit_ground.data.conditionType = HitConditionType.GroundOnly;
		hitData2 = new HitData();
		hitData2.hitBoxes = new List<HitBox>
		{
			hitBox
		};
		this.model.gustHit_air = new Hit(hitData2);
		this.model.gustHit_air.data.conditionType = HitConditionType.AirOnly;
		this.LoadShieldData(data);
		this.model.activeShieldHits.Clear();
		this.model.activeShieldHits.Add(ActiveShieldHitType.normal);
		this.RebuildShieldHits();
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HurtBoxes))
		{
			this.toggleHurtBoxCapsules(true);
		}
		if (base.events != null)
		{
			base.events.Subscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugDrawChannel));
		}
	}

	void IDestroyable.Destroy()
	{
		Material[] array = this.shieldMaterials;
		for (int i = 0; i < array.Length; i++)
		{
			Material obj = array[i];
			UnityEngine.Object.DestroyImmediate(obj);
		}
		if (base.events != null)
		{
			base.events.Unsubscribe(typeof(ToggleDebugDrawChannelCommand), new Events.EventHandler(this.onToggleDebugDrawChannel));
		}
		if (this.capsuleHurtBox != null)
		{
			this.capsuleHurtBox.Clear();
			this.capsuleHurtBox = null;
		}
	}

	private void RebuildShieldHits()
	{
		this.shieldHits.Clear();
		foreach (ActiveShieldHitType current in this.model.activeShieldHits)
		{
			if (current != ActiveShieldHitType.normal)
			{
				if (current != ActiveShieldHitType.gustHit_ground)
				{
					if (current == ActiveShieldHitType.gustHit_air)
					{
						this.shieldHits.Add(this.model.gustHit_air);
					}
				}
				else
				{
					this.shieldHits.Add(this.model.gustHit_ground);
				}
			}
			else
			{
				this.shieldHits.Add(this.model.normalHit);
			}
		}
	}

	public void LoadShieldData(ShieldConfig shieldData)
	{
		this.data = shieldData;
		this.setHitData(this.model.gustHit_air.data, true);
		this.setHitData(this.model.gustHit_ground.data, false);
	}

	private void setHitData(HitData gustHitData, bool isAir)
	{
		gustHitData.knockbackAngle = (float)((!isAir) ? this.gustData.gustKnockbackAngleClamp_ground : this.gustData.gustKnockbackAngleClamp_air);
		gustHitData.baseKnockback = ((!isAir) ? ((float)this.gustData.gustKnockback_ground) : ((float)this.gustData.gustKnockback_air));
		gustHitData.knockbackCausesFlinching = ((!isAir) ? this.gustData.causesFlinching_ground : this.gustData.causesFlinching_air);
		gustHitData.resetXVelocity = ((!isAir) ? this.gustData.resetsXVelocity_ground : this.gustData.resetsXVelocity_air);
		gustHitData.resetYVelocity = ((!isAir) ? this.gustData.resetsYVelocity_ground : this.gustData.resetsYVelocity_air);
		gustHitData.enableReverseHitboxes = true;
		gustHitData.ignoreWeight = this.gustData.gustShieldIgnoresWeight;
		gustHitData.reflectsProjectiles = true;
		gustHitData.disableType = HitDisableType.AlwaysForVictim;
		gustHitData.preventHelplessness = true;
		gustHitData.hitType = HitType.Gust;
	}

	public void TickFrame(InputButtonsData input)
	{
		if (this.shield == null)
		{
			return;
		}
		if (this.IsGusting && this.frameOwner.Frame - this.model.gustBeginFrame >= this.gustData.TotalActiveFrames)
		{
			this.model.isGusting = false;
			this.OnEndGust(input);
		}
		this.model.shieldHealth = FixedMath.Clamp(this.ShieldHealth, 0, this.data.maxShieldHealth);
		if (this.model.shieldActive)
		{
			this.drainHealth();
		}
		else if (!this.IsBroken)
		{
			this.gainHealth();
		}
		this.model.rotationAngle += ((!this.IsGusting) ? this.data.shieldRotationSpeed : this.data.shieldRotationSpeed) * WTime.fixedDeltaTime;
		Fixed src = (Fixed)1.0;
		Fixed other = (!this.gustData.subtractHealthOnUse) ? FixedMath.Max(this.gustShieldUseThreshold, this.shieldHealthAfterGusting) : FixedMath.Max(this.gustShieldUseThreshold, this.currentGustShieldCost);
		if (this.model.shieldHealth <= other)
		{
			src = (Fixed)1.0;
		}
		else
		{
			src = (Fixed)1.0 - (this.model.shieldHealth - other) / FixedMath.Max((Fixed)0.10000000149011612, this.data.maxShieldHealth - other);
		}
		float frac = this.data.shieldColorCurve.Evaluate((float)src);
		Color value;
		if (this.IsGusting)
		{
			value = this.getGustShieldColor(this.data.normalShieldColor, this.gustData.gustColor, this.gustData.gustColorCurve, this.frameOwner.Frame - this.model.gustBeginFrame);
		}
		else
		{
			value = WColor.Interpolate(this.data.normalShieldColor, this.data.lowShieldColor, frac, null);
		}
		for (int i = 0; i < this.shieldMaterials.Length; i++)
		{
			this.shieldMaterials[i].SetColor(this.data.shieldMaterialColorName, value);
		}
		if (!this.IsGusting || this.data.allowTiltDuringGust)
		{
			this.UpdateTiltOffset(input);
		}
		Vector3F shieldOffset = this.player.CharacterData.shield.shieldOffset;
		shieldOffset.x *= InputUtils.GetDirectionMultiplier(this.player.Facing);
		this.model.position = this.player.Physics.State.position + shieldOffset + this.model.tiltOffset;
		this.shield.transform.position = (Vector3)this.model.position;
		float num = Vector3.Distance(base.gameManager.Camera.current.transform.position, this.shield.transform.position);
		float num2 = (float)this.Radius;
		float num3 = num + this.displayOffset * num2;
		if (num3 != 0f)
		{
			this.shield.transform.localScale = Vector3.one * (num * num2) / num3;
		}
		this.updateShield();
		if (this.capsuleHurtBox)
		{
			this.capsuleHurtBox.SetPositions(this.model.position, this.model.position, true);
			this.capsuleHurtBox.Radius = num2;
		}
	}

	private void UpdateTiltOffset(InputButtonsData input)
	{
		if (input.horizontalAxisValue == 0 && input.verticalAxisValue == 0)
		{
			this.model.tiltOffset = FixedMath.MoveTowards(this.model.tiltOffset, Vector2F.zero, this.data.shieldTiltAmountPerFrame);
		}
		else
		{
			Vector2F vector2F = new Vector2F(input.horizontalAxisValue, input.verticalAxisValue);
			Vector2F normalized = vector2F.normalized;
			Vector2F v = new Vector2F(normalized.x * this.data.shieldTiltMaxDistanceX, normalized.y * this.data.shieldTiltMaxDistanceY);
			this.model.tiltOffset = FixedMath.MoveTowards(this.model.tiltOffset, v, this.data.shieldTiltAmountPerFrame);
		}
	}

	private void updateShield()
	{
		for (int i = 0; i < this.ShieldHits.Count; i++)
		{
			Hit hit = this.ShieldHits[i];
			foreach (HitBoxState current in hit.hitBoxes)
			{
				current.position = this.model.position;
				current.overrideRadius = this.Radius;
			}
		}
		if (this.model.shieldActive != this.model.shield.activeInHierarchy)
		{
			this.model.shield.SetActive(this.model.shieldActive);
		}
	}

	private void Update()
	{
		if (this.shield != null && base.gameManager != null)
		{
			this.shield.transform.LookAt(base.gameManager.Camera.current.transform);
			this.shield.transform.Rotate(base.transform.forward, (float)this.model.rotationAngle);
		}
	}

	public bool TryToGustObject(IHitOwner other)
	{
		if (!this.IsGusting)
		{
			return false;
		}
		if (other is PlayerController)
		{
			bool flag = this.tryGustEnemyPlayer(other as PlayerController);
			if (flag && this.gustData.gustPlayerTriggersSuccess)
			{
				this.model.isGustSuccessful = true;
				this.LinkSuccessGustMoves(InputButtonsData.EmptyInput);
			}
			return flag;
		}
		int num = this.frameOwner.Frame - this.model.gustBeginFrame;
		bool result = this.gustData.reflectFrameEnd <= 1 || (num >= this.gustData.reflectFrameStart && num <= this.gustData.reflectFrameEnd);
		this.model.isGustSuccessful = true;
		this.LinkSuccessGustMoves(InputButtonsData.EmptyInput);
		return result;
	}

	private bool tryGustEnemyPlayer(PlayerController other)
	{
		int num = this.frameOwner.Frame - this.model.gustBeginFrame;
		return this.gustData.knockbackFrameEnd <= 1 || (num >= this.gustData.knockbackFrameStart && num <= this.gustData.knockbackFrameEnd);
	}

	private Color getGustShieldColor(Color normalColor, Color gustColor, AnimationCurve colorCurve, int frame)
	{
		if (!this.IsGusting)
		{
			return normalColor;
		}
		if (frame < this.gustData.expandFrames)
		{
			float time = (float)frame / (float)this.gustData.expandFrames;
			float frac = colorCurve.Evaluate(time);
			return WColor.Interpolate(normalColor, gustColor, frac, null);
		}
		if (frame < this.gustData.expandFrames + this.gustData.holdFrames)
		{
			return gustColor;
		}
		if (this.gustData.shrinkFrames > 0)
		{
			int num = frame - (this.gustData.expandFrames + this.gustData.holdFrames);
			float time2 = (float)num / (float)this.gustData.shrinkFrames;
			float frac2 = colorCurve.Evaluate(time2);
			return WColor.Interpolate(gustColor, normalColor, frac2, null);
		}
		return normalColor;
	}

	private Fixed getGustShieldRadius(int frame, Fixed startRadius, Fixed maxRadius, Fixed endRadius)
	{
		if (this.IsGusting)
		{
			Fixed result;
			if (frame < this.gustData.expandFrames)
			{
				result = startRadius + frame / this.gustData.expandFrames * (maxRadius - startRadius);
			}
			else if (frame < this.gustData.expandFrames + this.gustData.holdFrames)
			{
				result = maxRadius;
			}
			else if (this.gustData.shrinkFrames > 0)
			{
				int src = frame - (this.gustData.expandFrames + this.gustData.holdFrames);
				result = maxRadius - src / this.gustData.shrinkFrames * (maxRadius - endRadius);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		return startRadius;
	}

	private void gainHealth()
	{
		this.ShieldHealth = FixedMath.Min(this.data.maxShieldHealth, this.ShieldHealth + (Fixed)1.0 / (Fixed)((double)WTime.fps) * this.data.regenerationPerSecond);
	}

	private void drainHealth()
	{
		if (this.IsGusting)
		{
			return;
		}
		this.ShieldHealth = FixedMath.Max(0, this.ShieldHealth - (Fixed)1.0 / (Fixed)((double)WTime.fps) * this.data.holdDepletionPerSecond);
	}

	public void OnHit(HitData hitData, IHitOwner other)
	{
		if (!this.gustData.gustShieldIgnoresDamage || !this.IsGusting)
		{
			Fixed one = this.combatCalculator.CalculateModifiedDamage(hitData, other);
			Fixed other2 = one * base.config.shieldConfig.shieldDamageMultiplier + hitData.bonusShieldDamage;
			this.ShieldHealth = FixedMath.Max(0, this.ShieldHealth - other2);
		}
	}

	public void ResetHealth()
	{
		this.ShieldHealth = this.data.shieldRestoreHealth;
	}

	private Fixed calculateShieldRadius(Fixed health, int framesIntoExpand)
	{
		Fixed @fixed = 0;
		Fixed fixed2 = this.data.maxShieldRadius;
		if (!this.player.CharacterData.shield.useDefaultShield)
		{
			fixed2 = (Fixed)((double)this.player.CharacterData.shield.maxShieldRadius);
		}
		Fixed other = this.data.minShieldPercent * fixed2;
		@fixed = health / this.data.maxShieldHealth * (fixed2 - other) + other;
		if (framesIntoExpand < this.data.shieldExpandFrames)
		{
			@fixed *= framesIntoExpand / this.data.shieldExpandFrames;
		}
		return @fixed;
	}

	public void BreakShield()
	{
		this.model.shieldHealth = 0;
	}

	public void OnShieldBegin(bool wasRunning)
	{
		this.model.shieldActive = true;
		this.model.shieldBeginFrame = this.frameOwner.Frame;
		this.model.wasRunningBeforeShield = wasRunning;
		this.updateShield();
	}

	public void OnShieldReleased()
	{
		this.model.shieldActive = false;
		this.updateShield();
	}

	void IShield.BeginGusting()
	{
		this.model.isGusting = true;
		this.model.isGustSuccessful = false;
		this.model.gustBeginFrame = this.frameOwner.Frame;
		if (!this.model.shieldActive)
		{
			this.player.StateActor.TryBeginShield(true);
		}
		this.model.Clear();
		this.model.gustHit_air.data.endFrame = this.gustData.TotalActiveFrames;
		this.model.gustHit_ground.data.endFrame = this.gustData.TotalActiveFrames;
		this.model.activeShieldHits.Clear();
		this.model.activeShieldHits.Add(ActiveShieldHitType.gustHit_air);
		this.model.activeShieldHits.Add(ActiveShieldHitType.gustHit_ground);
		this.RebuildShieldHits();
		this.player.GameVFX.PlayParticle(this.gustData.gustParticle, BodyPart.shield, TeamNum.None);
	}

	public void OnEndGust(InputButtonsData input)
	{
		bool isActive = this.IsActive;
		this.player.StateActor.ReleaseShield(false, true);
		this.model.shieldHealth = this.shieldHealthAfterGusting;
		this.model.activeShieldHits.Clear();
		this.model.activeShieldHits.Add(ActiveShieldHitType.normal);
		this.RebuildShieldHits();
		if (isActive)
		{
			this.LinkEndGustMoves(input);
		}
	}

	public void LinkEndGustMoves(InputButtonsData input)
	{
		if (this.gustMove == this.player.ActiveMove.Data)
		{
			InterruptData[] interrupts = this.player.ActiveMove.Data.interrupts;
			for (int i = 0; i < interrupts.Length; i++)
			{
				InterruptData interruptData = interrupts[i];
				if (interruptData.ShouldUseLink(LinkCheckType.GustUpdated, this.player, this.player.ActiveMove.Model, input))
				{
					IPlayerDelegate arg_EE_0 = this.player;
					MoveData move = interruptData.linkableMoves[0];
					HorizontalDirection inputDirection = HorizontalDirection.None;
					int uid = this.player.ActiveMove.Model.uid;
					int nextMoveStartupFrame = interruptData.nextMoveStartupFrame;
					List<MoveLinkComponentData> allLinkComponentData = this.player.ActiveMove.GetAllLinkComponentData();
					arg_EE_0.SetMove(move, input, inputDirection, uid, nextMoveStartupFrame, default(Vector3F), new MoveTransferSettings
					{
						transferHitDisableTargets = interruptData.transferHitDisabledTargets,
						transferChargeData = interruptData.transferChargeData
					}, allLinkComponentData, default(MoveSeedData), ButtonPress.None);
				}
			}
		}
	}

	public void LinkSuccessGustMoves(InputButtonsData input)
	{
		if (this.gustMove == this.player.ActiveMove.Data)
		{
			InterruptData[] interrupts = this.player.ActiveMove.Data.interrupts;
			for (int i = 0; i < interrupts.Length; i++)
			{
				InterruptData interruptData = interrupts[i];
				if (interruptData.ShouldUseLink(LinkCheckType.GustUpdated, this.player, this.player.ActiveMove.Model, input))
				{
					this.player.TryBeginMove(interruptData.linkableMoves[0], interruptData, ButtonPress.None, input);
				}
			}
		}
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ShieldModel>(ref this.model);
		this.RebuildShieldHits();
		return true;
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<ShieldModel>(this.model));
		return true;
	}

	private void onToggleDebugDrawChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		bool enabled = toggleDebugDrawChannelCommand.enabled;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HurtBoxes)
		{
			this.toggleHurtBoxCapsules(enabled);
		}
	}

	private void toggleHurtBoxCapsules(bool enabled)
	{
		if (enabled)
		{
			this.capsuleHurtBox = CapsulePool.Instance.GetCapsule(null);
			this.capsuleHurtBox.Load(this.model.position, this.model.position, this.Radius, WColor.DebugShieldColor, true);
			this.capsuleHurtBox.Visible = true;
		}
		else if (this.capsuleHurtBox != null)
		{
			this.capsuleHurtBox.Clear();
			this.capsuleHurtBox = null;
		}
	}

	private void OnDrawGizmos()
	{
		if (DebugDraw.Instance.IsChannelActive(DebugDrawChannel.HitBoxes))
		{
			if (this.IsGusting)
			{
				Gizmos.color = WColor.DebugHitboxColor_Counter;
			}
			else
			{
				Gizmos.color = WColor.DebugShieldColor;
			}
			Gizmos.DrawSphere(this.shield.transform.position, (float)this.Radius);
		}
	}
}
