// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Xft
{
	public class VertexPool
	{
		public class VertexSegment
		{
			public int VertStart;

			public int IndexStart;

			public int VertCount;

			public int IndexCount;

			public VertexPool Pool;

			public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
			{
				this.VertStart = start;
				this.VertCount = count;
				this.IndexCount = icount;
				this.IndexStart = istart;
				this.Pool = pool;
			}

			public void ClearIndices()
			{
				for (int i = this.IndexStart; i < this.IndexStart + this.IndexCount; i++)
				{
					this.Pool.Indices[i] = 0;
				}
				this.Pool.IndiceChanged = true;
			}
		}

		public Vector3[] Vertices;

		public int[] Indices;

		public Vector2[] UVs;

		public Color[] Colors;

		public bool IndiceChanged;

		public bool ColorChanged;

		public bool UVChanged;

		public bool VertChanged;

		public bool UV2Changed;

		protected int VertexTotal;

		protected int VertexUsed;

		protected int IndexTotal;

		protected int IndexUsed;

		public bool FirstUpdate = true;

		protected bool VertCountChanged;

		public const int BlockSize = 108;

		public float BoundsScheduleTime = 1f;

		public float ElapsedTime;

		protected XWeaponTrail _owner;

		protected MeshFilter _meshFilter;

		protected Mesh _mesh2d;

		protected Material _material;

		public Mesh MyMesh
		{
			get
			{
				if (!this._owner.UseWith2D)
				{
					return this._mesh2d;
				}
				if (this._meshFilter == null || this._meshFilter.gameObject == null)
				{
					return null;
				}
				return this._meshFilter.sharedMesh;
			}
		}

		public VertexPool(Material material, XWeaponTrail owner)
		{
			this.VertexTotal = (this.VertexUsed = 0);
			this.VertCountChanged = false;
			this._owner = owner;
			if (owner.UseWith2D)
			{
				this.CreateMeshObj(owner, material);
			}
			else
			{
				this._mesh2d = new Mesh();
			}
			this._material = material;
			this.InitArrays();
			this.IndiceChanged = (this.ColorChanged = (this.UVChanged = (this.UV2Changed = (this.VertChanged = true))));
		}

		public void RecalculateBounds()
		{
			this.MyMesh.RecalculateBounds();
		}

		private void CreateMeshObj(XWeaponTrail owner, Material material)
		{
			GameObject gameObject = new GameObject("_XWeaponTrailMesh:|material:" + material.name);
			gameObject.layer = owner.gameObject.layer;
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			this._meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
			MeshRenderer meshRenderer = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshRenderer.GetComponent<Renderer>().sharedMaterial = material;
			meshRenderer.sortingLayerName = this._owner.SortingLayerName;
			meshRenderer.sortingOrder = this._owner.SortingOrder;
			this._meshFilter.sharedMesh = new Mesh();
		}

		public void Destroy()
		{
			if (!this._owner.UseWith2D)
			{
				UnityEngine.Object.DestroyImmediate(this._mesh2d);
			}
			else
			{
				UnityEngine.Object.Destroy(this._meshFilter.gameObject);
			}
		}

		public VertexPool.VertexSegment GetVertices(int vcount, int icount)
		{
			int num = 0;
			int num2 = 0;
			if (this.VertexUsed + vcount >= this.VertexTotal)
			{
				num = (vcount / 108 + 1) * 108;
			}
			if (this.IndexUsed + icount >= this.IndexTotal)
			{
				num2 = (icount / 108 + 1) * 108;
			}
			this.VertexUsed += vcount;
			this.IndexUsed += icount;
			if (num != 0 || num2 != 0)
			{
				this.EnlargeArrays(num, num2);
				this.VertexTotal += num;
				this.IndexTotal += num2;
			}
			return new VertexPool.VertexSegment(this.VertexUsed - vcount, vcount, this.IndexUsed - icount, icount, this);
		}

		protected void InitArrays()
		{
			this.Vertices = new Vector3[4];
			this.UVs = new Vector2[4];
			this.Colors = new Color[4];
			this.Indices = new int[6];
			this.VertexTotal = 4;
			this.IndexTotal = 6;
		}

		public void EnlargeArrays(int count, int icount)
		{
			Vector3[] vertices = this.Vertices;
			this.Vertices = new Vector3[this.Vertices.Length + count];
			vertices.CopyTo(this.Vertices, 0);
			Vector2[] uVs = this.UVs;
			this.UVs = new Vector2[this.UVs.Length + count];
			uVs.CopyTo(this.UVs, 0);
			Color[] colors = this.Colors;
			this.Colors = new Color[this.Colors.Length + count];
			colors.CopyTo(this.Colors, 0);
			int[] indices = this.Indices;
			this.Indices = new int[this.Indices.Length + icount];
			indices.CopyTo(this.Indices, 0);
			this.VertCountChanged = true;
			this.IndiceChanged = true;
			this.ColorChanged = true;
			this.UVChanged = true;
			this.VertChanged = true;
			this.UV2Changed = true;
		}

		public void LateUpdate()
		{
			if (this.VertCountChanged)
			{
				this.MyMesh.Clear();
			}
			this.MyMesh.vertices = this.Vertices;
			if (this.UVChanged)
			{
				this.MyMesh.uv = this.UVs;
			}
			if (this.ColorChanged)
			{
				this.MyMesh.colors = this.Colors;
			}
			if (this.IndiceChanged)
			{
				this.MyMesh.triangles = this.Indices;
			}
			this.ElapsedTime += Time.deltaTime;
			if (this.ElapsedTime > this.BoundsScheduleTime || this.FirstUpdate)
			{
				this.RecalculateBounds();
				this.ElapsedTime = 0f;
			}
			if (this.ElapsedTime > this.BoundsScheduleTime)
			{
				this.FirstUpdate = false;
			}
			this.VertCountChanged = false;
			this.IndiceChanged = false;
			this.ColorChanged = false;
			this.UVChanged = false;
			this.UV2Changed = false;
			this.VertChanged = false;
			if (!this._owner.UseWith2D)
			{
				Graphics.DrawMesh(this.MyMesh, Matrix4x4.identity, this._material, this._owner.gameObject.layer, null, 0, null, false, false);
			}
		}
	}
}
