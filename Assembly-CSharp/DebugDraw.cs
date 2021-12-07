using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200092B RID: 2347
public class DebugDraw
{
	// Token: 0x17000E9E RID: 3742
	// (get) Token: 0x06003D2A RID: 15658 RVA: 0x0011AACA File Offset: 0x00118ECA
	public static DebugDraw Instance
	{
		get
		{
			if (DebugDraw.instance == null)
			{
				DebugDraw.instance = new DebugDraw();
			}
			return DebugDraw.instance;
		}
	}

	// Token: 0x17000E9F RID: 3743
	// (get) Token: 0x06003D2B RID: 15659 RVA: 0x0011AAE5 File Offset: 0x00118EE5
	// (set) Token: 0x06003D2C RID: 15660 RVA: 0x0011AAED File Offset: 0x00118EED
	public IEvents Events { get; set; }

	// Token: 0x17000EA0 RID: 3744
	// (get) Token: 0x06003D2D RID: 15661 RVA: 0x0011AAF6 File Offset: 0x00118EF6
	// (set) Token: 0x06003D2E RID: 15662 RVA: 0x0011AB00 File Offset: 0x00118F00
	public DebugDrawChannel ActiveChannels
	{
		get
		{
			return this.activeChannels;
		}
		set
		{
			DebugDrawChannel[] values = EnumUtil.GetValues<DebugDrawChannel>();
			foreach (DebugDrawChannel debugDrawChannel in values)
			{
				if ((value & debugDrawChannel) != (this.activeChannels & debugDrawChannel))
				{
					this.SetChannelActive(debugDrawChannel, (value & debugDrawChannel) != DebugDrawChannel.None);
				}
			}
		}
	}

	// Token: 0x17000EA1 RID: 3745
	// (get) Token: 0x06003D2F RID: 15663 RVA: 0x0011AB4D File Offset: 0x00118F4D
	// (set) Token: 0x06003D30 RID: 15664 RVA: 0x0011AB55 File Offset: 0x00118F55
	public int GridWidth
	{
		get
		{
			return this.gridWidth;
		}
		set
		{
			this.gridWidth = value;
		}
	}

	// Token: 0x17000EA2 RID: 3746
	// (get) Token: 0x06003D31 RID: 15665 RVA: 0x0011AB5E File Offset: 0x00118F5E
	// (set) Token: 0x06003D32 RID: 15666 RVA: 0x0011AB66 File Offset: 0x00118F66
	public int GridHeight
	{
		get
		{
			return this.gridHeight;
		}
		set
		{
			this.gridHeight = value;
		}
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x0011AB6F File Offset: 0x00118F6F
	public bool IsChannelActive(DebugDrawChannel channel)
	{
		return (this.activeChannels & channel) != DebugDrawChannel.None;
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x0011AB7F File Offset: 0x00118F7F
	public string getDebugString()
	{
		return this.activeChannels.ToString();
	}

	// Token: 0x06003D35 RID: 15669 RVA: 0x0011AB94 File Offset: 0x00118F94
	public void CreateArrow(Vector2F origin, Vector2F direction, float length, Color color, int lifespanFrames)
	{
		if (GizmoUtil.AreGizmosVisible)
		{
			DebugArrow debugArrow = new GameObject("DebugArrow").AddComponent<DebugArrow>();
			Vector3 vector = (Vector3)origin;
			Vector3 end = vector + (Vector3)direction * length;
			debugArrow.Init(vector, end, color, lifespanFrames);
		}
	}

	// Token: 0x06003D36 RID: 15670 RVA: 0x0011ABE4 File Offset: 0x00118FE4
	public void CreateCircle(Vector2F center, float radius, Color color, int lifespanFrames)
	{
		if (GizmoUtil.AreGizmosVisible)
		{
			DebugCircle debugCircle = new GameObject("DebugCircle").AddComponent<DebugCircle>();
			debugCircle.Init((Vector2)center, radius, color, lifespanFrames);
		}
	}

	// Token: 0x06003D37 RID: 15671 RVA: 0x0011AC1C File Offset: 0x0011901C
	public void CreateTrajectoryRenderer(IPlayerDelegate character, PhysicsModel templateModel, PlayerPhysicsModel playerTemplateModel, PhysicsSimulator globalPhysics, int lifespanFrames, Color color)
	{
		TrajectoryRenderer trajectoryRenderer = new GameObject("Trajectory Renderer").AddComponent<TrajectoryRenderer>();
		trajectoryRenderer.Init(character, character as IPhysicsDelegate, templateModel, playerTemplateModel, globalPhysics, lifespanFrames, color);
	}

	// Token: 0x06003D38 RID: 15672 RVA: 0x0011AC50 File Offset: 0x00119050
	public void SetChannelActive(DebugDrawChannel channel, bool active)
	{
		DebugDrawChannel debugDrawChannel = this.activeChannels;
		if (active)
		{
			this.activeChannels |= channel;
		}
		else
		{
			this.activeChannels &= ~channel;
		}
		if (this.Events != null && debugDrawChannel != this.activeChannels)
		{
			this.Events.Broadcast(new ToggleDebugDrawChannelCommand(channel, active));
		}
	}

	// Token: 0x06003D39 RID: 15673 RVA: 0x0011ACB8 File Offset: 0x001190B8
	public void DrawGridGizmos()
	{
		int num = -(this.gridHeight / 2);
		int num2 = this.gridHeight / 2;
		int num3 = -(this.gridWidth / 2);
		int num4 = this.gridWidth / 2;
		for (int i = num; i <= num2; i++)
		{
			Color color = (i != 0) ? ((i % 5 != 0) ? new Color(0.5f, 0.5f, 0.5f, 0.5f) : Color.white) : Color.red;
			Vector3 start = new Vector3((float)num3, (float)i, 0f);
			Vector3 end = new Vector3((float)num4, (float)i, 0f);
			GizmoUtil.GizmosDrawLine(start, end, color);
		}
		for (int j = num3; j <= num4; j++)
		{
			Color color2 = (j != 0) ? ((j % 5 != 0) ? new Color(0.5f, 0.5f, 0.5f, 0.5f) : Color.white) : Color.red;
			Vector3 start2 = new Vector3((float)j, (float)num2, 0f);
			Vector3 end2 = new Vector3((float)j, (float)num, 0f);
			GizmoUtil.GizmosDrawLine(start2, end2, color2);
		}
	}

	// Token: 0x040029CB RID: 10699
	private static DebugDraw instance;

	// Token: 0x040029CD RID: 10701
	private DebugDrawChannel activeChannels = DebugDrawChannel.Bounds;

	// Token: 0x040029CE RID: 10702
	private int gridWidth = 40;

	// Token: 0x040029CF RID: 10703
	private int gridHeight = 20;

	// Token: 0x0200092C RID: 2348
	public struct KeyCodeComparer : IEqualityComparer<KeyCode>
	{
		// Token: 0x06003D3A RID: 15674 RVA: 0x0011ADEF File Offset: 0x001191EF
		public bool Equals(KeyCode x, KeyCode y)
		{
			return x == y;
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x0011ADF5 File Offset: 0x001191F5
		public int GetHashCode(KeyCode obj)
		{
			return (int)obj;
		}
	}
}
