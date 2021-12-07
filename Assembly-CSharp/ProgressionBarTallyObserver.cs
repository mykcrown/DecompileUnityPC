using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200094C RID: 2380
public class ProgressionBarTallyObserver : MonoBehaviour
{
	// Token: 0x06003F3D RID: 16189 RVA: 0x0011FB75 File Offset: 0x0011DF75
	private void OnEnable()
	{
		this.bar.onValueUpdate.AddListener(new UnityAction(this.UpdateText));
		this.UpdateText();
	}

	// Token: 0x06003F3E RID: 16190 RVA: 0x0011FB99 File Offset: 0x0011DF99
	private void OnDisable()
	{
		this.bar.onValueUpdate.RemoveListener(new UnityAction(this.UpdateText));
	}

	// Token: 0x06003F3F RID: 16191 RVA: 0x0011FBB8 File Offset: 0x0011DFB8
	private void UpdateText()
	{
		if (this.displayDelta)
		{
			ulong num = this.bar.CurrentValue - this.bar.MostRecentStartValue;
			if (num > 0UL)
			{
				this.text.text = this.prefix + num;
			}
			else
			{
				this.text.text = string.Empty;
			}
		}
		else
		{
			this.text.text = this.prefix + this.bar.CurrentValue;
		}
	}

	// Token: 0x04002ADE RID: 10974
	[SerializeField]
	private ProgressionBar bar;

	// Token: 0x04002ADF RID: 10975
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04002AE0 RID: 10976
	[SerializeField]
	private string prefix = string.Empty;

	// Token: 0x04002AE1 RID: 10977
	[SerializeField]
	private bool displayDelta = true;
}
