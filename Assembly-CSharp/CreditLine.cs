using System;
using TMPro;
using UnityEngine;

// Token: 0x0200090A RID: 2314
public class CreditLine : MonoBehaviour
{
	// Token: 0x06003C23 RID: 15395 RVA: 0x00116C49 File Offset: 0x00115049
	public void Set(string name, string title)
	{
		this.nameText.text = name;
		this.titleText.text = title;
	}

	// Token: 0x04002938 RID: 10552
	public TextMeshPro nameText;

	// Token: 0x04002939 RID: 10553
	public TextMeshPro titleText;
}
