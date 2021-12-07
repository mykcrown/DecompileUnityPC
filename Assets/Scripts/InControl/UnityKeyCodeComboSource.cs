// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace InControl
{
	public class UnityKeyCodeComboSource : InputControlSource
	{
		public KeyCode[] KeyCodeList;

		public UnityKeyCodeComboSource()
		{
		}

		public UnityKeyCodeComboSource(params KeyCode[] keyCodeList)
		{
			this.KeyCodeList = keyCodeList;
		}

		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		public bool GetState(InputDevice inputDevice)
		{
			for (int i = 0; i < this.KeyCodeList.Length; i++)
			{
				if (!Input.GetKey(this.KeyCodeList[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
