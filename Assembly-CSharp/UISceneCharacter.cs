using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A67 RID: 2663
public class UISceneCharacter : MonoBehaviour
{
	// Token: 0x1700124A RID: 4682
	// (get) Token: 0x06004D46 RID: 19782 RVA: 0x0014648A File Offset: 0x0014488A
	// (set) Token: 0x06004D47 RID: 19783 RVA: 0x00146492 File Offset: 0x00144892
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x1700124B RID: 4683
	// (get) Token: 0x06004D48 RID: 19784 RVA: 0x0014649B File Offset: 0x0014489B
	// (set) Token: 0x06004D49 RID: 19785 RVA: 0x001464A3 File Offset: 0x001448A3
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x1700124C RID: 4684
	// (get) Token: 0x06004D4A RID: 19786 RVA: 0x001464AC File Offset: 0x001448AC
	// (set) Token: 0x06004D4B RID: 19787 RVA: 0x001464B4 File Offset: 0x001448B4
	[Inject]
	public IEquipmentModel equipmentModel { private get; set; }

	// Token: 0x1700124D RID: 4685
	// (get) Token: 0x06004D4C RID: 19788 RVA: 0x001464BD File Offset: 0x001448BD
	// (set) Token: 0x06004D4D RID: 19789 RVA: 0x001464C5 File Offset: 0x001448C5
	[Inject]
	public IDependencyInjection injector { private get; set; }

	// Token: 0x1700124E RID: 4686
	// (get) Token: 0x06004D4E RID: 19790 RVA: 0x001464CE File Offset: 0x001448CE
	// (set) Token: 0x06004D4F RID: 19791 RVA: 0x001464D6 File Offset: 0x001448D6
	[Inject]
	public IMoveAnimationCalculator moveAnimationCalculator { private get; set; }

	// Token: 0x1700124F RID: 4687
	// (get) Token: 0x06004D50 RID: 19792 RVA: 0x001464DF File Offset: 0x001448DF
	// (set) Token: 0x06004D51 RID: 19793 RVA: 0x001464E7 File Offset: 0x001448E7
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17001250 RID: 4688
	// (get) Token: 0x06004D52 RID: 19794 RVA: 0x001464F0 File Offset: 0x001448F0
	// (set) Token: 0x06004D53 RID: 19795 RVA: 0x001464F8 File Offset: 0x001448F8
	[Inject]
	public IMainThreadTimer timer { private get; set; }

	// Token: 0x17001251 RID: 4689
	// (get) Token: 0x06004D54 RID: 19796 RVA: 0x00146501 File Offset: 0x00144901
	// (set) Token: 0x06004D55 RID: 19797 RVA: 0x00146509 File Offset: 0x00144909
	public GameObject CharacterModel { get; private set; }

	// Token: 0x06004D56 RID: 19798 RVA: 0x00146514 File Offset: 0x00144914
	public void Init(CharacterMenusData characterMenusData, SkinDefinition initWithSkin)
	{
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
		this.currentSkin = initWithSkin.uniqueKey;
		this.skinDataManager.GetSkinData(initWithSkin, delegate(SkinData skinData)
		{
			if (this.currentSkin == initWithSkin.uniqueKey)
			{
				this.setLoadedSkin(skinData);
			}
		});
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x00146666 File Offset: 0x00144A66
	public void OnDestroy()
	{
		if (this.moveController != null)
		{
			this.moveController.Destroy();
		}
	}

	// Token: 0x06004D58 RID: 19800 RVA: 0x0014667E File Offset: 0x00144A7E
	public void Attach(Transform attachTo, Camera usingCamera)
	{
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x00146680 File Offset: 0x00144A80
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
		Debug.Log("Help " + this.characterMenusData.characterDefinition);
		Debug.Log(this.characterDataHelper.GetSkinPrefab(this.characterMenusData.characterDefinition, skinData));
		this.CharacterModel = UnityEngine.Object.Instantiate<GameObject>(this.characterDataHelper.GetSkinPrefab(this.characterMenusData.characterDefinition, skinData));
		this.CharacterModel.transform.SetParent(this.displayObject.transform, false);
		MeshRenderer[] componentsInChildren = this.CharacterModel.GetComponentsInChildren<MeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			MeshRenderer obj = componentsInChildren[i];
			UISceneCharacter $this = this;
			if (obj.name.StartsWith("shadow"))
			{
				obj.gameObject.SetActive(false);
				this.timer.SetTimeout(1, delegate
				{
					obj.gameObject.SetActive(true);
					$this.timer.SetTimeout(1, delegate
					{
						obj.gameObject.SetActive(false);
					});
				});
			}
		}
		this.resetPosition();
	}

	// Token: 0x06004D5A RID: 19802 RVA: 0x001467D8 File Offset: 0x00144BD8
	public void resetPosition()
	{
		if (this.CharacterModel != null && this.CharacterModel.transform != null)
		{
			this.CharacterModel.transform.localPosition = -(Vector3)this.characterMenusData.bounds.rotationCenterOffset;
		}
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x00146836 File Offset: 0x00144C36
	private void OnEnable()
	{
		this.resyncExistingAnimation();
	}

	// Token: 0x06004D5C RID: 19804 RVA: 0x00146840 File Offset: 0x00144C40
	public void SetSkin(SkinDefinition skinDef)
	{
		if (this.currentSkin == skinDef.uniqueKey)
		{
			return;
		}
		this.currentSkin = skinDef.uniqueKey;
		this.skinDataManager.GetSkinData(skinDef, delegate(SkinData skinData)
		{
			if (this.currentSkin == skinDef.uniqueKey)
			{
				this.setLoadedSkin(skinData);
			}
		});
	}

	// Token: 0x06004D5D RID: 19805 RVA: 0x001468AB File Offset: 0x00144CAB
	private void setLoadedSkin(SkinData skinData)
	{
		this.swapCharacterObject(skinData);
		this.createAnimationController();
		this.createMoveController(skinData);
		this.resyncExistingAnimation();
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x001468C8 File Offset: 0x00144CC8
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

	// Token: 0x06004D5F RID: 19807 RVA: 0x00146A28 File Offset: 0x00144E28
	private void createAnimationController()
	{
		this.animationPlayer = new AnimationController();
		this.injector.Inject(this.animationPlayer);
		this.animationPlayer.Init(this.characterMenusData.avatarData, this.playerObject, this.config, false, true, false);
		this.animator = this.CharacterModel.GetComponent<Animator>();
	}

	// Token: 0x06004D60 RID: 19808 RVA: 0x00146A88 File Offset: 0x00144E88
	private void createMoveController(SkinData skinData)
	{
		this.moveController = new UISceneMoveController();
		this.injector.Inject(this.moveController);
		this.moveController.Init(this.characterMenusData, skinData, this.CharacterModel, this.animationPlayer, this.animator, base.transform);
	}

	// Token: 0x06004D61 RID: 19809 RVA: 0x00146ADB File Offset: 0x00144EDB
	public void PlayAnimation(UISceneCharacterAnimRequest request)
	{
		this.playAnimation(request);
	}

	// Token: 0x06004D62 RID: 19810 RVA: 0x00146AE4 File Offset: 0x00144EE4
	public void PlayTransitionAnimation(UISceneCharacterAnimRequest request)
	{
		this.playAnimation(request);
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x00146AED File Offset: 0x00144EED
	public void PlayAnimations(List<UISceneCharacterAnimRequest> requests)
	{
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x00146AEF File Offset: 0x00144EEF
	public void SetDefaultAnimation(UISceneCharacterAnimRequest request)
	{
		this.defaultAnim = request;
		this.playAnimation(request);
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x00146AFF File Offset: 0x00144EFF
	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> animation)
	{
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x00146B01 File Offset: 0x00144F01
	public void Reactivate()
	{
		this.animationPlayer.ForceSyncAnimation();
		this.resetAnimationTime();
		this.resetPosition();
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x00146B1A File Offset: 0x00144F1A
	private void resetAnimationTime()
	{
		this.animStartTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06004D68 RID: 19816 RVA: 0x00146B28 File Offset: 0x00144F28
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

	// Token: 0x06004D69 RID: 19817 RVA: 0x00146B9C File Offset: 0x00144F9C
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

	// Token: 0x06004D6A RID: 19818 RVA: 0x00146D70 File Offset: 0x00145170
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

	// Token: 0x17001252 RID: 4690
	// (get) Token: 0x06004D6B RID: 19819 RVA: 0x00146E14 File Offset: 0x00145214
	private int gameFrame
	{
		get
		{
			float num = Time.realtimeSinceStartup - this.animStartTime;
			return (int)(num * 1000f / 16.666666f);
		}
	}

	// Token: 0x06004D6C RID: 19820 RVA: 0x00146E3C File Offset: 0x0014523C
	private void FixedUpdate()
	{
		if (this.animator != null)
		{
			this.animator.enabled = false;
		}
	}

	// Token: 0x06004D6D RID: 19821 RVA: 0x00146E5C File Offset: 0x0014525C
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

	// Token: 0x06004D6E RID: 19822 RVA: 0x00146EC6 File Offset: 0x001452C6
	public void PlayVictoryPose(UISceneCharacterAnimRequest request)
	{
		this.playAnimation(request);
	}

	// Token: 0x17001253 RID: 4691
	// (get) Token: 0x06004D6F RID: 19823 RVA: 0x00146ECF File Offset: 0x001452CF
	public CharacterID CharacterID
	{
		get
		{
			return this.characterMenusData.characterID;
		}
	}

	// Token: 0x040032B9 RID: 12985
	private Animator animator;

	// Token: 0x040032BA RID: 12986
	private IAnimationPlayer animationPlayer;

	// Token: 0x040032BB RID: 12987
	private UISceneMoveController moveController;

	// Token: 0x040032BD RID: 12989
	private GameObject displayObject;

	// Token: 0x040032BE RID: 12990
	private GameObject playerObject;

	// Token: 0x040032BF RID: 12991
	private CharacterMenusData characterMenusData;

	// Token: 0x040032C0 RID: 12992
	private UISceneCharacterAnimRequest currentAnim = default(UISceneCharacterAnimRequest);

	// Token: 0x040032C1 RID: 12993
	private UISceneCharacterAnimRequest defaultAnim;

	// Token: 0x040032C2 RID: 12994
	private float animStartTime;

	// Token: 0x040032C3 RID: 12995
	private UISceneCharacter.AnimationMode animationMode;

	// Token: 0x040032C4 RID: 12996
	public Vector3 InitialPosition;

	// Token: 0x040032C5 RID: 12997
	private string currentSkin;

	// Token: 0x02000A68 RID: 2664
	public enum AnimationMode
	{
		// Token: 0x040032C7 RID: 12999
		IN_GAME,
		// Token: 0x040032C8 RID: 13000
		VICTORY_POSE,
		// Token: 0x040032C9 RID: 13001
		TRANSITION
	}
}
