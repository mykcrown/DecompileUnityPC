using System;
using UnityEngine;

// Token: 0x02000440 RID: 1088
public class MaterialAnimationsController
{
	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x060016A0 RID: 5792 RVA: 0x0007AD1B File Offset: 0x0007911B
	// (set) Token: 0x060016A1 RID: 5793 RVA: 0x0007AD23 File Offset: 0x00079123
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x060016A2 RID: 5794 RVA: 0x0007AD2C File Offset: 0x0007912C
	// (set) Token: 0x060016A3 RID: 5795 RVA: 0x0007AD34 File Offset: 0x00079134
	public bool Overridden { get; private set; }

	// Token: 0x060016A4 RID: 5796 RVA: 0x0007AD3D File Offset: 0x0007913D
	public void Init(IMoveOwner moveOwner, MaterialTargetsData targets)
	{
		this.moveOwner = moveOwner;
		this.targets = targets;
		this.Overridden = false;
		targets.CacheAll();
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x0007AD5A File Offset: 0x0007915A
	public void OverrideAllMaterials(Material material)
	{
		this.Overridden = true;
		this.targets.MarkAllActive();
		this.targets.SetMaterialAll(material.shader.name, material, true);
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x0007AD86 File Offset: 0x00079186
	public void RemoveOverride()
	{
		this.targets.RestoreAll();
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x0007AD93 File Offset: 0x00079193
	public void AddAnimation(MaterialAnimationTrigger trigger)
	{
		this.AddAnimation(trigger.Data);
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x0007ADA4 File Offset: 0x000791A4
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

	// Token: 0x060016A9 RID: 5801 RVA: 0x0007AE10 File Offset: 0x00079210
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

	// Token: 0x060016AA RID: 5802 RVA: 0x0007AE78 File Offset: 0x00079278
	private void AddOnCompleteAnimations(MaterialAnimationData data)
	{
		for (int i = 0; i < data.spawnOnCompletion.Length; i++)
		{
			this.AddAnimation(data.spawnOnCompletion[i].Data);
		}
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x0007AEB4 File Offset: 0x000792B4
	public void OnDestroy()
	{
		for (int i = 0; i < this.animations.Count; i++)
		{
			this.animations[i].OnDestroy();
		}
		MaterialTarget.ResetCache();
	}

	// Token: 0x04001157 RID: 4439
	private IMoveOwner moveOwner;

	// Token: 0x04001159 RID: 4441
	private MaterialTargetsData targets;

	// Token: 0x0400115A RID: 4442
	private FixedCapacityList<MaterialAnimationController> animations = new FixedCapacityList<MaterialAnimationController>(100);
}
