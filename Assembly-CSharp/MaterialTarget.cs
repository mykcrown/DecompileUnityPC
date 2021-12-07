using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044C RID: 1100
[Serializable]
public class MaterialTarget : IEquatable<MaterialTarget>
{
	// Token: 0x060016CB RID: 5835 RVA: 0x0007B316 File Offset: 0x00079716
	public static void ResetCache()
	{
		if (MaterialTarget.CacheCount.Count > 0)
		{
		}
		MaterialTarget.CacheCount.Clear();
	}

	// Token: 0x060016CC RID: 5836 RVA: 0x0007B332 File Offset: 0x00079732
	public bool Equals(MaterialTarget rhs)
	{
		return rhs != null && this.Renderer == rhs.Renderer && this.MaterialIndex == rhs.MaterialIndex;
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x0007B364 File Offset: 0x00079764
	public override bool Equals(object compare)
	{
		return compare is MaterialTarget && this.Equals(compare as MaterialTarget);
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x0007B380 File Offset: 0x00079780
	public override int GetHashCode()
	{
		return HashCode.Of<Renderer>(this.Renderer).And<int>(this.MaterialIndex);
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x0007B3AC File Offset: 0x000797AC
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

	// Token: 0x060016D0 RID: 5840 RVA: 0x0007B440 File Offset: 0x00079840
	public void Cache()
	{
		if (this.Renderer.sharedMaterials.Length > this.MaterialIndex)
		{
			CachedMaterial value;
			if (MaterialTarget.CacheCount.TryGetValue(this, out value))
			{
				Debug.LogError("Attempted to cache the same material twice!");
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
				Debug.LogError("Null material found on: " + this.Renderer.gameObject.GetFullName());
			}
		}
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x0007B508 File Offset: 0x00079908
	public void Restore()
	{
		CachedMaterial cachedMaterial;
		if (this.Renderer.sharedMaterials.Length > this.MaterialIndex && MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial))
		{
			if (cachedMaterial.activeCount <= 0)
			{
				Debug.LogError("Tried to restore a material when there were no active controllers modifying it");
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

	// Token: 0x060016D2 RID: 5842 RVA: 0x0007B5A0 File Offset: 0x000799A0
	public void SetFloat(string shaderVariableName, float value)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			Debug.Log(string.Concat(new object[]
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

	// Token: 0x060016D3 RID: 5843 RVA: 0x0007B6B0 File Offset: 0x00079AB0
	public void SetColor(string shaderVariableName, Color value)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			Debug.Log(string.Concat(new object[]
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

	// Token: 0x060016D4 RID: 5844 RVA: 0x0007B75C File Offset: 0x00079B5C
	public void SetTexture(string shaderVariableName, Texture value)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			Debug.Log(string.Concat(new object[]
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

	// Token: 0x060016D5 RID: 5845 RVA: 0x0007B7F0 File Offset: 0x00079BF0
	public void SetMaterial(string shaderVariableName, Material value, bool adoptTextureAndColor = false)
	{
		CachedMaterial cachedMaterial;
		if (MaterialTarget.CacheCount.TryGetValue(this, out cachedMaterial) && cachedMaterial.activeCount <= 0)
		{
			Debug.Log(string.Concat(new object[]
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

	// Token: 0x060016D6 RID: 5846 RVA: 0x0007B90A File Offset: 0x00079D0A
	private bool ValidMaterial(int index)
	{
		return this.Renderer.sharedMaterials.Length > index && this.Renderer.sharedMaterials[index];
	}

	// Token: 0x0400118D RID: 4493
	public Renderer Renderer;

	// Token: 0x0400118E RID: 4494
	public int MaterialIndex;

	// Token: 0x0400118F RID: 4495
	private static Dictionary<MaterialTarget, CachedMaterial> CacheCount = new Dictionary<MaterialTarget, CachedMaterial>();
}
