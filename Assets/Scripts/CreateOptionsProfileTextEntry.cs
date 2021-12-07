// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class CreateOptionsProfileTextEntry : WavedashTextEntry
{
	public GameObject RedBorder;

	public GameObject ErrorText;

	public bool IsErrorState
	{
		get;
		set;
	}

	public override void UpdateHighlightState()
	{
		this.ErrorText.SetActive(false);
		this.RedBorder.SetActive(false);
		this.HighlightGameObject.SetActive(false);
		if (this.IsErrorState)
		{
			this.ErrorText.gameObject.SetActive(true);
			this.RedBorder.SetActive(true);
		}
		else
		{
			base.UpdateHighlightState();
		}
	}
}
