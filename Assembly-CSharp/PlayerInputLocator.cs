using System;

// Token: 0x020006AD RID: 1709
public class PlayerInputLocator
{
	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x06002A8D RID: 10893 RVA: 0x000E0B62 File Offset: 0x000DEF62
	// (set) Token: 0x06002A8E RID: 10894 RVA: 0x000E0B6A File Offset: 0x000DEF6A
	public PlayerInputManager Input { get; private set; }

	// Token: 0x06002A8F RID: 10895 RVA: 0x000E0B73 File Offset: 0x000DEF73
	public void Set(PlayerInputManager input)
	{
		this.Input = input;
	}
}
