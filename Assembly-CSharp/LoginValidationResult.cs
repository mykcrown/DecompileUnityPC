using System;

// Token: 0x0200099A RID: 2458
public class LoginValidationResult
{
	// Token: 0x0600431B RID: 17179 RVA: 0x001299D6 File Offset: 0x00127DD6
	public LoginValidationResult(LoginEntryType type, LoginValidationState state, int value = 0)
	{
		this.type = type;
		this.state = state;
		this.value = value;
	}

	// Token: 0x04002CB7 RID: 11447
	public LoginEntryType type;

	// Token: 0x04002CB8 RID: 11448
	public LoginValidationState state;

	// Token: 0x04002CB9 RID: 11449
	public int value;
}
