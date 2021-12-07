// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Runtime.Serialization;

[Serializable]
public class InputSettingsData : ICloneable
{
	public static int CURRENT_VERSION = 5;

	[NonSerialized]
	public PlayerInputActions inputActions = new PlayerInputActions();

	public bool tapToJumpEnabled = true;

	public bool tapToStrikeEnabled;

	public bool recoveryJumpingEnabled;

	public bool doubleTapToRun;

	public float leftStickDeadZone;

	public float rightStickDeadZone;

	public float leftTriggerDeadZone;

	public float rightTriggerDeadZone;

	public string bindings;

	public int version;

	public object Clone()
	{
		InputSettingsData inputSettingsData = (InputSettingsData)base.MemberwiseClone();
		inputSettingsData.inputActions = new PlayerInputActions();
		inputSettingsData.inputActions.Load(this.inputActions.Save());
		return inputSettingsData;
	}

	public override bool Equals(object obj)
	{
		InputSettingsData inputSettingsData = obj as InputSettingsData;
		return inputSettingsData != null && (this.inputActions.Save() != inputSettingsData.inputActions.Save() || this.recoveryJumpingEnabled != inputSettingsData.recoveryJumpingEnabled || this.tapToStrikeEnabled != inputSettingsData.tapToStrikeEnabled || this.tapToJumpEnabled != inputSettingsData.tapToJumpEnabled || this.doubleTapToRun != inputSettingsData.doubleTapToRun || this.leftStickDeadZone != inputSettingsData.leftStickDeadZone || this.rightStickDeadZone != inputSettingsData.rightStickDeadZone || this.leftTriggerDeadZone != inputSettingsData.leftTriggerDeadZone || this.rightTriggerDeadZone != inputSettingsData.rightTriggerDeadZone);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	[OnSerializing]
	internal void OnSerializingMethod(StreamingContext context)
	{
		this.bindings = this.inputActions.Save();
	}

	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		if (this.inputActions == null)
		{
			this.inputActions = new PlayerInputActions();
		}
		this.inputActions.Load(this.bindings);
	}
}
