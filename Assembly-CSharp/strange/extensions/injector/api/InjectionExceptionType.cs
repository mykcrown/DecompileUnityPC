using System;

namespace strange.extensions.injector.api
{
	// Token: 0x0200024A RID: 586
	public enum InjectionExceptionType
	{
		// Token: 0x0400075F RID: 1887
		CIRCULAR_DEPENDENCY,
		// Token: 0x04000760 RID: 1888
		ILLEGAL_BINDING_VALUE,
		// Token: 0x04000761 RID: 1889
		NO_BINDER,
		// Token: 0x04000762 RID: 1890
		NO_REFLECTOR,
		// Token: 0x04000763 RID: 1891
		NO_FACTORY,
		// Token: 0x04000764 RID: 1892
		NOT_INSTANTIABLE,
		// Token: 0x04000765 RID: 1893
		NULL_BINDING,
		// Token: 0x04000766 RID: 1894
		NULL_CONSTRUCTOR,
		// Token: 0x04000767 RID: 1895
		NULL_INJECTION_POINT,
		// Token: 0x04000768 RID: 1896
		NULL_REFLECTION,
		// Token: 0x04000769 RID: 1897
		NULL_TARGET,
		// Token: 0x0400076A RID: 1898
		NULL_VALUE_INJECTION,
		// Token: 0x0400076B RID: 1899
		SETTER_NAME_MISMATCH,
		// Token: 0x0400076C RID: 1900
		MISSING_CROSS_CONTEXT_INJECTOR,
		// Token: 0x0400076D RID: 1901
		IMPLICIT_BINDING_IMPLEMENTOR_DOES_NOT_IMPLEMENT_INTERFACE,
		// Token: 0x0400076E RID: 1902
		IMPLICIT_BINDING_TYPE_DOES_NOT_IMPLEMENT_DESIGNATED_INTERFACE,
		// Token: 0x0400076F RID: 1903
		UNINITIALIZED_ASSEMBLY
	}
}
