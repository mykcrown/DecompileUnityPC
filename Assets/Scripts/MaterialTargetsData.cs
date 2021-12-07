// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTargetsData : MonoBehaviour
{
	private static ColorGradientId nullGradient = new ColorGradientId();

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

	private static Dictionary<string, Texture> standardTextures = null;

	private List<MaterialTarget> AllTargets = new List<MaterialTarget>();

	public List<MaterialTargetData> Targets = new List<MaterialTargetData>();

	public List<ColorId> Colors = new List<ColorId>();

	public List<ColorGradientId> ColorGradients = new List<ColorGradientId>();

	public List<TextureId> Textures = new List<TextureId>();

	public List<MaterialId> Materials = new List<MaterialId>();

	public void MarkActive(string Id)
	{
		foreach (MaterialTargetData current in this.Targets)
		{
			if (current.Id == Id)
			{
				current.MarkActive();
			}
		}
	}

	public void MarkAllActive()
	{
		foreach (MaterialTarget current in this.AllTargets)
		{
			current.MarkActive();
		}
	}

	public void Cache(string Id)
	{
		foreach (MaterialTargetData current in this.Targets)
		{
			if (current.Id == Id)
			{
				current.Cache();
			}
		}
	}

	public void CacheAll()
	{
		foreach (MaterialTarget current in this.AllTargets)
		{
			current.Cache();
		}
	}

	public void Restore(string Id)
	{
		foreach (MaterialTargetData current in this.Targets)
		{
			if (current.Id == Id)
			{
				current.Restore();
			}
		}
	}

	public void RestoreAll()
	{
		foreach (MaterialTarget current in this.AllTargets)
		{
			current.Restore();
		}
	}

	public void SetFloat(string id, string shaderVariableName, float value)
	{
		foreach (MaterialTargetData current in this.Targets)
		{
			if (current.Id == id)
			{
				current.SetFloat(shaderVariableName, value);
			}
		}
	}

	public void SetFloatAll(string shaderVariableName, float value)
	{
		foreach (MaterialTarget current in this.AllTargets)
		{
			current.SetFloat(shaderVariableName, value);
		}
	}

	public void SetColor(string id, string shaderVariableName, Color value)
	{
		foreach (MaterialTargetData current in this.Targets)
		{
			if (current.Id == id)
			{
				current.SetColor(shaderVariableName, value);
			}
		}
	}

	public void SetColorAll(string shaderVariableName, Color value)
	{
		foreach (MaterialTarget current in this.AllTargets)
		{
			current.SetColor(shaderVariableName, value);
		}
	}

	public void SetTexture(string id, string shaderVariableName, Texture value)
	{
		foreach (MaterialTargetData current in this.Targets)
		{
			if (current.Id == id)
			{
				current.SetTexture(shaderVariableName, value);
			}
		}
	}

	public void SetTextureAll(string shaderVariableName, Texture value)
	{
		foreach (MaterialTarget current in this.AllTargets)
		{
			current.SetTexture(shaderVariableName, value);
		}
	}

	public void SetMaterial(string id, string shaderVariableName, Material value)
	{
		foreach (MaterialTargetData current in this.Targets)
		{
			if (current.Id == id)
			{
				current.SetMaterial(shaderVariableName, value, false);
			}
		}
	}

	public void SetMaterialAll(string shaderVariableName, Material value, bool adoptColorAndTexture = false)
	{
		foreach (MaterialTarget current in this.AllTargets)
		{
			current.SetMaterial(shaderVariableName, value, adoptColorAndTexture);
		}
	}

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
		Renderer[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Renderer renderer = array[i];
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
}
