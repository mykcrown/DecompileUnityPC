using System;

// Token: 0x020004C8 RID: 1224
public class AssistAttackComponent : MoveComponent
{
	// Token: 0x06001B16 RID: 6934 RVA: 0x0008A364 File Offset: 0x00088764
	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
	}

	// Token: 0x0400145A RID: 5210
	public AssistAttackSpawnAxisData spawnXData = new AssistAttackSpawnAxisData();

	// Token: 0x0400145B RID: 5211
	public AssistAttackSpawnAxisData spawnYData = new AssistAttackSpawnAxisData();

	// Token: 0x0400145C RID: 5212
	public AssistAttackSpawnAxisData spawnZData = new AssistAttackSpawnAxisData();

	// Token: 0x0400145D RID: 5213
	public bool assistAbsorbsHits;

	// Token: 0x0400145E RID: 5214
	public AssistTargetOption targetOption;

	// Token: 0x0400145F RID: 5215
	public int overrideAssistFrames;
}
