// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;

public class FutureWatermarkDisplay : BaseGamewideOverlay
{
	public TextMeshProUGUI MainText;

	[PostConstruct]
	public void Init()
	{
		this.onUpdated();
	}

	private void onUpdated()
	{
		this.MainText.text = "FUTURE";
	}
}
