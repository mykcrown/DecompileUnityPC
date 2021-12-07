using System;

// Token: 0x0200049C RID: 1180
public class GameModeDataType : Attribute
{
	// Token: 0x060019F9 RID: 6649 RVA: 0x00085F5D File Offset: 0x0008435D
	public GameModeDataType(GameMode type)
	{
		this.type = type;
	}

	// Token: 0x04001353 RID: 4947
	public readonly GameMode type;
}
