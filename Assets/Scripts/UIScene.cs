// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class UIScene : MonoBehaviour
{
	public GameObject VisualContainer;

	protected Camera myCamera;

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		get;
		set;
	}

	[Inject]
	public IVideoSettingsUtility videoSettingsUtility
	{
		get;
		set;
	}

	protected virtual void Awake()
	{
		this.myCamera = base.GetComponentInChildren<Camera>();
	}

	private void OnEnable()
	{
		if (this.videoSettingsUtility != null)
		{
			this.videoSettingsUtility.ApplyToCamera(this.myCamera);
		}
		this.onEnable();
	}

	protected virtual void onEnable()
	{
	}

	private void OnDisable()
	{
		this.onDisable();
	}

	protected virtual void onDisable()
	{
	}
}
