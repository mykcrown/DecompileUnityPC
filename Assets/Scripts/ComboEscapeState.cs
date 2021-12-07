// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class ComboEscapeState : RollbackStateTyped<ComboEscapeState>
{
	public Vector2F lastInput;

	public int inputsRead;

	public Fixed escapeMultiplier;

	public Fixed escapeAngleMultiplier;

	public bool isBounce;

	public bool isFlourish;

	public override void CopyTo(ComboEscapeState target)
	{
		target.lastInput = this.lastInput;
		target.inputsRead = this.inputsRead;
		target.escapeMultiplier = this.escapeMultiplier;
		target.escapeAngleMultiplier = this.escapeAngleMultiplier;
		target.isBounce = this.isBounce;
		target.isFlourish = this.isFlourish;
	}

	public override void Clear()
	{
		base.Clear();
		this.lastInput = Vector2F.zero;
		this.inputsRead = 0;
		this.escapeMultiplier = 0;
		this.escapeAngleMultiplier = 0;
		this.isBounce = false;
		this.isFlourish = false;
	}
}
