// Decompile from assembly: Assembly-CSharp.dll

using System;

public class ReferenceValue<T>
{
	public T Value
	{
		get;
		set;
	}

	public ReferenceValue()
	{
	}

	public ReferenceValue(T value)
	{
		this.Value = value;
	}

	public ReferenceValue(ReferenceValue<T> value)
	{
		this.Value = value.Value;
	}
}
