using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
public class VRAMPreloader : IVRAMPreloader
{
	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000E30E8 File Offset: 0x000E14E8
	// (set) Token: 0x06002B83 RID: 11139 RVA: 0x000E30F0 File Offset: 0x000E14F0
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x06002B84 RID: 11140 RVA: 0x000E30F9 File Offset: 0x000E14F9
	public void DebugPreload(Camera myCamera, List<GameObject> preloadObjects, Action callback)
	{
		this.preload(myCamera, preloadObjects, true, callback);
	}

	// Token: 0x06002B85 RID: 11141 RVA: 0x000E3105 File Offset: 0x000E1505
	public void Preload(Camera myCamera, List<GameObject> preloadObjects, Action callback)
	{
		this.preload(myCamera, preloadObjects, false, callback);
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x000E3114 File Offset: 0x000E1514
	private void preload(Camera myCamera, List<GameObject> preloadObjects, bool debug, Action callback)
	{
		List<VRAMPreloader.Item> list = new List<VRAMPreloader.Item>();
		foreach (GameObject gameObject in preloadObjects)
		{
			VRAMPreloader.Item item = default(VRAMPreloader.Item);
			item.obj = gameObject;
			item.originalActive = gameObject.activeSelf;
			item.originalPosition = gameObject.transform.position;
			item.originalScale = gameObject.transform.localScale;
			item.originalParent = gameObject.transform.parent;
			list.Add(item);
		}
		Vector3 position = myCamera.transform.position;
		Vector3 position2 = position;
		position2.z += 20f;
		foreach (VRAMPreloader.Item item2 in list)
		{
			GameObject obj = item2.obj;
			obj.SetActive(true);
			obj.transform.SetParent(null, false);
			obj.transform.position = position2;
		}
		List<ParticleSystemScalingMode> originalModes = null;
		originalModes = new List<ParticleSystemScalingMode>();
		foreach (VRAMPreloader.Item item3 in list)
		{
			item3.obj.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
			this.recursiveParticleScaling(item3.obj.transform, originalModes, false);
		}
		this.timer.NextFrame(delegate
		{
			if (!debug)
			{
				foreach (VRAMPreloader.Item item4 in list)
				{
					this.recursiveParticleScaling(item4.obj.transform, originalModes, true);
					item4.obj.transform.localScale = Vector3.one;
				}
				foreach (VRAMPreloader.Item item5 in list)
				{
					GameObject obj2 = item5.obj;
					obj2.transform.position = item5.originalPosition;
					obj2.transform.SetParent(item5.originalParent, false);
					obj2.SetActive(item5.originalActive);
				}
				callback();
			}
		});
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x000E3330 File Offset: 0x000E1730
	private void recursiveParticleScaling(Transform transform, List<ParticleSystemScalingMode> originalModes, bool restore)
	{
		ParticleSystem component = transform.GetComponent<ParticleSystem>();
		if (component != null)
		{
			ParticleSystem.MainModule main = component.main;
			if (restore)
			{
				ParticleSystemScalingMode scalingMode = originalModes[0];
				originalModes.RemoveAt(0);
				main.scalingMode = scalingMode;
			}
			else
			{
				originalModes.Add(component.main.scalingMode);
				main.scalingMode = ParticleSystemScalingMode.Hierarchy;
			}
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			this.recursiveParticleScaling(transform.GetChild(i), originalModes, restore);
		}
	}

	// Token: 0x020006C7 RID: 1735
	private struct Item
	{
		// Token: 0x04001F13 RID: 7955
		public GameObject obj;

		// Token: 0x04001F14 RID: 7956
		public Vector3 originalPosition;

		// Token: 0x04001F15 RID: 7957
		public Vector3 originalScale;

		// Token: 0x04001F16 RID: 7958
		public bool originalActive;

		// Token: 0x04001F17 RID: 7959
		public Transform originalParent;
	}
}
