// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ArticleSurfaceSlideComponent : ArticleComponent, IArticleMovementController, IResetable, IRollbackStateOwner
{
	[Serializable]
	public class ArticleSurfaceSlideComponentModel : RollbackStateTyped<ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel>
	{
		[IgnoreCopyValidation, IgnoreRollback(IgnoreRollbackType.ShallowCopy)]
		[NonSerialized]
		public IPhysicsCollider attachedCollider;

		public Fixed normStartPosition;

		public Fixed normTravelDistance;

		public Fixed maxTravelDistance;

		public bool enableLedgeProtection;

		public Fixed maxSlideSpeed;

		public Fixed normPosition
		{
			get
			{
				return this.normStartPosition + this.normTravelDistance;
			}
		}

		public override void CopyTo(ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel target)
		{
			target.attachedCollider = this.attachedCollider;
			target.normStartPosition = this.normStartPosition;
			target.normTravelDistance = this.normTravelDistance;
			target.maxTravelDistance = this.maxTravelDistance;
			target.enableLedgeProtection = this.enableLedgeProtection;
			target.maxSlideSpeed = this.maxSlideSpeed;
		}
	}

	public ArticleSurfaceSlideComponentData configData;

	private ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel model = new ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel();

	[Inject]
	public IRollbackStatePooling pooling
	{
		get;
		set;
	}

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

	public override void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		base.Init(articleDelegate, manager);
		this.model.enableLedgeProtection = this.configData.enableLedgeProtection;
		this.model.maxSlideSpeed = this.configData.maxSlideSpeed;
	}

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

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.pooling.Clone<ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ArticleSurfaceSlideComponent.ArticleSurfaceSlideComponentModel>(ref this.model);
		return true;
	}

	public void Reset()
	{
		this.model.attachedCollider = null;
		this.model.normStartPosition = 0;
		this.model.normTravelDistance = 0;
		this.model.maxTravelDistance = 999999;
	}

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

	private Fixed getLedgeClampedPosition(Fixed position, ref Fixed delta)
	{
		return this.model.attachedCollider.Edge.ClampNormalizedMovementToLandableSurface(position, ref delta);
	}
}
