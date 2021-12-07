using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020009BE RID: 2494
public class MousePlayerCursor : MonoBehaviour, IPlayerCursor
{
	// Token: 0x1700108F RID: 4239
	// (get) Token: 0x0600459E RID: 17822 RVA: 0x00130FBC File Offset: 0x0012F3BC
	public PlayerCursorActions Actions
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600459F RID: 17823 RVA: 0x00130FBF File Offset: 0x0012F3BF
	private void Awake()
	{
		this.theTransform = base.transform;
	}

	// Token: 0x060045A0 RID: 17824 RVA: 0x00130FCD File Offset: 0x0012F3CD
	public void Update()
	{
		this.lastPosition = this.theTransform.position;
		this.updatePosition();
	}

	// Token: 0x17001090 RID: 4240
	// (get) Token: 0x060045A1 RID: 17825 RVA: 0x00130FE6 File Offset: 0x0012F3E6
	public global::CursorMode CurrentMode
	{
		get
		{
			return global::CursorMode.Mouse;
		}
	}

	// Token: 0x060045A2 RID: 17826 RVA: 0x00130FE9 File Offset: 0x0012F3E9
	public void ResetPosition(Vector2 vect)
	{
		this.updatePosition();
	}

	// Token: 0x060045A3 RID: 17827 RVA: 0x00130FF1 File Offset: 0x0012F3F1
	public void SuppressKeyboard(bool suppress)
	{
	}

	// Token: 0x060045A4 RID: 17828 RVA: 0x00130FF4 File Offset: 0x0012F3F4
	private void updatePosition()
	{
		Vector2 v = Input.mousePosition;
		if (v.x > 0f && v.x < (float)Screen.width && v.y > 0f && v.y < (float)Screen.height)
		{
			this.theTransform.position = v;
		}
	}

	// Token: 0x17001091 RID: 4241
	// (get) Token: 0x060045A5 RID: 17829 RVA: 0x00131063 File Offset: 0x0012F463
	public Vector2 Position
	{
		get
		{
			return this.theTransform.position;
		}
	}

	// Token: 0x17001092 RID: 4242
	// (get) Token: 0x060045A6 RID: 17830 RVA: 0x00131075 File Offset: 0x0012F475
	public Vector3 PositionDelta
	{
		get
		{
			return this.theTransform.position - this.lastPosition;
		}
	}

	// Token: 0x17001093 RID: 4243
	// (get) Token: 0x060045A7 RID: 17831 RVA: 0x0013108D File Offset: 0x0012F48D
	public int PointerId
	{
		get
		{
			return -1;
		}
	}

	// Token: 0x17001094 RID: 4244
	// (get) Token: 0x060045A8 RID: 17832 RVA: 0x00131090 File Offset: 0x0012F490
	public bool SubmitPressed
	{
		get
		{
			return Input.GetMouseButtonDown(0);
		}
	}

	// Token: 0x17001095 RID: 4245
	// (get) Token: 0x060045A9 RID: 17833 RVA: 0x00131098 File Offset: 0x0012F498
	public bool SubmitHeld
	{
		get
		{
			return Input.GetMouseButton(0);
		}
	}

	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x060045AA RID: 17834 RVA: 0x001310A0 File Offset: 0x0012F4A0
	public bool SubmitReleased
	{
		get
		{
			return Input.GetMouseButtonUp(0);
		}
	}

	// Token: 0x17001097 RID: 4247
	// (get) Token: 0x060045AB RID: 17835 RVA: 0x001310A8 File Offset: 0x0012F4A8
	public bool CancelPressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17001098 RID: 4248
	// (get) Token: 0x060045AC RID: 17836 RVA: 0x001310AB File Offset: 0x0012F4AB
	public bool AltSubmitPressed
	{
		get
		{
			return Input.GetMouseButtonDown(1);
		}
	}

	// Token: 0x17001099 RID: 4249
	// (get) Token: 0x060045AD RID: 17837 RVA: 0x001310B3 File Offset: 0x0012F4B3
	public bool AltCancelPressed
	{
		get
		{
			return Input.GetMouseButtonDown(2);
		}
	}

	// Token: 0x1700109A RID: 4250
	// (get) Token: 0x060045AE RID: 17838 RVA: 0x001310BB File Offset: 0x0012F4BB
	public bool Advance1Pressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700109B RID: 4251
	// (get) Token: 0x060045AF RID: 17839 RVA: 0x001310BE File Offset: 0x0012F4BE
	public bool Previous1Pressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700109C RID: 4252
	// (get) Token: 0x060045B0 RID: 17840 RVA: 0x001310C1 File Offset: 0x0012F4C1
	public bool Advance2Pressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700109D RID: 4253
	// (get) Token: 0x060045B1 RID: 17841 RVA: 0x001310C4 File Offset: 0x0012F4C4
	public bool Previous2Pressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700109E RID: 4254
	// (get) Token: 0x060045B2 RID: 17842 RVA: 0x001310C7 File Offset: 0x0012F4C7
	public bool FaceButton3Pressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700109F RID: 4255
	// (get) Token: 0x060045B3 RID: 17843 RVA: 0x001310CA File Offset: 0x0012F4CA
	public bool RightStickUpPressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170010A0 RID: 4256
	// (get) Token: 0x060045B4 RID: 17844 RVA: 0x001310CD File Offset: 0x0012F4CD
	public bool RightStickDownPressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170010A1 RID: 4257
	// (get) Token: 0x060045B5 RID: 17845 RVA: 0x001310D0 File Offset: 0x0012F4D0
	public bool AdvanceSelectedPressed
	{
		get
		{
			return Input.GetAxis("Mouse ScrollWheel") > 0f;
		}
	}

	// Token: 0x170010A2 RID: 4258
	// (get) Token: 0x060045B6 RID: 17846 RVA: 0x001310E3 File Offset: 0x0012F4E3
	public bool PreviousSelectedPressed
	{
		get
		{
			return Input.GetAxis("Mouse ScrollWheel") < 0f;
		}
	}

	// Token: 0x170010A3 RID: 4259
	// (get) Token: 0x060045B7 RID: 17847 RVA: 0x001310F8 File Offset: 0x0012F4F8
	public bool AnythingPressed
	{
		get
		{
			return this.PositionDelta != Vector3.zero || this.SubmitPressed || this.CancelPressed || this.AltSubmitPressed || this.AltCancelPressed || this.Advance1Pressed || this.Previous1Pressed || this.Advance2Pressed || this.Previous2Pressed || this.FaceButton3Pressed || this.RightStickUpPressed || this.RightStickDownPressed || this.AdvanceSelectedPressed || this.PreviousSelectedPressed;
		}
	}

	// Token: 0x170010A4 RID: 4260
	// (get) Token: 0x060045B8 RID: 17848 RVA: 0x001311A7 File Offset: 0x0012F5A7
	// (set) Token: 0x060045B9 RID: 17849 RVA: 0x001311AF File Offset: 0x0012F5AF
	public bool IsHidden { get; set; }

	// Token: 0x170010A5 RID: 4261
	// (get) Token: 0x060045BA RID: 17850 RVA: 0x001311B8 File Offset: 0x0012F5B8
	// (set) Token: 0x060045BB RID: 17851 RVA: 0x001311C0 File Offset: 0x0012F5C0
	public bool IsPaused { get; set; }

	// Token: 0x170010A6 RID: 4262
	// (get) Token: 0x060045BC RID: 17852 RVA: 0x001311C9 File Offset: 0x0012F5C9
	public bool StartPressed
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170010A7 RID: 4263
	// (get) Token: 0x060045BD RID: 17853 RVA: 0x001311CC File Offset: 0x0012F5CC
	// (set) Token: 0x060045BE RID: 17854 RVA: 0x001311D4 File Offset: 0x0012F5D4
	public RaycastResult[] RaycastCache { get; set; }

	// Token: 0x170010A8 RID: 4264
	// (get) Token: 0x060045BF RID: 17855 RVA: 0x001311DD File Offset: 0x0012F5DD
	// (set) Token: 0x060045C0 RID: 17856 RVA: 0x001311E5 File Offset: 0x0012F5E5
	public GameObject LastSelectedObject { get; set; }

	// Token: 0x04002E31 RID: 11825
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x04002E32 RID: 11826
	private Transform theTransform;
}
