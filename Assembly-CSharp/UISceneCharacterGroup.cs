using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

// Token: 0x02000A69 RID: 2665
public class UISceneCharacterGroup : MonoBehaviour, IUISceneCharacter
{
	// Token: 0x17001254 RID: 4692
	// (get) Token: 0x06004D71 RID: 19825 RVA: 0x00146FC4 File Offset: 0x001453C4
	// (set) Token: 0x06004D72 RID: 19826 RVA: 0x00146FCC File Offset: 0x001453CC
	[Inject]
	public IDependencyInjection injector { private get; set; }

	// Token: 0x17001255 RID: 4693
	// (get) Token: 0x06004D73 RID: 19827 RVA: 0x00146FD5 File Offset: 0x001453D5
	// (set) Token: 0x06004D74 RID: 19828 RVA: 0x00146FDD File Offset: 0x001453DD
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x17001256 RID: 4694
	// (get) Token: 0x06004D75 RID: 19829 RVA: 0x00146FE6 File Offset: 0x001453E6
	// (set) Token: 0x06004D76 RID: 19830 RVA: 0x00146FEE File Offset: 0x001453EE
	[Inject]
	public ICharacterMenusDataLoader characterMenusDataLoader { private get; set; }

	// Token: 0x17001257 RID: 4695
	// (get) Token: 0x06004D77 RID: 19831 RVA: 0x00146FF7 File Offset: 0x001453F7
	// (set) Token: 0x06004D78 RID: 19832 RVA: 0x00146FFF File Offset: 0x001453FF
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x17001258 RID: 4696
	// (get) Token: 0x06004D79 RID: 19833 RVA: 0x00147008 File Offset: 0x00145408
	// (set) Token: 0x06004D7A RID: 19834 RVA: 0x00147010 File Offset: 0x00145410
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x17001259 RID: 4697
	// (get) Token: 0x06004D7B RID: 19835 RVA: 0x00147019 File Offset: 0x00145419
	// (set) Token: 0x06004D7C RID: 19836 RVA: 0x00147021 File Offset: 0x00145421
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x06004D7D RID: 19837 RVA: 0x0014702C File Offset: 0x0014542C
	public void Init(CharacterMenusData characterMenusData, SkinDefinition initWithSkin)
	{
		this.characterMenusData = characterMenusData;
		foreach (CharacterDefinition characterDefinition in this.characterDataHelper.GetLinkedCharacters(characterMenusData.characterDefinition))
		{
			UISceneCharacter uisceneCharacter = new GameObject("UISceneChar-" + characterDefinition.characterName).AddComponent<UISceneCharacter>();
			this.injector.Inject(uisceneCharacter);
			uisceneCharacter.Init(this.characterMenusDataLoader.GetData(characterDefinition), initWithSkin);
			this.list.Add(uisceneCharacter);
			uisceneCharacter.transform.SetParent(base.transform, false);
		}
		this.initRotation();
		foreach (UISceneCharacter uisceneCharacter2 in this.list)
		{
			this.averagePosition += uisceneCharacter2.transform.localPosition;
		}
		if (this.list.Count > 0)
		{
			this.averagePosition /= (float)this.list.Count;
		}
		foreach (UISceneCharacter uisceneCharacter3 in this.list)
		{
			uisceneCharacter3.InitialPosition = uisceneCharacter3.transform.localPosition - this.averagePosition;
		}
		this.Reinitialize();
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x001471C8 File Offset: 0x001455C8
	public void Reinitialize()
	{
		this.frontCharIndex = 0;
		this.ResetRotation();
		this.SetMode(UIAssetDisplayMode.Centered);
		this.Attach(null, null);
		this.initRotation();
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x001471EC File Offset: 0x001455EC
	private void initRotation()
	{
		Vector3 euler = default(Vector3) + this.characterMenusData.uiRotateOffset;
		base.transform.localRotation = Quaternion.Euler(euler);
	}

	// Token: 0x06004D80 RID: 19840 RVA: 0x00147224 File Offset: 0x00145624
	public void Activate(Transform setParent)
	{
		base.transform.SetParent(setParent, false);
		base.gameObject.SetActive(true);
		foreach (UISceneCharacter uisceneCharacter in this.list)
		{
			uisceneCharacter.Reactivate();
		}
	}

	// Token: 0x06004D81 RID: 19841 RVA: 0x00147298 File Offset: 0x00145698
	public void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition(true);
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x001472AF File Offset: 0x001456AF
	public void AddAligner(CharacterMenusData.UICharacterAdjustments aligner)
	{
		this.aligners.Add(aligner);
		this.updatePosition(true);
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x001472C4 File Offset: 0x001456C4
	public void RemoveAligners()
	{
		this.aligners.Clear();
		this.updatePosition(true);
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001472D8 File Offset: 0x001456D8
	public void SetSkin(SkinDefinition skinDefinition)
	{
		for (int i = 0; i < this.list.Count; i++)
		{
			this.list[i].SetSkin(skinDefinition);
		}
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x00147313 File Offset: 0x00145713
	private void reorderList(List<UISceneCharacterAnimRequest> listIn)
	{
		if (this.frontCharIndex == 1)
		{
			listIn.Reverse();
		}
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x00147328 File Offset: 0x00145728
	public void PlayAnimations(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].PlayAnimation(animations[i]);
		}
	}

	// Token: 0x06004D87 RID: 19847 RVA: 0x0014736C File Offset: 0x0014576C
	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].SetDefaultAnimation(animations[i]);
		}
	}

	// Token: 0x06004D88 RID: 19848 RVA: 0x001473AF File Offset: 0x001457AF
	private void Update()
	{
		this.updatePosition(true);
	}

	// Token: 0x06004D89 RID: 19849 RVA: 0x001473B8 File Offset: 0x001457B8
	private void updatePosition(bool updateRotation)
	{
		bool flag = false;
		if (this.attachToUI != null)
		{
			Vector3 position = this.attachToUI.position;
			position.z = Mathf.Abs(this.attachToUICamera.transform.position.z - base.transform.position.z);
			Vector3 position2 = this.attachToUICamera.ScreenToWorldPoint(position);
			position2.z = base.transform.position.z;
			base.transform.position = position2;
			flag = true;
		}
		if (this.aligners.Count > 0)
		{
			Vector3 localPosition = (!flag) ? Vector3.zero : base.transform.localPosition;
			Vector3 vector = Vector3.one;
			Vector3 vector2 = Vector3.zero + this.characterMenusData.uiRotateOffset;
			foreach (CharacterMenusData.UICharacterAdjustments uicharacterAdjustments in this.aligners)
			{
				vector *= uicharacterAdjustments.scale;
			}
			foreach (CharacterMenusData.UICharacterAdjustments uicharacterAdjustments2 in this.aligners)
			{
				localPosition.x += uicharacterAdjustments2.position.x * vector.x;
				localPosition.y += uicharacterAdjustments2.position.y * vector.y;
				localPosition.z += uicharacterAdjustments2.position.z * vector.z;
				vector2 += uicharacterAdjustments2.rotation;
			}
			base.transform.localScale = vector;
			base.transform.localPosition = localPosition;
			if (updateRotation)
			{
				base.transform.localRotation = Quaternion.Euler(vector2);
			}
		}
	}

	// Token: 0x06004D8A RID: 19850 RVA: 0x001475E8 File Offset: 0x001459E8
	public void SetMode(UIAssetDisplayMode mode)
	{
		if (this.currentMode != mode)
		{
			this.currentMode = mode;
			if (mode != UIAssetDisplayMode.Centered)
			{
				if (mode == UIAssetDisplayMode.OffsetRotate)
				{
					this.displacementMagnitude = 1f;
				}
			}
			else
			{
				this.displacementMagnitude = 0f;
			}
			this.updateCurrentAngle();
		}
	}

	// Token: 0x1700125A RID: 4698
	// (get) Token: 0x06004D8B RID: 19851 RVA: 0x00147640 File Offset: 0x00145A40
	// (set) Token: 0x06004D8C RID: 19852 RVA: 0x00147648 File Offset: 0x00145A48
	public float CurrentAngle
	{
		get
		{
			return this.currentAngle;
		}
		private set
		{
			this.currentAngle = value;
			this.updateCurrentAngle();
		}
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x00147658 File Offset: 0x00145A58
	private void updateCurrentAngle()
	{
		for (int i = 0; i < this.list.Count; i++)
		{
			UISceneCharacter uisceneCharacter = this.list[i];
			Vector3 b = Quaternion.Euler(0f, this.currentAngle, 0f) * uisceneCharacter.InitialPosition * this.displacementMagnitude;
			uisceneCharacter.transform.localPosition = this.averagePosition + b;
		}
	}

	// Token: 0x06004D8E RID: 19854 RVA: 0x001476D4 File Offset: 0x00145AD4
	public void PlayVictoryPose(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].PlayVictoryPose(animations[i]);
		}
	}

	// Token: 0x06004D8F RID: 19855 RVA: 0x00147717 File Offset: 0x00145B17
	public void ChangeFrontCharIndex(int value)
	{
		this.frontCharIndex = value;
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x00147720 File Offset: 0x00145B20
	public void PlayTransition(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].PlayTransitionAnimation(animations[i]);
		}
		float targetAngle = this.getTargetAngle();
		this.killRotateTween();
		this.rotationTweener = DOTween.To(new DOGetter<float>(this.get_CurrentAngle), delegate(float x)
		{
			this.CurrentAngle = x;
		}, targetAngle, 1f).SetEase(Ease.OutCirc).OnComplete(delegate
		{
			this.killRotateTween();
		});
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x001477B4 File Offset: 0x00145BB4
	private float getTargetAngle()
	{
		float num = 360f;
		if (this.list.Count > 0)
		{
			num /= (float)this.list.Count;
		}
		return num * (float)this.frontCharIndex;
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x001477F0 File Offset: 0x00145BF0
	public void InstantSyncFrontCharacter()
	{
		float targetAngle = this.getTargetAngle();
		this.killRotateTween();
		this.CurrentAngle = targetAngle;
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x00147811 File Offset: 0x00145C11
	public void ResetRotation()
	{
		this.CurrentAngle = 0f;
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x0014781E File Offset: 0x00145C1E
	private void killRotateTween()
	{
		TweenUtil.Destroy(ref this.rotationTweener);
	}

	// Token: 0x1700125B RID: 4699
	// (get) Token: 0x06004D95 RID: 19861 RVA: 0x0014782B File Offset: 0x00145C2B
	public bool IsCharacterSwapping
	{
		get
		{
			return this.rotationTweener != null && this.rotationTweener.ElapsedPercentage(true) < 0.75f;
		}
	}

	// Token: 0x1700125C RID: 4700
	// (get) Token: 0x06004D96 RID: 19862 RVA: 0x0014784E File Offset: 0x00145C4E
	public CharacterID CharacterID
	{
		get
		{
			return this.characterMenusData.characterID;
		}
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x0014785C File Offset: 0x00145C5C
	public int GetClickedCharacterIndex(Vector2 clickPosition, Camera theCamera)
	{
		Ray ray = theCamera.ScreenPointToRay(clickPosition);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			for (int i = 0; i < this.list.Count; i++)
			{
				if (raycastHit.collider.gameObject == this.list[i].gameObject)
				{
					return i;
				}
			}
		}
		return -1;
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001478C9 File Offset: 0x00145CC9
	public GameObject GetCharacterObject(int index = 0)
	{
		return this.list[index % this.list.Count].CharacterModel;
	}

	// Token: 0x040032D0 RID: 13008
	private Transform attachToUI;

	// Token: 0x040032D1 RID: 13009
	private Camera attachToUICamera;

	// Token: 0x040032D2 RID: 13010
	private List<CharacterMenusData.UICharacterAdjustments> aligners = new List<CharacterMenusData.UICharacterAdjustments>(2);

	// Token: 0x040032D3 RID: 13011
	private Vector3 averagePosition = Vector3.zero;

	// Token: 0x040032D4 RID: 13012
	private float displacementMagnitude;

	// Token: 0x040032D5 RID: 13013
	private float currentAngle;

	// Token: 0x040032D6 RID: 13014
	private const float TRANSITION_DURATION = 1f;

	// Token: 0x040032D7 RID: 13015
	private int frontCharIndex;

	// Token: 0x040032D8 RID: 13016
	private UIAssetDisplayMode currentMode = UIAssetDisplayMode.Centered;

	// Token: 0x040032D9 RID: 13017
	private Tweener rotationTweener;

	// Token: 0x040032DA RID: 13018
	private CharacterMenusData characterMenusData;

	// Token: 0x040032DB RID: 13019
	private List<UISceneCharacter> list = new List<UISceneCharacter>();
}
