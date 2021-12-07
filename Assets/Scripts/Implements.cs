// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class Implements : Attribute
{
	public object Name
	{
		get;
		set;
	}

	public Type DefaultInterface
	{
		get;
		set;
	}

	public InjectionBindingScope Scope
	{
		get;
		set;
	}

	public Implements()
	{
	}

	public Implements(InjectionBindingScope scope)
	{
		this.Scope = scope;
	}

	public Implements(Type t, InjectionBindingScope scope = InjectionBindingScope.SINGLE_CONTEXT)
	{
		this.DefaultInterface = t;
		this.Scope = scope;
	}

	public Implements(InjectionBindingScope scope, object name)
	{
		this.Scope = scope;
		this.Name = name;
	}

	public Implements(Type t, InjectionBindingScope scope, object name)
	{
		this.DefaultInterface = t;
		this.Name = name;
		this.Scope = scope;
	}
}
