// Decompile from assembly: Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UISceneCharacterGroup : MonoBehaviour, IUISceneCharacter
{
	private Transform attachToUI;

	private Camera attachToUICamera;

	private List<CharacterMenusData.UICharacterAdjustments> aligners = new List<CharacterMenusData.UICharacterAdjustments>(2);

	private Vector3 averagePosition = Vector3.zero;

	private float displacementMagnitude;

	private float currentAngle;

	private const float TRANSITION_DURATION = 1f;

	private int frontCharIndex;

	private UIAssetDisplayMode currentMode = UIAssetDisplayMode.Centered;

	private Tweener rotationTweener;

	private CharacterMenusData characterMenusData;

	private List<UISceneCharacter> list = new List<UISceneCharacter>();

	[Inject]
	public IDependencyInjection injector
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
	public ICharacterMenusDataLoader characterMenusDataLoader
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
	public GameDataManager gameDataManager
	{
		private get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		private get;
		set;
	}

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

	public bool IsCharacterSwapping
	{
		get
		{
			return this.rotationTweener != null && this.rotationTweener.ElapsedPercentage(true) < 0.75f;
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
		this.characterMenusData = characterMenusData;
		CharacterDefinition[] linkedCharacters = this.characterDataHelper.GetLinkedCharacters(characterMenusData.characterDefinition);
		for (int i = 0; i < linkedCharacters.Length; i++)
		{
			CharacterDefinition characterDefinition = linkedCharacters[i];
			UISceneCharacter uISceneCharacter = new GameObject("UISceneChar-" + characterDefinition.characterName).AddComponent<UISceneCharacter>();
			this.injector.Inject(uISceneCharacter);
			uISceneCharacter.Init(this.characterMenusDataLoader.GetData(characterDefinition), initWithSkin);
			this.list.Add(uISceneCharacter);
			uISceneCharacter.transform.SetParent(base.transform, false);
		}
		this.initRotation();
		foreach (UISceneCharacter current in this.list)
		{
			this.averagePosition += current.transform.localPosition;
		}
		if (this.list.Count > 0)
		{
			this.averagePosition /= (float)this.list.Count;
		}
		foreach (UISceneCharacter current2 in this.list)
		{
			current2.InitialPosition = current2.transform.localPosition - this.averagePosition;
		}
		this.Reinitialize();
	}

	public void Reinitialize()
	{
		this.frontCharIndex = 0;
		this.ResetRotation();
		this.SetMode(UIAssetDisplayMode.Centered);
		this.Attach(null, null);
		this.initRotation();
	}

	private void initRotation()
	{
		Vector3 euler = default(Vector3) + this.characterMenusData.uiRotateOffset;
		base.transform.localRotation = Quaternion.Euler(euler);
	}

	public void Activate(Transform setParent)
	{
		base.transform.SetParent(setParent, false);
		base.gameObject.SetActive(true);
		foreach (UISceneCharacter current in this.list)
		{
			current.Reactivate();
		}
	}

	public void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition(true);
	}

	public void AddAligner(CharacterMenusData.UICharacterAdjustments aligner)
	{
		this.aligners.Add(aligner);
		this.updatePosition(true);
	}

	public void RemoveAligners()
	{
		this.aligners.Clear();
		this.updatePosition(true);
	}

	public void SetSkin(SkinDefinition skinDefinition)
	{
		for (int i = 0; i < this.list.Count; i++)
		{
			this.list[i].SetSkin(skinDefinition);
		}
	}

	private void reorderList(List<UISceneCharacterAnimRequest> listIn)
	{
		if (this.frontCharIndex == 1)
		{
			listIn.Reverse();
		}
	}

	public void PlayAnimations(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].PlayAnimation(animations[i]);
		}
	}

	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].SetDefaultAnimation(animations[i]);
		}
	}

	private void Update()
	{
		this.updatePosition(true);
	}

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
			foreach (CharacterMenusData.UICharacterAdjustments current in this.aligners)
			{
				vector *= current.scale;
			}
			foreach (CharacterMenusData.UICharacterAdjustments current2 in this.aligners)
			{
				localPosition.x += current2.position.x * vector.x;
				localPosition.y += current2.position.y * vector.y;
				localPosition.z += current2.position.z * vector.z;
				vector2 += current2.rotation;
			}
			base.transform.localScale = vector;
			base.transform.localPosition = localPosition;
			if (updateRotation)
			{
				base.transform.localRotation = Quaternion.Euler(vector2);
			}
		}
	}

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

	private void updateCurrentAngle()
	{
		for (int i = 0; i < this.list.Count; i++)
		{
			UISceneCharacter uISceneCharacter = this.list[i];
			Vector3 b = Quaternion.Euler(0f, this.currentAngle, 0f) * uISceneCharacter.InitialPosition * this.displacementMagnitude;
			uISceneCharacter.transform.localPosition = this.averagePosition + b;
		}
	}

	public void PlayVictoryPose(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].PlayVictoryPose(animations[i]);
		}
	}

	public void ChangeFrontCharIndex(int value)
	{
		this.frontCharIndex = value;
	}

	public void PlayTransition(List<UISceneCharacterAnimRequest> animations)
	{
		this.reorderList(animations);
		for (int i = 0; i < animations.Count; i++)
		{
			this.list[i].PlayTransitionAnimation(animations[i]);
		}
		float targetAngle = this.getTargetAngle();
		this.killRotateTween();
		this.rotationTweener = DOTween.To(new DOGetter<float>(this.get_CurrentAngle), new DOSetter<float>(this._PlayTransition_m__0), targetAngle, 1f).SetEase(Ease.OutCirc).OnComplete(new TweenCallback(this._PlayTransition_m__1));
	}

	private float getTargetAngle()
	{
		float num = 360f;
		if (this.list.Count > 0)
		{
			num /= (float)this.list.Count;
		}
		return num * (float)this.frontCharIndex;
	}

	public void InstantSyncFrontCharacter()
	{
		float targetAngle = this.getTargetAngle();
		this.killRotateTween();
		this.CurrentAngle = targetAngle;
	}

	public void ResetRotation()
	{
		this.CurrentAngle = 0f;
	}

	private void killRotateTween()
	{
		TweenUtil.Destroy(ref this.rotationTweener);
	}

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

	public GameObject GetCharacterObject(int index = 0)
	{
		return this.list[index % this.list.Count].CharacterModel;
	}

	private void _PlayTransition_m__0(float x)
	{
		this.CurrentAngle = x;
	}

	private void _PlayTransition_m__1()
	{
		this.killRotateTween();
	}
}
