// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneUtil
{
	private static Dictionary<string, string> sceneNamesPaths;

	public static void SetRenderersVisbile(bool visible)
	{
		if (!Application.isPlaying)
		{
			UnityEngine.Debug.LogError("This can only be used during playmode to ensure we don't accidentally screw up the state of a scene.");
			return;
		}
		MeshRenderer[] array = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
		SkinnedMeshRenderer[] array2 = UnityEngine.Object.FindObjectsOfType<SkinnedMeshRenderer>();
		ParticleSystem[] array3 = UnityEngine.Object.FindObjectsOfType<ParticleSystem>();
		MeshRenderer[] array4 = array;
		for (int i = 0; i < array4.Length; i++)
		{
			MeshRenderer meshRenderer = array4[i];
			meshRenderer.enabled = visible;
		}
		SkinnedMeshRenderer[] array5 = array2;
		for (int j = 0; j < array5.Length; j++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = array5[j];
			skinnedMeshRenderer.enabled = visible;
		}
		ParticleSystem[] array6 = array3;
		for (int k = 0; k < array6.Length; k++)
		{
			ParticleSystem particleSystem = array6[k];
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

	public static void AssignCheapMaterialToTransforms(List<Transform> searchItems = null)
	{
		if (!Application.isPlaying)
		{
			UnityEngine.Debug.LogError("This can only be used during playmode to ensure we don't accidentally screw up the state of a scene.");
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

	public static void AssignCheapMaterialToRenderers(List<Renderer> renderers, Material defaultMaterial = null, Dictionary<Texture, Material> sharedMaterialCache = null)
	{
		if (!Application.isPlaying)
		{
			UnityEngine.Debug.LogError("This can only be used during playmode to ensure we don't accidentally screw up the state of a scene.");
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
		foreach (Renderer current in renderers)
		{
			if (!(current.sharedMaterial == null) && !(current.GetComponent<MaterialQualitySwitcher>() != null))
			{
				if (current is ParticleSystemRenderer)
				{
					if (current.sharedMaterial.shader.name.ToLower().Contains("additive"))
					{
						SceneUtil.ApplyToRenderer(current, ref dictionary, ref material2, ref stringBuilder);
					}
					else
					{
						SceneUtil.ApplyToRenderer(current, ref dictionary, ref material, ref stringBuilder);
					}
				}
				else
				{
					SceneUtil.ApplyToRenderer(current, ref sharedMaterialCache, ref defaultMaterial, ref stringBuilder);
				}
			}
		}
		string text = stringBuilder.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			UnityEngine.Debug.LogWarning(text);
		}
	}

	public static List<T> FindObjectsOfTypeAll<T>()
	{
		List<T> list = new List<T>();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene sceneAt = SceneManager.GetSceneAt(i);
			if (sceneAt.isLoaded)
			{
				GameObject[] rootGameObjects = sceneAt.GetRootGameObjects();
				for (int j = 0; j < rootGameObjects.Length; j++)
				{
					GameObject gameObject = rootGameObjects[j];
					list.AddRange(gameObject.GetComponentsInChildren<T>(true));
				}
			}
		}
		return list;
	}

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
}
