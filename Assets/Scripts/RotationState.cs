// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class RotationState : RollbackStateTyped<RotationState>
{
	public Vector3F eulerAngles;

	public QuaternionF quaternion;

	public override void CopyTo(RotationState target)
	{
		target.eulerAngles = this.eulerAngles;
		target.quaternion = this.quaternion;
	}
}
