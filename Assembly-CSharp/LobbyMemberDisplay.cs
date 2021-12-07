using System;
using TMPro;
using UnityEngine;

// Token: 0x02000922 RID: 2338
public class LobbyMemberDisplay : MonoBehaviour
{
	// Token: 0x17000E9A RID: 3738
	// (get) Token: 0x06003D07 RID: 15623 RVA: 0x0011A648 File Offset: 0x00118A48
	// (set) Token: 0x06003D08 RID: 15624 RVA: 0x0011A650 File Offset: 0x00118A50
	public bool IsVisible { get; private set; }

	// Token: 0x06003D09 RID: 15625 RVA: 0x0011A659 File Offset: 0x00118A59
	public void SetFade(bool faded)
	{
		this.canvasGroup.alpha = ((!faded) ? 1f : 0.5f);
	}

	// Token: 0x06003D0A RID: 15626 RVA: 0x0011A67C File Offset: 0x00118A7C
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

	// Token: 0x040029A6 RID: 10662
	public TextMeshProUGUI Text;

	// Token: 0x040029A7 RID: 10663
	public GameObject LeaderSymbol;

	// Token: 0x040029A8 RID: 10664
	public PopinAnimationElement popinAnimation;

	// Token: 0x040029A9 RID: 10665
	public MenuItemButton arrowLeft;

	// Token: 0x040029AA RID: 10666
	public MenuItemButton arrowRight;

	// Token: 0x040029AB RID: 10667
	public CanvasGroup canvasGroup;
}
