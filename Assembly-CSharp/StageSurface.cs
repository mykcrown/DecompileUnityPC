using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class StageSurface : StageProp, IStageSurface, ITickable, IMovingObject
{
	// Token: 0x170009B7 RID: 2487
	// (get) Token: 0x06002789 RID: 10121 RVA: 0x000C0D1F File Offset: 0x000BF11F
	public Vector3F DeltaPosition
	{
		get
		{
			return this.model.position - this.model.lastPosition;
		}
	}

	// Token: 0x170009B8 RID: 2488
	// (get) Token: 0x0600278A RID: 10122 RVA: 0x000C0D3C File Offset: 0x000BF13C
	public override bool IsSimulation
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170009B9 RID: 2489
	// (get) Token: 0x0600278B RID: 10123 RVA: 0x000C0D3F File Offset: 0x000BF13F
	// (set) Token: 0x0600278C RID: 10124 RVA: 0x000C0D4C File Offset: 0x000BF14C
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

	// Token: 0x170009BA RID: 2490
	// (get) Token: 0x0600278D RID: 10125 RVA: 0x000C0D9D File Offset: 0x000BF19D
	public List<PhysicsCollider> Colliders
	{
		get
		{
			return this.colliders;
		}
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x000C0DA8 File Offset: 0x000BF1A8
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

	// Token: 0x0600278F RID: 10127 RVA: 0x000C0DE8 File Offset: 0x000BF1E8
	public override void TickFrame()
	{
		this.model.lastPosition = this.model.position;
		this.performMovement();
		if (this.DeltaPosition.sqrMagnitude > 0)
		{
			this.UpdateCollisionData();
		}
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x000C0E30 File Offset: 0x000BF230
	protected virtual void performMovement()
	{
		this.model.position = (Vector3F)base.transform.position;
	}

	// Token: 0x170009BB RID: 2491
	// (get) Token: 0x06002791 RID: 10129 RVA: 0x000C0E4D File Offset: 0x000BF24D
	// (set) Token: 0x06002792 RID: 10130 RVA: 0x000C0E66 File Offset: 0x000BF266
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

	// Token: 0x06002793 RID: 10131 RVA: 0x000C0E94 File Offset: 0x000BF294
	public void InitPhysicsColliders()
	{
		this.colliders.Clear();
		this.relativePointsList.Clear();
		this.model.position = (Vector3F)base.transform.position;
		this.model.lastPosition = this.model.position;
		this.calculateCollisionPoints(!this.IsPlatform);
		this.UpdateCollisionData();
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000C0F00 File Offset: 0x000BF300
	public void UpdateCollisionData()
	{
		for (int i = 0; i < this.colliders.Count; i++)
		{
			PhysicsCollider physicsCollider = this.colliders[i];
			Vector2F[] points = this.relativePointsList[i];
			physicsCollider.SetPointsRelative(this.model.position, points);
		}
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x000C0F5C File Offset: 0x000BF35C
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
				Debug.LogError("Unable to find segment that connects to existing edge but there are still available segments remaining.  The mesh may be bad.");
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

	// Token: 0x06002796 RID: 10134 RVA: 0x000C138C File Offset: 0x000BF78C
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

	// Token: 0x06002797 RID: 10135 RVA: 0x000C1428 File Offset: 0x000BF828
	private bool tryFindUsableSegment(Vector3[] vertices, int[] triangles, int index, ref StageSurface.Vector2FSegment segment)
	{
		Vector3[] array = new Vector3[]
		{
			vertices[triangles[index * 3]],
			vertices[triangles[index * 3 + 1]],
			vertices[triangles[index * 3 + 2]]
		};
		ArrayUtil.Sort<Vector3>(array, (Vector3 a, Vector3 b) => a.z < b.z);
		if (array[1].z > 0f)
		{
			segment.pointA = new Vector2F(array[1]);
			segment.pointB = new Vector2F(array[2]);
			return true;
		}
		return false;
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x000C1500 File Offset: 0x000BF900
	public override bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(base.rollbackStatePooling.Clone<StageSurfaceModel>(this.model));
		return true;
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x000C151C File Offset: 0x000BF91C
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

	// Token: 0x0600279A RID: 10138 RVA: 0x000C1590 File Offset: 0x000BF990
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

	// Token: 0x0600279B RID: 10139 RVA: 0x000C160C File Offset: 0x000BFA0C
	public virtual void OnDrawGizmos()
	{
		for (int i = 0; i < this.colliders.Count; i++)
		{
			StageSurface.GizmoDrawCollider(this.colliders[i]);
		}
	}

	// Token: 0x04001CF6 RID: 7414
	private List<Vector2F[]> relativePointsList = new List<Vector2F[]>();

	// Token: 0x04001CF7 RID: 7415
	private List<PhysicsCollider> colliders = new List<PhysicsCollider>();

	// Token: 0x04001CF8 RID: 7416
	private StageSurfaceModel model = new StageSurfaceModel();

	// Token: 0x0200064F RID: 1615
	private struct Vector2FSegment
	{
		// Token: 0x04001CFA RID: 7418
		public Vector2F pointA;

		// Token: 0x04001CFB RID: 7419
		public Vector2F pointB;
	}
}
