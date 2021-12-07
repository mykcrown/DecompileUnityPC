using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x0200005A RID: 90
	public class KeyBindingSourceListener : BindingSourceListener
	{
		// Token: 0x060002DD RID: 733 RVA: 0x0001406C File Offset: 0x0001246C
		public void Reset()
		{
			this.wasAlreadyPressed.Clear();
			KeyCombo keyCombo = KeyCombo.Detect(true);
			for (int i = 0; i < keyCombo.IncludeCount; i++)
			{
				this.wasAlreadyPressed.Add(keyCombo.GetInclude(i));
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000140B8 File Offset: 0x000124B8
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

		// Token: 0x060002DF RID: 735 RVA: 0x0001414C File Offset: 0x0001254C
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

		// Token: 0x04000277 RID: 631
		private List<Key> wasAlreadyPressed = new List<Key>();
	}
}
