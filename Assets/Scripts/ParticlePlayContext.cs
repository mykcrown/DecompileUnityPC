// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public struct ParticlePlayContext
{
	public delegate bool EffectInstantiator(GameObject prefab, out Effect output);

	public ParticlePlayContext.EffectInstantiator effectInstantiator;

	public IBoneController boneController;

	public IFacing facing;

	public IPhysicsStateOwner physics;

	public Action<ParticleData, GameObject> onParticleCreated;
}
