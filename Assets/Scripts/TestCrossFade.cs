// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using UnityEngine;

public class TestCrossFade : MonoBehaviour
{
	public AnimationClip clip1;

	public AnimationClip clip2;

	public float blendTime = 0.2f;

	private MecanimControl mecanimControl;

	private RollbackStateContainer container = new RollbackStateContainer(true);

	private int savedIndex;

	private int frameIndex;

	[AllowCachedState]
	private RollbackState cachedState;

	private void Awake()
	{
		this.mecanimControl = base.gameObject.AddComponent<MecanimControl>();
		this.mecanimControl.AddClip(this.clip1, "Clip1");
		this.mecanimControl.AddClip(this.clip2, "Clip2");
	}

	private void Start()
	{
		this.mecanimControl.Play("Clip1");
		this.mecanimControl.UpdateAnimation(0);
	}

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
}
