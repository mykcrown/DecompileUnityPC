// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UICSSSceneCharacter : MonoBehaviour
{
	public Transform Container2D;

	public GameObject ReadyGlow;

	public GameObject NotReadyGlow;

	public GameObject LookAtCameraRotator;

	public GameObject CharacterContainer;

	public GameObject RandomMode;

	public Transform AdjustContainer;

	public float XPositionSpread = 0.15f;

	public float CharacterXPositionSpread = 0.08f;

	private IUISceneCharacter characterDisplay;

	private Transform attachToUI;

	private Camera attachToUICamera;

	private float yRotate;

	private float positionIndex;

	public CharacterMenusData characterMenusData
	{
		get;
		private set;
	}

	public CharacterID CharacterID
	{
		get
		{
			return this.characterMenusData.characterID;
		}
	}

	public IUISceneCharacter CharacterDisplay
	{
		get
		{
			return this.characterDisplay;
		}
	}

	public bool IsCharacterSwapping
	{
		get
		{
			return this.characterDisplay != null && this.characterDisplay.IsCharacterSwapping;
		}
	}

	public void Init(CharacterMenusData characterMenusData)
	{
		this.characterMenusData = characterMenusData;
		this.RandomMode.SetActive(characterMenusData.isRandom);
		this.CharacterContainer.SetActive(!characterMenusData.isRandom);
	}

	public int GetClickedCharacterIndex(Vector2 position, Camera theCamera)
	{
		if (this.characterDisplay != null)
		{
			return this.characterDisplay.GetClickedCharacterIndex(position, theCamera);
		}
		return -1;
	}

	public void SetCharacterDisplay(IUISceneCharacter characterDisplay)
	{
		this.characterDisplay = characterDisplay;
		(characterDisplay as Component).transform.SetParent(this.CharacterContainer.transform, false);
	}

	public void SetIndex(float value)
	{
		this.positionIndex = value;
		Vector3 localPosition = default(Vector3);
		localPosition.x = this.positionIndex * -this.CharacterXPositionSpread;
		this.AdjustContainer.localPosition = localPosition;
	}

	public void UnsetCharacterDisplay()
	{
		this.characterDisplay = null;
	}

	public void LoadAdjustments(CharacterMenusData.UICharacterAdjustments adjustments)
	{
		this.LookAtCameraRotator.transform.localPosition = adjustments.position;
		this.CharacterContainer.transform.localRotation = Quaternion.Euler(adjustments.rotation);
		this.LookAtCameraRotator.transform.localScale = new Vector3(adjustments.scale, adjustments.scale, adjustments.scale);
	}

	public void Activate(Transform setParent)
	{
		if (this.characterDisplay != null)
		{
			this.characterDisplay.Activate(this.CharacterContainer.transform);
		}
		base.transform.SetParent(setParent, false);
		base.gameObject.SetActive(true);
	}

	public void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition();
		this.lookAtCamera();
	}

	public void ChangeFrontCharIndex(int index)
	{
		this.characterDisplay.ChangeFrontCharIndex(index);
	}

	public void PlayTransition(List<UISceneCharacterAnimRequest> requests)
	{
		this.characterDisplay.PlayTransition(requests);
	}

	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.characterDisplay != null)
		{
			this.characterDisplay.SetDefaultAnimations(requests);
		}
	}

	public void SetGlowState(bool enabled)
	{
		this.ReadyGlow.SetActive(enabled);
		this.NotReadyGlow.SetActive(!enabled);
	}

	public void UpdateGlowStateWidth(float x)
	{
		Vector3 localScale = this.ReadyGlow.transform.localScale;
		localScale.x = x;
		this.ReadyGlow.transform.localScale = localScale;
		localScale = this.NotReadyGlow.transform.localScale;
		localScale.x = x;
		this.NotReadyGlow.transform.localScale = localScale;
	}

	private void Update()
	{
		this.updatePosition();
		this.lookAtCamera();
	}

	private void updatePosition()
	{
		if (this.attachToUI != null)
		{
			Vector3 position = this.attachToUI.position;
			position.z = Mathf.Abs(this.attachToUICamera.transform.position.z - base.transform.position.z);
			Vector3 position2 = this.attachToUICamera.ScreenToWorldPoint(position);
			position2.z = base.transform.position.z;
			base.transform.position = position2;
			position = this.attachToUI.position;
			position.z = Mathf.Abs(this.attachToUICamera.transform.position.z - this.Container2D.position.z);
			position2 = this.attachToUICamera.ScreenToWorldPoint(position);
			position2.z = this.Container2D.position.z;
			position2.x += this.positionIndex * this.XPositionSpread;
			this.Container2D.position = position2;
		}
	}

	private void lookAtCamera()
	{
		Quaternion localRotation = base.transform.localRotation;
		Vector3 up = base.transform.up;
		base.transform.LookAt(2f * Camera.main.transform.position - base.transform.position, up);
		this.yRotate = base.transform.localRotation.eulerAngles.y;
		base.transform.localRotation = localRotation;
		Vector3 eulerAngles = this.LookAtCameraRotator.transform.localRotation.eulerAngles;
		eulerAngles.y = this.yRotate;
		this.LookAtCameraRotator.transform.localRotation = Quaternion.Euler(eulerAngles);
	}
}
