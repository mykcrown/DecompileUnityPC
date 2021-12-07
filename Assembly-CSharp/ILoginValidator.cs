using System;

// Token: 0x02000999 RID: 2457
public interface ILoginValidator
{
	// Token: 0x0600431A RID: 17178
	LoginValidationResult Validate(LoginEntryType type, string name);
}
