// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DebugDraw
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct KeyCodeComparer : IEqualityComparer<KeyCode>
	{
		public bool Equals(KeyCode x, KeyCode y)
		{
			return x == y;
		}

		public int GetHashCode(KeyCode obj)
		{
			return (int)obj;
		}
	}

	private static DebugDraw instance;

	private DebugDrawChannel activeChannels = DebugDrawChannel.Bounds;

	private int gridWidth = 40;

	private int gridHeight = 20;

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

	public IEvents Events
	{
		get;
		set;
	}

	public DebugDrawChannel ActiveChannels
	{
		get
		{
			return this.activeChannels;
		}
		set
		{
			DebugDrawChannel[] values = EnumUtil.GetValues<DebugDrawChannel>();
			DebugDrawChannel[] array = values;
			for (int i = 0; i < array.Length; i++)
			{
				DebugDrawChannel debugDrawChannel = array[i];
				if ((value & debugDrawChannel) != (this.activeChannels & debugDrawChannel))
				{
					this.SetChannelActive(debugDrawChannel, (value & debugDrawChannel) != DebugDrawChannel.None);
				}
			}
		}
	}

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

	public bool IsChannelActive(DebugDrawChannel channel)
	{
		return (this.activeChannels & channel) != DebugDrawChannel.None;
	}

	public string getDebugString()
	{
		return this.activeChannels.ToString();
	}

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

	public void CreateCircle(Vector2F center, float radius, Color color, int lifespanFrames)
	{
		if (GizmoUtil.AreGizmosVisible)
		{
			DebugCircle debugCircle = new GameObject("DebugCircle").AddComponent<DebugCircle>();
			debugCircle.Init((Vector2)center, radius, color, lifespanFrames);
		}
	}

	public void CreateTrajectoryRenderer(IPlayerDelegate character, PhysicsModel templateModel, PlayerPhysicsModel playerTemplateModel, PhysicsSimulator globalPhysics, int lifespanFrames, Color color)
	{
		TrajectoryRenderer trajectoryRenderer = new GameObject("Trajectory Renderer").AddComponent<TrajectoryRenderer>();
		trajectoryRenderer.Init(character, character as IPhysicsDelegate, templateModel, playerTemplateModel, globalPhysics, lifespanFrames, color);
	}

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
}
