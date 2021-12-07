using System;
using UnityEngine;

// Token: 0x02000628 RID: 1576
public class StageSpawnModeData : MonoBehaviour
{
	// Token: 0x060026D2 RID: 9938 RVA: 0x000BDD12 File Offset: 0x000BC112
	public SpawnPointModeData GetSpawnPointModeData(SpawnModeType modeType)
	{
		return this.spawnModes[(int)modeType];
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x000BDD1C File Offset: 0x000BC11C
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

	// Token: 0x060026D4 RID: 9940 RVA: 0x000BDDB4 File Offset: 0x000BC1B4
	private void OnDrawGizmos()
	{
		if (this.spawnModes == null)
		{
			return;
		}
		foreach (SpawnPointModeData spawnPointModeData in this.spawnModes)
		{
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
						foreach (SpawnPoint point in spawnPointModeData.spawnPointSets[j])
						{
							this.drawSpawnPoint(point, color);
						}
					}
				}
			}
		}
		if (this.drawRespawnGizmos)
		{
			foreach (RespawnPoint point2 in this.respawnPoints)
			{
				this.drawRespawnPoint(point2, new Color(1f, 0.5f, 0f));
			}
		}
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x000BDEC8 File Offset: 0x000BC2C8
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

	// Token: 0x060026D6 RID: 9942 RVA: 0x000BDF2E File Offset: 0x000BC32E
	private void drawSpawnPoint(SpawnPoint point, Color color)
	{
		if (point == null)
		{
			return;
		}
		this.drawPositionAndFacing(point.transform.position, point.FacingDirection, color, true, 0.33f, 0.5f);
	}

	// Token: 0x060026D7 RID: 9943 RVA: 0x000BDF60 File Offset: 0x000BC360
	private void drawRespawnPoint(RespawnPoint point, Color color)
	{
		if (point == null)
		{
			return;
		}
		this.drawPositionAndFacing(point.transform.position, point.FacingDirection, color, false, 0.33f, 0.5f);
	}

	// Token: 0x04001C5E RID: 7262
	public SpawnPointModeData[] spawnModes;

	// Token: 0x04001C5F RID: 7263
	public RespawnPoint[] respawnPoints;

	// Token: 0x04001C60 RID: 7264
	public bool drawRespawnGizmos;

	// Token: 0x04001C61 RID: 7265
	private static readonly Color[] modeColors = new Color[]
	{
		Color.red,
		Color.blue,
		Color.yellow,
		Color.green
	};
}
