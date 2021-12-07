// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine.EventSystems;

public class LoginLoadingErrorPanel : LoginScreenPanel
{
	public CursorTargetButton ConnectButton;

	[PostConstruct]
	public void Init()
	{
		this.ConnectButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onConnectClicked);
		base.listen(LoginScreenAPI.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	private void onUpdate()
	{
	}

	private void onConnectClicked(CursorTargetButton button, PointerEventData eventData)
	{
		this.onConnect();
	}

	private void onConnect()
	{
		base.api.RetryConnection();
	}

	public override void OnHide()
	{
		this.ConnectButton.Removed();
	}
}
