using System;
using InControl;
using UnityEngine;

namespace InterfaceMovement
{
	// Token: 0x02000044 RID: 68
	public class ButtonManager : MonoBehaviour
	{
		// Token: 0x0600024E RID: 590 RVA: 0x00010838 File Offset: 0x0000EC38
		private void Awake()
		{
			this.filteredDirection = new TwoAxisInputControl();
			this.filteredDirection.StateThreshold = 0.5f;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00010858 File Offset: 0x0000EC58
		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			this.filteredDirection.Filter(activeDevice.Direction, Time.deltaTime);
			if (this.filteredDirection.Left.WasRepeated)
			{
				Debug.Log("!!!");
			}
			if (this.filteredDirection.Up.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.up);
			}
			if (this.filteredDirection.Down.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.down);
			}
			if (this.filteredDirection.Left.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.left);
			}
			if (this.filteredDirection.Right.WasPressed)
			{
				this.MoveFocusTo(this.focusedButton.right);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00010938 File Offset: 0x0000ED38
		private void MoveFocusTo(Button newFocusedButton)
		{
			if (newFocusedButton != null)
			{
				this.focusedButton = newFocusedButton;
			}
		}

		// Token: 0x040001BE RID: 446
		public Button focusedButton;

		// Token: 0x040001BF RID: 447
		private TwoAxisInputControl filteredDirection;
	}
}
