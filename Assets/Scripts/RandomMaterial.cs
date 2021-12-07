// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
	public Renderer targetRenderer;

	public Material[] materials;

	public void Start()
	{
		this.ChangeMaterial();
	}

	public void ChangeMaterial()
	{
		this.targetRenderer.sharedMaterial = this.materials[UnityEngine.Random.Range(0, this.materials.Length)];
	}
}
