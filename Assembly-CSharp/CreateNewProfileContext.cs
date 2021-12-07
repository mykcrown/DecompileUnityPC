using System;

// Token: 0x0200053C RID: 1340
public class CreateNewProfileContext
{
	// Token: 0x06001D4B RID: 7499 RVA: 0x00096058 File Offset: 0x00094458
	public void CompleteWithError(NameValidationResult error)
	{
		this.result.nameError = error;
		this.complete();
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x0009606C File Offset: 0x0009446C
	public void CompleteWithError(SaveOptionsProfileResult error)
	{
		this.result.saveResult = error;
		this.complete();
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x00096080 File Offset: 0x00094480
	public void PreviousInProgress()
	{
		this.result.previousRequestInProgress = true;
		this.complete();
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x00096094 File Offset: 0x00094494
	public void TooManyProfiles()
	{
		this.result.tooManyExist = true;
		this.complete();
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000960A8 File Offset: 0x000944A8
	public void Success()
	{
		this.complete();
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x000960B0 File Offset: 0x000944B0
	private void complete()
	{
		if (!this.result.previousRequestInProgress)
		{
			this.createProfileTracker.InProgress = false;
		}
		Action<CreateNewProfileResult> action = this.callback;
		this.callback = null;
		if (action != null)
		{
			action(this.result);
		}
	}

	// Token: 0x040017E2 RID: 6114
	public CreateNewProfileResult result = new CreateNewProfileResult();

	// Token: 0x040017E3 RID: 6115
	public Action<CreateNewProfileResult> callback;

	// Token: 0x040017E4 RID: 6116
	public CreateOptionsProfileInProgressTracker createProfileTracker;
}
