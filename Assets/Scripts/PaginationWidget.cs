// Decompile from assembly: Assembly-CSharp.dll

using System;

public class PaginationWidget : TrainingWidget
{
	public PaginatedContainer Container;

	public override void OnLeft()
	{
		this.Container.PreviousPage();
	}

	public override void OnRight()
	{
		this.Container.NextPage();
	}
}
