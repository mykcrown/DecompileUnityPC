// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VoiceTauntDisplay : BaseItem3DPreviewDisplay
{
	public Transform root;

	public MeshRenderer characterPortrait;

	private float previewTimer;

	private AudioClip audioClip;

	private AudioReference audioHandle = new AudioReference(null, -1);

	private Vector3 baseScale;

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		get;
		set;
	}

	private void Awake()
	{
		base.onClick += new Action(this.Replay);
		this.baseScale = this.characterPortrait.transform.localScale;
		this.characterPortrait.material = UnityEngine.Object.Instantiate<Material>(this.characterPortrait.material);
		this.tempBobAnimation();
	}

	private void OnDestroy()
	{
		if (this.audioHandle.sourceId >= 0)
		{
			this.audioManager.StopSound(this.audioHandle, 0f);
		}
	}

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

	private void tempBobAnimation()
	{
		Vector3 localPosition = this.root.localPosition;
		localPosition.y = Mathf.Sin(Time.time) * 0.2f;
		this.root.localPosition = localPosition;
	}

	public void SetCharacter(CharacterID characterId)
	{
		this.skinDataManager.GetSkinData(this.skinDataManager.GetDefaultSkin(characterId), new Action<SkinData>(this._SetCharacter_m__0));
	}

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

	public void Replay()
	{
		if (this.previewTimer <= 0f)
		{
			this.Play(this.audioClip, 0f);
		}
	}

	private void playClip()
	{
		if (this.audioClip != null)
		{
			if (this.audioHandle.sourceId >= 0)
			{
				this.audioManager.StopSound(this.audioHandle, 0f);
			}
			AudioRequest request = new AudioRequest(new AudioData(this.audioClip, 1f, AudioSyncMode.Synchronized), new Action<AudioReference, bool>(this._playClip_m__1));
			this.audioHandle = this.audioManager.PlayGameSound(request);
		}
	}

	private void _SetCharacter_m__0(SkinData skinData)
	{
		Texture2D texture = skinData.battlePortrait.texture;
		float num = (float)(texture.height / texture.width);
		float num2 = num - 1f;
		this.characterPortrait.transform.localScale = new Vector3(this.baseScale.x, this.baseScale.y * num, this.baseScale.z);
		float y = num2;
		this.characterPortrait.transform.localPosition = new Vector3(0f, y, 0f);
		this.characterPortrait.material.SetTexture("_MainTex", texture);
	}

	private void _playClip_m__1(AudioReference handle, bool _)
	{
		this.audioHandle = new AudioReference(null, -1);
	}
}
