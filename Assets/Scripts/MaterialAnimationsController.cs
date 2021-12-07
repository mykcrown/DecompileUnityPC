// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class MaterialAnimationsController
{
	private IMoveOwner moveOwner;

	private MaterialTargetsData targets;

	private FixedCapacityList<MaterialAnimationController> animations = new FixedCapacityList<MaterialAnimationController>(100);

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	public bool Overridden
	{
		get;
		private set;
	}

	public void Init(IMoveOwner moveOwner, MaterialTargetsData targets)
	{
		this.moveOwner = moveOwner;
		this.targets = targets;
		this.Overridden = false;
		targets.CacheAll();
	}

	public void OverrideAllMaterials(Material material)
	{
		this.Overridden = true;
		this.targets.MarkAllActive();
		this.targets.SetMaterialAll(material.shader.name, material, true);
	}

	public void RemoveOverride()
	{
		this.targets.RestoreAll();
	}

	public void AddAnimation(MaterialAnimationTrigger trigger)
	{
		this.AddAnimation(trigger.Data);
	}

	public void AddAnimation(MaterialAnimationData data)
	{
		if (data == null)
		{
			GameClient.Log(LogLevel.Warning, new object[]
			{
				"Null MaterialAnimationData tried to get added."
			});
			return;
		}
		if (data.totalFrames < 1)
		{
			this.AddOnCompleteAnimations(data);
		}
		else
		{
			MaterialAnimationController instance = this.injector.GetInstance<MaterialAnimationController>();
			instance.Init(this.targets, data, this.moveOwner);
			this.animations.Add(instance);
		}
	}

	public void TickFrame()
	{
		for (int i = 0; i < this.animations.Count; i++)
		{
			if (!this.animations[i].TickFrame())
			{
				this.AddOnCompleteAnimations(this.animations[i].data);
				this.animations.RemoveAt(i--);
			}
		}
	}

	private void AddOnCompleteAnimations(MaterialAnimationData data)
	{
		for (int i = 0; i < data.spawnOnCompletion.Length; i++)
		{
			this.AddAnimation(data.spawnOnCompletion[i].Data);
		}
	}

	public void OnDestroy()
	{
		for (int i = 0; i < this.animations.Count; i++)
		{
			this.animations[i].OnDestroy();
		}
		MaterialTarget.ResetCache();
	}
}
