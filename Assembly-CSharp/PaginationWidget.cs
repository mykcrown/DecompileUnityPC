using System;

// Token: 0x02000A3E RID: 2622
public class PaginationWidget : TrainingWidget
{
	// Token: 0x06004CC4 RID: 19652 RVA: 0x00145430 File Offset: 0x00143830
	public override void OnLeft()
	{
		this.Container.PreviousPage();
	}

	// Token: 0x06004CC5 RID: 19653 RVA: 0x0014543D File Offset: 0x0014383D
	public override void OnRight()
	{
		this.Container.NextPage();
	}

	// Token: 0x04003258 RID: 12888
	public PaginatedContainer Container;
}
