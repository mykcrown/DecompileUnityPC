// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, ITickable, IRollbackStateOwner
{
	private class CameraMovementData
	{
		public Vector3 currentPosition;

		public Quaternion currentRotation;

		public float currentDist;

		public Vector2 currentFocalPoint;

		public Vector2 currentVelocity = Vector2.one;

		public float currentDollyVelocity = 1f;

		public float yawOverride;

		public float yawOverrideVelocity;

		public Transform theTransform;

		public Camera theCamera;
	}

	public GameObject deathEffectPrefab;

	public float deathEffectDuration = 2f;

	private int isDeadForFrames = -1;

	private ICameraInfluencer influencerHighlight;

	private float frameDelta = WTime.frameTime;

	private List<CameraBoundMemory> cameraMemory = new List<CameraBoundMemory>();

	private List<CameraBoundMemory> cameraMemoryPool = new List<CameraBoundMemory>();

	private CameraConfig cameraSettings;

	private GUIClearSignal guiClearSignal;

	private CameraPresets presets = new CameraPresets();

	private CameraShakeActiveData shakeActiveData = new CameraShakeActiveData();

	private CameraShakeModel shakeModel = new CameraShakeModel();

	private Vector2 influencerRectTopLeft = Vector2.zero;

	private Vector2 influencerRectBottomRight = Vector2.zero;

	private Vector2 _topLeft = Vector2.zero;

	private Vector2 _bottomRight = Vector2.zero;

	private float _bottomCharBound;

	private bool isBoundsMode;

	private float currentFrustrumHeight = 1f;

	private Vector2 targetFocalPoint = Vector2.zero;

	private int ledgeGrabMode;

	private int ledgeGrabModeReleaseFrame;

	private int impactModeFrames;

	private int impactModeDelayFrames;

	private float impactModeStrength;

	private int impactModeDirection;

	private int impactModeReleaseFrames;

	private int movingInFrames;

	private List<CameraController.CameraMovementData> cameraList = new List<CameraController.CameraMovementData>();

	private float primaryFOV;

	private GameManager gameManager;

	private List<ICameraInfluencer> cameraInfluencers;

	private Camera theCamera;

	private AudioListener audioListener;

	private Rect bounds;

	private float boundsLeft;

	private float boundsRight;

	private float boundsTop;

	private float boundsBottom;

	private float boundsWidth;

	private float boundsHeight;

	private float spread;

	private float aspectRatio;

	private FlourishCameraInfluencer flourishInfluencer = new FlourishCameraInfluencer();

	[Inject]
	public GameController gameController
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public AudioManager audioManager
	{
		get;
		set;
	}

	[Inject]
	public DebugKeys debugKeys
	{
		get;
		set;
	}

	[Inject]
	public IRollbackStatePooling rollbackStatePooling
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		get;
		set;
	}

	public bool isFlourishMode
	{
		get;
		private set;
	}

	public bool isZoomMode
	{
		get;
		private set;
	}

	public Camera current
	{
		get
		{
			return this.theCamera;
		}
	}

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

	private bool isDisableYawRotation
	{
		get
		{
			return this.gameController.currentGame.Mode == GameMode.Training && this.cameraInfluencers.Count <= 1;
		}
	}

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
			UnityEngine.Debug.LogWarning("Failed to find camera bounds for the stage!");
		}
		this.ResetPosition();
		this.guiClearSignal = this.signalBus.GetSignal<GUIClearSignal>();
		this.primaryFOV = this.theCamera.fieldOfView;
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			Camera camera = allCameras[i];
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

	public void SetHighlightPosition(Vector2 position, float width, float height)
	{
		this.flourishInfluencer.SetHighlightPosition(position, width, height);
	}

	private void tickInfluencers()
	{
		foreach (ICameraInfluencer current in this.cameraInfluencers)
		{
			if (current.InfluencesCamera)
			{
				this.tickInfluencer(current);
			}
		}
	}

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
			foreach (ICameraInfluencer current in this.cameraInfluencers)
			{
				if (current.InfluencesCamera)
				{
					num++;
					this.processInfluencer(current, ref this._topLeft, ref this._bottomRight, ref this._bottomCharBound);
				}
			}
			foreach (CameraBoundMemory current2 in this.cameraMemory)
			{
				foreach (Rect current3 in current2.list)
				{
					if (current3.x < this._topLeft.x)
					{
						this._topLeft.x = current3.x;
					}
					if (current3.y > this._topLeft.y)
					{
						this._topLeft.y = current3.y;
					}
					if (current3.xMax > this._bottomRight.x)
					{
						this._bottomRight.x = current3.xMax;
					}
					if (current3.yMax < this._bottomRight.y)
					{
						this._bottomRight.y = current3.yMax;
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

	public Fixed GetFacingInterpolation(HorizontalDirection direction)
	{
		return (direction != HorizontalDirection.Left) ? 0 : (-1);
	}

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

	private bool getIsBoundsMode(ref Rect characterBounds)
	{
		Vector2 zero = Vector2.zero;
		this.getFocalPointFromCharacterBounds(ref characterBounds, ref zero);
		float num = 0.0001f;
		float num2 = this.currentFrustrumHeight * this.aspectRatio;
		return zero.x + num2 / 2f >= this.boundsRight - num || zero.x - num2 / 2f <= this.boundsLeft + num;
	}

	private void getFocalPointFromCharacterBounds(ref Rect characterBounds, ref Vector2 result)
	{
		result.x = characterBounds.x + characterBounds.width / 2f;
		result.y = characterBounds.y - characterBounds.height / 2f;
	}

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
			UnityEngine.Debug.Log(string.Concat(new object[]
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

	public void ResetPosition()
	{
		foreach (CameraController.CameraMovementData current in this.cameraList)
		{
			current.theTransform.localPosition = Vector3.zero;
			current.theTransform.position = Vector3.zero;
			current.theTransform.localRotation = Quaternion.Euler(this.stageOptions.pitch, 0f, 0f);
		}
	}

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
				foreach (CameraController.CameraMovementData current in this.cameraList)
				{
					current.yawOverride = current.currentRotation.y;
					if (Mathf.Abs(current.yawOverride) < Mathf.Abs(num))
					{
						current.yawOverrideVelocity = ((num <= 0f) ? (-this.cameraOptions.impactTransitionSpeed) : this.cameraOptions.impactTransitionSpeed);
					}
					else
					{
						current.yawOverrideVelocity = 0f;
					}
				}
			}
		}
		else if (this.impactModeFrames > 0)
		{
			this.impactModeFrames--;
			float num2 = (float)this.impactModeDirection * this.cameraOptions.impactYawMulti * this.getImpactMod();
			foreach (CameraController.CameraMovementData current2 in this.cameraList)
			{
				current2.yawOverride += current2.yawOverrideVelocity;
				if (Mathf.Abs(current2.yawOverride) >= Mathf.Abs(num2))
				{
					current2.yawOverride = num2;
					current2.yawOverrideVelocity = 0f;
				}
			}
			if (this.impactModeFrames == 0)
			{
				foreach (CameraController.CameraMovementData current3 in this.cameraList)
				{
					this.impactModeReleaseFrames = (int)Mathf.Ceil(Mathf.Abs(current3.yawOverride) / this.cameraOptions.impactReturnSpeed);
					current3.yawOverrideVelocity = -current3.yawOverride / (float)this.impactModeReleaseFrames;
				}
			}
		}
		else if (this.impactModeReleaseFrames > 0)
		{
			this.impactModeReleaseFrames--;
			foreach (CameraController.CameraMovementData current4 in this.cameraList)
			{
				current4.yawOverride += current4.yawOverrideVelocity;
				if ((current4.yawOverrideVelocity < 0f && current4.yawOverride < 0f) || (current4.yawOverrideVelocity > 0f && current4.yawOverride > 0f))
				{
					current4.yawOverride = 0f;
					current4.yawOverrideVelocity = 0f;
				}
			}
		}
	}

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

	private void syncAudioListener()
	{
		Vector3 position = this.theCamera.transform.position;
		position.z = 0f;
		this.audioListener.transform.position = position;
	}

	private void movePosition(bool instantMove = false)
	{
		this.updateModes();
		this.saveMemory();
		this.tickInfluencers();
		Rect rect = this.calculateCharacterCameraBounds();
		this.calculateTargetCameraBounds(ref rect);
		foreach (CameraController.CameraMovementData current in this.cameraList)
		{
			this.moveIndividualCamera(current, instantMove);
		}
		this.syncAudioListener();
	}

	private float getImpactMod()
	{
		float impactModMax = this.impactModeStrength;
		if (impactModMax > this.cameraOptions.impactModMax)
		{
			impactModMax = this.cameraOptions.impactModMax;
		}
		return impactModMax / this.cameraOptions.impactModMax;
	}

	private void moveIndividualCamera(CameraController.CameraMovementData movementData, bool instantMove)
	{
		float num = -this.currentFrustrumHeight * 0.5f / Mathf.Tan(movementData.theCamera.fieldOfView * 0.5f * 0.0174532924f);
		if (!this.isFlourishMode && !this.isZoomMode)
		{
			num = Mathf.Min(num, -this.stageOptions.minDolly);
		}
		float num2 = Mathf.Tan(this.primaryFOV * 0.5f * 0.0174532924f) / Mathf.Tan(movementData.theCamera.fieldOfView * 0.5f * 0.0174532924f);
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

	private float getSpeed(float speed, CameraController.CameraMovementData movementData, CameraSpeedScaling motionParams, float targetDist, float fovDampen, CameraMotionAxis type)
	{
		if (motionParams.scaleSpeed)
		{
			float num;
			if (type == CameraMotionAxis.DOLLY)
			{
				num = Mathf.Abs(movementData.currentDist - targetDist);
				num = Mathf.Tan(this.primaryFOV * 0.0174532924f * 0.5f) * num * 2f;
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

	private bool allowCameraShake()
	{
		return (!this.isFlourishMode && !this.isZoomMode) || !this.config.flourishConfig.disableCameraShake;
	}

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

	private Vector3 cameraShake()
	{
		Vector3 result = default(Vector3);
		if (this.shakeModel.amplitude > 0)
		{
			float num = (float)this.shakeModel.frameCount;
			this.advanceShakeModel();
			float wavelength = this.shakeActiveData.wavelength;
			float num2 = Mathf.Sin(num / wavelength * 3.14159274f * 0.5f);
			float num3 = Mathf.Cos(num / wavelength * 3.14159274f * 0.5f);
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

	private Vector3 calculateCameraRotation(CameraController.CameraMovementData movementData, float fovDampen, Vector2 baseXY)
	{
		return new Vector3(this.stageOptions.pitch, 0f, 0f)
		{
			y = this.calculateYaw(movementData, baseXY, fovDampen),
			x = this.calculatePitch(baseXY, fovDampen)
		};
	}

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

	private int highlightLedgeGrabState()
	{
		int num = 0;
		int num2 = 0;
		foreach (PlayerController current in this.gameController.currentGame.GetPlayers())
		{
			if (current.IsLedgeGrabbing)
			{
				if (current.Position.x < 0)
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

	private void updateGUI()
	{
		this.guiClearSignal.Dispatch();
	}

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
			foreach (CameraBoundMemory current in this.cameraMemory)
			{
				foreach (Rect current2 in current.list)
				{
					this.showGizmoBox(current2);
				}
			}
		}
	}

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

	private void showGizmoBox(ICameraInfluencer influencer)
	{
		Rect influencerRect = this.getInfluencerRect(influencer);
		this.showGizmoBox(influencerRect);
	}

	private void showGizmoBox(Rect rect)
	{
		GizmoUtil.GizmosDrawRectangle(rect, Color.yellow, true);
	}

	public bool ExportState(ref RollbackStateContainer container)
	{
		container.WriteState(this.rollbackStatePooling.Clone<CameraShakeModel>(this.shakeModel));
		return true;
	}

	public bool LoadState(RollbackStateContainer container)
	{
		container.ReadState<CameraShakeModel>(ref this.shakeModel);
		return true;
	}
}
