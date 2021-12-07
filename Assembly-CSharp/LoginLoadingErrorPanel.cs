using System;
using UnityEngine.EventSystems;

// Token: 0x02000990 RID: 2448
public class LoginLoadingErrorPanel : LoginScreenPanel
{
	// Token: 0x06004297 RID: 17047 RVA: 0x001286D7 File Offset: 0x00126AD7
	[PostConstruct]
	public void Init()
	{
		this.ConnectButton.SubmitCallback = new Action<CursorTargetButton, PointerEventData>(this.onConnectClicked);
		base.listen(LoginScreenAPI.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x06004298 RID: 17048 RVA: 0x0012870D File Offset: 0x00126B0D
	private void onUpdate()
	{
	}

	// Token: 0x06004299 RID: 17049 RVA: 0x0012870F File Offset: 0x00126B0F
	private void onConnectClicked(CursorTargetButton button, PointerEventData eventData)
	{
		this.onConnect();
	}

	// Token: 0x0600429A RID: 17050 RVA: 0x00128717 File Offset: 0x00126B17
	private void onConnect()
	{
		base.api.RetryConnection();
	}

	// Token: 0x0600429B RID: 17051 RVA: 0x00128724 File Offset: 0x00126B24
	public override void OnHide()
	{
		this.ConnectButton.Removed();
	}

	// Token: 0x04002C7D RID: 11389
	public CursorTargetButton ConnectButton;
}
