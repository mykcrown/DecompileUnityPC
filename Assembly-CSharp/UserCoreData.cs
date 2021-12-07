using System;

// Token: 0x02000A8B RID: 2699
public class UserCoreData : IUserCoreData
{
	// Token: 0x170012BD RID: 4797
	// (get) Token: 0x06004F08 RID: 20232 RVA: 0x0014B21A File Offset: 0x0014961A
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170012BE RID: 4798
	// (get) Token: 0x06004F09 RID: 20233 RVA: 0x0014B222 File Offset: 0x00149622
	public string Username
	{
		get
		{
			return this.username;
		}
	}

	// Token: 0x0400338B RID: 13195
	public string id;

	// Token: 0x0400338C RID: 13196
	public string username;
}
