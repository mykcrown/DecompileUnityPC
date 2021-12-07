// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class ArticleWaveMotionComponent : ArticleComponent, IArticleMovementController, IRollbackStateOwner, IResetable
{
	[Serializable]
	public class ArticleWaveMotionComponentModel : RollbackStateTyped<ArticleWaveMotionComponent.ArticleWaveMotionComponentModel>
	{
		public Fixed baseYPosition;

		public Fixed waveTime;

		public Fixed waveOffset;

		public override void CopyTo(ArticleWaveMotionComponent.ArticleWaveMotionComponentModel target)
		{
			target.baseYPosition = this.baseYPosition;
			target.waveTime = this.waveTime;
			target.waveOffset = this.waveOffset;
		}
	}

	public ArticleWaveMotionComponentData configData;

	private ArticleWaveMotionComponent.ArticleWaveMotionComponentModel model = new ArticleWaveMotionComponent.ArticleWaveMotionComponentModel();

	[Inject]
	public IRollbackStatePooling pooling
	{
		get;
		set;
	}

	private Fixed WaveHeightOffset
	{
		get
		{
			switch (this.configData.startType)
			{
			case ArticleWaveMotionStartType.StartAtCenter:
				IL_23:
				return 0;
			case ArticleWaveMotionStartType.StartAtBottom:
				return 1;
			case ArticleWaveMotionStartType.StartAtTop:
				return -1;
			}
			goto IL_23;
		}
	}

	public override void Init(IArticleDelegate articleDelegate, GameManager manager)
	{
		base.Init(articleDelegate, manager);
		this.Reset();
	}

	public override void OnArticleInstantiate()
	{
		this.model.baseYPosition = this.articleDelegate.Model.physicsModel.position.y;
	}

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

	public bool ExportState(ref RollbackStateContainer container)
	{
		return container.WriteState(this.pooling.Clone<ArticleWaveMotionComponent.ArticleWaveMotionComponentModel>(this.model));
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<ArticleWaveMotionComponent.ArticleWaveMotionComponentModel>(ref this.model);
		return true;
	}
}
