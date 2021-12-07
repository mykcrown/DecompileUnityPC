using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044D RID: 1101
[Serializable]
public class MaterialTargetData
{
	// Token: 0x060016D9 RID: 5849 RVA: 0x0007B954 File Offset: 0x00079D54
	public void MarkActive()
	{
		foreach (MaterialTarget materialTarget in this.Materials)
		{
			materialTarget.MarkActive();
		}
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x0007B9B0 File Offset: 0x00079DB0
	public void Cache()
	{
		foreach (MaterialTarget materialTarget in this.Materials)
		{
			materialTarget.Cache();
		}
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x0007BA0C File Offset: 0x00079E0C
	public void Restore()
	{
		foreach (MaterialTarget materialTarget in this.Materials)
		{
			materialTarget.Restore();
		}
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x0007BA68 File Offset: 0x00079E68
	public void SetFloat(string shaderVariableName, float value)
	{
		foreach (MaterialTarget materialTarget in this.Materials)
		{
			materialTarget.SetFloat(shaderVariableName, value);
		}
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x0007BAC8 File Offset: 0x00079EC8
	public void SetColor(string shaderVariableName, Color value)
	{
		foreach (MaterialTarget materialTarget in this.Materials)
		{
			materialTarget.SetColor(shaderVariableName, value);
		}
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x0007BB28 File Offset: 0x00079F28
	public void SetTexture(string shaderVariableName, Texture value)
	{
		foreach (MaterialTarget materialTarget in this.Materials)
		{
			materialTarget.SetTexture(shaderVariableName, value);
		}
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x0007BB88 File Offset: 0x00079F88
	public void SetMaterial(string shaderVariableName, Material value, bool adoptColorAndTexture = false)
	{
		foreach (MaterialTarget materialTarget in this.Materials)
		{
			materialTarget.SetMaterial(shaderVariableName, value, adoptColorAndTexture);
		}
	}

	// Token: 0x04001190 RID: 4496
	public string Id;

	// Token: 0x04001191 RID: 4497
	public List<MaterialTarget> Materials = new List<MaterialTarget>();
}
