// Decompile from assembly: Assembly-CSharp.dll

using Beebyte.Obfuscator;
using System;

[Skip]
public enum LoginValidationState
{
	OK,
	TOO_SHORT,
	INVALID_CHARACTERS,
	INVALID_EMAIL,
	PW_NEED_CAPITAL,
	PW_LENGTH,
	PW_NEED_SYMBOL,
	PW_NEED_NUMBER,
	PW_NO_SPACES,
	MUST_ACCEPT_TERMS
}
