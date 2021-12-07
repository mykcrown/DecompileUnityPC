using System;

// Token: 0x0200099F RID: 2463
public class NewAccountWindowAPI : INewAccountWindowAPI
{
	// Token: 0x17000FEE RID: 4078
	// (get) Token: 0x06004346 RID: 17222 RVA: 0x00129DCE File Offset: 0x001281CE
	// (set) Token: 0x06004347 RID: 17223 RVA: 0x00129DD6 File Offset: 0x001281D6
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17000FEF RID: 4079
	// (get) Token: 0x06004348 RID: 17224 RVA: 0x00129DDF File Offset: 0x001281DF
	// (set) Token: 0x06004349 RID: 17225 RVA: 0x00129DE7 File Offset: 0x001281E7
	public bool TermsAccepted
	{
		get
		{
			return this.termsAcceptedState;
		}
		set
		{
			if (this.termsAcceptedState != value)
			{
				this.termsAcceptedState = value;
				this.signalBus.Dispatch(NewAccountWindowAPI.UPDATED);
			}
		}
	}

	// Token: 0x04002CDA RID: 11482
	public static string UPDATED = "NewAccountWindowAPI.UPDATED";

	// Token: 0x04002CDC RID: 11484
	private bool termsAcceptedState;
}
