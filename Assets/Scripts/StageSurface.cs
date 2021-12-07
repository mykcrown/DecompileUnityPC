// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StageSurface : StageProp, IStageSurface, ITickable, IMovingObject
{
	private struct Vector2FSegment
	{
		public Vector2F pointA;

		public Vector2F pointB;
	}

	private List<Vector2F[]> relativePointsList = new List<Vector2F[]>();

	private List<PhysicsCollider> colliders = new List<PhysicsCollider>();

	private StageSurfaceModel model = new StageSurfaceModel();

	private static ArrayUtil.LessThanDelegate<Vector3> __f__am_cache0;

	public Vector3F DeltaPosition
	{
		get
		{
			return this.model.position - this.model.lastPosition;
		}
	}

	public override bool IsSimulation
	{
		get
		{
			return true;
		}
	}

	public bool CollidersEnabled
	{
		get
		{
			return this.model.collidersEnabled;
		}
		set
		{
			this.model.collidersEnabled = value;
			for (int i = 0; i < this.colliders.Count; i++)
			{
				this.colliders[i].Enabled = this.model.collidersEnabled;
			}
		}
	}

	public List<PhysicsCollider> Colliders
	{
		get
		{
			return this.colliders;
		}
	}

	public bool IsPlatform
	{
		get
		{
			return base.gameObject.layer == LayerMask.NameToLayer(Layers.Platform);
		}
		set
		{
			base.gameObject.layer = ((!value) ? base.gameObject.layer : LayerMask.NameToLayer(Layers.Platform));
		}
	}

	public override void Awake()
	{
		base.Awake();
		Rigidbody component = base.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.Sleep();
		}
		this.model.shouldValidate = this.IsSimulation;
	}

	public override void TickFrame()
	{
		this.model.lastPosition = this.model.position;
		this.performMovement();
		if (this.DeltaPosition.sqrMagnitude > 0)
		{
			this.UpdateCollisionData();
		}
	}

	protected virtual void performMovement()
	{
		this.model.position = (Vector3F)base.transform.position;
	}

	public void InitPhysicsColliders()
	{
		this.colliders.Clear();
		this.relativePointsList.Clear();
		this.model.position = (Vector3F)base.transform.position;
		this.model.lastPosition = this.model.position;
		this.calculateCollisionPoints(!this.IsPlatform);
		this.UpdateCollisionData();
	}

	public void UpdateCollisionData()
	{
		for (int i = 0; i < this.colliders.Count; i++)
		{
			PhysicsCollider physicsCollider = this.colliders[i];
			Vector2F[] points = this.relativePointsList[i];
			physicsCollider.SetPointsRelative(this.model.position, points);
		}
	}

	private void calculateCollisionPoints(bool isLoop)
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		Mesh sharedMesh = component.sharedMesh;
		Vector3[] vertices = sharedMesh.vertices;
		int[] triangles = sharedMesh.triangles;
		List<StageSurface.Vector2FSegment> list = new List<StageSurface.Vector2FSegment>();
		int i = 0;
		int num = triangles.Length / 3;
		while (i < num)
		{
			StageSurface.Vector2FSegment item = default(StageSurface.Vector2FSegment);
			if (this.tryFindUsableSegment(vertices, triangles, i, ref item))
			{
				list.Add(item);
			}
			i++;
		}
		if (list.Count == 0)
		{
			return;
		}
		List<Vector2F> list2 = new List<Vector2F>();
		list2.Add(list[0].pointA);
		list2.Add(list[0].pointB);
		list.RemoveAt(0);
		Vector2F rhs = list2[0];
		Vector2F rhs2 = list2[1];
		while (list.Count > 0)
		{
			int num2 = -1;
			int j = 0;
			while (j < list.Count)
			{
				if (list[j].pointA == rhs)
				{
					list2.Insert(0, list[j].pointB);
					rhs = list2[0];
				}
				else if (list[j].pointB == rhs)
				{
					list2.Insert(0, list[j].pointA);
					rhs = list2[0];
				}
				else if (list[j].pointA == rhs2)
				{
					list2.Add(list[j].pointB);
					rhs2 = list2[list2.Count - 1];
				}
				else
				{
					if (!(list[j].pointB == rhs2))
					{
						j++;
						continue;
					}
					list2.Add(list[j].pointA);
					rhs2 = list2[list2.Count - 1];
				}
				num2 = j;
				break;
			}
			if (num2 < 0)
			{
				UnityEngine.Debug.LogError("Unable to find segment that connects to existing edge but there are still available segments remaining.  The mesh may be bad.");
				break;
			}
			list.RemoveAt(num2);
		}
		if (isLoop)
		{
			list2.RemoveAt(list2.Count - 1);
		}
		this.relativePointsList.Add(list2.ToArray());
		Vector2F[] array = this.relativePointsList[0];
		if (!this.arePointsClockwise(array))
		{
			Array.Reverse(array);
		}
		int num3 = 0;
		Fixed x = array[num3].x;
		for (int k = 1; k < array.Length; k++)
		{
			if (array[k].x < x)
			{
				x = array[k].x;
				num3 = k;
			}
		}
		ArrayUtil.RotateLeft<Vector2F>(array, num3);
		Vector2F b = new Vector2F((Fixed)((double)base.transform.localScale.x), (Fixed)((double)base.transform.localScale.y));
		for (int l = 0; l < array.Length; l++)
		{
			array[l] *= b;
		}
		PhysicsCollider item2 = new PhysicsCollider(base.gameObject, this.model.position, array, isLoop, this);
		this.colliders.Add(item2);
		if (this.IsPlatform)
		{
			this.relativePointsList.Add(new Vector2F[this.relativePointsList[0].Length]);
			Vector2F[] array2 = this.relativePointsList[1];
			Array.Copy(array, array2, array.Length);
			Array.Reverse(array2);
			PhysicsCollider physicsCollider = new PhysicsCollider(base.gameObject, this.model.position, array2, isLoop, this);
			physicsCollider.Layer = PhysicsSimulator.PlatformUndersideLayer;
			this.colliders.Add(physicsCollider);
		}
	}

	private bool arePointsClockwise(Vector2F[] points)
	{
		Fixed one = 0;
		for (int i = 0; i < points.Length; i++)
		{
			Vector2F vector2F = points[i];
			Vector2F vector2F2 = (i != points.Length - 1) ? points[i + 1] : points[0];
			one += (vector2F2.x - vector2F.x) * (vector2F2.y + vector2F.y);
		}
		return one >= 0;
	}

	private bool tryFindUsableSegment(Vector3[] vertices, int[] triangles, int index, ref StageSurface.Vector2FSegment segment)
	{
		Vector3[] array = new Vector3[]
		{
			vertices[triangles[index * 3]],
			vertices[triangles[index * 3 + 1]],
			vertices[triangles[index * 3 + 2]]
		};
		Vector3[] arg_7D_0 = array;
		if (StageSurface.__f__am_cache0 == null)
		{
			StageSurface.__f__am_cache0 = new ArrayUtil.LessThanDelegate<Vector3>(StageSurface._tryFindUsableSegment_m__0);
		}
		ArrayUtil.Sort<Vector3>(arg_7D_0, StageSurface.__f__am_cache0);
		if (array[1].z > 0f)
		{
			segment.pointA = new Vector2F(array[1]);
			segment.pointB = new Vector2F(array[2]);
			return true;
		}
		return false;
	}

	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<StageSurfaceModel>(this.model));
		return true;
	}

	public override bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<StageSurfaceModel>(ref this.model);
		base.transform.position = (Vector3)this.model.position;
		this.UpdateCollisionData();
		for (int i = 0; i < this.colliders.Count; i++)
		{
			this.colliders[i].Enabled = this.model.collidersEnabled;
		}
		return true;
	}

	private static void GizmoDrawCollider(PhysicsCollider collider)
	{
		if (collider.Enabled)
		{
			Color color = Color.cyan;
			if (collider.Layer == PhysicsSimulator.PlatformUndersideLayer)
			{
				color = Color.yellow;
			}
			EdgeData edge = collider.Edge;
			Color edgeColor = color;
			Color red = Color.red;
			Color wallColor = new Color(0f, 0.5f, 1f);
			GizmoUtil.GizmoDrawEdgeData(edge, edgeColor, default(Color), red, default(Color), true, wallColor, Color.blue);
		}
	}

	public virtual void OnDrawGizmos()
	{
		for (int i = 0; i < this.colliders.Count; i++)
		{
			StageSurface.GizmoDrawCollider(this.colliders[i]);
		}
	}

	private static bool _tryFindUsableSegment_m__0(Vector3 a, Vector3 b)
	{
		return a.z < b.z;
	}
}
