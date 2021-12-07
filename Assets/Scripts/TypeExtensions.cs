// Decompile from assembly: Assembly-CSharp.dll

using System;

public static class TypeExtensions
{
	public static bool ImplementsInterface(this Type type, Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new ArgumentException("Expected interface type.");
		}
		return interfaceType.IsAssignableFrom(type);
	}
}
