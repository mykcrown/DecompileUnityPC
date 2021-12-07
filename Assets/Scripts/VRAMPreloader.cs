// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VRAMPreloader : IVRAMPreloader
{
	private struct Item
	{
		public GameObject obj;

		public Vector3 originalPosition;

		public Vector3 originalScale;

		public bool originalActive;

		public Transform originalParent;
	}

	private sealed class _preload_c__AnonStorey0
	{
		internal bool debug;

		internal List<VRAMPreloader.Item> list;

		internal List<ParticleSystemScalingMode> originalModes;

		internal Action callback;

		internal VRAMPreloader _this;

		internal void __m__0()
		{
			if (!this.debug)
			{
				foreach (VRAMPreloader.Item current in this.list)
				{
					this._this.recursiveParticleScaling(current.obj.transform, this.originalModes, true);
					current.obj.transform.localScale = Vector3.one;
				}
				foreach (VRAMPreloader.Item current2 in this.list)
				{
					GameObject obj = current2.obj;
					obj.transform.position = current2.originalPosition;
					obj.transform.SetParent(current2.originalParent, false);
					obj.SetActive(current2.originalActive);
				}
				this.callback();
			}
		}
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	public void DebugPreload(Camera myCamera, List<GameObject> preloadObjects, Action callback)
	{
		this.preload(myCamera, preloadObjects, true, callback);
	}

	public void Preload(Camera myCamera, List<GameObject> preloadObjects, Action callback)
	{
		this.preload(myCamera, preloadObjects, false, callback);
	}

	private void preload(Camera myCamera, List<GameObject> preloadObjects, bool debug, Action callback)
	{
		VRAMPreloader._preload_c__AnonStorey0 _preload_c__AnonStorey = new VRAMPreloader._preload_c__AnonStorey0();
		_preload_c__AnonStorey.debug = debug;
		_preload_c__AnonStorey.callback = callback;
		_preload_c__AnonStorey._this = this;
		_preload_c__AnonStorey.list = new List<VRAMPreloader.Item>();
		foreach (GameObject current in preloadObjects)
		{
			VRAMPreloader.Item item = default(VRAMPreloader.Item);
			item.obj = current;
			item.originalActive = current.activeSelf;
			item.originalPosition = current.transform.position;
			item.originalScale = current.transform.localScale;
			item.originalParent = current.transform.parent;
			_preload_c__AnonStorey.list.Add(item);
		}
		Vector3 position = myCamera.transform.position;
		Vector3 position2 = position;
		position2.z += 20f;
		foreach (VRAMPreloader.Item current2 in _preload_c__AnonStorey.list)
		{
			GameObject obj = current2.obj;
			obj.SetActive(true);
			obj.transform.SetParent(null, false);
			obj.transform.position = position2;
		}
		_preload_c__AnonStorey.originalModes = null;
		_preload_c__AnonStorey.originalModes = new List<ParticleSystemScalingMode>();
		foreach (VRAMPreloader.Item current3 in _preload_c__AnonStorey.list)
		{
			current3.obj.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
			this.recursiveParticleScaling(current3.obj.transform, _preload_c__AnonStorey.originalModes, false);
		}
		this.timer.NextFrame(new Action(_preload_c__AnonStorey.__m__0));
	}

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
}
