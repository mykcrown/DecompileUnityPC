using System;

// Token: 0x0200066F RID: 1647
public interface IUserEnteredNameValidator
{
	// Token: 0x060028B7 RID: 10423
	NameValidationResult CheckOptionsProfile(string text);

	// Token: 0x060028B8 RID: 10424
	string FixSpaces(string text);

	// Token: 0x060028B9 RID: 10425
	int GetMinNameLength();

	// Token: 0x060028BA RID: 10426
	int GetMaxOptionsProfileNameLength();
}
