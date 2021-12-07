// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace InControl
{
	public class TouchButtonControl : TouchControl
	{
		[Header("Position"), SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomRight;

		[SerializeField]
		private TouchUnitType offsetUnitType;

		[SerializeField]
		private Vector2 offset = new Vector2(-10f, 10f);

		[SerializeField]
		private bool lockAspectRatio = true;

		[Header("Options")]
		public TouchControl.ButtonTarget target = TouchControl.ButtonTarget.Action1;

		public bool allowSlideToggle = true;

		public bool toggleOnLeave;

		public bool pressureSensitive;

		[Header("Sprites")]
		public TouchSprite button = new TouchSprite(15f);

		private bool buttonState;

		private Touch currentTouch;

		private bool dirty;

		private bool ButtonState
		{
			get
			{
				return this.buttonState;
			}
			set
			{
				if (this.buttonState != value)
				{
					this.buttonState = value;
					this.button.State = value;
				}
			}
		}

		public Vector3 ButtonPosition
		{
			get
			{
				return (!this.button.Ready) ? base.transform.position : this.button.Position;
			}
			set
			{
				if (this.button.Ready)
				{
					this.button.Position = value;
				}
			}
		}

		public TouchControlAnchor Anchor
		{
			get
			{
				return this.anchor;
			}
			set
			{
				if (this.anchor != value)
				{
					this.anchor = value;
					this.dirty = true;
				}
			}
		}

		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (this.offset != value)
				{
					this.offset = value;
					this.dirty = true;
				}
			}
		}

		public TouchUnitType OffsetUnitType
		{
			get
			{
				return this.offsetUnitType;
			}
			set
			{
				if (this.offsetUnitType != value)
				{
					this.offsetUnitType = value;
					this.dirty = true;
				}
			}
		}

		public override void CreateControl()
		{
			this.button.Create("Button", base.transform, 1000);
		}

		public override void DestroyControl()
		{
			this.button.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		public override void ConfigureControl()
		{
			base.transform.position = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, this.lockAspectRatio);
			this.button.Update(true);
		}

		public override void DrawGizmos()
		{
			this.button.DrawGizmos(this.ButtonPosition, Color.yellow);
		}

		private void Update()
		{
			if (this.dirty)
			{
				this.ConfigureControl();
				this.dirty = false;
			}
			else
			{
				this.button.Update();
			}
		}

		public override void SubmitControlState(ulong updateTick, float deltaTime)
		{
			if (this.pressureSensitive)
			{
				float num = 0f;
				if (this.currentTouch == null)
				{
					if (this.allowSlideToggle)
					{
						int touchCount = TouchManager.TouchCount;
						for (int i = 0; i < touchCount; i++)
						{
							Touch touch = TouchManager.GetTouch(i);
							if (this.button.Contains(touch))
							{
								num = Utility.Max(num, touch.normalizedPressure);
							}
						}
					}
				}
				else
				{
					num = this.currentTouch.normalizedPressure;
				}
				this.ButtonState = (num > 0f);
				base.SubmitButtonValue(this.target, num, updateTick, deltaTime);
				return;
			}
			if (this.currentTouch == null && this.allowSlideToggle)
			{
				this.ButtonState = false;
				int touchCount2 = TouchManager.TouchCount;
				for (int j = 0; j < touchCount2; j++)
				{
					this.ButtonState = (this.ButtonState || this.button.Contains(TouchManager.GetTouch(j)));
				}
			}
			base.SubmitButtonState(this.target, this.ButtonState, updateTick, deltaTime);
		}

		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitButton(this.target);
		}

		public override void TouchBegan(Touch touch)
		{
			if (this.currentTouch != null)
			{
				return;
			}
			if (this.button.Contains(touch))
			{
				this.ButtonState = true;
				this.currentTouch = touch;
			}
		}

		public override void TouchMoved(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			if (this.toggleOnLeave && !this.button.Contains(touch))
			{
				this.ButtonState = false;
				this.currentTouch = null;
			}
		}

		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.ButtonState = false;
			this.currentTouch = null;
		}
	}
}
