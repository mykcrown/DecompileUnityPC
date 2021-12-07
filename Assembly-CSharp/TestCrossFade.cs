using System;
using FixedPoint;
using UnityEngine;

// Token: 0x02000ABB RID: 2747
public class TestCrossFade : MonoBehaviour
{
	// Token: 0x06005078 RID: 20600 RVA: 0x0014FAE8 File Offset: 0x0014DEE8
	private void Awake()
	{
		this.mecanimControl = base.gameObject.AddComponent<MecanimControl>();
		this.mecanimControl.AddClip(this.clip1, "Clip1");
		this.mecanimControl.AddClip(this.clip2, "Clip2");
	}

	// Token: 0x06005079 RID: 20601 RVA: 0x0014FB27 File Offset: 0x0014DF27
	private void Start()
	{
		this.mecanimControl.Play("Clip1");
		this.mecanimControl.UpdateAnimation(0);
	}

	// Token: 0x0600507A RID: 20602 RVA: 0x0014FB4C File Offset: 0x0014DF4C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			this.frameIndex++;
			this.mecanimControl.UpdateAnimation(this.frameIndex * WTime.fixedDeltaTime);
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			this.frameIndex = 0;
			this.mecanimControl.Play("Clip1", 0, this.frameIndex * WTime.fixedDeltaTime, false);
			this.mecanimControl.Play("Clip2", (Fixed)((double)this.blendTime), 0, false);
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			this.container.Clear();
			this.mecanimControl.ExportState(ref this.container);
			this.savedIndex = this.frameIndex;
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			RollbackState state = this.container.GetState(0).Clone() as RollbackState;
			this.mecanimControl.LoadState(this.container);
			this.container.WriteState(state);
			this.frameIndex = this.savedIndex;
		}
	}

	// Token: 0x040033BD RID: 13245
	public AnimationClip clip1;

	// Token: 0x040033BE RID: 13246
	public AnimationClip clip2;

	// Token: 0x040033BF RID: 13247
	public float blendTime = 0.2f;

	// Token: 0x040033C0 RID: 13248
	private MecanimControl mecanimControl;

	// Token: 0x040033C1 RID: 13249
	private RollbackStateContainer container = new RollbackStateContainer(true);

	// Token: 0x040033C2 RID: 13250
	private int savedIndex;

	// Token: 0x040033C3 RID: 13251
	private int frameIndex;

	// Token: 0x040033C4 RID: 13252
	[AllowCachedState]
	private RollbackState cachedState;
}
