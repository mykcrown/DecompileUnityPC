using System;
using FixedPoint;

// Token: 0x0200036D RID: 877
public class ArticleWaveMotionComponent : ArticleComponent, IArticleMovementController, IRollbackStateOwner, IResetable
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x060012A5 RID: 4773 RVA: 0x0006ADA8 File Offset: 0x000691A8
	// (set) Token: 0x060012A6 RID: 4774 RVA: 0x0006ADB0 File Offset: 0x000691B0
	[Inject]
	public IRollbackStatePooling pooling { get; set; }

	// Token: 0x060012A7 RID: 4775 RVA: 0x0006ADB9 File Offset: 0x000691B9
	public override void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		base.Init(articleDelegate, manager);
		this.Reset();
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x0006ADC9 File Offset: 0x000691C9
	public override void OnArticleInstantiate()
	{
		this.model.baseYPosition = this.articleDelegate.Model.physicsModel.position.y;
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x0006ADF0 File Offset: 0x000691F0
	public bool TickMovement(ref Vector3F velocity, ref Vector3F position)
	{
		this.model.waveTime += WTime.fixedDeltaTime;
		Fixed one = (FixedMath.Sin(this.model.waveOffset + this.model.waveTime * this.configData.frequency * FixedMath.TwoPI) + this.WaveHeightOffset) * this.configData.amplitude;
		Fixed @fixed = one + this.model.baseYPosition;
		velocity = this.articleDelegate.Model.physicsModel.movementVelocity;
		velocity.y += @fixed - position.y;
		position.y = @fixed;
		return true;
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x060012AA RID: 4778 RVA: 0x0006AEC0 File Offset: 0x000692C0
	private Fixed WaveHeightOffset
	{
		get
		{
			switch (this.configData.startType)
			{
			default:
				return 0;
			case ArticleWaveMotionStartType.StartAtBottom:
				return 1;
			case ArticleWaveMotionStartType.StartAtTop:
				return -1;
			}
		}
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x0006AF04 File Offset: 0x00069304
	public void Reset()
	{
		this.model.waveTime = 0;
		ArticleWaveMotionStartType startType = this.configData.startType;
		if (startType != ArticleWaveMotionStartType.StartAtCenter)
		{
			if (startType != ArticleWaveMotionStartType.StartAtTop)
			{
				if (startType == ArticleWaveMotionStartType.StartAtBottom)
				{
					this.model.waveOffset = -FixedMath.HalfPI;
				}
			}
			else
			{
				this.model.waveOffset = FixedMath.HalfPI;
			}
		}
		else
		{
			this.model.waveOffset = 0;
		}
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x0006AF8C File Offset: 0x0006938C
	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.pooling.Clone<ArticleWaveMotionComponent.ArticleWaveMotionComponentModel>(this.model));
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x0006AFA6 File Offset: 0x000693A6
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ArticleWaveMotionComponent.ArticleWaveMotionComponentModel>(ref this.model);
		return true;
	}

	// Token: 0x04000C2B RID: 3115
	public ArticleWaveMotionComponentData configData;

	// Token: 0x04000C2C RID: 3116
	private ArticleWaveMotionComponent.ArticleWaveMotionComponentModel model = new ArticleWaveMotionComponent.ArticleWaveMotionComponentModel();

	// Token: 0x0200036E RID: 878
	[Serializable]
	public class ArticleWaveMotionComponentModel : RollbackStateTyped<ArticleWaveMotionComponent.ArticleWaveMotionComponentModel>
	{
		// Token: 0x060012AF RID: 4783 RVA: 0x0006AFBE File Offset: 0x000693BE
		public override void CopyTo(ArticleWaveMotionComponent.ArticleWaveMotionComponentModel target)
		{
			target.baseYPosition = this.baseYPosition;
			target.waveTime = this.waveTime;
			target.waveOffset = this.waveOffset;
		}

		// Token: 0x04000C2D RID: 3117
		public Fixed baseYPosition;

		// Token: 0x04000C2E RID: 3118
		public Fixed waveTime;

		// Token: 0x04000C2F RID: 3119
		public Fixed waveOffset;
	}
}
