// Decompile from assembly: Assembly-CSharp.dll

using System;

public class CreateNewProfileContext
{
	public CreateNewProfileResult result = new CreateNewProfileResult();

	public Action<CreateNewProfileResult> callback;

	public CreateOptionsProfileInProgressTracker createProfileTracker;

	public void CompleteWithError(NameValidationResult error)
	{
		this.result.nameError = error;
		this.complete();
	}

	public void CompleteWithError(SaveOptionsProfileResult error)
	{
		this.result.saveResult = error;
		this.complete();
	}

	public void PreviousInProgress()
	{
		this.result.previousRequestInProgress = true;
		this.complete();
	}

	public void TooManyProfiles()
	{
		this.result.tooManyExist = true;
		this.complete();
	}

	public void Success()
	{
		this.complete();
	}

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
}
