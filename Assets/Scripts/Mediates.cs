// Decompile from assembly: Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class Mediates : Attribute
{
	public Type ViewType
	{
		get;
		set;
	}

	public Mediates(Type t)
	{
		this.ViewType = t;
	}
}
