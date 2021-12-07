// Decompile from assembly: Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ProgressionBarTallyObserver : MonoBehaviour
{
	[SerializeField]
	private ProgressionBar bar;

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private string prefix = string.Empty;

	[SerializeField]
	private bool displayDelta = true;

	private void OnEnable()
	{
		this.bar.onValueUpdate.AddListener(new UnityAction(this.UpdateText));
		this.UpdateText();
	}

	private void OnDisable()
	{
		this.bar.onValueUpdate.RemoveListener(new UnityAction(this.UpdateText));
	}

	private void UpdateText()
	{
		if (this.displayDelta)
		{
			ulong num = this.bar.CurrentValue - this.bar.MostRecentStartValue;
			if (num > 0uL)
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
}
