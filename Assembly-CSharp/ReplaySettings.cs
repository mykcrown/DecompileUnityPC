using System;

// Token: 0x020003CD RID: 973
[Serializable]
public class ReplaySettings
{
	// Token: 0x04000E59 RID: 3673
	public bool replayButtonEnabled;

	// Token: 0x04000E5A RID: 3674
	public string replayName;

	// Token: 0x04000E5B RID: 3675
	public bool recordStates;

	// Token: 0x04000E5C RID: 3676
	public bool readFromFile;

	// Token: 0x04000E5D RID: 3677
	public ReplayType replayType;

	// Token: 0x04000E5E RID: 3678
	public bool flattenRollbackInput;

	// Token: 0x04000E5F RID: 3679
	public Serialization.SerializeType serializeType = Serialization.SerializeType.XML;

	// Token: 0x04000E60 RID: 3680
	public bool testReplayEquality;

	// Token: 0x04000E61 RID: 3681
	public string testReplayFilePath;

	// Token: 0x04000E62 RID: 3682
	public bool enableRuntimeReplayValidation;

	// Token: 0x04000E63 RID: 3683
	public bool autoValidateOverwrites;

	// Token: 0x04000E64 RID: 3684
	public int maxReplayLength;
}
