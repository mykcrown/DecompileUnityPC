// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class AimComponent : MoveComponent, IMoveTickGameFrameComponent, IMoveLinkComponent
{
	public int AimStartFrame;

	public int AimEndFrame;

	public int MinAngle;

	public int MaxAngle;

	public Fixed InputReadThreshold;

	public Fixed AimSensitivity;

	public bool InitializeFromInput;

	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
		this.moveDelegate = moveDelegate;
		this.playerDelegate = playerDelegate;
		moveDelegate.Model.overrideFireAngle = true;
	}

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

	public MoveLinkComponentData GetLinkComponentData()
	{
		return new AimLinkComponentData
		{
			FireAngle = this.moveDelegate.Model.articleFireAngle
		};
	}
}
