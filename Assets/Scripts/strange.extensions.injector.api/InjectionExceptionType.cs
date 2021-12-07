// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace strange.extensions.injector.api
{
	public enum InjectionExceptionType
	{
		CIRCULAR_DEPENDENCY,
		ILLEGAL_BINDING_VALUE,
		NO_BINDER,
		NO_REFLECTOR,
		NO_FACTORY,
		NOT_INSTANTIABLE,
		NULL_BINDING,
		NULL_CONSTRUCTOR,
		NULL_INJECTION_POINT,
		NULL_REFLECTION,
		NULL_TARGET,
		NULL_VALUE_INJECTION,
		SETTER_NAME_MISMATCH,
		MISSING_CROSS_CONTEXT_INJECTOR,
		IMPLICIT_BINDING_IMPLEMENTOR_DOES_NOT_IMPLEMENT_INTERFACE,
		IMPLICIT_BINDING_TYPE_DOES_NOT_IMPLEMENT_DESIGNATED_INTERFACE,
		UNINITIALIZED_ASSEMBLY
	}
}
