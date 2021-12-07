// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserEnteredNameValidator
{
	NameValidationResult CheckOptionsProfile(string text);

	string FixSpaces(string text);

	int GetMinNameLength();

	int GetMaxOptionsProfileNameLength();
}
