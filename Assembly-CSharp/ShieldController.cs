using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000602 RID: 1538
public class ShieldController : GameBehavior, IShield, IRollbackStateOwner, IDestroyable
{
	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06002592 RID: 9618 RVA: 0x000B9DA3 File Offset: 0x000B81A3
	// (set) Token: 0x06002593 RID: 9619 RVA: 0x000B9DAB File Offset: 0x000B81AB
	[Inject]
	public ICombatCalculator combatCalculator { get; set; }

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x06002594 RID: 9620 RVA: 0x000B9DB4 File Offset: 0x000B81B4
	// (set) Token: 0x06002595 RID: 9621 RVA: 0x000B9DBC File Offset: 0x000B81BC
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x06002596 RID: 9622 RVA: 0x000B9DC5 File Offset: 0x000B81C5
	// (set) Token: 0x06002597 RID: 9623 RVA: 0x000B9DD2 File Offset: 0x000B81D2
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

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x06002598 RID: 9624 RVA: 0x000B9DE0 File Offset: 0x000B81E0
	// (set) Token: 0x06002599 RID: 9625 RVA: 0x000B9DED File Offset: 0x000B81ED
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

	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x0600259A RID: 9626 RVA: 0x000B9DFB File Offset: 0x000B81FB
	public bool IsActive
	{
		get
		{
			return this.model.shieldActive;
		}
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x0600259B RID: 9627 RVA: 0x000B9E08 File Offset: 0x000B8208
	public bool IsBroken
	{
		get
		{
			return this.ShieldHealth <= 0;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x0600259C RID: 9628 RVA: 0x000B9E16 File Offset: 0x000B8216
	public int ShieldBeginFrame
	{
		get
		{
			return this.model.shieldBeginFrame;
		}
	}

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x0600259D RID: 9629 RVA: 0x000B9E23 File Offset: 0x000B8223
	public bool WasRunning
	{
		get
		{
			return this.model.wasRunningBeforeShield;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x0600259E RID: 9630 RVA: 0x000B9E30 File Offset: 0x000B8230
	public List<Hit> ShieldHits
	{
		get
		{
			return this.shieldHits;
		}
	}

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x0600259F RID: 9631 RVA: 0x000B9E38 File Offset: 0x000B8238
	public Vector3F ShieldPosition
	{
		get
		{
			return this.model.position;
		}
	}

	// Token: 0x1700092F RID: 2351
	// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000B9E45 File Offset: 0x000B8245
	ShieldConfig IShield.Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x060025A1 RID: 9633 RVA: 0x000B9E4D File Offset: 0x000B824D
	GustShieldData IShield.GustData
	{
		get
		{
			return this.gustData;
		}
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000B9E58 File Offset: 0x000B8258
	public void Initialize(IPlayerDelegate player, ShieldConfig data, MoveData[] gustShieldMoves, IFrameOwner frameOwner)
	{
		GustShieldComponent gustShieldComponent = null;
		foreach (MoveData moveData in gustShieldMoves)
		{
			gustShieldComponent = moveData.GetComponent<GustShieldComponent>();
			if (gustShieldComponent != null)
			{
				this.gustMove = moveData;
				break;
			}
		}
		if (gustShieldComponent == null)
		{
			Debug.LogError("No GustShield labeled move contains a GustShieldComponent");
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
			Debug.LogError("Unhandled number of shield children");
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

	// Token: 0x060025A3 RID: 9635 RVA: 0x000BA170 File Offset: 0x000B8570
	void IDestroyable.Destroy()
	{
		foreach (Material obj in this.shieldMaterials)
		{
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

	// Token: 0x060025A4 RID: 9636 RVA: 0x000BA1F4 File Offset: 0x000B85F4
	private void RebuildShieldHits()
	{
		this.shieldHits.Clear();
		foreach (ActiveShieldHitType activeShieldHitType in this.model.activeShieldHits)
		{
			if (activeShieldHitType != ActiveShieldHitType.normal)
			{
				if (activeShieldHitType != ActiveShieldHitType.gustHit_ground)
				{
					if (activeShieldHitType == ActiveShieldHitType.gustHit_air)
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

	// Token: 0x060025A5 RID: 9637 RVA: 0x000BA2C4 File Offset: 0x000B86C4
	public void LoadShieldData(ShieldConfig shieldData)
	{
		this.data = shieldData;
		this.setHitData(this.model.gustHit_air.data, true);
		this.setHitData(this.model.gustHit_ground.data, false);
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000BA2FC File Offset: 0x000B86FC
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

	// Token: 0x060025A7 RID: 9639 RVA: 0x000BA40C File Offset: 0x000B880C
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

	// Token: 0x060025A8 RID: 9640 RVA: 0x000BA7F8 File Offset: 0x000B8BF8
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

	// Token: 0x060025A9 RID: 9641 RVA: 0x000BA8D8 File Offset: 0x000B8CD8
	private void updateShield()
	{
		for (int i = 0; i < this.ShieldHits.Count; i++)
		{
			Hit hit = this.ShieldHits[i];
			foreach (HitBoxState hitBoxState in hit.hitBoxes)
			{
				hitBoxState.position = this.model.position;
				hitBoxState.overrideRadius = this.Radius;
			}
		}
		if (this.model.shieldActive != this.model.shield.activeInHierarchy)
		{
			this.model.shield.SetActive(this.model.shieldActive);
		}
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000BA9B0 File Offset: 0x000B8DB0
	private void Update()
	{
		if (this.shield != null && base.gameManager != null)
		{
			this.shield.transform.LookAt(base.gameManager.Camera.current.transform);
			this.shield.transform.Rotate(base.transform.forward, (float)this.model.rotationAngle);
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x060025AB RID: 9643 RVA: 0x000BAA30 File Offset: 0x000B8E30
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

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x060025AC RID: 9644 RVA: 0x000BAAA8 File Offset: 0x000B8EA8
	private Fixed currentGustShieldCost
	{
		get
		{
			return this.gustData.shieldHealthCostFlatValue + this.gustData.shieldHealthCostOfMaxPercent * this.data.maxShieldHealth + this.gustData.shieldHealthCostOfCurrentPercent * this.model.shieldHealth;
		}
	}

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x060025AD RID: 9645 RVA: 0x000BAB00 File Offset: 0x000B8F00
	private Fixed gustShieldUseThreshold
	{
		get
		{
			return this.gustData.gustUsableThresholdValue + this.gustData.gustUsableThresholdPercent * this.data.maxShieldHealth;
		}
	}

	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x060025AE RID: 9646 RVA: 0x000BAB30 File Offset: 0x000B8F30
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

	// Token: 0x060025AF RID: 9647 RVA: 0x000BAB90 File Offset: 0x000B8F90
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

	// Token: 0x060025B0 RID: 9648 RVA: 0x000BAC5C File Offset: 0x000B905C
	private bool tryGustEnemyPlayer(PlayerController other)
	{
		int num = this.frameOwner.Frame - this.model.gustBeginFrame;
		return this.gustData.knockbackFrameEnd <= 1 || (num >= this.gustData.knockbackFrameStart && num <= this.gustData.knockbackFrameEnd);
	}

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000BACBC File Offset: 0x000B90BC
	public bool IsGusting
	{
		get
		{
			return this.model.isGusting;
		}
	}

	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000BACC9 File Offset: 0x000B90C9
	public bool GustSuccess
	{
		get
		{
			return this.model.isGustSuccessful;
		}
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x000BACD8 File Offset: 0x000B90D8
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

	// Token: 0x060025B4 RID: 9652 RVA: 0x000BAD98 File Offset: 0x000B9198
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

	// Token: 0x060025B5 RID: 9653 RVA: 0x000BAE7C File Offset: 0x000B927C
	private void gainHealth()
	{
		this.ShieldHealth = FixedMath.Min(this.data.maxShieldHealth, this.ShieldHealth + (Fixed)1.0 / (Fixed)((double)WTime.fps) * this.data.regenerationPerSecond);
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x000BAED8 File Offset: 0x000B92D8
	private void drainHealth()
	{
		if (this.IsGusting)
		{
			return;
		}
		this.ShieldHealth = FixedMath.Max(0, this.ShieldHealth - (Fixed)1.0 / (Fixed)((double)WTime.fps) * this.data.holdDepletionPerSecond);
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x000BAF3C File Offset: 0x000B933C
	public void OnHit(HitData hitData, IHitOwner other)
	{
		if (!this.gustData.gustShieldIgnoresDamage || !this.IsGusting)
		{
			Fixed one = this.combatCalculator.CalculateModifiedDamage(hitData, other);
			Fixed other2 = one * base.config.shieldConfig.shieldDamageMultiplier + hitData.bonusShieldDamage;
			this.ShieldHealth = FixedMath.Max(0, this.ShieldHealth - other2);
		}
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x000BAFB1 File Offset: 0x000B93B1
	public void ResetHealth()
	{
		this.ShieldHealth = this.data.shieldRestoreHealth;
	}

	// Token: 0x060025B9 RID: 9657 RVA: 0x000BAFC4 File Offset: 0x000B93C4
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

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x060025BA RID: 9658 RVA: 0x000BB081 File Offset: 0x000B9481
	public Fixed ShieldPercentage
	{
		get
		{
			return this.model.shieldHealth / base.config.shieldConfig.maxShieldHealth;
		}
	}

	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x060025BB RID: 9659 RVA: 0x000BB0A4 File Offset: 0x000B94A4
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

	// Token: 0x060025BC RID: 9660 RVA: 0x000BB15D File Offset: 0x000B955D
	public void BreakShield()
	{
		this.model.shieldHealth = 0;
	}

	// Token: 0x060025BD RID: 9661 RVA: 0x000BB170 File Offset: 0x000B9570
	public void OnShieldBegin(bool wasRunning)
	{
		this.model.shieldActive = true;
		this.model.shieldBeginFrame = this.frameOwner.Frame;
		this.model.wasRunningBeforeShield = wasRunning;
		this.updateShield();
	}

	// Token: 0x060025BE RID: 9662 RVA: 0x000BB1A6 File Offset: 0x000B95A6
	public void OnShieldReleased()
	{
		this.model.shieldActive = false;
		this.updateShield();
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x060025BF RID: 9663 RVA: 0x000BB1BC File Offset: 0x000B95BC
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

	// Token: 0x060025C0 RID: 9664 RVA: 0x000BB26C File Offset: 0x000B966C
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

	// Token: 0x060025C1 RID: 9665 RVA: 0x000BB36C File Offset: 0x000B976C
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

	// Token: 0x060025C2 RID: 9666 RVA: 0x000BB3D8 File Offset: 0x000B97D8
	public void LinkEndGustMoves(InputButtonsData input)
	{
		if (this.gustMove == this.player.ActiveMove.Data)
		{
			foreach (InterruptData interruptData in this.player.ActiveMove.Data.interrupts)
			{
				if (interruptData.ShouldUseLink(LinkCheckType.GustUpdated, this.player, this.player.ActiveMove.Model, input))
				{
					IPlayerDelegate playerDelegate = this.player;
					MoveData move = interruptData.linkableMoves[0];
					HorizontalDirection inputDirection = HorizontalDirection.None;
					int uid = this.player.ActiveMove.Model.uid;
					int nextMoveStartupFrame = interruptData.nextMoveStartupFrame;
					List<MoveLinkComponentData> allLinkComponentData = this.player.ActiveMove.GetAllLinkComponentData();
					playerDelegate.SetMove(move, input, inputDirection, uid, nextMoveStartupFrame, default(Vector3F), new MoveTransferSettings
					{
						transferHitDisableTargets = interruptData.transferHitDisabledTargets,
						transferChargeData = interruptData.transferChargeData
					}, allLinkComponentData, default(MoveSeedData), ButtonPress.None);
				}
			}
		}
	}

	// Token: 0x060025C3 RID: 9667 RVA: 0x000BB4E8 File Offset: 0x000B98E8
	public void LinkSuccessGustMoves(InputButtonsData input)
	{
		if (this.gustMove == this.player.ActiveMove.Data)
		{
			foreach (InterruptData interruptData in this.player.ActiveMove.Data.interrupts)
			{
				if (interruptData.ShouldUseLink(LinkCheckType.GustUpdated, this.player, this.player.ActiveMove.Model, input))
				{
					this.player.TryBeginMove(interruptData.linkableMoves[0], interruptData, ButtonPress.None, input);
				}
			}
		}
	}

	// Token: 0x060025C4 RID: 9668 RVA: 0x000BB57E File Offset: 0x000B997E
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ShieldModel>(ref this.model);
		this.RebuildShieldHits();
		return true;
	}

	// Token: 0x060025C5 RID: 9669 RVA: 0x000BB594 File Offset: 0x000B9994
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<ShieldModel>(this.model));
		return true;
	}

	// Token: 0x060025C6 RID: 9670 RVA: 0x000BB5B0 File Offset: 0x000B99B0
	private void onToggleDebugDrawChannel(GameEvent message)
	{
		ToggleDebugDrawChannelCommand toggleDebugDrawChannelCommand = message as ToggleDebugDrawChannelCommand;
		bool enabled = toggleDebugDrawChannelCommand.enabled;
		if (toggleDebugDrawChannelCommand.channel == DebugDrawChannel.HurtBoxes)
		{
			this.toggleHurtBoxCapsules(enabled);
		}
	}

	// Token: 0x060025C7 RID: 9671 RVA: 0x000BB5E0 File Offset: 0x000B99E0
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

	// Token: 0x060025C8 RID: 9672 RVA: 0x000BB668 File Offset: 0x000B9A68
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

	// Token: 0x04001BB0 RID: 7088
	private ShieldConfig data;

	// Token: 0x04001BB1 RID: 7089
	private GustShieldData gustData;

	// Token: 0x04001BB2 RID: 7090
	private ShieldModel model;

	// Token: 0x04001BB3 RID: 7091
	private Renderer shieldRenderer;

	// Token: 0x04001BB4 RID: 7092
	private Material[] shieldMaterials;

	// Token: 0x04001BB5 RID: 7093
	private IPlayerDelegate player;

	// Token: 0x04001BB6 RID: 7094
	private IFrameOwner frameOwner;

	// Token: 0x04001BB7 RID: 7095
	private List<Hit> shieldHits = new List<Hit>();

	// Token: 0x04001BB8 RID: 7096
	private MoveData gustMove;

	// Token: 0x04001BB9 RID: 7097
	private float displayOffset;

	// Token: 0x04001BBA RID: 7098
	private CapsuleShape capsuleHurtBox;
}
