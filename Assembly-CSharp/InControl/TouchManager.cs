using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000130 RID: 304
	[ExecuteInEditMode]
	public class TouchManager : SingletonMonoBehavior<TouchManager, InControlManager>
	{
		// Token: 0x060006B2 RID: 1714 RVA: 0x0002D590 File Offset: 0x0002B990
		protected TouchManager()
		{
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060006B3 RID: 1715 RVA: 0x0002D5B0 File Offset: 0x0002B9B0
		// (remove) Token: 0x060006B4 RID: 1716 RVA: 0x0002D5E4 File Offset: 0x0002B9E4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnSetup;

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002D618 File Offset: 0x0002BA18
		private void OnEnable()
		{
			InControlManager component = base.GetComponent<InControlManager>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("Touch Manager component can only be added to the InControl Manager object.");
				UnityEngine.Object.DestroyImmediate(this);
				return;
			}
			if (!base.EnforceSingletonComponent())
			{
				UnityEngine.Debug.LogWarning("There is already a Touch Manager component on this game object.");
				return;
			}
			this.touchControls = base.GetComponentsInChildren<TouchControl>(true);
			if (Application.isPlaying)
			{
				InputManager.OnSetup += this.Setup;
				InputManager.OnUpdateDevices += this.UpdateDevice;
				InputManager.OnCommitDevices += this.CommitDevice;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002D6AC File Offset: 0x0002BAAC
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				InputManager.OnSetup -= this.Setup;
				InputManager.OnUpdateDevices -= this.UpdateDevice;
				InputManager.OnCommitDevices -= this.CommitDevice;
			}
			this.Reset();
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002D6FC File Offset: 0x0002BAFC
		private void Setup()
		{
			this.UpdateScreenSize(this.GetCurrentScreenSize());
			this.CreateDevice();
			this.CreateTouches();
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002D730 File Offset: 0x0002BB30
		private void Reset()
		{
			this.device = null;
			this.mouseTouch = null;
			this.cachedTouches = null;
			this.activeTouches = null;
			this.readOnlyActiveTouches = null;
			this.touchControls = null;
			TouchManager.OnSetup = null;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002D764 File Offset: 0x0002BB64
		private IEnumerator UpdateScreenSizeAtEndOfFrame()
		{
			yield return new WaitForEndOfFrame();
			this.UpdateScreenSize(this.GetCurrentScreenSize());
			yield return null;
			yield break;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002D780 File Offset: 0x0002BB80
		private void Update()
		{
			Vector2 currentScreenSize = this.GetCurrentScreenSize();
			if (!this.isReady)
			{
				base.StartCoroutine(this.UpdateScreenSizeAtEndOfFrame());
				this.UpdateScreenSize(currentScreenSize);
				this.isReady = true;
				return;
			}
			if (this.screenSize != currentScreenSize)
			{
				this.UpdateScreenSize(currentScreenSize);
			}
			if (TouchManager.OnSetup != null)
			{
				TouchManager.OnSetup();
				TouchManager.OnSetup = null;
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002D7F0 File Offset: 0x0002BBF0
		private void CreateDevice()
		{
			this.device = new TouchInputDevice();
			this.device.AddControl(InputControlType.LeftStickLeft, "LeftStickLeft");
			this.device.AddControl(InputControlType.LeftStickRight, "LeftStickRight");
			this.device.AddControl(InputControlType.LeftStickUp, "LeftStickUp");
			this.device.AddControl(InputControlType.LeftStickDown, "LeftStickDown");
			this.device.AddControl(InputControlType.RightStickLeft, "RightStickLeft");
			this.device.AddControl(InputControlType.RightStickRight, "RightStickRight");
			this.device.AddControl(InputControlType.RightStickUp, "RightStickUp");
			this.device.AddControl(InputControlType.RightStickDown, "RightStickDown");
			this.device.AddControl(InputControlType.DPadUp, "DPadUp");
			this.device.AddControl(InputControlType.DPadDown, "DPadDown");
			this.device.AddControl(InputControlType.DPadLeft, "DPadLeft");
			this.device.AddControl(InputControlType.DPadRight, "DPadRight");
			this.device.AddControl(InputControlType.LeftTrigger, "LeftTrigger");
			this.device.AddControl(InputControlType.RightTrigger, "RightTrigger");
			this.device.AddControl(InputControlType.LeftBumper, "LeftBumper");
			this.device.AddControl(InputControlType.RightBumper, "RightBumper");
			for (InputControlType inputControlType = InputControlType.Action1; inputControlType <= InputControlType.Action4; inputControlType++)
			{
				this.device.AddControl(inputControlType, inputControlType.ToString());
			}
			this.device.AddControl(InputControlType.Menu, "Menu");
			for (InputControlType inputControlType2 = InputControlType.Button0; inputControlType2 <= InputControlType.Button19; inputControlType2++)
			{
				this.device.AddControl(inputControlType2, inputControlType2.ToString());
			}
			InputManager.AttachDevice(this.device);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002D9B1 File Offset: 0x0002BDB1
		private void UpdateDevice(ulong updateTick, float deltaTime)
		{
			this.UpdateTouches(updateTick, deltaTime);
			this.SubmitControlStates(updateTick, deltaTime);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002D9C3 File Offset: 0x0002BDC3
		private void CommitDevice(ulong updateTick, float deltaTime)
		{
			this.CommitControlStates(updateTick, deltaTime);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002D9D0 File Offset: 0x0002BDD0
		private void SubmitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.SubmitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002DA24 File Offset: 0x0002BE24
		private void CommitControlStates(ulong updateTick, float deltaTime)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.CommitControlState(updateTick, deltaTime);
				}
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002DA78 File Offset: 0x0002BE78
		private void UpdateScreenSize(Vector2 currentScreenSize)
		{
			this.touchCamera.rect = new Rect(0f, 0f, 0.99f, 1f);
			this.touchCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.screenSize = currentScreenSize;
			this.halfScreenSize = this.screenSize / 2f;
			this.viewSize = this.ConvertViewToWorldPoint(Vector2.one) * 0.02f;
			this.percentToWorld = Mathf.Min(this.viewSize.x, this.viewSize.y);
			this.halfPercentToWorld = this.percentToWorld / 2f;
			if (this.touchCamera != null)
			{
				this.halfPixelToWorld = this.touchCamera.orthographicSize / this.screenSize.y;
				this.pixelToWorld = this.halfPixelToWorld * 2f;
			}
			if (this.touchControls != null)
			{
				int num = this.touchControls.Length;
				for (int i = 0; i < num; i++)
				{
					this.touchControls[i].ConfigureControl();
				}
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002DBAC File Offset: 0x0002BFAC
		private void CreateTouches()
		{
			this.cachedTouches = new TouchPool();
			this.mouseTouch = new Touch();
			this.mouseTouch.fingerId = Touch.FingerID_Mouse;
			this.activeTouches = new List<Touch>(32);
			this.readOnlyActiveTouches = new ReadOnlyCollection<Touch>(this.activeTouches);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002DC00 File Offset: 0x0002C000
		private void UpdateTouches(ulong updateTick, float deltaTime)
		{
			this.activeTouches.Clear();
			this.cachedTouches.FreeEndedTouches();
			if (this.mouseTouch.SetWithMouseData(updateTick, deltaTime))
			{
				this.activeTouches.Add(this.mouseTouch);
			}
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				Touch touch2 = this.cachedTouches.FindOrCreateTouch(touch.fingerId);
				touch2.SetWithTouchData(touch, updateTick, deltaTime);
				this.activeTouches.Add(touch2);
			}
			int count = this.cachedTouches.Touches.Count;
			for (int j = 0; j < count; j++)
			{
				Touch touch3 = this.cachedTouches.Touches[j];
				if (touch3.phase != TouchPhase.Ended && touch3.updateTick != updateTick)
				{
					touch3.phase = TouchPhase.Ended;
					this.activeTouches.Add(touch3);
				}
			}
			this.InvokeTouchEvents();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002DCFC File Offset: 0x0002C0FC
		private void SendTouchBegan(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchBegan(touch);
				}
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002DD50 File Offset: 0x0002C150
		private void SendTouchMoved(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchMoved(touch);
				}
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002DDA4 File Offset: 0x0002C1A4
		private void SendTouchEnded(Touch touch)
		{
			int num = this.touchControls.Length;
			for (int i = 0; i < num; i++)
			{
				TouchControl touchControl = this.touchControls[i];
				if (touchControl.enabled && touchControl.gameObject.activeInHierarchy)
				{
					touchControl.TouchEnded(touch);
				}
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002DDF8 File Offset: 0x0002C1F8
		private void InvokeTouchEvents()
		{
			int count = this.activeTouches.Count;
			if (this.enableControlsOnTouch && count > 0 && !this.controlsEnabled)
			{
				TouchManager.Device.RequestActivation();
				this.controlsEnabled = true;
			}
			for (int i = 0; i < count; i++)
			{
				Touch touch = this.activeTouches[i];
				switch (touch.phase)
				{
				case TouchPhase.Began:
					this.SendTouchBegan(touch);
					break;
				case TouchPhase.Moved:
					this.SendTouchMoved(touch);
					break;
				case TouchPhase.Ended:
					this.SendTouchEnded(touch);
					break;
				case TouchPhase.Canceled:
					this.SendTouchEnded(touch);
					break;
				}
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002DEB4 File Offset: 0x0002C2B4
		private bool TouchCameraIsValid()
		{
			return !(this.touchCamera == null) && !Utility.IsZero(this.touchCamera.orthographicSize) && (!Utility.IsZero(this.touchCamera.rect.width) || !Utility.IsZero(this.touchCamera.rect.height)) && (!Utility.IsZero(this.touchCamera.pixelRect.width) || !Utility.IsZero(this.touchCamera.pixelRect.height));
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0002DF64 File Offset: 0x0002C364
		private Vector3 ConvertScreenToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002DFBC File Offset: 0x0002C3BC
		private Vector3 ConvertViewToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ViewportToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002E014 File Offset: 0x0002C414
		private Vector3 ConvertScreenToViewPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToViewportPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002E069 File Offset: 0x0002C469
		private Vector2 GetCurrentScreenSize()
		{
			if (this.TouchCameraIsValid())
			{
				return new Vector2((float)this.touchCamera.pixelWidth, (float)this.touchCamera.pixelHeight);
			}
			return new Vector2((float)Screen.width, (float)Screen.height);
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0002E0A5 File Offset: 0x0002C4A5
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x0002E0B0 File Offset: 0x0002C4B0
		public bool controlsEnabled
		{
			get
			{
				return this._controlsEnabled;
			}
			set
			{
				if (this._controlsEnabled != value)
				{
					int num = this.touchControls.Length;
					for (int i = 0; i < num; i++)
					{
						this.touchControls[i].enabled = value;
					}
					this._controlsEnabled = value;
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0002E0F9 File Offset: 0x0002C4F9
		public static ReadOnlyCollection<Touch> Touches
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.readOnlyActiveTouches;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0002E105 File Offset: 0x0002C505
		public static int TouchCount
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.activeTouches.Count;
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002E116 File Offset: 0x0002C516
		public static Touch GetTouch(int touchIndex)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.activeTouches[touchIndex];
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002E128 File Offset: 0x0002C528
		public static Touch GetTouchByFingerId(int fingerId)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.cachedTouches.FindTouch(fingerId);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002E13A File Offset: 0x0002C53A
		public static Vector3 ScreenToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertScreenToWorldPoint(point);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002E147 File Offset: 0x0002C547
		public static Vector3 ViewToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertViewToWorldPoint(point);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002E154 File Offset: 0x0002C554
		public static Vector3 ScreenToViewPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertScreenToViewPoint(point);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002E161 File Offset: 0x0002C561
		public static float ConvertToWorld(float value, TouchUnitType unitType)
		{
			return value * ((unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorld : TouchManager.PixelToWorld);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002E17C File Offset: 0x0002C57C
		public static Rect PercentToWorldRect(Rect rect)
		{
			return new Rect((rect.xMin - 50f) * TouchManager.ViewSize.x, (rect.yMin - 50f) * TouchManager.ViewSize.y, rect.width * TouchManager.ViewSize.x, rect.height * TouchManager.ViewSize.y);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002E1F0 File Offset: 0x0002C5F0
		public static Rect PixelToWorldRect(Rect rect)
		{
			return new Rect(Mathf.Round(rect.xMin - TouchManager.HalfScreenSize.x) * TouchManager.PixelToWorld, Mathf.Round(rect.yMin - TouchManager.HalfScreenSize.y) * TouchManager.PixelToWorld, Mathf.Round(rect.width) * TouchManager.PixelToWorld, Mathf.Round(rect.height) * TouchManager.PixelToWorld);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002E266 File Offset: 0x0002C666
		public static Rect ConvertToWorld(Rect rect, TouchUnitType unitType)
		{
			return (unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorldRect(rect) : TouchManager.PixelToWorldRect(rect);
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x0002E280 File Offset: 0x0002C680
		public static Camera Camera
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.touchCamera;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0002E28C File Offset: 0x0002C68C
		public static InputDevice Device
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.device;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0002E298 File Offset: 0x0002C698
		public static Vector3 ViewSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.viewSize;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0002E2A4 File Offset: 0x0002C6A4
		public static float PercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.percentToWorld;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0002E2B0 File Offset: 0x0002C6B0
		public static float HalfPercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfPercentToWorld;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0002E2BC File Offset: 0x0002C6BC
		public static float PixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.pixelToWorld;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0002E2C8 File Offset: 0x0002C6C8
		public static float HalfPixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfPixelToWorld;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0002E2D4 File Offset: 0x0002C6D4
		public static Vector2 ScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.screenSize;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0002E2E0 File Offset: 0x0002C6E0
		public static Vector2 HalfScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfScreenSize;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0002E2EC File Offset: 0x0002C6EC
		public static TouchManager.GizmoShowOption ControlsShowGizmos
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.controlsShowGizmos;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0002E2F8 File Offset: 0x0002C6F8
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x0002E304 File Offset: 0x0002C704
		public static bool ControlsEnabled
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.controlsEnabled;
			}
			set
			{
				SingletonMonoBehavior<TouchManager, InControlManager>.Instance.controlsEnabled = value;
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002E311 File Offset: 0x0002C711
		public static implicit operator bool(TouchManager instance)
		{
			return instance != null;
		}

		// Token: 0x04000542 RID: 1346
		[Space(10f)]
		public Camera touchCamera;

		// Token: 0x04000543 RID: 1347
		public TouchManager.GizmoShowOption controlsShowGizmos = TouchManager.GizmoShowOption.Always;

		// Token: 0x04000544 RID: 1348
		[HideInInspector]
		public bool enableControlsOnTouch;

		// Token: 0x04000545 RID: 1349
		[SerializeField]
		[HideInInspector]
		private bool _controlsEnabled = true;

		// Token: 0x04000546 RID: 1350
		[HideInInspector]
		public int controlsLayer = 5;

		// Token: 0x04000548 RID: 1352
		private InputDevice device;

		// Token: 0x04000549 RID: 1353
		private Vector3 viewSize;

		// Token: 0x0400054A RID: 1354
		private Vector2 screenSize;

		// Token: 0x0400054B RID: 1355
		private Vector2 halfScreenSize;

		// Token: 0x0400054C RID: 1356
		private float percentToWorld;

		// Token: 0x0400054D RID: 1357
		private float halfPercentToWorld;

		// Token: 0x0400054E RID: 1358
		private float pixelToWorld;

		// Token: 0x0400054F RID: 1359
		private float halfPixelToWorld;

		// Token: 0x04000550 RID: 1360
		private TouchControl[] touchControls;

		// Token: 0x04000551 RID: 1361
		private TouchPool cachedTouches;

		// Token: 0x04000552 RID: 1362
		private List<Touch> activeTouches;

		// Token: 0x04000553 RID: 1363
		private ReadOnlyCollection<Touch> readOnlyActiveTouches;

		// Token: 0x04000554 RID: 1364
		private Vector2 lastMousePosition;

		// Token: 0x04000555 RID: 1365
		private bool isReady;

		// Token: 0x04000556 RID: 1366
		private Touch mouseTouch;

		// Token: 0x02000131 RID: 305
		public enum GizmoShowOption
		{
			// Token: 0x04000558 RID: 1368
			Never,
			// Token: 0x04000559 RID: 1369
			WhenSelected,
			// Token: 0x0400055A RID: 1370
			UnlessPlaying,
			// Token: 0x0400055B RID: 1371
			Always
		}
	}
}
