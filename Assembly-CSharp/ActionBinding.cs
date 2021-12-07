using System;
using InControl;

// Token: 0x020009C0 RID: 2496
internal class ActionBinding
{
	// Token: 0x06004602 RID: 17922 RVA: 0x001322F7 File Offset: 0x001306F7
	public ActionBinding(PlayerAction action, Key key)
	{
		this.Action = action;
		this.Key = new KeyBindingSource(new Key[]
		{
			key
		});
	}

	// Token: 0x06004603 RID: 17923 RVA: 0x0013231B File Offset: 0x0013071B
	public ActionBinding(PlayerAction action, InputControlType button)
	{
		this.Action = action;
		this.Button = new DeviceBindingSource(button);
	}

	// Token: 0x04002E52 RID: 11858
	public PlayerAction Action;

	// Token: 0x04002E53 RID: 11859
	public KeyBindingSource Key;

	// Token: 0x04002E54 RID: 11860
	public DeviceBindingSource Button;
}
