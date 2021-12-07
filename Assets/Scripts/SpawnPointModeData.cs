// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class SpawnPointModeData : ISerializationCallbackReceiver
{
	public SpawnModeType spawnModeType;

	public int pointCount;

	public int setCount;

	public bool debugShowPoints;

	public SpawnPoint[] set0SpawnPoints;

	public SpawnPoint[] set1SpawnPoints;

	public SpawnPoint[] set2SpawnPoints;

	public SpawnPoint[] set3SpawnPoints;

	public SpawnPoint[] set4SpawnPoints;

	public SpawnPoint[] set5SpawnPoints;

	private const int MAX_SET_COUNT = 6;

	public SpawnPoint[][] spawnPointSets;

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

	public SpawnPoint GetSpawnPoint(SpawnPointReference reference)
	{
		if (this.spawnPointSets[reference.setIndex].Length <= reference.spawnerIndex)
		{
			return this.spawnPointSets[reference.setIndex][0];
		}
		return this.spawnPointSets[reference.setIndex][reference.spawnerIndex];
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		this.InitForUse();
	}
}
