// Decompile from assembly: Assembly-CSharp.dll

using System;

[Serializable]
public class RollbackInput : IRollbackInput, IResetable
{
	public bool usedPreviousFrame;

	public InputValuesSnapshot values
	{
		get;
		set;
	}

	public int frame
	{
		get;
		set;
	}

	public int playerID
	{
		get;
		set;
	}

	public RollbackInput()
	{
		this.values = new InputValuesSnapshot();
	}

	public RollbackInput(RollbackInput other) : this()
	{
		this.CopyFrom(other);
	}

	public bool Equals(IRollbackInput other)
	{
		return this.values.Equals(other.values) && this.playerID == other.playerID;
	}

	public void CopyFrom(RollbackInput other)
	{
		this.frame = other.frame;
		this.playerID = other.playerID;
		this.values.CopyFrom(other.values);
		this.usedPreviousFrame = other.usedPreviousFrame;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"{Frame: ",
			this.frame,
			", playerID: ",
			this.playerID,
			", values: ",
			this.values.ToString(),
			"}"
		});
	}

	public void Reset()
	{
		this.usedPreviousFrame = false;
		this.frame = 0;
		this.playerID = 0;
		this.values.Clear();
	}
}
