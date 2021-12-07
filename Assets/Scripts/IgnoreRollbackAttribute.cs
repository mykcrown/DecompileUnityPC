// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class IgnoreRollbackAttribute : PropertyAttribute
{
	private IgnoreRollbackType ignoreType;

	public IgnoreRollbackAttribute(IgnoreRollbackType ignoreType)
	{
		this.ignoreType = ignoreType;
	}
}
