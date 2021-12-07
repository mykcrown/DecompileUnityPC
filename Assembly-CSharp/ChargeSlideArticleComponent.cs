using System;
using FixedPoint;

// Token: 0x020004CF RID: 1231
public class ChargeSlideArticleComponent : MoveComponent, IMoveChargeComponent, IMoveEndComponent
{
	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0008A7DE File Offset: 0x00088BDE
	// (set) Token: 0x06001B2A RID: 6954 RVA: 0x0008A7E6 File Offset: 0x00088BE6
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06001B2B RID: 6955 RVA: 0x0008A7EF File Offset: 0x00088BEF
	// (set) Token: 0x06001B2C RID: 6956 RVA: 0x0008A7F7 File Offset: 0x00088BF7
	[Inject]
	public MoveArticleSpawnCalculator articleSpawnCalculator { get; set; }

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0008A800 File Offset: 0x00088C00
	private Fixed chargeTravelDist
	{
		get
		{
			return FixedMath.Lerp(this.configData.spawnDistanceOffset, this.configData.fullChargeMaxDistance, this.moveDelegate.Model.ChargeFraction);
		}
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x0008A830 File Offset: 0x00088C30
	public override void RegisterPreload(PreloadContext context)
	{
		base.RegisterPreload(context);
		if (this.configData.slideAimArticle != null)
		{
			this.configData.slideAimArticle.RegisterPreload(context);
		}
		this.configData.hitArticle.RegisterPreload(context);
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x0008A87C File Offset: 0x00088C7C
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

	// Token: 0x06001B30 RID: 6960 RVA: 0x0008A8E0 File Offset: 0x00088CE0
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

	// Token: 0x06001B31 RID: 6961 RVA: 0x0008AB29 File Offset: 0x00088F29
	public void OnContinueCharge()
	{
		this.slideComponent.MaxTravelDist = this.chargeTravelDist;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x0008AB3C File Offset: 0x00088F3C
	private void removeSlideArticle()
	{
		if (this.slideController != null && !this.slideController.IsExpired)
		{
			this.slideController.Explode(true);
		}
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x0008AB6B File Offset: 0x00088F6B
	public void OnEndCharge()
	{
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x0008AB70 File Offset: 0x00088F70
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

	// Token: 0x06001B35 RID: 6965 RVA: 0x0008ACCC File Offset: 0x000890CC
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

	// Token: 0x06001B36 RID: 6966 RVA: 0x0008ADA0 File Offset: 0x000891A0
	public void OnEnd()
	{
		this.removeSlideArticle();
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x06001B37 RID: 6967 RVA: 0x0008ADA8 File Offset: 0x000891A8
	public int ChargeFireDelay
	{
		get
		{
			return this.configData.fireDelay;
		}
	}

	// Token: 0x0400147C RID: 5244
	public ChargeSliderArticleComponentData configData;

	// Token: 0x0400147D RID: 5245
	private ArticleController slideController;

	// Token: 0x0400147E RID: 5246
	private ArticleSurfaceSlideComponent slideComponent;
}
