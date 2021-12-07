// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class IsClonedManually : PropertyAttribute
{
	public IsClonedManually() : this(IsClonedManuallyType.Manual)
	{
	}

	public IsClonedManually(IsClonedManuallyType type)
	{
	}
}
