using System;
using UnityEngine;

// Token: 0x02000774 RID: 1908
public class VoiceTauntDisplay : BaseItem3DPreviewDisplay
{
	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x06002F3C RID: 12092 RVA: 0x000ED041 File Offset: 0x000EB441
	// (set) Token: 0x06002F3D RID: 12093 RVA: 0x000ED049 File Offset: 0x000EB449
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x06002F3E RID: 12094 RVA: 0x000ED052 File Offset: 0x000EB452
	// (set) Token: 0x06002F3F RID: 12095 RVA: 0x000ED05A File Offset: 0x000EB45A
	[Inject]
	public ISkinDataManager skinDataManager { get; set; }

	// Token: 0x06002F40 RID: 12096 RVA: 0x000ED064 File Offset: 0x000EB464
	private void Awake()
	{
		base.onClick += this.Replay;
		this.baseScale = this.characterPortrait.transform.localScale;
		this.characterPortrait.material = UnityEngine.Object.Instantiate<Material>(this.characterPortrait.material);
		this.tempBobAnimation();
	}

	// Token: 0x06002F41 RID: 12097 RVA: 0x000ED0BA File Offset: 0x000EB4BA
	private void OnDestroy()
	{
		if (this.audioHandle.sourceId >= 0)
		{
			this.audioManager.StopSound(this.audioHandle, 0f);
		}
	}

	// Token: 0x06002F42 RID: 12098 RVA: 0x000ED0E4 File Offset: 0x000EB4E4
	protected override void Update()
	{
		base.Update();
		if (this.previewTimer >= 0f)
		{
			this.previewTimer -= Time.deltaTime;
			if (this.previewTimer <= 0f)
			{
				this.playClip();
			}
		}
		this.tempBobAnimation();
	}

	// Token: 0x06002F43 RID: 12099 RVA: 0x000ED138 File Offset: 0x000EB538
	private void tempBobAnimation()
	{
		Vector3 localPosition = this.root.localPosition;
		localPosition.y = Mathf.Sin(Time.time) * 0.2f;
		this.root.localPosition = localPosition;
	}

	// Token: 0x06002F44 RID: 12100 RVA: 0x000ED174 File Offset: 0x000EB574
	public void SetCharacter(CharacterID characterId)
	{
		this.skinDataManager.GetSkinData(this.skinDataManager.GetDefaultSkin(characterId), delegate(SkinData skinData)
		{
			Texture2D texture = skinData.battlePortrait.texture;
			float num = (float)(texture.height / texture.width);
			float num2 = num - 1f;
			this.characterPortrait.transform.localScale = new Vector3(this.baseScale.x, this.baseScale.y * num, this.baseScale.z);
			float y = num2;
			this.characterPortrait.transform.localPosition = new Vector3(0f, y, 0f);
			this.characterPortrait.material.SetTexture("_MainTex", texture);
		});
	}

	// Token: 0x06002F45 RID: 12101 RVA: 0x000ED199 File Offset: 0x000EB599
	public void Play(AudioClip clip, float delay)
	{
		this.audioClip = clip;
		if (delay <= 0f)
		{
			this.playClip();
		}
		else
		{
			this.previewTimer = delay;
		}
	}

	// Token: 0x06002F46 RID: 12102 RVA: 0x000ED1BF File Offset: 0x000EB5BF
	public void Replay()
	{
		if (this.previewTimer <= 0f)
		{
			this.Play(this.audioClip, 0f);
		}
	}

	// Token: 0x06002F47 RID: 12103 RVA: 0x000ED1E4 File Offset: 0x000EB5E4
	private void playClip()
	{
		if (this.audioClip != null)
		{
			if (this.audioHandle.sourceId >= 0)
			{
				this.audioManager.StopSound(this.audioHandle, 0f);
			}
			AudioRequest request = new AudioRequest(new AudioData(this.audioClip, 1f, AudioSyncMode.Synchronized), delegate(AudioReference handle, bool _)
			{
				this.audioHandle = new AudioReference(null, -1);
			});
			this.audioHandle = this.audioManager.PlayGameSound(request);
		}
	}

	// Token: 0x04002103 RID: 8451
	public Transform root;

	// Token: 0x04002104 RID: 8452
	public MeshRenderer characterPortrait;

	// Token: 0x04002105 RID: 8453
	private float previewTimer;

	// Token: 0x04002106 RID: 8454
	private AudioClip audioClip;

	// Token: 0x04002107 RID: 8455
	private AudioReference audioHandle = new AudioReference(null, -1);

	// Token: 0x04002108 RID: 8456
	private Vector3 baseScale;
}
