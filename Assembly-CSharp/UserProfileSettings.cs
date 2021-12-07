using System;
using UnityEngine;

// Token: 0x02000410 RID: 1040
[Serializable]
public class UserProfileSettings : ScriptableObject
{
	// Token: 0x040010A6 RID: 4262
	public int minNameLength = 4;

	// Token: 0x040010A7 RID: 4263
	public int minPWLength = 8;

	// Token: 0x040010A8 RID: 4264
	public int maxNameLength = 16;

	// Token: 0x040010A9 RID: 4265
	public int maxOptionProfileNameLength = 18;

	// Token: 0x040010AA RID: 4266
	public int maxOptionProfiles = 4;
}
