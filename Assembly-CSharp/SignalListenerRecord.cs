using System;

// Token: 0x02000392 RID: 914
internal class SignalListenerRecord
{
	// Token: 0x060013A4 RID: 5028 RVA: 0x00070203 File Offset: 0x0006E603
	public SignalListenerRecord(string name, Action theFunction)
	{
		this.name = name;
		this.theFunction = theFunction;
	}

	// Token: 0x04000CFD RID: 3325
	public string name;

	// Token: 0x04000CFE RID: 3326
	public Action theFunction;
}
