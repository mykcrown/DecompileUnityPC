// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace InControl
{
	public class KeyBindingSourceListener : BindingSourceListener
	{
		private List<Key> wasAlreadyPressed = new List<Key>();

		public void Reset()
		{
			this.wasAlreadyPressed.Clear();
			KeyCombo keyCombo = KeyCombo.Detect(true);
			for (int i = 0; i < keyCombo.IncludeCount; i++)
			{
				this.wasAlreadyPressed.Add(keyCombo.GetInclude(i));
			}
		}

		public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
		{
			if (!listenOptions.IncludeKeys)
			{
				return null;
			}
			KeyCombo keyCombo = KeyCombo.Detect(true);
			if (keyCombo.IncludeCount > 0)
			{
				Key include = keyCombo.GetInclude(0);
				if (!this.wasAlreadyPressed.Contains(include))
				{
					return new KeyBindingSource(keyCombo);
				}
			}
			for (int i = this.wasAlreadyPressed.Count - 1; i >= 0; i--)
			{
				if (!this.hasKey(keyCombo, this.wasAlreadyPressed[i]))
				{
					this.wasAlreadyPressed.RemoveAt(i);
				}
			}
			return null;
		}

		private bool hasKey(KeyCombo combo, Key key)
		{
			for (int i = 0; i < combo.IncludeCount; i++)
			{
				if (combo.GetInclude(i) == key)
				{
					return true;
				}
			}
			return false;
		}
	}
}
