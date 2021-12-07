using System;
using TMPro;

// Token: 0x02000B71 RID: 2929
public interface IUITextHelper
{
	// Token: 0x060054B3 RID: 21683
	void TrackText(TextMeshProUGUI textField, Action sizeChangedCallback);

	// Token: 0x060054B4 RID: 21684
	void UntrackText(TextMeshProUGUI textField);

	// Token: 0x060054B5 RID: 21685
	void UpdateText(TextMeshProUGUI textField, string text);
}
