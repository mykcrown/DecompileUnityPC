using System;

// Token: 0x02000872 RID: 2162
[Serializable]
public class RollbackInput : IRollbackInput, IResetable
{
	// Token: 0x060035E8 RID: 13800 RVA: 0x000FEA34 File Offset: 0x000FCE34
	public RollbackInput()
	{
		this.values = new InputValuesSnapshot();
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x000FEA47 File Offset: 0x000FCE47
	public RollbackInput(RollbackInput other) : this()
	{
		this.CopyFrom(other);
	}

	// Token: 0x17000D18 RID: 3352
	// (get) Token: 0x060035EA RID: 13802 RVA: 0x000FEA56 File Offset: 0x000FCE56
	// (set) Token: 0x060035EB RID: 13803 RVA: 0x000FEA5E File Offset: 0x000FCE5E
	public InputValuesSnapshot values { get; set; }

	// Token: 0x17000D19 RID: 3353
	// (get) Token: 0x060035EC RID: 13804 RVA: 0x000FEA67 File Offset: 0x000FCE67
	// (set) Token: 0x060035ED RID: 13805 RVA: 0x000FEA6F File Offset: 0x000FCE6F
	public int frame { get; set; }

	// Token: 0x17000D1A RID: 3354
	// (get) Token: 0x060035EE RID: 13806 RVA: 0x000FEA78 File Offset: 0x000FCE78
	// (set) Token: 0x060035EF RID: 13807 RVA: 0x000FEA80 File Offset: 0x000FCE80
	public int playerID { get; set; }

	// Token: 0x060035F0 RID: 13808 RVA: 0x000FEA89 File Offset: 0x000FCE89
	public bool Equals(IRollbackInput other)
	{
		return this.values.Equals(other.values) && this.playerID == other.playerID;
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x000FEAB2 File Offset: 0x000FCEB2
	public void CopyFrom(RollbackInput other)
	{
		this.frame = other.frame;
		this.playerID = other.playerID;
		this.values.CopyFrom(other.values);
		this.usedPreviousFrame = other.usedPreviousFrame;
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x000FEAEC File Offset: 0x000FCEEC
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

	// Token: 0x060035F3 RID: 13811 RVA: 0x000FEB4E File Offset: 0x000FCF4E
	public void Reset()
	{
		this.usedPreviousFrame = false;
		this.frame = 0;
		this.playerID = 0;
		this.values.Clear();
	}

	// Token: 0x040024DF RID: 9439
	public bool usedPreviousFrame;
}
