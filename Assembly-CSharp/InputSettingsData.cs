using System;
using System.Runtime.Serialization;

// Token: 0x02000615 RID: 1557
[Serializable]
public class InputSettingsData : ICloneable
{
	// Token: 0x06002664 RID: 9828 RVA: 0x000BCBF8 File Offset: 0x000BAFF8
	public object Clone()
	{
		InputSettingsData inputSettingsData = (InputSettingsData)base.MemberwiseClone();
		inputSettingsData.inputActions = new PlayerInputActions();
		inputSettingsData.inputActions.Load(this.inputActions.Save());
		return inputSettingsData;
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x000BCC34 File Offset: 0x000BB034
	public override bool Equals(object obj)
	{
		InputSettingsData inputSettingsData = obj as InputSettingsData;
		return inputSettingsData != null && (this.inputActions.Save() != inputSettingsData.inputActions.Save() || this.recoveryJumpingEnabled != inputSettingsData.recoveryJumpingEnabled || this.tapToStrikeEnabled != inputSettingsData.tapToStrikeEnabled || this.tapToJumpEnabled != inputSettingsData.tapToJumpEnabled || this.doubleTapToRun != inputSettingsData.doubleTapToRun || this.leftStickDeadZone != inputSettingsData.leftStickDeadZone || this.rightStickDeadZone != inputSettingsData.rightStickDeadZone || this.leftTriggerDeadZone != inputSettingsData.leftTriggerDeadZone || this.rightTriggerDeadZone != inputSettingsData.rightTriggerDeadZone);
	}

	// Token: 0x06002666 RID: 9830 RVA: 0x000BCCFB File Offset: 0x000BB0FB
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06002667 RID: 9831 RVA: 0x000BCD03 File Offset: 0x000BB103
	[OnSerializing]
	internal void OnSerializingMethod(StreamingContext context)
	{
		this.bindings = this.inputActions.Save();
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x000BCD16 File Offset: 0x000BB116
	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		if (this.inputActions == null)
		{
			this.inputActions = new PlayerInputActions();
		}
		this.inputActions.Load(this.bindings);
	}

	// Token: 0x04001C15 RID: 7189
	public static int CURRENT_VERSION = 5;

	// Token: 0x04001C16 RID: 7190
	[NonSerialized]
	public PlayerInputActions inputActions = new PlayerInputActions();

	// Token: 0x04001C17 RID: 7191
	public bool tapToJumpEnabled = true;

	// Token: 0x04001C18 RID: 7192
	public bool tapToStrikeEnabled;

	// Token: 0x04001C19 RID: 7193
	public bool recoveryJumpingEnabled;

	// Token: 0x04001C1A RID: 7194
	public bool doubleTapToRun;

	// Token: 0x04001C1B RID: 7195
	public float leftStickDeadZone;

	// Token: 0x04001C1C RID: 7196
	public float rightStickDeadZone;

	// Token: 0x04001C1D RID: 7197
	public float leftTriggerDeadZone;

	// Token: 0x04001C1E RID: 7198
	public float rightTriggerDeadZone;

	// Token: 0x04001C1F RID: 7199
	public string bindings;

	// Token: 0x04001C20 RID: 7200
	public int version;
}
