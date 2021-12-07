using System;
using FixedPoint;

// Token: 0x02000362 RID: 866
public interface IArticleMovementController
{
	// Token: 0x06001285 RID: 4741
	bool TickMovement(ref Vector3F velocity, ref Vector3F position);
}
