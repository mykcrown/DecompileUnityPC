using System;
using TMPro;
using UnityEngine;

// Token: 0x02000991 RID: 2449
public class LoginLoadingPanel : LoginScreenPanel
{
	// Token: 0x0600429D RID: 17053 RVA: 0x00128739 File Offset: 0x00126B39
	[PostConstruct]
	public void Init()
	{
		base.listen(LoginScreenAPI.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	// Token: 0x0600429E RID: 17054 RVA: 0x00128758 File Offset: 0x00126B58
	private void onUpdate()
	{
	}

	// Token: 0x0600429F RID: 17055 RVA: 0x0012875A File Offset: 0x00126B5A
	public void UpdateState(bool isLoading)
	{
		this.Spinny.SetActive(isLoading);
	}

	// Token: 0x060042A0 RID: 17056 RVA: 0x00128768 File Offset: 0x00126B68
	public void UpdateText(string text)
	{
		this.StatusText.text = text;
	}

	// Token: 0x04002C7E RID: 11390
	public GameObject Spinny;

	// Token: 0x04002C7F RID: 11391
	public TextMeshProUGUI StatusText;
}
