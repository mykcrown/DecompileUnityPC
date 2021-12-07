using System;
using System.Collections.Generic;
using FixedPoint;
using InControl;
using UnityEngine;

// Token: 0x02000975 RID: 2421
public class SensitivityTestWidget : ClientBehavior
{
	// Token: 0x17000F4E RID: 3918
	// (get) Token: 0x060040F5 RID: 16629 RVA: 0x00124BA8 File Offset: 0x00122FA8
	// (set) Token: 0x060040F6 RID: 16630 RVA: 0x00124BB0 File Offset: 0x00122FB0
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x060040F7 RID: 16631 RVA: 0x00124BB9 File Offset: 0x00122FB9
	// (set) Token: 0x060040F8 RID: 16632 RVA: 0x00124BC1 File Offset: 0x00122FC1
	[Inject]
	public ISettingsScreenAPI settingsScreenAPI { get; set; }

	// Token: 0x17000F50 RID: 3920
	// (get) Token: 0x060040F9 RID: 16633 RVA: 0x00124BCA File Offset: 0x00122FCA
	// (set) Token: 0x060040FA RID: 16634 RVA: 0x00124BD2 File Offset: 0x00122FD2
	[Inject]
	public IInputSettingsScreenAPI inputSettingsScreenAPI { get; set; }

	// Token: 0x17000F51 RID: 3921
	// (get) Token: 0x060040FB RID: 16635 RVA: 0x00124BDB File Offset: 0x00122FDB
	private InputDevice inputDevice
	{
		get
		{
			return (InputDevice)this.userInputManager.GetDeviceWithPort(this.settingsScreenAPI.InputPort);
		}
	}

	// Token: 0x060040FC RID: 16636 RVA: 0x00124BF8 File Offset: 0x00122FF8
	public override void Awake()
	{
		base.Awake();
		this.leftTiltArrow = UnityEngine.Object.Instantiate<GameObject>(this.TiltArrowPrefab, this.LeftStickArrowAnchor);
		this.rightTiltArrow = UnityEngine.Object.Instantiate<GameObject>(this.TiltArrowPrefab, this.RightStickArrowAnchor);
		for (int i = 0; i < this.TAP_ARROW_POOL_SIZE; i++)
		{
			TapArrowElement tapArrowElement = UnityEngine.Object.Instantiate<TapArrowElement>(this.TapArrowPrefab, this.LeftStickArrowAnchor);
			tapArrowElement.Init();
			this.tapArrows.Add(tapArrowElement);
		}
		this.leftXTapData = new SensitivityTestWidget.TapData();
		this.leftYTapData = new SensitivityTestWidget.TapData();
		this.rightXTapData = new SensitivityTestWidget.TapData();
		this.rightYTapData = new SensitivityTestWidget.TapData();
	}

	// Token: 0x060040FD RID: 16637 RVA: 0x00124CA0 File Offset: 0x001230A0
	private void Update()
	{
		this.updateStick(InputControlType.LeftStickX, InputControlType.LeftStickY, this.leftTiltArrow, this.LeftStickArrowAnchor, this.leftXTapData, this.leftYTapData);
		this.updateStick(InputControlType.RightStickX, InputControlType.RightStickY, this.rightTiltArrow, this.RightStickArrowAnchor, this.rightXTapData, this.rightYTapData);
		this.updateTrigger(InputControlType.LeftTrigger, this.LeftTriggerOnImage, this.LeftTriggerOffImage);
		this.updateTrigger(InputControlType.RightTrigger, this.RightTriggerOnImage, this.RightTriggerOffImage);
	}

	// Token: 0x060040FE RID: 16638 RVA: 0x00124D28 File Offset: 0x00123128
	private void updateStick(InputControlType controlTypeX, InputControlType controlTypeY, GameObject tiltArrow, Transform stickAnchor, SensitivityTestWidget.TapData xTapData, SensitivityTestWidget.TapData yTapData)
	{
		DeviceBindingSource deviceBindingSource = new DeviceBindingSource(controlTypeX);
		float value = deviceBindingSource.GetValue(this.inputDevice);
		DeviceBindingSource deviceBindingSource2 = new DeviceBindingSource(controlTypeY);
		float value2 = deviceBindingSource2.GetValue(this.inputDevice);
		this.updateTapData(xTapData, value);
		this.updateTapData(yTapData, value2);
		float num = Mathf.Atan2(value2, value);
		if (value == 0f && value2 == 0f)
		{
			tiltArrow.SetActive(false);
		}
		else
		{
			tiltArrow.transform.rotation = Quaternion.Euler(0f, 0f, 57.29578f * num);
			tiltArrow.SetActive(true);
		}
		if (xTapData.isFirstFrameTapped || yTapData.isFirstFrameTapped)
		{
			Debug.Log(xTapData.lastValue + " " + yTapData.lastValue);
			foreach (TapArrowElement tapArrowElement in this.tapArrows)
			{
				if (!tapArrowElement.IsActive())
				{
					tapArrowElement.transform.SetParent(stickAnchor, false);
					tapArrowElement.transform.rotation = Quaternion.Euler(0f, 0f, 57.29578f * num);
					tapArrowElement.Play(this.AlphaDuration);
					break;
				}
			}
		}
	}

	// Token: 0x060040FF RID: 16639 RVA: 0x00124E9C File Offset: 0x0012329C
	private void updateTapData(SensitivityTestWidget.TapData tapData, float value)
	{
		if (Mathf.Sign(tapData.lastValue) != Mathf.Sign(value) || value == 0f)
		{
			tapData.heldDuration = 0f;
		}
		if (value != 0f)
		{
			tapData.heldDuration += Time.deltaTime * WTime.fps;
		}
		bool isTapped = tapData.isTapped;
		if (InputUtils.IsTappedInputValue(base.config.inputConfig, (Fixed)((double)value), (int)tapData.heldDuration))
		{
			tapData.isTapped = true;
		}
		else
		{
			tapData.isTapped = false;
		}
		tapData.isFirstFrameTapped = (tapData.isTapped && !isTapped);
		tapData.lastValue = value;
	}

	// Token: 0x06004100 RID: 16640 RVA: 0x00124F54 File Offset: 0x00123354
	private void updateTrigger(InputControlType controlType, GameObject onImage, GameObject offImage)
	{
		DeviceBindingSource deviceBindingSource = new DeviceBindingSource(controlType);
		bool flag = deviceBindingSource.GetValue(this.inputDevice) > 0f;
		onImage.SetActive(flag);
		offImage.SetActive(!flag);
	}

	// Token: 0x04002BD6 RID: 11222
	public GameObject TiltArrowPrefab;

	// Token: 0x04002BD7 RID: 11223
	public TapArrowElement TapArrowPrefab;

	// Token: 0x04002BD8 RID: 11224
	public Transform LeftStickArrowAnchor;

	// Token: 0x04002BD9 RID: 11225
	public Transform RightStickArrowAnchor;

	// Token: 0x04002BDA RID: 11226
	public float AlphaDuration;

	// Token: 0x04002BDB RID: 11227
	public GameObject LeftTriggerOnImage;

	// Token: 0x04002BDC RID: 11228
	public GameObject LeftTriggerOffImage;

	// Token: 0x04002BDD RID: 11229
	public GameObject RightTriggerOnImage;

	// Token: 0x04002BDE RID: 11230
	public GameObject RightTriggerOffImage;

	// Token: 0x04002BDF RID: 11231
	private GameObject leftTiltArrow;

	// Token: 0x04002BE0 RID: 11232
	private GameObject rightTiltArrow;

	// Token: 0x04002BE1 RID: 11233
	private List<TapArrowElement> tapArrows = new List<TapArrowElement>();

	// Token: 0x04002BE2 RID: 11234
	private SensitivityTestWidget.TapData leftXTapData;

	// Token: 0x04002BE3 RID: 11235
	private SensitivityTestWidget.TapData leftYTapData;

	// Token: 0x04002BE4 RID: 11236
	private SensitivityTestWidget.TapData rightXTapData;

	// Token: 0x04002BE5 RID: 11237
	private SensitivityTestWidget.TapData rightYTapData;

	// Token: 0x04002BE6 RID: 11238
	private int TAP_ARROW_POOL_SIZE = 10;

	// Token: 0x02000976 RID: 2422
	private class TapData
	{
		// Token: 0x04002BE7 RID: 11239
		public float heldDuration;

		// Token: 0x04002BE8 RID: 11240
		public float lastValue;

		// Token: 0x04002BE9 RID: 11241
		public bool isTapped;

		// Token: 0x04002BEA RID: 11242
		public bool isFirstFrameTapped;
	}
}
