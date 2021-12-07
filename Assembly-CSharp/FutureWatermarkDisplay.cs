using System;
using TMPro;

// Token: 0x02000A7D RID: 2685
public class FutureWatermarkDisplay : BaseGamewideOverlay
{
	// Token: 0x06004E77 RID: 20087 RVA: 0x00149A49 File Offset: 0x00147E49
	[PostConstruct]
	public void Init()
	{
		this.onUpdated();
	}

	// Token: 0x06004E78 RID: 20088 RVA: 0x00149A51 File Offset: 0x00147E51
	private void onUpdated()
	{
		this.MainText.text = "FUTURE";
	}

	// Token: 0x04003335 RID: 13109
	public TextMeshProUGUI MainText;
}
