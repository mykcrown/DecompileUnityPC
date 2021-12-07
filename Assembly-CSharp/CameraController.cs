using System;
using System.Collections.Generic;
using FixedPoint;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class CameraController : MonoBehaviour, ITickable, IRollbackStateOwner
{
	// Token: 0x1700035A RID: 858
	// (get) Token: 0x060012C7 RID: 4807 RVA: 0x0006B5EB File Offset: 0x000699EB
	// (set) Token: 0x060012C8 RID: 4808 RVA: 0x0006B5F3 File Offset: 0x000699F3
	[Inject]
	public GameController gameController { get; set; }

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0006B5FC File Offset: 0x000699FC
	// (set) Token: 0x060012CA RID: 4810 RVA: 0x0006B604 File Offset: 0x00069A04
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x060012CB RID: 4811 RVA: 0x0006B60D File Offset: 0x00069A0D
	// (set) Token: 0x060012CC RID: 4812 RVA: 0x0006B615 File Offset: 0x00069A15
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x060012CD RID: 4813 RVA: 0x0006B61E File Offset: 0x00069A1E
	// (set) Token: 0x060012CE RID: 4814 RVA: 0x0006B626 File Offset: 0x00069A26
	[Inject]
	public DebugKeys debugKeys { get; set; }

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x060012CF RID: 4815 RVA: 0x0006B62F File Offset: 0x00069A2F
	// (set) Token: 0x060012D0 RID: 4816 RVA: 0x0006B637 File Offset: 0x00069A37
	[Inject]
	public IRollbackStatePooling rollbackStatePooling { get; set; }

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0006B640 File Offset: 0x00069A40
	// (set) Token: 0x060012D2 RID: 4818 RVA: 0x0006B648 File Offset: 0x00069A48
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060012D3 RID: 4819 RVA: 0x0006B651 File Offset: 0x00069A51
	// (set) Token: 0x060012D4 RID: 4820 RVA: 0x0006B659 File Offset: 0x00069A59
	[Inject]
	public IDependencyInjection injector { get; set; }

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060012D5 RID: 4821 RVA: 0x0006B662 File Offset: 0x00069A62
	// (set) Token: 0x060012D6 RID: 4822 RVA: 0x0006B66A File Offset: 0x00069A6A
	public bool isFlourishMode { get; private set; }

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x060012D7 RID: 4823 RVA: 0x0006B673 File Offset: 0x00069A73
	// (set) Token: 0x060012D8 RID: 4824 RVA: 0x0006B67B File Offset: 0x00069A7B
	public bool isZoomMode { get; private set; }

	// Token: 0x060012D9 RID: 4825 RVA: 0x0006B684 File Offset: 0x00069A84
	public void Init()
	{
		this.theCamera = Camera.main;
		if (this.theCamera.GetComponent<RenderTracker>() == null)
		{
			this.theCamera.gameObject.AddComponent<RenderTracker>();
		}
		this.audioManager.UIAudioListener.gameObject.SetActive(false);
		this.audioListener = new GameObject("AudioListener").AddComponent<AudioListener>();
		this.gameManager = this.gameController.currentGame;
		this.cameraInfluencers = this.gameManager.CameraInfluencers;
		this.bounds = this.gameManager.Stage.SimulationData.CameraBounds;
		this.boundsLeft = this.bounds.x;
		this.boundsRight = this.bounds.xMax;
		this.boundsTop = this.bounds.y;
		this.boundsBottom = this.bounds.y - this.bounds.height;
		this.boundsWidth = this.bounds.width;
		this.boundsHeight = this.bounds.height;
		this.aspectRatio = this.theCamera.aspect;
		this.cameraSettings = this.config.cameraConfig;
		this.presets.Init(this.cameraSettings);
		this.deathEffectPrefab = this.config.respawnConfig.deathEffectPrefab;
		if (this.gameManager.Stage == null)
		{
			Debug.LogWarning("Failed to find camera bounds for the stage!");
		}
		this.ResetPosition();
		this.guiClearSignal = this.signalBus.GetSignal<GUIClearSignal>();
		this.primaryFOV = this.theCamera.fieldOfView;
		foreach (Camera camera in Camera.allCameras)
		{
			if (camera.transform.parent == null || camera.transform.parent.GetComponent<Camera>() == null)
			{
				CameraController.CameraMovementData cameraMovementData = new CameraController.CameraMovementData();
				cameraMovementData.theTransform = camera.transform;
				cameraMovementData.theCamera = camera;
				this.cameraList.Add(cameraMovementData);
			}
		}
		this.movePosition(true);
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x0006B8A9 File Offset: 0x00069CA9
	public void SetHighlightPosition(Vector2 position, float width, float height)
	{
		this.flourishInfluencer.SetHighlightPosition(position, width, height);
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x060012DB RID: 4827 RVA: 0x0006B8B9 File Offset: 0x00069CB9
	public Camera current
	{
		get
		{
			return this.theCamera;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x060012DC RID: 4828 RVA: 0x0006B8C4 File Offset: 0x00069CC4
	private CameraConfig.StageData stageOptions
	{
		get
		{
			if (this.cameraOptions == null)
			{
				return null;
			}
			if (this.gameManager.stageData != null && this.gameManager.stageData.useCustomCameraData)
			{
				return this.gameManager.stageData.cameraData;
			}
			return this.cameraOptions.defaultStageData;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x060012DD RID: 4829 RVA: 0x0006B925 File Offset: 0x00069D25
	public CameraConfig cameraOptions
	{
		get
		{
			if (this.cameraSettings == null)
			{
				return null;
			}
			if (this.cameraSettings.usePreset == CameraPreset.CUSTOM)
			{
				return this.cameraSettings;
			}
			return this.presets.GetConfig(this.cameraSettings.usePreset);
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x060012DE RID: 4830 RVA: 0x0006B961 File Offset: 0x00069D61
	private bool isDisableYawRotation
	{
		get
		{
			return this.gameController.currentGame.Mode == GameMode.Training && this.cameraInfluencers.Count <= 1;
		}
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x0006B990 File Offset: 0x00069D90
	private void tickInfluencers()
	{
		foreach (ICameraInfluencer cameraInfluencer in this.cameraInfluencers)
		{
			if (cameraInfluencer.InfluencesCamera)
			{
				this.tickInfluencer(cameraInfluencer);
			}
		}
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0006B9F8 File Offset: 0x00069DF8
	private void tickInfluencer(ICameraInfluencer influencer)
	{
		HorizontalDirection facing = influencer.Facing;
		if (facing != influencer.WaitingForFacingTurnaround)
		{
			influencer.FacingTurnaroundWait = this.cameraOptions.characterTurnaroundDelay;
			influencer.WaitingForFacingTurnaround = facing;
		}
		if (influencer.FacingTurnaroundWait <= 0)
		{
			Fixed facingInterpolation = this.GetFacingInterpolation(facing);
			Fixed other = (Fixed)((double)(1f / (float)this.cameraOptions.characterTurnaroundFrames));
			if (FixedMath.Abs(influencer.FacingInterpolation - facingInterpolation) <= other)
			{
				influencer.FacingInterpolation = facingInterpolation;
			}
			else if (influencer.FacingInterpolation > facingInterpolation)
			{
				influencer.FacingInterpolation -= other;
			}
			else
			{
				influencer.FacingInterpolation += other;
			}
		}
		if (influencer.FacingTurnaroundWait > 0)
		{
			influencer.FacingTurnaroundWait--;
		}
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x0006BADC File Offset: 0x00069EDC
	private Rect calculateCharacterCameraBounds()
	{
		this._topLeft.x = 100000f;
		this._topLeft.y = -100000f;
		this._bottomRight.x = -100000f;
		this._bottomRight.y = 100000f;
		this._bottomCharBound = 100000f;
		this.influencerRectTopLeft.x = 100000f;
		this.influencerRectTopLeft.y = -100000f;
		this.influencerRectBottomRight.x = -100000f;
		this.influencerRectBottomRight.y = 100000f;
		int num = 0;
		if (this.influencerHighlight != null)
		{
			this.processInfluencer(this.influencerHighlight, ref this._topLeft, ref this._bottomRight, ref this._bottomCharBound);
			num = 1;
		}
		else
		{
			foreach (ICameraInfluencer cameraInfluencer in this.cameraInfluencers)
			{
				if (cameraInfluencer.InfluencesCamera)
				{
					num++;
					this.processInfluencer(cameraInfluencer, ref this._topLeft, ref this._bottomRight, ref this._bottomCharBound);
				}
			}
			foreach (CameraBoundMemory cameraBoundMemory in this.cameraMemory)
			{
				foreach (Rect rect in cameraBoundMemory.list)
				{
					if (rect.x < this._topLeft.x)
					{
						this._topLeft.x = rect.x;
					}
					if (rect.y > this._topLeft.y)
					{
						this._topLeft.y = rect.y;
					}
					if (rect.xMax > this._bottomRight.x)
					{
						this._bottomRight.x = rect.xMax;
					}
					if (rect.yMax < this._bottomRight.y)
					{
						this._bottomRight.y = rect.yMax;
					}
				}
			}
			if (num == 0)
			{
				this._topLeft.x = 0f;
				this._topLeft.y = 0f;
				this._bottomRight.x = 0f;
				this._bottomRight.y = 0f;
			}
			bool flag = this.cameraInfluencers.Count == 1 && this.gameManager.Mode == GameMode.Training;
			CameraConfig.Padding padding = (!flag) ? this.stageOptions.padding : this.cameraOptions.trainingModePadding;
			float num2 = padding.top;
			float num3 = padding.bottom;
			float a = this._bottomRight.x - this._topLeft.x;
			float b = (this._topLeft.y - this._bottomRight.y) * this.aspectRatio;
			this.spread = Mathf.Max(a, b);
			if (this.cameraOptions.useAdjustPaddingByZoom && !flag && this.spread < this.cameraOptions.closeUpPaddingMax)
			{
				float num4 = this.cameraOptions.closeUpPaddingMax - this.cameraOptions.closeUpPaddingMin;
				float num5 = this.cameraOptions.closeUpPaddingMax - this.spread;
				float num6 = Mathf.Min(1f, num5 / num4);
				num2 += num6 * this.cameraOptions.closeUpPaddingAdjust;
				num3 -= num6 * this.cameraOptions.closeUpPaddingAdjust;
			}
			this._topLeft.x = this._topLeft.x - padding.sides;
			this._topLeft.y = this._topLeft.y + num2;
			this._bottomRight.x = this._bottomRight.x + padding.sides;
			this._bottomRight.y = this._bottomRight.y - num3;
			this.enforceStageBounds(ref this._topLeft);
			this.enforceStageBounds(ref this._bottomRight);
		}
		this.enforceStageBounds(ref this.influencerRectTopLeft);
		this.enforceStageBounds(ref this.influencerRectBottomRight);
		return new Rect
		{
			x = this._topLeft.x,
			y = this._topLeft.y,
			width = this._bottomRight.x - this._topLeft.x,
			height = this._topLeft.y - this._bottomRight.y
		};
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x0006BFB4 File Offset: 0x0006A3B4
	private void enforceStageBounds(ref Vector2 pt)
	{
		if (pt.y > this.boundsTop)
		{
			pt.y = this.boundsTop;
		}
		if (pt.y < this.boundsBottom)
		{
			pt.y = this.boundsBottom;
		}
		if (pt.x < this.boundsLeft)
		{
			pt.x = this.boundsLeft;
		}
		if (pt.x > this.boundsRight)
		{
			pt.x = this.boundsRight;
		}
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x0006C035 File Offset: 0x0006A435
	public Fixed GetFacingInterpolation(HorizontalDirection direction)
	{
		return (direction != HorizontalDirection.Left) ? 0 : -1;
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0006C04C File Offset: 0x0006A44C
	private void processInfluencer(ICameraInfluencer influencer, ref Vector2 topLeft, ref Vector2 bottomRight, ref float bottomCharBound)
	{
		Rect cameraInfluenceBox = influencer.CameraInfluenceBox;
		float num = cameraInfluenceBox.x;
		float num2 = cameraInfluenceBox.xMax;
		float num3 = this.stageOptions.characterForwardCameraExtend;
		if (this.isFlourishMode || this.isZoomMode)
		{
			num3 = 0f;
		}
		HorizontalDirection facing = influencer.Facing;
		if (num3 != 0f)
		{
			float num4 = (float)influencer.FacingInterpolation;
			num += num3 * num4;
			num2 += num3 * (num4 + 1f);
		}
		if (this.cameraOptions.adjustForLargeCharacters)
		{
			this.adjustForLargeCharacters(ref cameraInfluenceBox, ref num, ref num2);
		}
		if (num < topLeft.x)
		{
			topLeft.x = num;
		}
		if (cameraInfluenceBox.y > topLeft.y)
		{
			topLeft.y = cameraInfluenceBox.y;
		}
		if (num2 > bottomRight.x)
		{
			bottomRight.x = num2;
		}
		bottomRight.y = Mathf.Min(bottomRight.y, cameraInfluenceBox.y - cameraInfluenceBox.height);
		bottomCharBound = Mathf.Min(bottomRight.y, bottomCharBound);
		float x = influencer.Position.x;
		float y = influencer.Position.y;
		this.influencerRectTopLeft.x = Mathf.Min(this.influencerRectBottomRight.x, x);
		this.influencerRectBottomRight.x = Mathf.Max(this.influencerRectBottomRight.x, x);
		this.influencerRectTopLeft.y = Mathf.Max(this.influencerRectBottomRight.y, y);
		this.influencerRectBottomRight.y = Mathf.Min(this.influencerRectBottomRight.y, y);
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x0006C1F4 File Offset: 0x0006A5F4
	private void adjustForLargeCharacters(ref Rect cameraBox, ref float cameraBoxLeftExtended, ref float cameraBoxRightExtended)
	{
		if (cameraBox.height > this.cameraOptions.defaultCameraHeightBox)
		{
			float num = this.stageOptions.padding.top / this.cameraOptions.defaultCameraHeightBox;
			float num2 = cameraBox.height * num - this.stageOptions.padding.top;
			cameraBox.y += num2;
			cameraBox.height += num2;
			float num3 = this.stageOptions.padding.bottom / this.cameraOptions.defaultCameraHeightBox;
			float num4 = cameraBox.height * num3 - this.stageOptions.padding.bottom;
			cameraBox.height += num4;
			float num5 = (num4 / this.stageOptions.padding.bottom + num2 / this.stageOptions.padding.top) / 2f;
			float num6 = this.stageOptions.padding.sides * num5;
			cameraBoxRightExtended += num6;
			cameraBoxLeftExtended -= num6;
		}
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x0006C300 File Offset: 0x0006A700
	private float getFrustrumFromCharacterBounds(ref Rect characterBounds)
	{
		float num = characterBounds.width / characterBounds.height;
		bool flag = num >= this.aspectRatio;
		float value;
		if (flag)
		{
			value = characterBounds.width / this.aspectRatio;
		}
		else
		{
			value = characterBounds.height;
		}
		return Mathf.Clamp(value, 0f, Mathf.Min(this.boundsHeight, this.boundsWidth / this.aspectRatio));
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x0006C370 File Offset: 0x0006A770
	private bool getIsBoundsMode(ref Rect characterBounds)
	{
		Vector2 zero = Vector2.zero;
		this.getFocalPointFromCharacterBounds(ref characterBounds, ref zero);
		float num = 0.0001f;
		float num2 = this.currentFrustrumHeight * this.aspectRatio;
		return zero.x + num2 / 2f >= this.boundsRight - num || zero.x - num2 / 2f <= this.boundsLeft + num;
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x0006C3DB File Offset: 0x0006A7DB
	private void getFocalPointFromCharacterBounds(ref Rect characterBounds, ref Vector2 result)
	{
		result.x = characterBounds.x + characterBounds.width / 2f;
		result.y = characterBounds.y - characterBounds.height / 2f;
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x0006C410 File Offset: 0x0006A810
	private bool isKeepStableDolly()
	{
		if (!this.isFlourishMode && !this.isZoomMode)
		{
			if (this.cameraOptions.stabilizeEdges && this.isBoundsMode)
			{
				return true;
			}
			if (this.isDeadForFrames != -1 && this.isDeadForFrames < this.cameraOptions.deadPlayerDollyDelayFrames)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x0006C478 File Offset: 0x0006A878
	private void calculateTargetCameraBounds(ref Rect characterBounds)
	{
		float num = this.currentFrustrumHeight;
		this.currentFrustrumHeight = this.getFrustrumFromCharacterBounds(ref characterBounds);
		this.isBoundsMode = this.getIsBoundsMode(ref characterBounds);
		bool flag = num >= this.currentFrustrumHeight;
		if (flag)
		{
			this.movingInFrames++;
		}
		else
		{
			this.movingInFrames = 0;
		}
		if (this.isKeepStableDolly())
		{
			this.currentFrustrumHeight = Mathf.Max(num, this.currentFrustrumHeight);
		}
		else if (flag && this.movingInFrames < this.cameraOptions.dollyInDelay && !this.isFlourishMode && !this.isZoomMode)
		{
			float num2 = (float)this.movingInFrames / (float)this.cameraOptions.dollyInDelay;
			float num3 = this.currentFrustrumHeight - num;
			this.currentFrustrumHeight = num + num3 * num2;
		}
		this.getFocalPointFromCharacterBounds(ref characterBounds, ref this.targetFocalPoint);
		float num4 = this.currentFrustrumHeight * this.aspectRatio;
		this.targetFocalPoint.x = Mathf.Clamp(this.targetFocalPoint.x, this.boundsLeft + num4 / 2f, this.boundsRight - num4 / 2f);
		this.targetFocalPoint.y = Mathf.Clamp(this.targetFocalPoint.y, this.boundsBottom + this.currentFrustrumHeight / 2f, this.boundsTop - this.currentFrustrumHeight / 2f);
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x0006C5E8 File Offset: 0x0006A9E8
	public void StartImpact(CameraImpactModeRequest request)
	{
		if (this.cameraOptions.useImpactHighlight && request.strength >= this.cameraOptions.impactModMin)
		{
			this.impactModeFrames = (int)((float)request.frameCount * this.cameraOptions.impactFramesMulti);
			this.impactModeStrength = request.strength;
			this.impactModeDirection = request.direction;
			this.impactModeDelayFrames = request.delayFrames;
			this.impactModeReleaseFrames = 0;
		}
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x0006C668 File Offset: 0x0006AA68
	public void ShakeCamera(CameraShakeRequest request)
	{
		if ((Fixed)((double)request.amplitude) > this.shakeModel.amplitude)
		{
			int num = (int)request.framesUntilReduction + request.extraFrames;
			if (num < request.minFrames)
			{
				num = request.minFrames;
			}
			this.shakeModel.frameCount = 0;
			this.shakeModel.amplitude = (Fixed)((double)request.amplitude);
			this.shakeActiveData.framesUntilReduction = num;
			this.shakeActiveData.wavelength = request.wavelength;
			this.shakeActiveData.originalAmplitude = this.shakeModel.amplitude;
			this.shakeActiveData.shakeRandomizer = request.shakeRandomizer;
			this.shakeActiveData.lateralMotion = request.lateralMotion;
			this.shakeActiveData.xFactor = request.xFactor;
			this.shakeActiveData.yFactor = request.yFactor;
			this.shakeActiveData.lateralXFactor = request.lateralXFactor;
			this.shakeActiveData.lateralYFactor = request.lateralYFactor;
		}
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x0006C784 File Offset: 0x0006AB84
	public void OnCharacterDeath(PlayerController character, Vector3F velocity)
	{
		Vector3 position = character.transform.position;
		Vector3 vector = new Vector3(Mathf.Clamp(position.x, this.boundsLeft, this.boundsRight), Mathf.Clamp(position.y, this.boundsBottom, this.boundsTop));
		float num = 2.7f;
		Vector3 position2 = vector;
		float num2;
		if (MathUtil.almostEqual(vector.x, this.boundsLeft, 0.01f))
		{
			if (MathUtil.almostEqual(vector.y, this.boundsTop, 0.01f))
			{
				num2 = -45f;
				position2.x -= num;
			}
			else if (MathUtil.almostEqual(vector.y, this.boundsBottom, 0.01f))
			{
				num2 = 45f;
				position2.x -= num;
				position2.y -= num;
			}
			else
			{
				num2 = 0f;
				position2.x -= num;
			}
		}
		else if (MathUtil.almostEqual(vector.x, this.boundsRight, 0.01f))
		{
			if (MathUtil.almostEqual(vector.y, this.boundsTop, 0.01f))
			{
				num2 = 225f;
				position2.x += num;
			}
			else if (MathUtil.almostEqual(vector.y, this.boundsBottom, 0.01f))
			{
				num2 = 135f;
				position2.x += num;
				position2.y -= num;
			}
			else
			{
				num2 = 180f;
				position2.x += num;
			}
		}
		else if (MathUtil.almostEqual(vector.y, this.boundsTop, 0.01f))
		{
			num2 = 270f;
		}
		else if (MathUtil.almostEqual(vector.y, this.boundsBottom, 0.01f))
		{
			num2 = 90f;
			position2.y -= num;
		}
		else
		{
			Debug.Log(string.Concat(new object[]
			{
				"Not on edge: ",
				vector,
				" bounds ",
				this.bounds
			}));
			num2 = Vector3.Angle(vector, new Vector3(1f, 0f, 0f));
		}
		Quaternion rotation = Quaternion.Euler(0f, 0f, num2 - 90f);
		Effect effect = this.gameManager.DynamicObjects.InstantiateDynamicObject<Effect>(this.deathEffectPrefab, 4, true);
		GameObject gameObject = effect.gameObject;
		gameObject.transform.position = position2;
		gameObject.transform.rotation = rotation;
		float num3 = 33f;
		float value = (float)velocity.magnitude / num3;
		CameraShakeRequest request = new CameraShakeRequest(this.cameraOptions.shakeData.deathShake);
		request.useMulti(value);
		request.useAngle(num2, false);
		this.ShakeCamera(request);
		this.audioManager.PlayGameSound(new AudioRequest(this.config.respawnConfig.deathEffectSound, (Vector3)character.Position, null));
		effect.Init((int)(this.deathEffectDuration * (float)this.config.fps), null, 1f);
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x0006CAF0 File Offset: 0x0006AEF0
	public void ResetPosition()
	{
		foreach (CameraController.CameraMovementData cameraMovementData in this.cameraList)
		{
			cameraMovementData.theTransform.localPosition = Vector3.zero;
			cameraMovementData.theTransform.position = Vector3.zero;
			cameraMovementData.theTransform.localRotation = Quaternion.Euler(this.stageOptions.pitch, 0f, 0f);
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0006CB8C File Offset: 0x0006AF8C
	public void TickFrame()
	{
		if (this.debugKeys.CameraMovementDisabled)
		{
			return;
		}
		this.frameDelta = WTime.frameTime;
		if (GameClient.IsCurrentFrame)
		{
			this.movePosition(false);
			this.updateGUI();
			this.tickEffects();
		}
		else
		{
			this.advanceShakeModel();
		}
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0006CBE0 File Offset: 0x0006AFE0
	private void tickEffects()
	{
		if (this.ledgeGrabModeReleaseFrame > 0)
		{
			this.ledgeGrabModeReleaseFrame--;
		}
		if (this.impactModeDelayFrames > 0)
		{
			this.impactModeDelayFrames--;
			if (this.impactModeDelayFrames == 0)
			{
				float num = (float)this.impactModeDirection * this.cameraOptions.impactYawMulti * this.getImpactMod();
				foreach (CameraController.CameraMovementData cameraMovementData in this.cameraList)
				{
					cameraMovementData.yawOverride = cameraMovementData.currentRotation.y;
					if (Mathf.Abs(cameraMovementData.yawOverride) < Mathf.Abs(num))
					{
						cameraMovementData.yawOverrideVelocity = ((num <= 0f) ? (-this.cameraOptions.impactTransitionSpeed) : this.cameraOptions.impactTransitionSpeed);
					}
					else
					{
						cameraMovementData.yawOverrideVelocity = 0f;
					}
				}
			}
		}
		else if (this.impactModeFrames > 0)
		{
			this.impactModeFrames--;
			float num2 = (float)this.impactModeDirection * this.cameraOptions.impactYawMulti * this.getImpactMod();
			foreach (CameraController.CameraMovementData cameraMovementData2 in this.cameraList)
			{
				cameraMovementData2.yawOverride += cameraMovementData2.yawOverrideVelocity;
				if (Mathf.Abs(cameraMovementData2.yawOverride) >= Mathf.Abs(num2))
				{
					cameraMovementData2.yawOverride = num2;
					cameraMovementData2.yawOverrideVelocity = 0f;
				}
			}
			if (this.impactModeFrames == 0)
			{
				foreach (CameraController.CameraMovementData cameraMovementData3 in this.cameraList)
				{
					this.impactModeReleaseFrames = (int)Mathf.Ceil(Mathf.Abs(cameraMovementData3.yawOverride) / this.cameraOptions.impactReturnSpeed);
					cameraMovementData3.yawOverrideVelocity = -cameraMovementData3.yawOverride / (float)this.impactModeReleaseFrames;
				}
			}
		}
		else if (this.impactModeReleaseFrames > 0)
		{
			this.impactModeReleaseFrames--;
			foreach (CameraController.CameraMovementData cameraMovementData4 in this.cameraList)
			{
				cameraMovementData4.yawOverride += cameraMovementData4.yawOverrideVelocity;
				if ((cameraMovementData4.yawOverrideVelocity < 0f && cameraMovementData4.yawOverride < 0f) || (cameraMovementData4.yawOverrideVelocity > 0f && cameraMovementData4.yawOverride > 0f))
				{
					cameraMovementData4.yawOverride = 0f;
					cameraMovementData4.yawOverrideVelocity = 0f;
				}
			}
		}
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x0006CF18 File Offset: 0x0006B318
	private CameraBoundMemory getNewCameraBoundMemory()
	{
		if (this.cameraMemoryPool.Count > 0)
		{
			CameraBoundMemory result = this.cameraMemoryPool[0];
			this.cameraMemoryPool.RemoveAt(0);
			return result;
		}
		return new CameraBoundMemory();
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0006CF58 File Offset: 0x0006B358
	private void saveMemory()
	{
		if (!this.cameraOptions.useBoundsMemory)
		{
			this.cameraMemory.Clear();
		}
		else
		{
			CameraBoundMemory newCameraBoundMemory = this.getNewCameraBoundMemory();
			if (this.influencerHighlight != null)
			{
				Rect influencerRect = this.getInfluencerRect(this.influencerHighlight);
				newCameraBoundMemory.list.Add(influencerRect);
			}
			else
			{
				for (int i = 0; i < this.cameraInfluencers.Count; i++)
				{
					ICameraInfluencer cameraInfluencer = this.cameraInfluencers[i];
					if (cameraInfluencer.InfluencesCamera)
					{
						Rect influencerRect2 = this.getInfluencerRect(cameraInfluencer);
						newCameraBoundMemory.list.Add(influencerRect2);
					}
				}
			}
			this.cameraMemory.Add(newCameraBoundMemory);
			while (this.cameraMemory.Count > this.cameraOptions.memoryFrames)
			{
				CameraBoundMemory cameraBoundMemory = this.cameraMemory[0];
				cameraBoundMemory.Reset();
				this.cameraMemoryPool.Add(cameraBoundMemory);
				this.cameraMemory.RemoveAt(0);
			}
		}
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x0006D060 File Offset: 0x0006B460
	private void updateModes()
	{
		this.isFlourishMode = false;
		this.isZoomMode = false;
		this.influencerHighlight = null;
		this.isDeadForFrames = -1;
		for (int i = 0; i < this.cameraInfluencers.Count; i++)
		{
			ICameraInfluencer cameraInfluencer = this.cameraInfluencers[i];
			if (cameraInfluencer.InfluencesCamera)
			{
				if (cameraInfluencer.IsFlourishMode)
				{
					this.isFlourishMode = true;
				}
				else if (cameraInfluencer.IsZoomMode)
				{
					this.isZoomMode = true;
					this.influencerHighlight = cameraInfluencer;
				}
			}
			if (cameraInfluencer.IsDeadForFrames > this.isDeadForFrames)
			{
				this.isDeadForFrames = cameraInfluencer.IsDeadForFrames;
			}
		}
		if (this.isFlourishMode)
		{
			this.influencerHighlight = this.flourishInfluencer;
		}
		this.ledgeGrabMode = 0;
		if (this.cameraOptions.useLedgeGrabYaw)
		{
			this.ledgeGrabMode = this.highlightLedgeGrabState();
			if (this.ledgeGrabMode != 0)
			{
				this.ledgeGrabModeReleaseFrame = this.cameraSettings.ledgeGrabRotateReleaseFrames;
			}
		}
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0006D160 File Offset: 0x0006B560
	private void syncAudioListener()
	{
		Vector3 position = this.theCamera.transform.position;
		position.z = 0f;
		this.audioListener.transform.position = position;
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0006D19C File Offset: 0x0006B59C
	private void movePosition(bool instantMove = false)
	{
		this.updateModes();
		this.saveMemory();
		this.tickInfluencers();
		Rect rect = this.calculateCharacterCameraBounds();
		this.calculateTargetCameraBounds(ref rect);
		foreach (CameraController.CameraMovementData movementData in this.cameraList)
		{
			this.moveIndividualCamera(movementData, instantMove);
		}
		this.syncAudioListener();
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x0006D220 File Offset: 0x0006B620
	private float getImpactMod()
	{
		float impactModMax = this.impactModeStrength;
		if (impactModMax > this.cameraOptions.impactModMax)
		{
			impactModMax = this.cameraOptions.impactModMax;
		}
		return impactModMax / this.cameraOptions.impactModMax;
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x0006D260 File Offset: 0x0006B660
	private void moveIndividualCamera(CameraController.CameraMovementData movementData, bool instantMove)
	{
		float num = -this.currentFrustrumHeight * 0.5f / Mathf.Tan(movementData.theCamera.fieldOfView * 0.5f * 0.017453292f);
		if (!this.isFlourishMode && !this.isZoomMode)
		{
			num = Mathf.Min(num, -this.stageOptions.minDolly);
		}
		float num2 = Mathf.Tan(this.primaryFOV * 0.5f * 0.017453292f) / Mathf.Tan(movementData.theCamera.fieldOfView * 0.5f * 0.017453292f);
		Vector3 vector = this.calculateCameraRotation(movementData, num2, this.targetFocalPoint);
		Vector3 currentPosition = this.calculateCameraPosition(vector, this.targetFocalPoint, num) * num2;
		num *= num2;
		Quaternion quaternion = Quaternion.Euler(vector);
		bool flag = movementData.currentDist <= num;
		Vector3 b = this.cameraShake();
		if (instantMove)
		{
			movementData.currentRotation = quaternion;
			movementData.currentPosition = currentPosition;
			movementData.currentDist = num;
			movementData.currentFocalPoint = this.targetFocalPoint;
		}
		else
		{
			float num3 = 1f;
			float num4;
			if (flag)
			{
				num4 = this.cameraOptions.dollySpeedIn;
			}
			else
			{
				num4 = this.cameraOptions.dollySpeedOut;
			}
			float num5 = this.cameraOptions.xMotionOut;
			float num6 = this.cameraOptions.yMotionOut;
			num4 *= num3;
			num5 *= num3;
			num6 *= num3;
			if (this.isFlourishMode)
			{
				num4 = (float)this.config.flourishConfig.cameraZoomSpeed;
				num5 = (float)this.config.flourishConfig.cameraZoomSpeed;
				num6 = (float)this.config.flourishConfig.cameraZoomSpeed;
			}
			else if (this.isZoomMode)
			{
				num4 = (float)this.config.flourishConfig.miniZoomSpeed;
				num5 = (float)this.config.flourishConfig.miniZoomSpeed;
				num6 = (float)this.config.flourishConfig.miniZoomSpeed;
			}
			else
			{
				num4 = this.getSpeed(num4, movementData, this.cameraOptions.dollySpeedScale, num, num2, CameraMotionAxis.DOLLY);
			}
			Vector2 vector2 = this.targetFocalPoint - movementData.currentFocalPoint;
			if (vector2.y + vector2.x != 0f)
			{
				float num7 = Mathf.Abs(vector2.x);
				float num8 = Mathf.Abs(vector2.y);
				float num9 = num7 / (num7 + num8);
				float num10 = num5 * num9 + num6 * (1f - num9);
				num10 = this.getSpeed(num10, movementData, this.cameraOptions.xySpeedScale, num, num2, CameraMotionAxis.PAN);
				movementData.currentFocalPoint = Vector2.Lerp(movementData.currentFocalPoint, this.targetFocalPoint, this.frameDelta * num10);
				movementData.currentRotation = Quaternion.Lerp(movementData.currentRotation, quaternion, this.frameDelta * num10);
			}
			movementData.currentDist = Mathf.Lerp(movementData.currentDist, num, this.frameDelta * num4);
			movementData.currentPosition = this.calculateCameraPosition(movementData.currentRotation.eulerAngles, movementData.currentFocalPoint, movementData.currentDist);
		}
		movementData.theTransform.position = movementData.currentPosition + b;
		movementData.theTransform.localRotation = movementData.currentRotation;
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0006D5BC File Offset: 0x0006B9BC
	private float getSpeed(CameraController.CameraMovementData movementData, CameraMotionParams motionParams, Vector3 targetCameraPosition, CameraMotionAxis type)
	{
		float num = motionParams.baseSpeed;
		if (motionParams.quadraticScaling)
		{
			float num2;
			if (type == CameraMotionAxis.DOLLY)
			{
				num2 = Mathf.Abs(movementData.currentPosition.z - targetCameraPosition.z);
			}
			else
			{
				if (type != CameraMotionAxis.PAN)
				{
					throw new UnityException("Not supported");
				}
				Vector2 a = movementData.currentPosition;
				Vector2 b = targetCameraPosition;
				num2 = (a - b).magnitude;
			}
			num2 /= motionParams.quadraticScalingDistance;
			num *= Mathf.Pow(num2, 0.5f);
			num = Mathf.Min(num, motionParams.maxSpeed);
		}
		return num;
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0006D668 File Offset: 0x0006BA68
	private float getSpeed(float speed, CameraController.CameraMovementData movementData, CameraSpeedScaling motionParams, float targetDist, float fovDampen, CameraMotionAxis type)
	{
		if (motionParams.scaleSpeed)
		{
			float num;
			if (type == CameraMotionAxis.DOLLY)
			{
				num = Mathf.Abs(movementData.currentDist - targetDist);
				num = Mathf.Tan(this.primaryFOV * 0.017453292f * 0.5f) * num * 2f;
			}
			else
			{
				if (type != CameraMotionAxis.PAN)
				{
					throw new UnityException("Not supported");
				}
				num = (movementData.currentFocalPoint - this.targetFocalPoint).magnitude;
			}
			float num2 = motionParams.minSpeedPortion * speed;
			float num3 = speed;
			float num4 = num3 - num2;
			float num5 = Mathf.Min(1f, num / (motionParams.distanceForMaxSpeed * fovDampen));
			speed = num2 + num5 * num4;
		}
		return speed;
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0006D724 File Offset: 0x0006BB24
	private bool allowCameraShake()
	{
		return (!this.isFlourishMode && !this.isZoomMode) || !this.config.flourishConfig.disableCameraShake;
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0006D754 File Offset: 0x0006BB54
	private void advanceShakeModel()
	{
		if (this.shakeModel.amplitude > 0)
		{
			if (this.shakeModel.frameCount > this.shakeActiveData.framesUntilReduction)
			{
				Fixed other = FixedMath.Max(this.shakeActiveData.originalAmplitude * 0.03f, this.shakeModel.amplitude * 0.12f);
				this.shakeModel.amplitude = FixedMath.Max(this.shakeModel.amplitude - other, 0);
			}
			this.shakeModel.frameCount++;
		}
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0006D7FC File Offset: 0x0006BBFC
	private Vector3 cameraShake()
	{
		Vector3 result = default(Vector3);
		if (this.shakeModel.amplitude > 0)
		{
			float num = (float)this.shakeModel.frameCount;
			this.advanceShakeModel();
			float wavelength = this.shakeActiveData.wavelength;
			float num2 = Mathf.Sin(num / wavelength * 3.1415927f * 0.5f);
			float num3 = Mathf.Cos(num / wavelength * 3.1415927f * 0.5f);
			float num4 = (float)this.shakeModel.amplitude * 0.01f;
			float num5 = (UnityEngine.Random.value - 0.5f) * this.shakeActiveData.shakeRandomizer * 2f;
			float num6 = (UnityEngine.Random.value - 0.5f) * this.shakeActiveData.shakeRandomizer * 2f;
			if (this.allowCameraShake())
			{
				result.x = (1f + num6) * num2 * num4 * this.shakeActiveData.xFactor;
				result.y = (1f + num6) * num2 * num4 * this.shakeActiveData.yFactor;
				result.x += (1f + num5) * num3 * num4 * this.shakeActiveData.lateralXFactor * this.shakeActiveData.lateralMotion;
				result.y += (1f + num5) * num3 * num4 * this.shakeActiveData.lateralYFactor * this.shakeActiveData.lateralMotion;
			}
		}
		return result;
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0006D97C File Offset: 0x0006BD7C
	private Vector3 calculateCameraPosition(Vector3 rotation, Vector3 focalPoint, float dolly)
	{
		Vector3 result;
		if (this.cameraOptions.adjustPositionWithRotation)
		{
			float num = rotation.y;
			if (this.cameraOptions.horizontalDriftBack)
			{
				num -= num * this.cameraOptions.driftbackStrength;
			}
			Vector3 a = Quaternion.Euler(rotation.x, num, 0f) * -Vector3.forward;
			result = focalPoint - a * dolly;
		}
		else
		{
			Vector3 a2 = Quaternion.Euler(this.stageOptions.pitch, 0f, 0f) * -Vector3.forward;
			result = focalPoint - a2 * dolly;
		}
		return result;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0006DA30 File Offset: 0x0006BE30
	private Vector3 calculateCameraRotation(CameraController.CameraMovementData movementData, float fovDampen, Vector2 baseXY)
	{
		return new Vector3(this.stageOptions.pitch, 0f, 0f)
		{
			y = this.calculateYaw(movementData, baseXY, fovDampen),
			x = this.calculatePitch(baseXY, fovDampen)
		};
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x0006DA7C File Offset: 0x0006BE7C
	private float calculatePitch(Vector2 baseXY, float fovDampen)
	{
		float num = this.stageOptions.pitch;
		if (this.cameraOptions.dynamicPitch)
		{
			float num2 = baseXY.y - this.cameraOptions.pitchMidpoint;
			if (this.ledgeGrabMode != 0)
			{
				num = this.cameraOptions.ledgeGrabSlowPitch;
			}
			else
			{
				if (this.stageOptions.pitchFactor != 0f)
				{
					if (this.cameraOptions.pitchBelowBounds)
					{
						num2 = Mathf.Abs(num2);
						num2 -= this.cameraOptions.pitchDeadZone;
						num2 = Mathf.Max(0f, num2);
						float pitchBottomBound = this.cameraOptions.pitchBottomBound;
						if (this._bottomCharBound < pitchBottomBound)
						{
							num2 += pitchBottomBound - this._bottomCharBound;
						}
					}
					else if (!this.cameraOptions.pitchNegative && num2 < 0f)
					{
						num2 = 0f;
					}
					if (num2 != 0f)
					{
						float num3 = this.getTheta(num2, this.cameraOptions.pitchEquation, this.cameraOptions.pitchCurve, this.stageOptions.pitchFactor);
						num3 *= fovDampen;
						num += num3;
					}
				}
				num = Mathf.Clamp(num, -this.stageOptions.pitchMax * fovDampen, this.stageOptions.pitchMax * fovDampen);
				if (this.cameraOptions.pitchNegative)
				{
					num = Mathf.Max(this.cameraOptions.pitchMin, num);
				}
			}
		}
		return num;
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0006DBE8 File Offset: 0x0006BFE8
	private int highlightLedgeGrabState()
	{
		int num = 0;
		int num2 = 0;
		foreach (PlayerController playerController in this.gameController.currentGame.GetPlayers())
		{
			if (playerController.IsLedgeGrabbing)
			{
				if (playerController.Position.x < 0)
				{
					num = -1;
				}
				else
				{
					num2 = 1;
				}
			}
		}
		return num + num2;
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0006DC7C File Offset: 0x0006C07C
	private float calculateYaw(CameraController.CameraMovementData movementData, Vector2 baseXY, float fovDampen)
	{
		float num = 0f;
		if (this.cameraOptions.dynamicYaw && !this.isDisableYawRotation)
		{
			if (this.ledgeGrabMode != 0)
			{
				num = (float)this.ledgeGrabMode * this.cameraOptions.ledgeGrabSlowYaw;
			}
			else if (this.stageOptions.yawFactor != 0f)
			{
				float num2 = 0f;
				float num3 = baseXY.x;
				if (this.cameraOptions.yawDeadZone == 0f || Mathf.Abs(num3) > this.cameraOptions.yawDeadZone)
				{
					if (num3 > 0f)
					{
						num3 -= this.cameraOptions.yawDeadZone;
					}
					else
					{
						num3 += this.cameraOptions.yawDeadZone;
					}
					if (this.cameraOptions.useVerticalityYawReduce && num3 != 0f)
					{
						float num4 = this.influencerRectBottomRight.x - this.influencerRectTopLeft.x;
						float num5 = this.influencerRectTopLeft.y - this.influencerRectBottomRight.y;
						num4 = Mathf.Max(num4, this.cameraOptions.verticalityMinWidth);
						float num6 = num4 * this.cameraOptions.verticalityYawRatioSplit;
						if (num5 > num6)
						{
							float num7 = num6 / num5;
							if (this.cameraOptions.verticalitySoften != 1f)
							{
								num7 = Mathf.Pow(num7, this.cameraOptions.verticalitySoften);
							}
							num3 *= num7;
						}
					}
					num2 = this.getTheta(num3, this.cameraOptions.yawEquation, this.cameraOptions.yawCurve, this.stageOptions.yawFactor);
				}
				num += num2 * fovDampen;
				num = Mathf.Clamp(num, -this.stageOptions.yawMax * fovDampen, this.stageOptions.yawMax * fovDampen);
			}
			if ((this.impactModeFrames > 0 && this.impactModeDelayFrames <= 0) || this.impactModeReleaseFrames > 0)
			{
				num += movementData.yawOverride;
			}
		}
		return num;
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0006DE74 File Offset: 0x0006C274
	private float getTheta(float value, CameraEquation equation, float curve, float factor)
	{
		if (equation == CameraEquation.ATAN)
		{
			float num = 0.003157f;
			float num2 = 6f;
			float num3 = value * factor * num;
			float x = Mathf.Sqrt(num2 * num2 - num3 * num3);
			return Mathf.Atan2(num3, x) * 57.29578f;
		}
		if (equation == CameraEquation.LOG)
		{
			float num4 = Mathf.Log10(1f + Mathf.Abs(value) * curve) * factor * 0.033f;
			if (value < 0f)
			{
				num4 *= -1f;
			}
			return num4;
		}
		if (equation == CameraEquation.EXPONENENT)
		{
			float num5 = Mathf.Pow(Mathf.Abs(value) * 0.033f * factor, curve);
			if (value < 0f && num5 > 0f)
			{
				num5 *= -1f;
			}
			return num5;
		}
		return value * 0.033f * factor;
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x0006DF3E File Offset: 0x0006C33E
	private void updateGUI()
	{
		this.guiClearSignal.Dispatch();
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x0006DF4C File Offset: 0x0006C34C
	public void OnDrawGizmos()
	{
		if (this.cameraOptions != null)
		{
			Rect rect = this.calculateCharacterCameraBounds();
			GizmoUtil.GizmosDrawRectangle(rect, Color.yellow, true);
			for (int i = 0; i < this.cameraInfluencers.Count; i++)
			{
				ICameraInfluencer cameraInfluencer = this.cameraInfluencers[i];
				if (cameraInfluencer.InfluencesCamera)
				{
					this.showGizmoBox(cameraInfluencer);
				}
			}
			foreach (CameraBoundMemory cameraBoundMemory in this.cameraMemory)
			{
				foreach (Rect rect2 in cameraBoundMemory.list)
				{
					this.showGizmoBox(rect2);
				}
			}
		}
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x0006E050 File Offset: 0x0006C450
	private Rect getInfluencerRect(ICameraInfluencer influencer)
	{
		Rect cameraInfluenceBox = influencer.CameraInfluenceBox;
		float num = cameraInfluenceBox.x;
		float num2 = cameraInfluenceBox.xMax;
		HorizontalDirection facing = influencer.Facing;
		float num3 = this.stageOptions.characterForwardCameraExtend;
		if (this.isFlourishMode || this.isZoomMode)
		{
			num3 = 0f;
		}
		if (num3 != 0f)
		{
			num += num3 * (float)influencer.FacingInterpolation;
			num2 += num3 * (float)(influencer.FacingInterpolation + 1);
		}
		return new Rect
		{
			x = num,
			y = cameraInfluenceBox.yMin,
			height = cameraInfluenceBox.height,
			xMax = num2
		};
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x0006E110 File Offset: 0x0006C510
	private void showGizmoBox(ICameraInfluencer influencer)
	{
		Rect influencerRect = this.getInfluencerRect(influencer);
		this.showGizmoBox(influencerRect);
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x0006E12C File Offset: 0x0006C52C
	private void showGizmoBox(Rect rect)
	{
		GizmoUtil.GizmosDrawRectangle(rect, Color.yellow, true);
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x0006E13A File Offset: 0x0006C53A
	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<CameraShakeModel>(this.shakeModel));
		return true;
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x0006E156 File Offset: 0x0006C556
	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<CameraShakeModel>(ref this.shakeModel);
		return true;
	}

	// Token: 0x04000C3B RID: 3131
	public GameObject deathEffectPrefab;

	// Token: 0x04000C3C RID: 3132
	public float deathEffectDuration = 2f;

	// Token: 0x04000C3D RID: 3133
	private int isDeadForFrames = -1;

	// Token: 0x04000C40 RID: 3136
	private ICameraInfluencer influencerHighlight;

	// Token: 0x04000C41 RID: 3137
	private float frameDelta = WTime.frameTime;

	// Token: 0x04000C42 RID: 3138
	private List<CameraBoundMemory> cameraMemory = new List<CameraBoundMemory>();

	// Token: 0x04000C43 RID: 3139
	private List<CameraBoundMemory> cameraMemoryPool = new List<CameraBoundMemory>();

	// Token: 0x04000C44 RID: 3140
	private CameraConfig cameraSettings;

	// Token: 0x04000C45 RID: 3141
	private GUIClearSignal guiClearSignal;

	// Token: 0x04000C46 RID: 3142
	private CameraPresets presets = new CameraPresets();

	// Token: 0x04000C47 RID: 3143
	private CameraShakeActiveData shakeActiveData = new CameraShakeActiveData();

	// Token: 0x04000C48 RID: 3144
	private CameraShakeModel shakeModel = new CameraShakeModel();

	// Token: 0x04000C49 RID: 3145
	private Vector2 influencerRectTopLeft = Vector2.zero;

	// Token: 0x04000C4A RID: 3146
	private Vector2 influencerRectBottomRight = Vector2.zero;

	// Token: 0x04000C4B RID: 3147
	private Vector2 _topLeft = Vector2.zero;

	// Token: 0x04000C4C RID: 3148
	private Vector2 _bottomRight = Vector2.zero;

	// Token: 0x04000C4D RID: 3149
	private float _bottomCharBound;

	// Token: 0x04000C4E RID: 3150
	private bool isBoundsMode;

	// Token: 0x04000C4F RID: 3151
	private float currentFrustrumHeight = 1f;

	// Token: 0x04000C50 RID: 3152
	private Vector2 targetFocalPoint = Vector2.zero;

	// Token: 0x04000C51 RID: 3153
	private int ledgeGrabMode;

	// Token: 0x04000C52 RID: 3154
	private int ledgeGrabModeReleaseFrame;

	// Token: 0x04000C53 RID: 3155
	private int impactModeFrames;

	// Token: 0x04000C54 RID: 3156
	private int impactModeDelayFrames;

	// Token: 0x04000C55 RID: 3157
	private float impactModeStrength;

	// Token: 0x04000C56 RID: 3158
	private int impactModeDirection;

	// Token: 0x04000C57 RID: 3159
	private int impactModeReleaseFrames;

	// Token: 0x04000C58 RID: 3160
	private int movingInFrames;

	// Token: 0x04000C59 RID: 3161
	private List<CameraController.CameraMovementData> cameraList = new List<CameraController.CameraMovementData>();

	// Token: 0x04000C5A RID: 3162
	private float primaryFOV;

	// Token: 0x04000C5B RID: 3163
	private GameManager gameManager;

	// Token: 0x04000C5C RID: 3164
	private List<ICameraInfluencer> cameraInfluencers;

	// Token: 0x04000C5D RID: 3165
	private Camera theCamera;

	// Token: 0x04000C5E RID: 3166
	private AudioListener audioListener;

	// Token: 0x04000C5F RID: 3167
	private Rect bounds;

	// Token: 0x04000C60 RID: 3168
	private float boundsLeft;

	// Token: 0x04000C61 RID: 3169
	private float boundsRight;

	// Token: 0x04000C62 RID: 3170
	private float boundsTop;

	// Token: 0x04000C63 RID: 3171
	private float boundsBottom;

	// Token: 0x04000C64 RID: 3172
	private float boundsWidth;

	// Token: 0x04000C65 RID: 3173
	private float boundsHeight;

	// Token: 0x04000C66 RID: 3174
	private float spread;

	// Token: 0x04000C67 RID: 3175
	private float aspectRatio;

	// Token: 0x04000C68 RID: 3176
	private FlourishCameraInfluencer flourishInfluencer = new FlourishCameraInfluencer();

	// Token: 0x02000373 RID: 883
	private class CameraMovementData
	{
		// Token: 0x04000C69 RID: 3177
		public Vector3 currentPosition;

		// Token: 0x04000C6A RID: 3178
		public Quaternion currentRotation;

		// Token: 0x04000C6B RID: 3179
		public float currentDist;

		// Token: 0x04000C6C RID: 3180
		public Vector2 currentFocalPoint;

		// Token: 0x04000C6D RID: 3181
		public Vector2 currentVelocity = Vector2.one;

		// Token: 0x04000C6E RID: 3182
		public float currentDollyVelocity = 1f;

		// Token: 0x04000C6F RID: 3183
		public float yawOverride;

		// Token: 0x04000C70 RID: 3184
		public float yawOverrideVelocity;

		// Token: 0x04000C71 RID: 3185
		public Transform theTransform;

		// Token: 0x04000C72 RID: 3186
		public Camera theCamera;
	}
}
