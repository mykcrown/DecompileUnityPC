using System;
using FixedPoint;

// Token: 0x020004C4 RID: 1220
public class AimComponent : MoveComponent, IMoveTickGameFrameComponent, IMoveLinkComponent
{
	// Token: 0x06001AFF RID: 6911 RVA: 0x00089DEC File Offset: 0x000881EC
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.moveDelegate = moveDelegate;
		this.playerDelegate = playerDelegate;
		moveDelegate.Model.overrideFireAngle = true;
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x00089E08 File Offset: 0x00088208
	public void TickGameFrame(InputButtonsData input)
	{
		MoveModel model = this.moveDelegate.Model;
		if (model.internalFrame < this.AimStartFrame || model.internalFrame >= this.AimEndFrame)
		{
			return;
		}
		bool flag = false;
		if (!model.isArticleFireAngleInitialized)
		{
			model.isArticleFireAngleInitialized = true;
			if (this.InitializeFromInput)
			{
				flag = true;
			}
		}
		if (FixedMath.Abs(input.verticalAxisValue) >= this.InputReadThreshold || FixedMath.Abs(input.horizontalAxisValue) >= this.InputReadThreshold)
		{
			Fixed @fixed = this.ConvertInputToAngle(input);
			if (flag)
			{
				model.articleFireAngle = @fixed;
			}
			else
			{
				Fixed maxDelta = WTime.fixedDeltaTime * this.AimSensitivity;
				model.articleFireAngle = FixedMath.MoveTowardsAngle(model.articleFireAngle, @fixed, maxDelta);
			}
			model.articleFireAngle = FixedMath.Clamp(model.articleFireAngle, this.MinAngle, this.MaxAngle);
		}
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x00089F00 File Offset: 0x00088300
	private Fixed ConvertInputToAngle(InputButtonsData input)
	{
		Vector2F vector2F = new Vector2F(input.horizontalAxisValue, input.verticalAxisValue);
		Vector2F normalized = vector2F.normalized;
		Vector2F vector2F2 = normalized;
		if (this.playerDelegate.Facing == HorizontalDirection.Left)
		{
			vector2F2.x = -vector2F2.x;
		}
		Fixed @fixed = FixedMath.Rad2Deg * FixedMath.Atan2(vector2F2.y, vector2F2.x);
		if ((this.MinAngle != 0 || this.MaxAngle != 0) && (@fixed < this.MinAngle || @fixed > this.MaxAngle))
		{
			Fixed one = FixedMath.Abs(FixedMath.DeltaAngle(@fixed, this.MinAngle));
			Fixed other = FixedMath.Abs(FixedMath.DeltaAngle(@fixed, this.MaxAngle));
			if (one < other)
			{
				@fixed = this.MinAngle;
			}
			else
			{
				@fixed = this.MaxAngle;
			}
		}
		return @fixed;
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x0008A000 File Offset: 0x00088400
	public MoveLinkComponentData GetLinkComponentData()
	{
		return new AimLinkComponentData
		{
			FireAngle = this.moveDelegate.Model.articleFireAngle
		};
	}

	// Token: 0x0400144B RID: 5195
	public int AimStartFrame;

	// Token: 0x0400144C RID: 5196
	public int AimEndFrame;

	// Token: 0x0400144D RID: 5197
	public int MinAngle;

	// Token: 0x0400144E RID: 5198
	public int MaxAngle;

	// Token: 0x0400144F RID: 5199
	public Fixed InputReadThreshold;

	// Token: 0x04001450 RID: 5200
	public Fixed AimSensitivity;

	// Token: 0x04001451 RID: 5201
	public bool InitializeFromInput;
}
