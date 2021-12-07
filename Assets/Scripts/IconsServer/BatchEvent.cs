// Decompile from assembly: Assembly-CSharp.dll

using network;
using System;
using UnityEngine;

namespace IconsServer
{
	public abstract class BatchEvent : ServerEvent, ICloneable
	{
		public virtual void UpdateNetMessage(ref INetMsg message)
		{
			UnityEngine.Debug.LogError("Unsupported message");
		}

		public virtual object Clone()
		{
			return base.MemberwiseClone();
		}
	}
}
