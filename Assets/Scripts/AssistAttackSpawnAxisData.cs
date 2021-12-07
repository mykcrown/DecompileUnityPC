// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;

[Serializable]
public class AssistAttackSpawnAxisData
{
	public AssistSpawnMethod spawnMethod = AssistSpawnMethod.Teammate;

	public AssistSpawnOffscreenOption spawnOffscreenOption = AssistSpawnOffscreenOption.Teammmate;

	public Fixed spawnOffset = 0;

	public AssistSpawnFlipOffsetOption spawnFlipOffsetOption = AssistSpawnFlipOffsetOption.Facing;

	public bool enforceMinValue;

	public Fixed minValue = 0;

	public bool enforceMaxValue;

	public Fixed maxValue = 0;
}
