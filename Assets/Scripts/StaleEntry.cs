// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class StaleEntry : CloneableObject
{
	public MoveLabel label
	{
		get;
		private set;
	}

	public string name
	{
		get;
		private set;
	}

	public int uid
	{
		get;
		private set;
	}

	public StaleEntry()
	{
	}

	public StaleEntry(MoveLabel label, string name, int uid)
	{
		this.label = label;
		this.name = name;
		this.uid = uid;
	}
}
