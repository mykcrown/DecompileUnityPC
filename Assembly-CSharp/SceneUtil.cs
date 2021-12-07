using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000B5B RID: 2907
public static class SceneUtil
{
	// Token: 0x0600542D RID: 21549 RVA: 0x001B1298 File Offset: 0x001AF698
	public static void SetRenderersVisbile(bool visible)
	{
		if (!Application.isPlaying)
		{
			Debug.LogError("This can only be used during playmode to ensure we don't accidentally screw up the state of a scene.");
			return;
		}
		MeshRenderer[] array = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
		SkinnedMeshRenderer[] array2 = UnityEngine.Object.FindObjectsOfType<SkinnedMeshRenderer>();
		ParticleSystem[] array3 = UnityEngine.Object.FindObjectsOfType<ParticleSystem>();
		foreach (MeshRenderer meshRenderer in array)
		{
			meshRenderer.enabled = visible;
		}
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array2)
		{
			skinnedMeshRenderer.enabled = visible;
		}
		foreach (ParticleSystem particleSystem in array3)
		{
			if (!visible)
			{
				particleSystem.Stop(true);
				particleSystem.Clear(true);
			}
			else
			{
				particleSystem.Play(true);
			}
		}
	}

	// Token: 0x0600542E RID: 21550 RVA: 0x001B1368 File Offset: 0x001AF768
	private static void ApplyToRenderer(Renderer system, ref Dictionary<Texture, Material> sharedMaterialCache, ref Material defaultMaterial, ref StringBuilder errors)
	{
		Material[] sharedMaterials = system.sharedMaterials;
		for (int i = 0; i < system.sharedMaterials.Length; i++)
		{
			if (!(sharedMaterials[i] == null))
			{
				try
				{
					if (sharedMaterials[i].HasProperty("_MainTex"))
					{
						Material material = null;
						if (sharedMaterials[i].mainTexture != null && !sharedMaterialCache.TryGetValue(sharedMaterials[i].mainTexture, out material))
						{
							defaultMaterial.mainTexture = sharedMaterials[i].mainTexture;
							if (sharedMaterials[i].HasProperty("_Color"))
							{
								defaultMaterial.color = sharedMaterials[i].color;
							}
							material = new Material(defaultMaterial);
							sharedMaterialCache.Add(material.mainTexture, material);
						}
						sharedMaterials[i] = material;
					}
				}
				catch (Exception)
				{
					errors.Append("Failed to assign material to [").Append(system.name).Append("]\n");
				}
			}
		}
		system.sharedMaterials = sharedMaterials;
	}

	// Token: 0x0600542F RID: 21551 RVA: 0x001B1474 File Offset: 0x001AF874
	public static void AssignCheapMaterialToTransforms(List<Transform> searchItems = null)
	{
		if (!Application.isPlaying)
		{
			Debug.LogError("This can only be used during playmode to ensure we don't accidentally screw up the state of a scene.");
			return;
		}
		Dictionary<Texture, Material> sharedMaterialCache = new Dictionary<Texture, Material>();
		if (searchItems != null)
		{
			for (int i = 0; i < searchItems.Count; i++)
			{
				SceneUtil.AssignCheapMaterialToRenderers(searchItems[i].GetComponentsInChildren<Renderer>(true).ToList<Renderer>(), null, sharedMaterialCache);
			}
		}
	}

	// Token: 0x06005430 RID: 21552 RVA: 0x001B14D4 File Offset: 0x001AF8D4
	public static void AssignCheapMaterialToRenderers(List<Renderer> renderers, Material defaultMaterial = null, Dictionary<Texture, Material> sharedMaterialCache = null)
	{
		if (!Application.isPlaying)
		{
			Debug.LogError("This can only be used during playmode to ensure we don't accidentally screw up the state of a scene.");
			return;
		}
		if (sharedMaterialCache == null)
		{
			sharedMaterialCache = new Dictionary<Texture, Material>();
		}
		if (defaultMaterial == null)
		{
			defaultMaterial = new Material(Shader.Find("Unlit/Texture"));
		}
		Dictionary<Texture, Material> dictionary = new Dictionary<Texture, Material>();
		Material material = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
		Material material2 = new Material(Shader.Find("Mobile/Particles/Additive"));
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Renderer renderer in renderers)
		{
			if (!(renderer.sharedMaterial == null) && !(renderer.GetComponent<MaterialQualitySwitcher>() != null))
			{
				if (renderer is ParticleSystemRenderer)
				{
					if (renderer.sharedMaterial.shader.name.ToLower().Contains("additive"))
					{
						SceneUtil.ApplyToRenderer(renderer, ref dictionary, ref material2, ref stringBuilder);
					}
					else
					{
						SceneUtil.ApplyToRenderer(renderer, ref dictionary, ref material, ref stringBuilder);
					}
				}
				else
				{
					SceneUtil.ApplyToRenderer(renderer, ref sharedMaterialCache, ref defaultMaterial, ref stringBuilder);
				}
			}
		}
		string text = stringBuilder.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			Debug.LogWarning(text);
		}
	}

	// Token: 0x06005431 RID: 21553 RVA: 0x001B1638 File Offset: 0x001AFA38
	public static List<T> FindObjectsOfTypeAll<T>()
	{
		List<T> list = new List<T>();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene sceneAt = SceneManager.GetSceneAt(i);
			if (sceneAt.isLoaded)
			{
				foreach (GameObject gameObject in sceneAt.GetRootGameObjects())
				{
					list.AddRange(gameObject.GetComponentsInChildren<T>(true));
				}
			}
		}
		return list;
	}

	// Token: 0x06005432 RID: 21554 RVA: 0x001B16AC File Offset: 0x001AFAAC
	public static Dictionary<string, string> GetSceneNamePathMap()
	{
		if (SceneUtil.sceneNamesPaths == null)
		{
			SceneUtil.sceneNamesPaths = new Dictionary<string, string>();
			for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				string scenePathByBuildIndex = SceneUtility.GetScenePathByBuildIndex(i);
				int num = scenePathByBuildIndex.LastIndexOf("/");
				string key = scenePathByBuildIndex.Substring(num + 1, scenePathByBuildIndex.LastIndexOf(".") - num - 1);
				SceneUtil.sceneNamesPaths.Add(key, scenePathByBuildIndex);
			}
		}
		return SceneUtil.sceneNamesPaths;
	}

	// Token: 0x0400355A RID: 13658
	private static Dictionary<string, string> sceneNamesPaths;
}
