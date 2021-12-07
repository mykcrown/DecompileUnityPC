// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

public class LobbyMemberDisplay : MonoBehaviour
{
	public TextMeshProUGUI Text;

	public GameObject LeaderSymbol;

	public PopinAnimationElement popinAnimation;

	public MenuItemButton arrowLeft;

	public MenuItemButton arrowRight;

	public CanvasGroup canvasGroup;

	public bool IsVisible
	{
		get;
		private set;
	}

	public void SetFade(bool faded)
	{
		this.canvasGroup.alpha = ((!faded) ? 1f : 0.5f);
	}

	public void Setup(string name, bool isLeader)
	{
		if (name != null)
		{
			this.Text.text = name;
			this.LeaderSymbol.SetActive(isLeader);
			this.popinAnimation.SetVisible(true);
			this.IsVisible = true;
		}
		else
		{
			this.popinAnimation.SetVisible(false);
			this.IsVisible = false;
		}
	}
}
