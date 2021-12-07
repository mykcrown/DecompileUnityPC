using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000123 RID: 291
	public class TouchButtonControl : TouchControl
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x0002BE25 File Offset: 0x0002A225
		public override void CreateControl()
		{
			this.button.Create("Button", base.transform, 1000);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0002BE42 File Offset: 0x0002A242
		public override void DestroyControl()
		{
			this.button.Delete();
			if (this.currentTouch != null)
			{
				this.TouchEnded(this.currentTouch);
				this.currentTouch = null;
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0002BE6D File Offset: 0x0002A26D
		public override void ConfigureControl()
		{
			base.transform.position = base.OffsetToWorldPosition(this.anchor, this.offset, this.offsetUnitType, this.lockAspectRatio);
			this.button.Update(true);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0002BEA4 File Offset: 0x0002A2A4
		public override void DrawGizmos()
		{
			this.button.DrawGizmos(this.ButtonPosition, Color.yellow);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0002BEBC File Offset: 0x0002A2BC
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

		// Token: 0x06000649 RID: 1609 RVA: 0x0002BEE8 File Offset: 0x0002A2E8
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

		// Token: 0x0600064A RID: 1610 RVA: 0x0002BFFD File Offset: 0x0002A3FD
		public override void CommitControlState(ulong updateTick, float deltaTime)
		{
			base.CommitButton(this.target);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0002C00B File Offset: 0x0002A40B
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

		// Token: 0x0600064C RID: 1612 RVA: 0x0002C038 File Offset: 0x0002A438
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

		// Token: 0x0600064D RID: 1613 RVA: 0x0002C071 File Offset: 0x0002A471
		public override void TouchEnded(Touch touch)
		{
			if (this.currentTouch != touch)
			{
				return;
			}
			this.ButtonState = false;
			this.currentTouch = null;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0002C08E File Offset: 0x0002A48E
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0002C096 File Offset: 0x0002A496
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

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0002C0B7 File Offset: 0x0002A4B7
		// (set) Token: 0x06000651 RID: 1617 RVA: 0x0002C0E4 File Offset: 0x0002A4E4
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

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x0002C102 File Offset: 0x0002A502
		// (set) Token: 0x06000653 RID: 1619 RVA: 0x0002C10A File Offset: 0x0002A50A
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

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0002C126 File Offset: 0x0002A526
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x0002C12E File Offset: 0x0002A52E
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0002C14F File Offset: 0x0002A54F
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x0002C157 File Offset: 0x0002A557
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

		// Token: 0x0400049D RID: 1181
		[Header("Position")]
		[SerializeField]
		private TouchControlAnchor anchor = TouchControlAnchor.BottomRight;

		// Token: 0x0400049E RID: 1182
		[SerializeField]
		private TouchUnitType offsetUnitType;

		// Token: 0x0400049F RID: 1183
		[SerializeField]
		private Vector2 offset = new Vector2(-10f, 10f);

		// Token: 0x040004A0 RID: 1184
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x040004A1 RID: 1185
		[Header("Options")]
		public TouchControl.ButtonTarget target = TouchControl.ButtonTarget.Action1;

		// Token: 0x040004A2 RID: 1186
		public bool allowSlideToggle = true;

		// Token: 0x040004A3 RID: 1187
		public bool toggleOnLeave;

		// Token: 0x040004A4 RID: 1188
		public bool pressureSensitive;

		// Token: 0x040004A5 RID: 1189
		[Header("Sprites")]
		public TouchSprite button = new TouchSprite(15f);

		// Token: 0x040004A6 RID: 1190
		private bool buttonState;

		// Token: 0x040004A7 RID: 1191
		private Touch currentTouch;

		// Token: 0x040004A8 RID: 1192
		private bool dirty;
	}
}
