// Decompile from assembly: Assembly-CSharp.dll

using System;

public class LoginValidationResult
{
	public LoginEntryType type;

	public LoginValidationState state;

	public int value;

	public LoginValidationResult(LoginEntryType type, LoginValidationState state, int value = 0)
	{
		this.type = type;
		this.state = state;
		this.value = value;
	}
}
