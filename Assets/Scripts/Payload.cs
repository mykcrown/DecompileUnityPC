// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class Payload : ICloneable
{
	public virtual object Clone()
	{
		return Serialization.DeepClone<Payload>(this);
	}
}
