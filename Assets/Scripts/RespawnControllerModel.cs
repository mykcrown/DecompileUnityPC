// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class RespawnControllerModel : RollbackStateTyped<RespawnControllerModel>
{
	public Vector3F velocity;

	public Vector3F targetPoint;

	public Vector3F position;

	public bool arrived;

	public int framesAlive;

	public bool isDead;

	public override void CopyTo(RespawnControllerModel target)
	{
		target.velocity = this.velocity;
		target.targetPoint = this.targetPoint;
		target.position = this.position;
		target.arrived = this.arrived;
		target.framesAlive = this.framesAlive;
		target.isDead = this.isDead;
	}
}
