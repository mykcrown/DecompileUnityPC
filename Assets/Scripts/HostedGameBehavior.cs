// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public abstract class HostedGameBehavior : MonoBehaviour, IPooledComponent, IResetable
{
	protected GameObject hostObject;

	public virtual void Reset()
	{
		this.hostObject = null;
	}

	public virtual void SetHost(GameObject hostObject)
	{
		this.hostObject = hostObject;
	}
}
