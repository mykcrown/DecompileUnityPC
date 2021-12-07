// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UISceneCharacter : MonoBehaviour
{
	public enum AnimationMode
	{
		IN_GAME,
		VICTORY_POSE,
		TRANSITION
	}

	private sealed class _Init_c__AnonStorey0
	{
		internal SkinDefinition initWithSkin;

		internal UISceneCharacter _this;

		internal void __m__0(SkinData skinData)
		{
			if (this._this.currentSkin == this.initWithSkin.uniqueKey)
			{
				this._this.setLoadedSkin(skinData);
			}
		}
	}

	private sealed class _swapCharacterObject_c__AnonStorey1
	{
		internal MeshRenderer obj;

		internal UISceneCharacter _this;

		internal void __m__0()
		{
			this.obj.gameObject.SetActive(true);
			this._this.timer.SetTimeout(1, new Action(this.__m__1));
		}

		internal void __m__1()
		{
			this.obj.gameObject.SetActive(false);
		}
	}

	private sealed class _SetSkin_c__AnonStorey2
	{
		internal SkinDefinition skinDef;

		internal UISceneCharacter _this;

		internal void __m__0(SkinData skinData)
		{
			if (this._this.currentSkin == this.skinDef.uniqueKey)
			{
				this._this.setLoadedSkin(skinData);
			}
		}
	}

	private Animator animator;

	private IAnimationPlayer animationPlayer;

	private UISceneMoveController moveController;

	private GameObject displayObject;

	private GameObject playerObject;

	private CharacterMenusData characterMenusData;

	private UISceneCharacterAnimRequest currentAnim = default(UISceneCharacterAnimRequest);

	private UISceneCharacterAnimRequest defaultAnim;

	private float animStartTime;

	private UISceneCharacter.AnimationMode animationMode;

	public Vector3 InitialPosition;

	private string currentSkin;

	[Inject]
	public ConfigData config
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		private get;
		set;
	}

	[Inject]
	public IDependencyInjection injector
	{
		private get;
		set;
	}

	[Inject]
	public IMoveAnimationCalculator moveAnimationCalculator
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[Inject]
	public IMainThreadTimer timer
	{
		private get;
		set;
	}

	public GameObject CharacterModel
	{
		get;
		private set;
	}

	private int gameFrame
	{
		get
		{
			float num = Time.realtimeSinceStartup - this.animStartTime;
			return (int)(num * 1000f / 16.666666f);
		}
	}

	public CharacterID CharacterID
	{
		get
		{
			return this.characterMenusData.characterID;
		}
	}

	public void Init(CharacterMenusData characterMenusData, SkinDefinition initWithSkin)
	{
		UISceneCharacter._Init_c__AnonStorey0 _Init_c__AnonStorey = new UISceneCharacter._Init_c__AnonStorey0();
		_Init_c__AnonStorey.initWithSkin = initWithSkin;
		_Init_c__AnonStorey._this = this;
		this.characterMenusData = characterMenusData;
		this.displayObject = new GameObject("Display");
		this.playerObject = new GameObject("Character");
		GameObject gameObject = new GameObject("Centered");
		gameObject.transform.Translate((Vector3)characterMenusData.bounds.rotationCenterOffset);
		this.displayObject.transform.SetParent(gameObject.transform, false);
		gameObject.transform.SetParent(this.playerObject.transform, false);
		this.playerObject.transform.SetParent(base.transform, false);
		Vector3 vector = base.transform.localPosition;
		vector += characterMenusData.uiCharacterPositionOffset;
		base.transform.localPosition = vector;
		if (characterMenusData.characterID == CharacterID.AfiGalu)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			boxCollider.center = new Vector3(0f, 0.75f, 0f);
			boxCollider.size = new Vector3(0.7f, 1.5f, 0.7f);
		}
		this.currentSkin = _Init_c__AnonStorey.initWithSkin.uniqueKey;
		this.skinDataManager.GetSkinData(_Init_c__AnonStorey.initWithSkin, new Action<SkinData>(_Init_c__AnonStorey.__m__0));
	}

	public void OnDestroy()
	{
		if (this.moveController != null)
		{
			this.moveController.Destroy();
		}
	}

	public void Attach(Transform attachTo, Camera usingCamera)
	{
	}

	private void swapCharacterObject(SkinData skinData)
	{
		if (this.moveController != null)
		{
			this.moveController.Destroy();
			this.moveController = null;
		}
		if (this.CharacterModel != null)
		{
			this.CharacterModel.SetActive(false);
			this.CharacterModel.transform.SetParent(null, false);
			UnityEngine.Object.DestroyImmediate(this.CharacterModel);
		}
		UnityEngine.Debug.Log("Help " + this.characterMenusData.characterDefinition);
		UnityEngine.Debug.Log(this.characterDataHelper.GetSkinPrefab(this.characterMenusData.characterDefinition, skinData));
		this.CharacterModel = UnityEngine.Object.Instantiate<GameObject>(this.characterDataHelper.GetSkinPrefab(this.characterMenusData.characterDefinition, skinData));
		this.CharacterModel.transform.SetParent(this.displayObject.transform, false);
		MeshRenderer[] componentsInChildren = this.CharacterModel.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UISceneCharacter._swapCharacterObject_c__AnonStorey1 _swapCharacterObject_c__AnonStorey = new UISceneCharacter._swapCharacterObject_c__AnonStorey1();
			_swapCharacterObject_c__AnonStorey.obj = componentsInChildren[i];
			_swapCharacterObject_c__AnonStorey._this = this;
			if (_swapCharacterObject_c__AnonStorey.obj.name.StartsWith("shadow"))
			{
				_swapCharacterObject_c__AnonStorey.obj.gameObject.SetActive(false);
				this.timer.SetTimeout(1, new Action(_swapCharacterObject_c__AnonStorey.__m__0));
			}
		}
		this.resetPosition();
	}

	public void resetPosition()
	{
		if (this.CharacterModel != null && this.CharacterModel.transform != null)
		{
			this.CharacterModel.transform.localPosition = -(Vector3)this.characterMenusData.bounds.rotationCenterOffset;
		}
	}

	private void OnEnable()
	{
		this.resyncExistingAnimation();
	}

	public void SetSkin(SkinDefinition skinDef)
	{
		UISceneCharacter._SetSkin_c__AnonStorey2 _SetSkin_c__AnonStorey = new UISceneCharacter._SetSkin_c__AnonStorey2();
		_SetSkin_c__AnonStorey.skinDef = skinDef;
		_SetSkin_c__AnonStorey._this = this;
		if (this.currentSkin == _SetSkin_c__AnonStorey.skinDef.uniqueKey)
		{
			return;
		}
		this.currentSkin = _SetSkin_c__AnonStorey.skinDef.uniqueKey;
		this.skinDataManager.GetSkinData(_SetSkin_c__AnonStorey.skinDef, new Action<SkinData>(_SetSkin_c__AnonStorey.__m__0));
	}

	private void setLoadedSkin(SkinData skinData)
	{
		this.swapCharacterObject(skinData);
		this.createAnimationController();
		this.createMoveController(skinData);
		this.resyncExistingAnimation();
	}

	private void resyncExistingAnimation()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.currentAnim.animData != null || this.currentAnim.moveData != null)
		{
			this.animator.enabled = true;
			if (this.currentAnim.type == UISceneCharacterAnimRequest.AnimRequestType.AnimData)
			{
				if (!this.animationPlayer.HasAnimation(this.currentAnim.animData.animationId))
				{
					this.animationPlayer.LoadAnimation(this.currentAnim.animData);
				}
				this.animationPlayer.PlayAnimation(this.currentAnim.animData.animationId, false, 0, 0f, 0f);
			}
			else
			{
				string baseAnimationClipName = this.moveAnimationCalculator.GetBaseAnimationClipName(this.currentAnim.moveData, HorizontalDirection.Right);
				if (!this.animationPlayer.HasAnimation(baseAnimationClipName))
				{
					this.animationPlayer.LoadMove(this.currentAnim.moveData, false);
				}
				this.animationPlayer.PlayAnimation(baseAnimationClipName, false, 0, 0f, 0f);
			}
			this.animationPlayer.ForceSyncAnimation();
			this.animationPlayer.Update(this.gameFrame);
			this.animator.Update(0f);
			this.moveController.SetMove(this.currentAnim.moveData);
		}
	}

	private void createAnimationController()
	{
		this.animationPlayer = new AnimationController();
		this.injector.Inject(this.animationPlayer);
		this.animationPlayer.Init(this.characterMenusData.avatarData, this.playerObject, this.config, false, true, false);
		this.animator = this.CharacterModel.GetComponent<Animator>();
	}

	private void createMoveController(SkinData skinData)
	{
		this.moveController = new UISceneMoveController();
		this.injector.Inject(this.moveController);
		this.moveController.Init(this.characterMenusData, skinData, this.CharacterModel, this.animationPlayer, this.animator, base.transform);
	}

	public void PlayAnimation(UISceneCharacterAnimRequest request)
	{
		this.playAnimation(request);
	}

	public void PlayTransitionAnimation(UISceneCharacterAnimRequest request)
	{
		this.playAnimation(request);
	}

	public void PlayAnimations(List<UISceneCharacterAnimRequest> requests)
	{
	}

	public void SetDefaultAnimation(UISceneCharacterAnimRequest request)
	{
		this.defaultAnim = request;
		this.playAnimation(request);
	}

	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> animation)
	{
	}

	public void Reactivate()
	{
		this.animationPlayer.ForceSyncAnimation();
		this.resetAnimationTime();
		this.resetPosition();
	}

	private void resetAnimationTime()
	{
		this.animStartTime = Time.realtimeSinceStartup;
	}

	private void setAnimationMode(UISceneCharacter.AnimationMode mode)
	{
		this.animationMode = mode;
		if (mode == UISceneCharacter.AnimationMode.VICTORY_POSE)
		{
			this.CharacterModel.transform.localRotation = Quaternion.Euler(0f, -this.characterMenusData.uiRotateOffset.y, 0f);
		}
		else
		{
			this.CharacterModel.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}
	}

	private void playAnimation(UISceneCharacterAnimRequest animationRequest)
	{
		if (animationRequest.animData == null && animationRequest.moveData == null)
		{
			animationRequest = this.defaultAnim;
		}
		if (this.currentAnim == this.defaultAnim && animationRequest == this.defaultAnim)
		{
			return;
		}
		this.currentAnim = animationRequest;
		this.resetPosition();
		this.animator.enabled = true;
		bool flag = false;
		if (this.animationMode != UISceneCharacter.AnimationMode.TRANSITION)
		{
			flag = true;
		}
		this.setAnimationMode(animationRequest.mode);
		if (animationRequest.type == UISceneCharacterAnimRequest.AnimRequestType.AnimData && (animationRequest.animData == null || animationRequest.animData.animationClip == null))
		{
			this.animationPlayer.SetPause(true);
		}
		else
		{
			this.animationPlayer.SetPause(false);
			string animName;
			if (animationRequest.type == UISceneCharacterAnimRequest.AnimRequestType.AnimData)
			{
				animName = animationRequest.animData.animationId;
				if (!this.animationPlayer.HasAnimation(animName))
				{
					this.animationPlayer.LoadAnimation(animationRequest.animData);
				}
			}
			else
			{
				animName = this.moveAnimationCalculator.GetBaseAnimationClipName(animationRequest.moveData, HorizontalDirection.Right);
				if (!this.animationPlayer.HasAnimation(animName))
				{
					this.animationPlayer.LoadMove(animationRequest.moveData, false);
				}
			}
			this.resetAnimationTime();
			float blendDuration = -1f;
			float blendOutDuration = -1f;
			if (flag)
			{
				blendDuration = 0f;
				blendOutDuration = 0f;
			}
			if (base.gameObject.activeInHierarchy)
			{
				this.animationPlayer.PlayAnimation(animName, false, 0, blendDuration, blendOutDuration);
				this.animationPlayer.Update(this.gameFrame);
				this.animator.Update(0f);
			}
			this.moveController.SetMove(this.currentAnim.moveData);
		}
	}

	private void onAnimationEnd()
	{
		this.moveController.OnMoveEnd();
		UISceneCharacterAnimRequest animationRequest = default(UISceneCharacterAnimRequest);
		if (this.currentAnim.type == UISceneCharacterAnimRequest.AnimRequestType.AnimData && this.currentAnim.loopingAnimData != null)
		{
			animationRequest = this.currentAnim;
			animationRequest.animData = animationRequest.loopingAnimData;
		}
		else if (this.currentAnim.type == UISceneCharacterAnimRequest.AnimRequestType.MoveData && this.currentAnim.loopingMoveData != null)
		{
			animationRequest = this.currentAnim;
			animationRequest.moveData = animationRequest.loopingMoveData;
		}
		this.resetAnimationTime();
		this.playAnimation(animationRequest);
	}

	private void FixedUpdate()
	{
		if (this.animator != null)
		{
			this.animator.enabled = false;
		}
	}

	private void Update()
	{
		if (this.animationPlayer != null && this.animationPlayer.CurrentAnimationData != null)
		{
			int gameFrame = this.gameFrame;
			if (gameFrame >= this.animationPlayer.CurrentAnimationGameFramelength)
			{
				this.onAnimationEnd();
				gameFrame = this.gameFrame;
			}
			this.animationPlayer.Update(gameFrame);
		}
		this.moveController.Update(this.gameFrame);
	}

	public void PlayVictoryPose(UISceneCharacterAnimRequest request)
	{
		this.playAnimation(request);
	}
}
