using System;
using FixedPoint;

// Token: 0x02000369 RID: 873
public class ArticleSurfaceSlideComponent : ArticleComponent, IArticleMovementController, IResetable, IRollbackStateOwner
{
	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06001290 RID: 4752 RVA: 0x0006A985 File Offset: 0x00068D85
	// (set) Token: 0x06001291 RID: 4753 RVA: 0x0006A98D File Offset: 0x00068D8D
	[Inject]
	public IRollbackStatePooling pooling { get; set; }

	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06001292 RID: 4754 RVA: 0x0006A996 File Offset: 0x00068D96
	// (set) Token: 0x06001293 RID: 4755 RVA: 0x0006A9A3 File Offset: 0x00068DA3
	public bool EnableLedgeProtection
	{
		get
		{
			return this.model.enableLedgeProtection;
		}
		set
		{
			this.model.enableLedgeProtection = value;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06001294 RID: 4756 RVA: 0x0006A9B1 File Offset: 0x00068DB1
	// (set) Token: 0x06001295 RID: 4757 RVA: 0x0006A9BE File Offset: 0x00068DBE
	public Fixed MaxSlideSpeed
	{
		get
		{
			return this.model.maxSlideSpeed;
		}
		set
		{
			this.model.maxSlideSpeed = value;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06001296 RID: 4758 RVA: 0x0006A9CC File Offset: 0x00068DCC
	// (set) Token: 0x06001297 RID: 4759 RVA: 0x0006A9D9 File Offset: 0x00068DD9
	public Fixed MaxTravelDist
	{
		get
		{
			return this.model.maxTravelDistance;
		}
		set
		{
			this.model.maxTravelDistance = value;
		}
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x0006A9E7 File Offset: 0x00068DE7
	public override void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		base.Init(articleDelegate, manager);
		this.model.enableLedgeProtection = this.configData.enableLedgeProtection;
		this.model.maxSlideSpeed = this.configData.maxSlideSpeed;
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x0006AA20 File Offset: 0x00068E20
	public void InitMovement(IPhysicsCollider collider, Fixed normStartPosition, Fixed maxTravelDistance)
	{
		this.model.attachedCollider = collider;
		this.model.normStartPosition = normStartPosition;
		this.model.maxTravelDistance = maxTravelDistance;
		this.model.normTravelDistance = 0;
		Vector3F b = this.model.attachedCollider.Edge.GetPositionAtNormalizedLocation(this.model.normPosition);
		this.ForceSlide(maxTravelDistance * InputUtils.GetDirectionMultiplier(this.articleDelegate.Facing) / this.model.attachedCollider.Edge.EdgeLength);
		this.articleDelegate.Model.physicsModel.position = this.model.attachedCollider.Edge.GetPositionAtNormalizedLocation(this.model.normPosition);
		this.articleDelegate.Model.physicsModel.SetVelocity(this.articleDelegate.Model.physicsModel.position - b, VelocityType.Movement);
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x0006AB29 File Offset: 0x00068F29
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.pooling.Clone<ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel>(this.model));
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x0006AB43 File Offset: 0x00068F43
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel>(ref this.model);
		return true;
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0006AB54 File Offset: 0x00068F54
	public void Reset()
	{
		this.model.attachedCollider = null;
		this.model.normStartPosition = 0;
		this.model.normTravelDistance = 0;
		this.model.maxTravelDistance = 999999;
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x0006ABA4 File Offset: 0x00068FA4
	public bool TickMovement(ref Vector3F velocity, ref Vector3F position)
	{
		if (this.model.attachedCollider == null)
		{
			return false;
		}
		Fixed other = InputUtils.GetDirectionMultiplier(this.articleDelegate.Facing);
		Fixed one = this.MaxSlideSpeed / this.model.attachedCollider.Edge.EdgeLength;
		Fixed normVelocity = one * WTime.frameTime * other;
		Vector3F b = this.model.attachedCollider.Edge.GetPositionAtNormalizedLocation(this.model.normPosition);
		this.ForceSlide(normVelocity);
		position = this.model.attachedCollider.Edge.GetPositionAtNormalizedLocation(this.model.normPosition);
		velocity = position - b;
		return true;
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0006AC78 File Offset: 0x00069078
	public void ForceSlide(Fixed normVelocity)
	{
		if (this.EnableLedgeProtection)
		{
			this.getLedgeClampedPosition(this.model.normPosition, ref normVelocity);
		}
		Fixed @fixed = this.model.maxTravelDistance / this.model.attachedCollider.Edge.EdgeLength;
		this.model.normTravelDistance += normVelocity;
		this.model.normTravelDistance = FixedMath.Clamp(this.model.normTravelDistance, -@fixed, @fixed);
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0006AD03 File Offset: 0x00069103
	private Fixed getLedgeClampedPosition(Fixed position, ref Fixed delta)
	{
		return this.model.attachedCollider.Edge.ClampNormalizedMovementToLandableSurface(position, ref delta);
	}

	// Token: 0x04000C1B RID: 3099
	public ArticleSurfaceSlideComponentData configData;

	// Token: 0x04000C1C RID: 3100
	private ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel model = new ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel();

	// Token: 0x0200036A RID: 874
	[Serializable]
	public class ArticleSurfaceSlideComponentModel : RollbackStateTyped<ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel>
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x0006AD24 File Offset: 0x00069124
		public Fixed normPosition
		{
			get
			{
				return this.normStartPosition + this.normTravelDistance;
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0006AD38 File Offset: 0x00069138
		public override void CopyTo(ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel target)
		{
			target.attachedCollider = this.attachedCollider;
			target.normStartPosition = this.normStartPosition;
			target.normTravelDistance = this.normTravelDistance;
			target.maxTravelDistance = this.maxTravelDistance;
			target.enableLedgeProtection = this.enableLedgeProtection;
			target.maxSlideSpeed = this.maxSlideSpeed;
		}

		// Token: 0x04000C1D RID: 3101
		[IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[IgnoreCopyValidation]
		[NonSerialized]
		public IPhysicsCollider attachedCollider;

		// Token: 0x04000C1E RID: 3102
		public Fixed normStartPosition;

		// Token: 0x04000C1F RID: 3103
		public Fixed normTravelDistance;

		// Token: 0x04000C20 RID: 3104
		public Fixed maxTravelDistance;

		// Token: 0x04000C21 RID: 3105
		public bool enableLedgeProtection;

		// Token: 0x04000C22 RID: 3106
		public Fixed maxSlideSpeed;
	}
}
