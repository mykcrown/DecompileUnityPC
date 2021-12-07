using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class MaterialTargetsData : MonoBehaviour
{
	// Token: 0x060016E6 RID: 5862 RVA: 0x0007BCA4 File Offset: 0x0007A0A4
	public void MarkActive(string Id)
	{
		foreach (MaterialTargetData materialTargetData in this.Targets)
		{
			if (materialTargetData.Id == Id)
			{
				materialTargetData.MarkActive();
			}
		}
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x0007BD10 File Offset: 0x0007A110
	public void MarkAllActive()
	{
		foreach (MaterialTarget materialTarget in this.AllTargets)
		{
			materialTarget.MarkActive();
		}
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x0007BD6C File Offset: 0x0007A16C
	public void Cache(string Id)
	{
		foreach (MaterialTargetData materialTargetData in this.Targets)
		{
			if (materialTargetData.Id == Id)
			{
				materialTargetData.Cache();
			}
		}
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x0007BDD8 File Offset: 0x0007A1D8
	public void CacheAll()
	{
		foreach (MaterialTarget materialTarget in this.AllTargets)
		{
			materialTarget.Cache();
		}
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x0007BE34 File Offset: 0x0007A234
	public void Restore(string Id)
	{
		foreach (MaterialTargetData materialTargetData in this.Targets)
		{
			if (materialTargetData.Id == Id)
			{
				materialTargetData.Restore();
			}
		}
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x0007BEA0 File Offset: 0x0007A2A0
	public void RestoreAll()
	{
		foreach (MaterialTarget materialTarget in this.AllTargets)
		{
			materialTarget.Restore();
		}
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x0007BEFC File Offset: 0x0007A2FC
	public void SetFloat(string id, string shaderVariableName, float value)
	{
		foreach (MaterialTargetData materialTargetData in this.Targets)
		{
			if (materialTargetData.Id == id)
			{
				materialTargetData.SetFloat(shaderVariableName, value);
			}
		}
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x0007BF6C File Offset: 0x0007A36C
	public void SetFloatAll(string shaderVariableName, float value)
	{
		foreach (MaterialTarget materialTarget in this.AllTargets)
		{
			materialTarget.SetFloat(shaderVariableName, value);
		}
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x0007BFCC File Offset: 0x0007A3CC
	public void SetColor(string id, string shaderVariableName, Color value)
	{
		foreach (MaterialTargetData materialTargetData in this.Targets)
		{
			if (materialTargetData.Id == id)
			{
				materialTargetData.SetColor(shaderVariableName, value);
			}
		}
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x0007C03C File Offset: 0x0007A43C
	public void SetColorAll(string shaderVariableName, Color value)
	{
		foreach (MaterialTarget materialTarget in this.AllTargets)
		{
			materialTarget.SetColor(shaderVariableName, value);
		}
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x0007C09C File Offset: 0x0007A49C
	public void SetTexture(string id, string shaderVariableName, Texture value)
	{
		foreach (MaterialTargetData materialTargetData in this.Targets)
		{
			if (materialTargetData.Id == id)
			{
				materialTargetData.SetTexture(shaderVariableName, value);
			}
		}
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x0007C10C File Offset: 0x0007A50C
	public void SetTextureAll(string shaderVariableName, Texture value)
	{
		foreach (MaterialTarget materialTarget in this.AllTargets)
		{
			materialTarget.SetTexture(shaderVariableName, value);
		}
	}

	// Token: 0x060016F2 RID: 5874 RVA: 0x0007C16C File Offset: 0x0007A56C
	public void SetMaterial(string id, string shaderVariableName, Material value)
	{
		foreach (MaterialTargetData materialTargetData in this.Targets)
		{
			if (materialTargetData.Id == id)
			{
				materialTargetData.SetMaterial(shaderVariableName, value, false);
			}
		}
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x0007C1DC File Offset: 0x0007A5DC
	public void SetMaterialAll(string shaderVariableName, Material value, bool adoptColorAndTexture = false)
	{
		foreach (MaterialTarget materialTarget in this.AllTargets)
		{
			materialTarget.SetMaterial(shaderVariableName, value, adoptColorAndTexture);
		}
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x0007C23C File Offset: 0x0007A63C
	public ColorGradientId GradientForId(string id)
	{
		for (int i = 0; i < this.ColorGradients.Count; i++)
		{
			if (this.ColorGradients[i].Id == id)
			{
				return this.ColorGradients[i];
			}
		}
		return MaterialTargetsData.nullGradient;
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x0007C294 File Offset: 0x0007A694
	public Color ColorForId(string id)
	{
		for (int i = 0; i < this.Colors.Count; i++)
		{
			if (this.Colors[i].Id == id)
			{
				return this.Colors[i].Color;
			}
		}
		Color result;
		if (MaterialTargetsData.standardColors.TryGetValue(id, out result))
		{
			return result;
		}
		GameClient.Log(LogLevel.Warning, new object[]
		{
			"MaterialTargets:: No color for id: ",
			id
		});
		return Color.clear;
	}

	// Token: 0x060016F6 RID: 5878 RVA: 0x0007C31C File Offset: 0x0007A71C
	public Texture TextureForId(string id)
	{
		for (int i = 0; i < this.Textures.Count; i++)
		{
			if (this.Textures[i].Id == id)
			{
				return this.Textures[i].Texture;
			}
		}
		Texture result;
		if (MaterialTargetsData.standardTextures.TryGetValue(id, out result))
		{
			return result;
		}
		GameClient.Log(LogLevel.Warning, new object[]
		{
			"MaterialTargets:: No texture for id: ",
			id
		});
		return null;
	}

	// Token: 0x060016F7 RID: 5879 RVA: 0x0007C3A0 File Offset: 0x0007A7A0
	public Material MaterialForId(string id)
	{
		for (int i = 0; i < this.Materials.Count; i++)
		{
			if (this.Materials[i].Id == id)
			{
				return this.Materials[i].Material;
			}
		}
		GameClient.Log(LogLevel.Warning, new object[]
		{
			"MaterialTargets:: No material for id: ",
			id
		});
		return null;
	}

	// Token: 0x060016F8 RID: 5880 RVA: 0x0007C410 File Offset: 0x0007A810
	private void Awake()
	{
		if (MaterialTargetsData.standardTextures == null)
		{
			MaterialTargetsData.standardTextures = new Dictionary<string, Texture>
			{
				{
					"null",
					null
				},
				{
					"white",
					Texture2D.whiteTexture
				},
				{
					"black",
					Texture2D.blackTexture
				}
			};
		}
		this.AllTargets = new List<MaterialTarget>();
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			for (int j = 0; j < renderer.sharedMaterials.Length; j++)
			{
				this.AllTargets.Add(new MaterialTarget
				{
					MaterialIndex = j,
					Renderer = renderer
				});
			}
		}
	}

	// Token: 0x0400119B RID: 4507
	private static ColorGradientId nullGradient = new ColorGradientId();

	// Token: 0x0400119C RID: 4508
	private static Dictionary<string, Color> standardColors = new Dictionary<string, Color>
	{
		{
			"white",
			Color.white
		},
		{
			"black",
			Color.black
		},
		{
			"red",
			Color.red
		},
		{
			"green",
			Color.green
		},
		{
			"blue",
			Color.blue
		},
		{
			"yellow",
			Color.yellow
		},
		{
			"clear",
			Color.clear
		},
		{
			"cyan",
			Color.cyan
		},
		{
			"gray",
			Color.gray
		},
		{
			"magenta",
			Color.magenta
		}
	};

	// Token: 0x0400119D RID: 4509
	private static Dictionary<string, Texture> standardTextures = null;

	// Token: 0x0400119E RID: 4510
	private List<MaterialTarget> AllTargets = new List<MaterialTarget>();

	// Token: 0x0400119F RID: 4511
	public List<MaterialTargetData> Targets = new List<MaterialTargetData>();

	// Token: 0x040011A0 RID: 4512
	public List<ColorId> Colors = new List<ColorId>();

	// Token: 0x040011A1 RID: 4513
	public List<ColorGradientId> ColorGradients = new List<ColorGradientId>();

	// Token: 0x040011A2 RID: 4514
	public List<TextureId> Textures = new List<TextureId>();

	// Token: 0x040011A3 RID: 4515
	public List<MaterialId> Materials = new List<MaterialId>();
}
