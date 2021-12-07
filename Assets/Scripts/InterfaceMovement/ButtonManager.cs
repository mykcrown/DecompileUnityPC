// Decompile from assembly: Assembly-CSharp.dll

using InControl;
using System;
using UnityEngine;

namespace InterfaceMovement
{
	public class ButtonManager : MonoBehaviour
	{
		public Button focusedButton;

		private TwoAxisInputControl filteredDirection;

		private void Awake()
		{
			this.filteredDirection = new TwoAxisInputControl();
			this.filteredDirection.StateThreshold = 0.5f;
		}

		private void Update()
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			this.filteredDirection.Filter(activeDevice.Direction, Time.deltaTime);
			if (this.filteredDirection.Left.WasRepeated)
			{
				UnityEngine.Debug.Log("!!!");
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

		private void MoveFocusTo(Button newFocusedButton)
		{
			if (newFocusedButton != null)
			{
				this.focusedButton = newFocusedButton;
			}
		}
	}
}
