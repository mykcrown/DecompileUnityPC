// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class ReplaySettings
{
	public bool replayButtonEnabled;

	public string replayName;

	public bool recordStates;

	public bool readFromFile;

	public ReplayType replayType;

	public bool flattenRollbackInput;

	public Serialization.SerializeType serializeType = Serialization.SerializeType.XML;

	public bool testReplayEquality;

	public string testReplayFilePath;

	public bool enableRuntimeReplayValidation;

	public bool autoValidateOverwrites;

	public int maxReplayLength;
}
