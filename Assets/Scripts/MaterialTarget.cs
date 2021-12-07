// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MaterialTarget : IEquatable<MaterialTarget>
{
	public Renderer Renderer;

	public int MaterialIndex;

	private static Dictionary<MaterialTarget, CachedMaterial> CacheCount = new Dictionary<MaterialTarget, CachedMaterial>();

	public static void ResetCache()
	{
		if (MaterialTarget.CacheCount.Count > 0)
		{
		}
		MaterialTarget.CacheCount.Clear();
	}

	public bool Equals(MaterialTarget rhs)
	{
		return rhs != null && this.Renderer == rhs.Renderer && this.MaterialIndex == rhs.MaterialIndex;
	}

	public override bool Equals(object compare)
	{
		return compare is MaterialTarget && this.Equals(compare as MaterialTarget);
	}

	public override int GetHashCode()
	{
		return HashCode.Of<Renderer>(this.Renderer).And<int>(this.MaterialIndex);
	}

	public void MarkActive()
	{
		CachedMaterial cachedMaterial;
		if (this.Renderer.sharedMaterials.Length > this.MaterialIndex && MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial))
		{
			if (cachedMaterial.activeCount == 0)
			{
				Material[] sharedMaterials = this.Renderer.sharedMaterials;
				sharedMaterials[this.MaterialIndex] = ((!cachedMaterial.Material) ? null : new Material(cachedMaterial.Material));
				this.Renderer.sharedMaterials = sharedMaterials;
			}
			cachedMaterial.activeCount++;
		}
	}

	public void Cache()
	{
		if (this.Renderer.sharedMaterials.Length > this.MaterialIndex)
		{
			CachedMaterial value;
			if (MaterialTarget.CacheCount.TryGetValue(this, out value))
			{
				UnityEngine.Debug.LogError("Attempted to cache the same material twice!");
				return;
			}
			if (this.Renderer.sharedMaterials[this.MaterialIndex] != null)
			{
				Material origin = new Material(this.Renderer.sharedMaterials[this.MaterialIndex]);
				value = new CachedMaterial(origin);
				Material[] sharedMaterials = this.Renderer.sharedMaterials;
				this.Renderer.sharedMaterials = sharedMaterials;
				MaterialTarget.CacheCount[this] = value;
			}
			else
			{
				UnityEngine.Debug.LogError("Null material found on: " + this.Renderer.gameObject.GetFullName());
			}
		}
	}

	public void Restore()
	{
		CachedMaterial cachedMaterial;
		if (this.Renderer.sharedMaterials.Length > this.MaterialIndex && MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial))
		{
			if (cachedMaterial.activeCount <= 0)
			{
				UnityEngine.Debug.LogError("Tried to restore a material when there were no active controllers modifying it");
			}
			else
			{
				cachedMaterial.activeCount--;
			}
			Material[] sharedMaterials = this.Renderer.sharedMaterials;
			if (cachedMaterial.activeCount == 0)
			{
				sharedMaterials[this.MaterialIndex] = cachedMaterial.Material;
			}
			this.Renderer.sharedMaterials = sharedMaterials;
		}
	}

	public void SetFloat(string shaderVariableName, float value)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Incorrectly modifying ",
				shaderVariableName,
				" to ",
				value,
				" on original material!"
			}));
		}
		Material[] sharedMaterials = this.Renderer.sharedMaterials;
		if (this.ValidMaterial(this.MaterialIndex))
		{
			if (shaderVariableName == "_Transparency")
			{
				sharedMaterials[this.MaterialIndex].SetOverrideTag("RenderType", "Transparent");
				sharedMaterials[this.MaterialIndex].SetOverrideTag("Queue", "Transparent");
				sharedMaterials[this.MaterialIndex].SetInt("_SrcBlend", 5);
				sharedMaterials[this.MaterialIndex].SetInt("_DstBlend", 10);
				sharedMaterials[this.MaterialIndex].SetInt("_ZWrite", 0);
			}
			sharedMaterials[this.MaterialIndex].SetFloat(shaderVariableName, value);
			this.Renderer.sharedMaterials = sharedMaterials;
		}
	}

	public void SetColor(string shaderVariableName, Color value)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Incorrectly modifying ",
				shaderVariableName,
				" to ",
				value,
				" on original material!"
			}));
		}
		Material[] sharedMaterials = this.Renderer.sharedMaterials;
		if (this.ValidMaterial(this.MaterialIndex))
		{
			sharedMaterials[this.MaterialIndex].SetColor(shaderVariableName, value);
			sharedMaterials[this.MaterialIndex].EnableKeyword("_EMISSION");
			this.Renderer.sharedMaterials = sharedMaterials;
		}
	}

	public void SetTexture(string shaderVariableName, Texture value)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Incorrectly modifying ",
				shaderVariableName,
				" to ",
				value,
				" on original material!"
			}));
		}
		Material[] sharedMaterials = this.Renderer.sharedMaterials;
		if (this.ValidMaterial(this.MaterialIndex))
		{
			sharedMaterials[this.MaterialIndex].SetTexture(shaderVariableName, value);
			this.Renderer.sharedMaterials = sharedMaterials;
		}
	}

	public void SetMaterial(string shaderVariableName, Material value, bool adoptTextureAndColor = false)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Incorrectly modifying ",
				shaderVariableName,
				" to ",
				value,
				" on original material!"
			}));
		}
		if (value && this.ValidMaterial(this.MaterialIndex))
		{
			Material[] sharedMaterials = this.Renderer.sharedMaterials;
			if (adoptTextureAndColor)
			{
				value = new Material(value);
				if (value.HasProperty("_Color") && sharedMaterials[this.MaterialIndex].HasProperty("_Color"))
				{
					value.color = sharedMaterials[this.MaterialIndex].color;
				}
				if (value.HasProperty("_MainTex") && sharedMaterials[this.MaterialIndex].HasProperty("_MainTex"))
				{
					value.mainTexture = sharedMaterials[this.MaterialIndex].mainTexture;
				}
			}
			sharedMaterials[this.MaterialIndex] = value;
			this.Renderer.sharedMaterials = sharedMaterials;
		}
	}

	private bool ValidMaterial(int index)
	{
		return this.Renderer.sharedMaterials.Length > index && this.Renderer.sharedMaterials[index];
	}
}
