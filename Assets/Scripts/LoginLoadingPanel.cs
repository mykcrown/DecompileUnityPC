// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class LoginLoadingPanel : LoginScreenPanel
{
	public GameObject Spinny;

	public TextMeshProUGUI StatusText;

	[PostConstruct]
	public void Init()
	{
		base.listen(LoginScreenAPI.UPDATED, new Action(this.onUpdate));
		this.onUpdate();
	}

	private void onUpdate()
	{
	}

	public void UpdateState(bool isLoading)
	{
		this.Spinny.SetActive(isLoading);
	}

	public void UpdateText(string text)
	{
		this.StatusText.text = text;
	}
}
