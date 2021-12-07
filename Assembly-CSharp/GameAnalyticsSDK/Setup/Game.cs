using System;

namespace GameAnalyticsSDK.Setup
{
	// Token: 0x02000037 RID: 55
	public class Game
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x0000E3BB File Offset: 0x0000C7BB
		public Game(string name, int id, string gameKey, string secretKey)
		{
			this.Name = name;
			this.ID = id;
			this.GameKey = gameKey;
			this.SecretKey = secretKey;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000E3E0 File Offset: 0x0000C7E0
		// (set) Token: 0x060001CB RID: 459 RVA: 0x0000E3E8 File Offset: 0x0000C7E8
		public string Name { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000E3F1 File Offset: 0x0000C7F1
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000E3F9 File Offset: 0x0000C7F9
		public int ID { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000E402 File Offset: 0x0000C802
		// (set) Token: 0x060001CF RID: 463 RVA: 0x0000E40A File Offset: 0x0000C80A
		public string GameKey { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000E413 File Offset: 0x0000C813
		// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000E41B File Offset: 0x0000C81B
		public string SecretKey { get; private set; }
	}
}
