﻿using System;
using UnityEngine;

namespace InterfaceMovement
{
	// Token: 0x02000043 RID: 67
	public class ButtonFocus : MonoBehaviour
	{
		// Token: 0x0600024C RID: 588 RVA: 0x000107DC File Offset: 0x0000EBDC
		private void Update()
		{
			Button focusedButton = base.transform.parent.GetComponent<ButtonManager>().focusedButton;
			base.transform.position = Vector3.MoveTowards(base.transform.position, focusedButton.transform.position, Time.deltaTime * 10f);
		}
	}
}
