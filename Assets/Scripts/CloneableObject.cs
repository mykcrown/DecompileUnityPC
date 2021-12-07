// Decompile from assembly: Assembly-CSharp.dll

using MemberwiseEquality;
using System;

[Serializable]
public abstract class CloneableObject : MemberwiseEqualityObject, ICloneable
{
	public virtual object Clone()
	{
		return base.MemberwiseClone();
	}

	public virtual object DeepClone()
	{
		object obj = base.MemberwiseClone();
		((CloneableObject)obj).Assign(this);
		return obj;
	}
}
