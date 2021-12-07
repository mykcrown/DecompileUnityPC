// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public interface IPhysicsCollider
{
	EdgeData Edge
	{
		get;
	}

	bool Enabled
	{
		get;
		set;
	}

	GameObject GameObject
	{
		get;
	}

	int Layer
	{
		get;
	}

	int Mask
	{
		get;
	}

	FixedRect BoundingBox
	{
		get;
	}

	IMovingObject MovingObject
	{
		get;
	}

	bool LayerIntersects(int mask);

	bool ContainsPoint(Vector2F point);

	void SetPoints(params Vector2F[] points);

	void SetPointsRelative(Vector2F rootPosition, Vector2F point1, Vector2F point2, Vector2F point3, Vector2F point4);

	void SetPointsRelative(Vector2F rootPosition, params Vector2F[] points);
}
