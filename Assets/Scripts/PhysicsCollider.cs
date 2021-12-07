// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class PhysicsCollider : IPhysicsCollider
{
	public EdgeData Edge
	{
		get;
		private set;
	}

	public bool Enabled
	{
		get;
		set;
	}

	public GameObject GameObject
	{
		get;
		private set;
	}

	public FixedRect BoundingBox
	{
		get
		{
			return this.Edge.BoundingBox;
		}
	}

	public int Layer
	{
		get;
		set;
	}

	public IMovingObject MovingObject
	{
		get;
		private set;
	}

	public Vector2F Center
	{
		get
		{
			return this.BoundingBox.Center;
		}
	}

	public int Mask
	{
		get
		{
			return 1 << this.Layer;
		}
	}

	public PhysicsCollider(GameObject gameObject, Vector2F[] points, bool isLoop, IMovingObject movingObject)
	{
		this.Edge = new EdgeData(points, isLoop, EdgeData.CacheFlag.All);
		this.MovingObject = movingObject;
		this.Enabled = true;
		this.GameObject = gameObject;
		this.Layer = gameObject.layer;
	}

	public PhysicsCollider(GameObject gameObject, Vector2F rootPosition, Vector2F[] points, bool isLoop, IMovingObject movingObject)
	{
		this.Edge = new EdgeData(rootPosition, points, isLoop, EdgeData.CacheFlag.All);
		this.MovingObject = movingObject;
		this.Enabled = true;
		this.GameObject = gameObject;
		this.Layer = gameObject.layer;
	}

	public PhysicsCollider(EdgeData edgeData, int layer = 0)
	{
		this.Edge = edgeData;
		this.Enabled = true;
		this.Layer = layer;
	}

	public bool LayerIntersects(int mask)
	{
		return Layers.Intersects(this.Layer, mask);
	}

	public bool ContainsPoint(Vector2F point)
	{
		if (this.Edge.IsLoop && this.BoundingBox.ContainsPoint(point))
		{
			Vector2F vector2F = this.BoundingBox.TopRight + Vector2F.one;
			Vector2F zero = Vector2F.zero;
			Fixed maxDistance = 0;
			(point - vector2F).Decompose(ref zero, ref maxDistance);
			int num = PhysicsWorld.RaycastCollider(this, vector2F, zero, maxDistance, this.Mask, null, RaycastFlags.EnableMultipleHits, default(Fixed));
			return num % 2 == 1;
		}
		return false;
	}

	public void SetPoints(params Vector2F[] points)
	{
		this.Edge.SetPoints(points);
	}

	public void SetPointsRelative(Vector2F rootPosition, Vector2F point1, Vector2F point2, Vector2F point3, Vector2F point4)
	{
		this.Edge.SetPointsRelative(rootPosition, point1, point2, point3, point4);
	}

	public void SetPointsRelative(Vector2F rootPosition, params Vector2F[] points)
	{
		this.Edge.SetPointsRelative(rootPosition, points);
	}
}
