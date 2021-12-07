using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class RandomMaterial : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x000021D2 File Offset: 0x000005D2
	public void Start()
	{
		this.ChangeMaterial();
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000021DA File Offset: 0x000005DA
	public void ChangeMaterial()
	{
		this.targetRenderer.sharedMaterial = this.materials[UnityEngine.Random.Range(0, this.materials.Length)];
	}

	// Token: 0x04000001 RID: 1
	public Renderer targetRenderer;

	// Token: 0x04000002 RID: 2
	public Material[] materials;
}
