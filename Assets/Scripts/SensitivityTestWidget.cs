// Decompile from assembly: Assembly-CSharp.dll

using FixedPoint;
using InControl;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityTestWidget : ClientBehavior
{
	private class TapData
	{
		public float heldDuration;

		public float lastValue;

		public bool isTapped;

		public bool isFirstFrameTapped;
	}

	public GameObject TiltArrowPrefab;

	public TapArrowElement TapArrowPrefab;

	public Transform LeftStickArrowAnchor;

	public Transform RightStickArrowAnchor;

	public float AlphaDuration;

	public GameObject LeftTriggerOnImage;

	public GameObject LeftTriggerOffImage;

	public GameObject RightTriggerOnImage;

	public GameObject RightTriggerOffImage;

	private GameObject leftTiltArrow;

	private GameObject rightTiltArrow;

	private List<TapArrowElement> tapArrows = new List<TapArrowElement>();

	private SensitivityTestWidget.TapData leftXTapData;

	private SensitivityTestWidget.TapData leftYTapData;

	private SensitivityTestWidget.TapData rightXTapData;

	private SensitivityTestWidget.TapData rightYTapData;

	private int TAP_ARROW_POOL_SIZE = 10;

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	[Inject]
	public ISettingsScreenAPI settingsScreenAPI
	{
		get;
		set;
	}

	[Inject]
	public IInputSettingsScreenAPI inputSettingsScreenAPI
	{
		get;
		set;
	}

	private InputDevice inputDevice
	{
		get
		{
			return (InputDevice)this.userInputManager.GetDeviceWithPort(this.settingsScreenAPI.InputPort);
		}
	}

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

	private void Update()
	{
		this.updateStick(InputControlType.LeftStickX, InputControlType.LeftStickY, this.leftTiltArrow, this.LeftStickArrowAnchor, this.leftXTapData, this.leftYTapData);
		this.updateStick(InputControlType.RightStickX, InputControlType.RightStickY, this.rightTiltArrow, this.RightStickArrowAnchor, this.rightXTapData, this.rightYTapData);
		this.updateTrigger(InputControlType.LeftTrigger, this.LeftTriggerOnImage, this.LeftTriggerOffImage);
		this.updateTrigger(InputControlType.RightTrigger, this.RightTriggerOnImage, this.RightTriggerOffImage);
	}

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
			UnityEngine.Debug.Log(xTapData.lastValue + " " + yTapData.lastValue);
			foreach (TapArrowElement current in this.tapArrows)
			{
				if (!current.IsActive())
				{
					current.transform.SetParent(stickAnchor, false);
					current.transform.rotation = Quaternion.Euler(0f, 0f, 57.29578f * num);
					current.Play(this.AlphaDuration);
					break;
				}
			}
		}
	}

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

	private void updateTrigger(InputControlType controlType, GameObject onImage, GameObject offImage)
	{
		DeviceBindingSource deviceBindingSource = new DeviceBindingSource(controlType);
		bool flag = deviceBindingSource.GetValue(this.inputDevice) > 0f;
		onImage.SetActive(flag);
		offImage.SetActive(!flag);
	}
}
