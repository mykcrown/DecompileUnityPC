using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008FC RID: 2300
public class UICSSSceneCharacter : MonoBehaviour
{
	// Token: 0x17000E46 RID: 3654
	// (get) Token: 0x06003B85 RID: 15237 RVA: 0x00115251 File Offset: 0x00113651
	// (set) Token: 0x06003B86 RID: 15238 RVA: 0x00115259 File Offset: 0x00113659
	public CharacterMenusData characterMenusData { get; private set; }

	// Token: 0x06003B87 RID: 15239 RVA: 0x00115262 File Offset: 0x00113662
	public void Init(CharacterMenusData characterMenusData)
	{
		this.characterMenusData = characterMenusData;
		this.RandomMode.SetActive(characterMenusData.isRandom);
		this.CharacterContainer.SetActive(!characterMenusData.isRandom);
	}

	// Token: 0x17000E47 RID: 3655
	// (get) Token: 0x06003B88 RID: 15240 RVA: 0x00115290 File Offset: 0x00113690
	public CharacterID CharacterID
	{
		get
		{
			return this.characterMenusData.characterID;
		}
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x0011529D File Offset: 0x0011369D
	public int GetClickedCharacterIndex(Vector2 position, Camera theCamera)
	{
		if (this.characterDisplay != null)
		{
			return this.characterDisplay.GetClickedCharacterIndex(position, theCamera);
		}
		return -1;
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x001152B9 File Offset: 0x001136B9
	public void SetCharacterDisplay(IUISceneCharacter characterDisplay)
	{
		this.characterDisplay = characterDisplay;
		(characterDisplay as Component).transform.SetParent(this.CharacterContainer.transform, false);
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x001152E0 File Offset: 0x001136E0
	public void SetIndex(float value)
	{
		this.positionIndex = value;
		Vector3 localPosition = default(Vector3);
		localPosition.x = this.positionIndex * -this.CharacterXPositionSpread;
		this.AdjustContainer.localPosition = localPosition;
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x0011531D File Offset: 0x0011371D
	public void UnsetCharacterDisplay()
	{
		this.characterDisplay = null;
	}

	// Token: 0x17000E48 RID: 3656
	// (get) Token: 0x06003B8D RID: 15245 RVA: 0x00115326 File Offset: 0x00113726
	public IUISceneCharacter CharacterDisplay
	{
		get
		{
			return this.characterDisplay;
		}
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x00115330 File Offset: 0x00113730
	public void LoadAdjustments(CharacterMenusData.UICharacterAdjustments adjustments)
	{
		this.LookAtCameraRotator.transform.localPosition = adjustments.position;
		this.CharacterContainer.transform.localRotation = Quaternion.Euler(adjustments.rotation);
		this.LookAtCameraRotator.transform.localScale = new Vector3(adjustments.scale, adjustments.scale, adjustments.scale);
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x00115395 File Offset: 0x00113795
	public void Activate(Transform setParent)
	{
		if (this.characterDisplay != null)
		{
			this.characterDisplay.Activate(this.CharacterContainer.transform);
		}
		base.transform.SetParent(setParent, false);
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x001153D1 File Offset: 0x001137D1
	public void Attach(Transform attachTo, Camera usingCamera)
	{
		this.attachToUI = attachTo;
		this.attachToUICamera = usingCamera;
		this.updatePosition();
		this.lookAtCamera();
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x001153ED File Offset: 0x001137ED
	public void ChangeFrontCharIndex(int index)
	{
		this.characterDisplay.ChangeFrontCharIndex(index);
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x001153FB File Offset: 0x001137FB
	public void PlayTransition(List<UISceneCharacterAnimRequest> requests)
	{
		this.characterDisplay.PlayTransition(requests);
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x00115409 File Offset: 0x00113809
	public void SetDefaultAnimations(List<UISceneCharacterAnimRequest> requests)
	{
		if (this.characterDisplay != null)
		{
			this.characterDisplay.SetDefaultAnimations(requests);
		}
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x00115422 File Offset: 0x00113822
	public void SetGlowState(bool enabled)
	{
		this.ReadyGlow.SetActive(enabled);
		this.NotReadyGlow.SetActive(!enabled);
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x00115440 File Offset: 0x00113840
	public void UpdateGlowStateWidth(float x)
	{
		Vector3 localScale = this.ReadyGlow.transform.localScale;
		localScale.x = x;
		this.ReadyGlow.transform.localScale = localScale;
		localScale = this.NotReadyGlow.transform.localScale;
		localScale.x = x;
		this.NotReadyGlow.transform.localScale = localScale;
	}

	// Token: 0x17000E49 RID: 3657
	// (get) Token: 0x06003B96 RID: 15254 RVA: 0x001154A1 File Offset: 0x001138A1
	public bool IsCharacterSwapping
	{
		get
		{
			return this.characterDisplay != null && this.characterDisplay.IsCharacterSwapping;
		}
	}

	// Token: 0x06003B97 RID: 15255 RVA: 0x001154BB File Offset: 0x001138BB
	private void Update()
	{
		this.updatePosition();
		this.lookAtCamera();
	}

	// Token: 0x06003B98 RID: 15256 RVA: 0x001154CC File Offset: 0x001138CC
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

	// Token: 0x06003B99 RID: 15257 RVA: 0x001155F8 File Offset: 0x001139F8
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

	// Token: 0x040028F7 RID: 10487
	public Transform Container2D;

	// Token: 0x040028F8 RID: 10488
	public GameObject ReadyGlow;

	// Token: 0x040028F9 RID: 10489
	public GameObject NotReadyGlow;

	// Token: 0x040028FA RID: 10490
	public GameObject LookAtCameraRotator;

	// Token: 0x040028FB RID: 10491
	public GameObject CharacterContainer;

	// Token: 0x040028FC RID: 10492
	public GameObject RandomMode;

	// Token: 0x040028FD RID: 10493
	public Transform AdjustContainer;

	// Token: 0x040028FE RID: 10494
	public float XPositionSpread = 0.15f;

	// Token: 0x040028FF RID: 10495
	public float CharacterXPositionSpread = 0.08f;

	// Token: 0x04002901 RID: 10497
	private IUISceneCharacter characterDisplay;

	// Token: 0x04002902 RID: 10498
	private Transform attachToUI;

	// Token: 0x04002903 RID: 10499
	private Camera attachToUICamera;

	// Token: 0x04002904 RID: 10500
	private float yRotate;

	// Token: 0x04002905 RID: 10501
	private float positionIndex;
}
