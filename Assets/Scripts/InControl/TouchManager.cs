// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace InControl
{
	[ExecuteInEditMode]
	public class TouchManager : SingletonMonoBehavior<TouchManager, InControlManager>
	{
		public enum GizmoShowOption
		{
			Never,
			WhenSelected,
			UnlessPlaying,
			Always
		}

		private sealed class _UpdateScreenSizeAtEndOfFrame_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal TouchManager _this;

			internal object _current;

			internal bool _disposing;

			internal int _PC;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public _UpdateScreenSizeAtEndOfFrame_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this._current = new WaitForEndOfFrame();
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				case 1u:
					this._this.UpdateScreenSize(this._this.GetCurrentScreenSize());
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 2;
					}
					return true;
				case 2u:
					this._PC = -1;
					break;
				}
				return false;
			}

			public void Dispose()
			{
				this._disposing = true;
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		[Space(10f)]
		public Camera touchCamera;

		public TouchManager.GizmoShowOption controlsShowGizmos = TouchManager.GizmoShowOption.Always;

		[HideInInspector]
		public bool enableControlsOnTouch;

		[HideInInspector, SerializeField]
		private bool _controlsEnabled = true;

		[HideInInspector]
		public int controlsLayer = 5;

		private InputDevice device;

		private Vector3 viewSize;

		private Vector2 screenSize;

		private Vector2 halfScreenSize;

		private float percentToWorld;

		private float halfPercentToWorld;

		private float pixelToWorld;

		private float halfPixelToWorld;

		private TouchControl[] touchControls;

		private TouchPool cachedTouches;

		private List<Touch> activeTouches;

		private ReadOnlyCollection<Touch> readOnlyActiveTouches;

		private Vector2 lastMousePosition;

		private bool isReady;

		private Touch mouseTouch;

		public static event Action OnSetup;

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

		public static ReadOnlyCollection<Touch> Touches
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.readOnlyActiveTouches;
			}
		}

		public static int TouchCount
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.activeTouches.Count;
			}
		}

		public static Camera Camera
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.touchCamera;
			}
		}

		public static InputDevice Device
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.device;
			}
		}

		public static Vector3 ViewSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.viewSize;
			}
		}

		public static float PercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.percentToWorld;
			}
		}

		public static float HalfPercentToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfPercentToWorld;
			}
		}

		public static float PixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.pixelToWorld;
			}
		}

		public static float HalfPixelToWorld
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfPixelToWorld;
			}
		}

		public static Vector2 ScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.screenSize;
			}
		}

		public static Vector2 HalfScreenSize
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.halfScreenSize;
			}
		}

		public static TouchManager.GizmoShowOption ControlsShowGizmos
		{
			get
			{
				return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.controlsShowGizmos;
			}
		}

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

		protected TouchManager()
		{
		}

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
				InputManager.OnSetup += new Action(this.Setup);
				InputManager.OnUpdateDevices += new Action<ulong, float>(this.UpdateDevice);
				InputManager.OnCommitDevices += new Action<ulong, float>(this.CommitDevice);
			}
		}

		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				InputManager.OnSetup -= new Action(this.Setup);
				InputManager.OnUpdateDevices -= new Action<ulong, float>(this.UpdateDevice);
				InputManager.OnCommitDevices -= new Action<ulong, float>(this.CommitDevice);
			}
			this.Reset();
		}

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

		private IEnumerator UpdateScreenSizeAtEndOfFrame()
		{
			TouchManager._UpdateScreenSizeAtEndOfFrame_c__Iterator0 _UpdateScreenSizeAtEndOfFrame_c__Iterator = new TouchManager._UpdateScreenSizeAtEndOfFrame_c__Iterator0();
			_UpdateScreenSizeAtEndOfFrame_c__Iterator._this = this;
			return _UpdateScreenSizeAtEndOfFrame_c__Iterator;
		}

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

		private void UpdateDevice(ulong updateTick, float deltaTime)
		{
			this.UpdateTouches(updateTick, deltaTime);
			this.SubmitControlStates(updateTick, deltaTime);
		}

		private void CommitDevice(ulong updateTick, float deltaTime)
		{
			this.CommitControlStates(updateTick, deltaTime);
		}

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

		private void CreateTouches()
		{
			this.cachedTouches = new TouchPool();
			this.mouseTouch = new Touch();
			this.mouseTouch.fingerId = Touch.FingerID_Mouse;
			this.activeTouches = new List<Touch>(32);
			this.readOnlyActiveTouches = new ReadOnlyCollection<Touch>(this.activeTouches);
		}

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
				UnityEngine.Touch touch = Input.GetTouch(i);
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

		private bool TouchCameraIsValid()
		{
			return !(this.touchCamera == null) && !Utility.IsZero(this.touchCamera.orthographicSize) && (!Utility.IsZero(this.touchCamera.rect.width) || !Utility.IsZero(this.touchCamera.rect.height)) && (!Utility.IsZero(this.touchCamera.pixelRect.width) || !Utility.IsZero(this.touchCamera.pixelRect.height));
		}

		private Vector3 ConvertScreenToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		private Vector3 ConvertViewToWorldPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ViewportToWorldPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		private Vector3 ConvertScreenToViewPoint(Vector2 point)
		{
			if (this.TouchCameraIsValid())
			{
				return this.touchCamera.ScreenToViewportPoint(new Vector3(point.x, point.y, -this.touchCamera.transform.position.z));
			}
			return Vector3.zero;
		}

		private Vector2 GetCurrentScreenSize()
		{
			if (this.TouchCameraIsValid())
			{
				return new Vector2((float)this.touchCamera.pixelWidth, (float)this.touchCamera.pixelHeight);
			}
			return new Vector2((float)Screen.width, (float)Screen.height);
		}

		public static Touch GetTouch(int touchIndex)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.activeTouches[touchIndex];
		}

		public static Touch GetTouchByFingerId(int fingerId)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.cachedTouches.FindTouch(fingerId);
		}

		public static Vector3 ScreenToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertScreenToWorldPoint(point);
		}

		public static Vector3 ViewToWorldPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertViewToWorldPoint(point);
		}

		public static Vector3 ScreenToViewPoint(Vector2 point)
		{
			return SingletonMonoBehavior<TouchManager, InControlManager>.Instance.ConvertScreenToViewPoint(point);
		}

		public static float ConvertToWorld(float value, TouchUnitType unitType)
		{
			return value * ((unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorld : TouchManager.PixelToWorld);
		}

		public static Rect PercentToWorldRect(Rect rect)
		{
			return new Rect((rect.xMin - 50f) * TouchManager.ViewSize.x, (rect.yMin - 50f) * TouchManager.ViewSize.y, rect.width * TouchManager.ViewSize.x, rect.height * TouchManager.ViewSize.y);
		}

		public static Rect PixelToWorldRect(Rect rect)
		{
			return new Rect(Mathf.Round(rect.xMin - TouchManager.HalfScreenSize.x) * TouchManager.PixelToWorld, Mathf.Round(rect.yMin - TouchManager.HalfScreenSize.y) * TouchManager.PixelToWorld, Mathf.Round(rect.width) * TouchManager.PixelToWorld, Mathf.Round(rect.height) * TouchManager.PixelToWorld);
		}

		public static Rect ConvertToWorld(Rect rect, TouchUnitType unitType)
		{
			return (unitType != TouchUnitType.Pixels) ? TouchManager.PercentToWorldRect(rect) : TouchManager.PixelToWorldRect(rect);
		}

		public static implicit operator bool(TouchManager instance)
		{
			return instance != null;
		}
	}
}
