using System;
using FixedPoint;

// Token: 0x0200061C RID: 1564
[Serializable]
public class RespawnControllerModel : RollbackStateTyped<RespawnControllerModel>
{
	// Token: 0x06002697 RID: 9879 RVA: 0x000BD3B8 File Offset: 0x000BB7B8
	public override void CopyTo(RespawnControllerModel target)
	{
		target.velocity = this.velocity;
		target.targetPoint = this.targetPoint;
		target.position = this.position;
		target.arrived = this.arrived;
		target.framesAlive = this.framesAlive;
		target.isDead = this.isDead;
	}

	// Token: 0x04001C30 RID: 7216
	public Vector3F velocity;

	// Token: 0x04001C31 RID: 7217
	public Vector3F targetPoint;

	// Token: 0x04001C32 RID: 7218
	public Vector3F position;

	// Token: 0x04001C33 RID: 7219
	public bool arrived;

	// Token: 0x04001C34 RID: 7220
	public int framesAlive;

	// Token: 0x04001C35 RID: 7221
	public bool isDead;
}
