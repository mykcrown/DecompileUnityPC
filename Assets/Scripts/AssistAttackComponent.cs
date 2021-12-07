// Decompile from assembly: Assembly-CSharp.dll

using System;

public class AssistAttackComponent : MoveComponent
{
	public AssistAttackSpawnAxisData spawnXData = new AssistAttackSpawnAxisData();

	public AssistAttackSpawnAxisData spawnYData = new AssistAttackSpawnAxisData();

	public AssistAttackSpawnAxisData spawnZData = new AssistAttackSpawnAxisData();

	public bool assistAbsorbsHits;

	public AssistTargetOption targetOption;

	public int overrideAssistFrames;

	public override void Init(IMoveDelegate moveDelegate, IPlayerDelegate playerDelegate, InputButtonsData input)
	{
	}
}
