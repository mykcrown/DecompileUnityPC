// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using System;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class ImplementedBy : Attribute
{
	public Type DefaultType
	{
		get;
		set;
	}

	public InjectionBindingScope Scope
	{
		get;
		set;
	}

	public ImplementedBy(Type t, InjectionBindingScope scope = InjectionBindingScope.SINGLE_CONTEXT)
	{
		this.DefaultType = t;
		this.Scope = scope;
	}
}
