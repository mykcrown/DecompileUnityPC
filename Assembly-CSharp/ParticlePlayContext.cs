using System;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public struct ParticlePlayContext
{
	// Token: 0x04001129 RID: 4393
	public ParticlePlayContext.EffectInstantiator effectInstantiator;

	// Token: 0x0400112A RID: 4394
	public IBoneController boneController;

	// Token: 0x0400112B RID: 4395
	public IFacing facing;

	// Token: 0x0400112C RID: 4396
	public IPhysicsStateOwner physics;

	// Token: 0x0400112D RID: 4397
	public Action<ParticleData, GameObject> onParticleCreated;

	// Token: 0x0200043B RID: 1083
	// (Invoke) Token: 0x0600165A RID: 5722
	public delegate bool EffectInstantiator(GameObject prefab, out Effect output);
}
