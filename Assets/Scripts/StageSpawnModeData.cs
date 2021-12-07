// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class StageSpawnModeData : MonoBehaviour
{
	public SpawnPointModeData[] spawnModes;

	public RespawnPoint[] respawnPoints;

	public bool drawRespawnGizmos;

	private static readonly Color[] modeColors = new Color[]
	{
		Color.red,
		Color.blue,
		Color.yellow,
		Color.green
	};

	public SpawnPointModeData GetSpawnPointModeData(SpawnModeType modeType)
	{
		return this.spawnModes[(int)modeType];
	}

	public void ResetSpawnModeData()
	{
		this.spawnModes = new SpawnPointModeData[7];
		this.spawnModes[0] = new SpawnPointModeData(SpawnModeType.FFA_2P, 2, 1);
		this.spawnModes[1] = new SpawnPointModeData(SpawnModeType.FFA_3P, 3, 1);
		this.spawnModes[2] = new SpawnPointModeData(SpawnModeType.FFA_4P, 4, 1);
		this.spawnModes[3] = new SpawnPointModeData(SpawnModeType.FFA_5P, 5, 1);
		this.spawnModes[4] = new SpawnPointModeData(SpawnModeType.FFA_6P, 6, 1);
		this.spawnModes[5] = new SpawnPointModeData(SpawnModeType.TeamFFA_2v2, 2, 2);
		this.spawnModes[6] = new SpawnPointModeData(SpawnModeType.TeamFFA_3v3, 3, 2);
		this.respawnPoints = new RespawnPoint[4];
	}

	private void OnDrawGizmos()
	{
		if (this.spawnModes == null)
		{
			return;
		}
		SpawnPointModeData[] array = this.spawnModes;
		for (int i = 0; i < array.Length; i++)
		{
			SpawnPointModeData spawnPointModeData = array[i];
			if (spawnPointModeData != null && spawnPointModeData.debugShowPoints)
			{
				for (int j = 0; j < spawnPointModeData.setCount; j++)
				{
					Color color = Color.cyan;
					if (spawnPointModeData.setCount > 1)
					{
						color = StageSpawnModeData.modeColors[j];
					}
					if (spawnPointModeData.spawnPointSets != null)
					{
						SpawnPoint[] array2 = spawnPointModeData.spawnPointSets[j];
						for (int k = 0; k < array2.Length; k++)
						{
							SpawnPoint point = array2[k];
							this.drawSpawnPoint(point, color);
						}
					}
				}
			}
		}
		if (this.drawRespawnGizmos)
		{
			RespawnPoint[] array3 = this.respawnPoints;
			for (int l = 0; l < array3.Length; l++)
			{
				RespawnPoint point2 = array3[l];
				this.drawRespawnPoint(point2, new Color(1f, 0.5f, 0f));
			}
		}
	}

	private void drawPositionAndFacing(Vector3 position, HorizontalDirection facing, Color color, bool useWire, float pointSize = 0.5f, float arrowLength = 1f)
	{
		Gizmos.color = color;
		if (useWire)
		{
			Gizmos.DrawWireSphere(position, pointSize);
		}
		else
		{
			Gizmos.DrawSphere(position, pointSize);
		}
		Vector3 a = (facing != HorizontalDirection.Right) ? Vector3.left : Vector3.right;
		GizmoUtil.GizmosDrawArrow(position, position + a * arrowLength, color, false, 0f, 33f);
	}

	private void drawSpawnPoint(SpawnPoint point, Color color)
	{
		if (point == null)
		{
			return;
		}
		this.drawPositionAndFacing(point.transform.position, point.FacingDirection, color, true, 0.33f, 0.5f);
	}

	private void drawRespawnPoint(RespawnPoint point, Color color)
	{
		if (point == null)
		{
			return;
		}
		this.drawPositionAndFacing(point.transform.position, point.FacingDirection, color, false, 0.33f, 0.5f);
	}
}
