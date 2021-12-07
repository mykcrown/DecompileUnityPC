using System;

// Token: 0x02000B6F RID: 2927
public static class TypeExtensions
{
	// Token: 0x060054AC RID: 21676 RVA: 0x001B2C13 File Offset: 0x001B1013
	public static bool ImplementsInterface(this Type type, Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new ArgumentException("Expected interface type.");
		}
		return interfaceType.IsAssignableFrom(type);
	}
}
