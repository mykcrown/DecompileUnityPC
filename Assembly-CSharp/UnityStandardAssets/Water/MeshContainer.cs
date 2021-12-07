using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
	// Token: 0x02000B9A RID: 2970
	public class MeshContainer
	{
		// Token: 0x060055CE RID: 21966 RVA: 0x001B6351 File Offset: 0x001B4751
		public MeshContainer(Mesh m)
		{
			this.mesh = m;
			this.vertices = m.vertices;
			this.normals = m.normals;
		}

		// Token: 0x060055CF RID: 21967 RVA: 0x001B6378 File Offset: 0x001B4778
		public void Update()
		{
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
		}

		// Token: 0x0400367C RID: 13948
		public Mesh mesh;

		// Token: 0x0400367D RID: 13949
		public Vector3[] vertices;

		// Token: 0x0400367E RID: 13950
		public Vector3[] normals;
	}
}
