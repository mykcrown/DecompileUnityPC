// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
	public string Key;

	[ReadOnly]
	public string EnglishText;

	[ReadOnly]
	public string RawLocalizedText;

	public List<string> replacements = new List<string>();

	[Inject]
	public ILocalization localization
	{
		get;
		set;
	}

	private void Awake()
	{
		this.findInjectionContext();
		this.UpdateAll();
	}

	public void SetReplacement(int index, string value, bool immediate = true)
	{
		if (this.replacements.Count <= index)
		{
			this.replacements.AddRange(Enumerable.Repeat<string>(string.Empty, index + 1 - this.replacements.Count));
		}
		this.replacements[index] = value;
		if (immediate)
		{
			this.UpdateReplacements();
		}
	}

	public void UpdateReplacements()
	{
		base.gameObject.GetComponent<TextMeshProUGUI>().text = this.replaceValues(this.RawLocalizedText);
	}

	public void UpdateAll()
	{
		if (this.localization != null)
		{
			this.RawLocalizedText = this.localization.GetText(this.Key);
			if (this.RawLocalizedText == null)
			{
				this.RawLocalizedText = this.Key;
			}
			this.UpdateReplacements();
		}
	}

	private void findInjectionContext()
	{
		StaticInject.Inject(this);
	}

	private string replaceValues(string input)
	{
		if (input == null)
		{
			return null;
		}
		try
		{
			if (this.replacements != null && this.replacements.Count > 0)
			{
				input = string.Format(input, this.replacements.ToArray());
			}
		}
		catch
		{
		}
		return input;
	}
}
