// Decompile from assembly: Assembly-CSharp.dll

using System;

public class NewAccountWindowAPI : INewAccountWindowAPI
{
	public static string UPDATED = "NewAccountWindowAPI.UPDATED";

	private bool termsAcceptedState;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

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
}
