// Decompile from assembly: Assembly-CSharp.dll

using System;

public struct AudioReference
{
	public IAudioOwner owner;

	public int sourceId;

	public AudioReference(IAudioOwner owner, int sourceId)
	{
		this.owner = owner;
		this.sourceId = sourceId;
	}
}
