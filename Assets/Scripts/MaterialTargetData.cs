// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialTargetData
{
	public string Id;

	public List<MaterialTarget> Materials = new List<MaterialTarget>();

	public void MarkActive()
	{
		foreach (MaterialTarget current in this.Materials)
		{
			current.MarkActive();
		}
	}

	public void Cache()
	{
		foreach (MaterialTarget current in this.Materials)
		{
			current.Cache();
		}
	}

	public void Restore()
	{
		foreach (MaterialTarget current in this.Materials)
		{
			current.Restore();
		}
	}

	public void SetFloat(string shaderVariableName, float value)
	{
		foreach (MaterialTarget current in this.Materials)
		{
			current.SetFloat(shaderVariableName, value);
		}
	}

	public void SetColor(string shaderVariableName, Color value)
	{
		foreach (MaterialTarget current in this.Materials)
		{
			current.SetColor(shaderVariableName, value);
		}
	}

	public void SetTexture(string shaderVariableName, Texture value)
	{
		foreach (MaterialTarget current in this.Materials)
		{
			current.SetTexture(shaderVariableName, value);
		}
	}

	public void SetMaterial(string shaderVariableName, Material value, bool adoptColorAndTexture = false)
	{
		foreach (MaterialTarget current in this.Materials)
		{
			current.SetMaterial(shaderVariableName, value, adoptColorAndTexture);
		}
	}
}
