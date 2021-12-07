// Decompile from assembly: Assembly-CSharp.dll

using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class PostConstruct : Attribute
{
	public int priority
	{
		get;
		set;
	}

	public PostConstruct()
	{
	}

	public PostConstruct(int p)
	{
		this.priority = p;
	}
}
