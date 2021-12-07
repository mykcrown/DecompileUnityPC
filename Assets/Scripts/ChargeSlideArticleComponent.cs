// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ChargeSlideArticleComponent : MoveComponent, IMoveChargeComponent, IMoveEndComponent
{
	public ChargeSliderArticleComponentData configData;

	private ArticleController slideController;

	private ArticleSurfaceSlideComponent slideComponent;

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public MoveArticleSpawnCalculator articleSpawnCalculator
	{
		get;
		set;
	}

	private Fixed chargeTravelDist
	{
		get
		{
			return FixedMath.Lerp(this.configData.spawnDistanceOffset, this.configData.fullChargeMaxDistance, this.moveDelegate.Model.ChargeFraction);
		}
	}

	public int ChargeFireDelay
	{
		get
		{
			return this.configData.fireDelay;
		}
	}

	public override void RegisterPreload(PreloadContext context)
	{
		base.RegisterPreload(context);
		if (this.configData.slideAimArticle != null)
		{
			this.configData.slideAimArticle.RegisterPreload(context);
		}
		this.configData.hitArticle.RegisterPreload(context);
	}

	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		base.Init(moveDelegate, playerDelegate, input);
		if (this.configData.fireOnStart)
		{
			if (this.configData.fireDelay > moveDelegate.Model.chargeFireDelay)
			{
				moveDelegate.Model.chargeFireDelay = this.configData.fireDelay;
			}
			else
			{
				this.OnFireCharge();
			}
		}
	}

	public void OnStartCharge()
	{
		this.slideController = ArticleData.CreateArticleController(this.gameController.currentGame.DynamicObjects, this.configData.slideAimArticle.type, this.configData.slideAimArticle.prefab, 4);
		this.slideController.model.physicsModel.Reset();
		this.slideController.model.physicsModel.position = this.playerDelegate.Position;
		this.slideController.model.currentFacing = this.playerDelegate.Facing;
		this.slideController.model.playerOwner = this.playerDelegate.PlayerNum;
		this.slideController.model.team = this.playerDelegate.Team;
		this.slideController.model.moveLabel = this.moveDelegate.Data.label;
		this.slideController.model.moveName = this.moveDelegate.Data.moveName;
		this.slideController.model.moveUID = this.moveDelegate.Model.uid;
		this.slideController.model.staleDamageMultiplier = this.moveDelegate.Model.staleDamageMultiplier;
		this.slideController.model.chargeData = this.moveDelegate.Model.ChargeData;
		this.slideController.model.chargeFraction = this.moveDelegate.Model.ChargeFraction;
		this.slideController.Init(this.configData.slideAimArticle);
		this.slideController.SyncToRotation(InputUtils.GetDirectionAngle(this.slideController.model.currentFacing));
		this.slideComponent = this.slideController.GetArticleComponent<ArticleSurfaceSlideComponent>();
		this.slideComponent.EnableLedgeProtection = this.configData.enableLedgeProtection;
		this.slideComponent.MaxSlideSpeed = 100;
		RaycastHitData raycastHitData;
		if (this.playerDelegate.IsStandingOnStageSurface(out raycastHitData))
		{
			Fixed nearestNormalizedLocation = raycastHitData.collider.Edge.GetNearestNormalizedLocation(raycastHitData.segmentIndex, raycastHitData.point);
			this.slideComponent.InitMovement(raycastHitData.collider, nearestNormalizedLocation, this.chargeTravelDist);
		}
	}

	public void OnContinueCharge()
	{
		this.slideComponent.MaxTravelDist = this.chargeTravelDist;
	}

	private void removeSlideArticle()
	{
		if (this.slideController != null && !this.slideController.IsExpired)
		{
			this.slideController.Explode(true);
		}
	}

	public void OnEndCharge()
	{
	}

	private ArticleController createHitArticle(Vector3F position)
	{
		ArticleController articleController = ArticleData.CreateArticleController(this.gameController.currentGame.DynamicObjects, this.configData.hitArticle.type, this.configData.hitArticle.prefab, 4);
		articleController.model.physicsModel.Reset();
		articleController.model.physicsModel.position = position;
		articleController.model.currentFacing = this.playerDelegate.Facing;
		articleController.model.playerOwner = this.playerDelegate.PlayerNum;
		articleController.model.team = this.playerDelegate.Team;
		articleController.model.moveLabel = this.moveDelegate.Data.label;
		articleController.model.moveName = this.moveDelegate.Data.moveName;
		articleController.model.moveUID = this.moveDelegate.Model.uid;
		articleController.model.staleDamageMultiplier = this.moveDelegate.Model.staleDamageMultiplier;
		articleController.model.chargeData = this.moveDelegate.Model.ChargeData;
		articleController.model.chargeFraction = this.moveDelegate.Model.ChargeFraction;
		articleController.Init(this.configData.hitArticle);
		return articleController;
	}

	public void OnFireCharge()
	{
		Vector3F position = Vector3F.zero;
		RaycastHitData raycastHitData;
		if (this.slideController != null)
		{
			position = this.slideController.Position;
		}
		else if (this.playerDelegate.IsStandingOnStageSurface(out raycastHitData))
		{
			EdgeData edge = raycastHitData.collider.Edge;
			Fixed @fixed = edge.GetNearestNormalizedLocation(raycastHitData.segmentIndex, raycastHitData.point);
			Fixed other = this.chargeTravelDist / edge.EdgeLength * InputUtils.GetDirectionMultiplier(this.playerDelegate.Facing);
			if (this.configData.enableLedgeProtection)
			{
				@fixed = edge.ClampNormalizedMovementToLandableSurface(@fixed, ref other);
			}
			else
			{
				@fixed += other;
			}
			position = edge.GetPositionAtNormalizedLocation(@fixed);
		}
		this.createHitArticle(position);
		this.removeSlideArticle();
	}

	public void OnEnd()
	{
		this.removeSlideArticle();
	}
}
