using System;
using UnityEngine;

// Token: 0x02000625 RID: 1573
[Serializable]
public class SpawnPointModeData : ISerializationCallbackReceiver
{
	// Token: 0x060026CB RID: 9931 RVA: 0x000BDBA0 File Offset: 0x000BBFA0
	public SpawnPointModeData(SpawnModeType spawnModeType, int pointCount, int setCount = 1)
	{
		this.spawnModeType = spawnModeType;
		this.pointCount = pointCount;
		this.setCount = setCount;
		this.initPointArray<SpawnPoint>(ref this.set0SpawnPoints, pointCount);
		this.initPointArray<SpawnPoint>(ref this.set1SpawnPoints, pointCount);
		this.initPointArray<SpawnPoint>(ref this.set2SpawnPoints, pointCount);
		this.initPointArray<SpawnPoint>(ref this.set3SpawnPoints, pointCount);
		this.initPointArray<SpawnPoint>(ref this.set4SpawnPoints, pointCount);
		this.initPointArray<SpawnPoint>(ref this.set5SpawnPoints, pointCount);
		this.InitForUse();
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x000BDC1C File Offset: 0x000BC01C
	public void InitForUse()
	{
		this.spawnPointSets = new SpawnPoint[6][];
		this.spawnPointSets[0] = this.set0SpawnPoints;
		this.spawnPointSets[1] = this.set1SpawnPoints;
		this.spawnPointSets[2] = this.set2SpawnPoints;
		this.spawnPointSets[3] = this.set3SpawnPoints;
		this.spawnPointSets[4] = this.set4SpawnPoints;
		this.spawnPointSets[5] = this.set5SpawnPoints;
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x000BDC89 File Offset: 0x000BC089
	private void initPointArray<T>(ref T[] points, int pointCount) where T : class
	{
		if (points == null)
		{
			points = new T[pointCount];
		}
		else if (points.Length != pointCount)
		{
			Array.Resize<T>(ref points, pointCount);
		}
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x000BDCB0 File Offset: 0x000BC0B0
	public SpawnPoint GetSpawnPoint(SpawnPointReference reference)
	{
		if (this.spawnPointSets[reference.setIndex].Length <= reference.spawnerIndex)
		{
			return this.spawnPointSets[reference.setIndex][0];
		}
		return this.spawnPointSets[reference.setIndex][reference.spawnerIndex];
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x000BDD00 File Offset: 0x000BC100
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x000BDD02 File Offset: 0x000BC102
	public void OnAfterDeserialize()
	{
		this.InitForUse();
	}

	// Token: 0x04001C47 RID: 7239
	public SpawnModeType spawnModeType;

	// Token: 0x04001C48 RID: 7240
	public int pointCount;

	// Token: 0x04001C49 RID: 7241
	public int setCount;

	// Token: 0x04001C4A RID: 7242
	public bool debugShowPoints;

	// Token: 0x04001C4B RID: 7243
	public SpawnPoint[] set0SpawnPoints;

	// Token: 0x04001C4C RID: 7244
	public SpawnPoint[] set1SpawnPoints;

	// Token: 0x04001C4D RID: 7245
	public SpawnPoint[] set2SpawnPoints;

	// Token: 0x04001C4E RID: 7246
	public SpawnPoint[] set3SpawnPoints;

	// Token: 0x04001C4F RID: 7247
	public SpawnPoint[] set4SpawnPoints;

	// Token: 0x04001C50 RID: 7248
	public SpawnPoint[] set5SpawnPoints;

	// Token: 0x04001C51 RID: 7249
	private const int MAX_SET_COUNT = 6;

	// Token: 0x04001C52 RID: 7250
	public SpawnPoint[][] spawnPointSets;
}
