// Decompile from assembly: Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class Inject : Attribute
{
	public object name
	{
		get;
		set;
	}

	public Inject()
	{
	}

	public Inject(object n)
	{
		this.name = n;
	}
}
