// Decompile from assembly: Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
public class Name : Attribute
{
	public object name
	{
		get;
		set;
	}

	public Name(object n)
	{
		this.name = n;
	}
}
