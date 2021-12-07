// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;

public interface IUITextHelper
{
	void TrackText(TextMeshProUGUI textField, Action sizeChangedCallback);

	void UntrackText(TextMeshProUGUI textField);

	void UpdateText(TextMeshProUGUI textField, string text);
}
