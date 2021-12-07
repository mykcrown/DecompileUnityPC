// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

public class MoveArticleSpawnCalculator
{
	public ArticleSpawnParameters Calculate(CreateArticleAction action, InputButtonsData input, IPlayerDelegate playerDelegate, IMoveDelegate moveDelegate)
	{
		ArticleSpawnParameters result = default(ArticleSpawnParameters);
		bool forceInvert = false;
		if (playerDelegate.Facing == HorizontalDirection.Left && moveDelegate != null && moveDelegate.Model.data.animationClipLeft != null)
		{
			forceInvert = true;
		}
		result.sourcePosition = playerDelegate.Body.GetBonePosition(action.fireBodyPart, forceInvert);
		Vector3F b = (Vector3F)action.worldOffset;
		b.x *= InputUtils.GetDirectionMultiplier(playerDelegate.Facing);
		result.sourcePosition += b;
		result.sourcePosition.z = 0;
		result.facing = playerDelegate.Facing;
		Vector3F velocity = playerDelegate.Physics.Velocity;
		result.velocity = new Vector2F(velocity.x * action.inheritSpeedAmount.x, velocity.y * action.inheritSpeedAmount.y);
		Vector3F a = MathUtil.AngleToVector((moveDelegate == null || !moveDelegate.Model.overrideFireAngle) ? action.fireAngle : moveDelegate.Model.articleFireAngle);
		Vector3F b2 = a * (Fixed)((double)action.speed);
		if (result.facing == HorizontalDirection.Left)
		{
			b2.x *= -1;
		}
		result.velocity += b2;
		result.rotation = MathUtil.VectorToAngle(ref result.velocity);
		Fixed d = 1;
		ChargeConfig chargeConfig = (moveDelegate != null) ? moveDelegate.Model.ChargeData : null;
		if (chargeConfig != null)
		{
			Fixed chargeFraction = moveDelegate.Model.ChargeFraction;
			Fixed @fixed = chargeConfig.maxChargeAngleAdjustment * chargeFraction;
			result.rotation += @fixed;
			d = chargeConfig.GetScaledValue(chargeConfig.maxChargeProjectileSpeedMultiplier, chargeFraction);
			result.velocity = d * MathUtil.RotateVector(result.velocity, (result.facing != HorizontalDirection.Left) ? @fixed : (-@fixed));
		}
		return result;
	}

	public Fixed ConvertInputToAngle(InputButtonsData input, IPlayerDelegate playerDelegate, Fixed min, Fixed max)
	{
		Vector2F vector2F = new Vector2F(input.horizontalAxisValue, input.verticalAxisValue);
		Vector2F normalized = vector2F.normalized;
		Vector2F vector2F2 = normalized;
		if (playerDelegate.Facing == HorizontalDirection.Left)
		{
			vector2F2.x = -vector2F2.x;
		}
		Fixed @fixed = FixedMath.Rad2Deg * FixedMath.Atan2(vector2F2.y, vector2F2.x);
		if ((min != 0 || max != 0) && (@fixed < min || @fixed > max))
		{
			Fixed one = FixedMath.Abs(FixedMath.DeltaAngle(@fixed, min));
			Fixed other = FixedMath.Abs(FixedMath.DeltaAngle(@fixed, max));
			if (one < other)
			{
				@fixed = min;
			}
			else
			{
				@fixed = max;
			}
		}
		return @fixed;
	}
}
