using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Token: 0x02000945 RID: 2373
public class LocalizationText : MonoBehaviour
{
	// Token: 0x17000EDF RID: 3807
	// (get) Token: 0x06003EA6 RID: 16038 RVA: 0x0011D302 File Offset: 0x0011B702
	// (set) Token: 0x06003EA7 RID: 16039 RVA: 0x0011D30A File Offset: 0x0011B70A
	[Inject]
	public ILocalization localization { get; set; }

	// Token: 0x06003EA8 RID: 16040 RVA: 0x0011D313 File Offset: 0x0011B713
	private void Awake()
	{
		this.findInjectionContext();
		this.UpdateAll();
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x0011D324 File Offset: 0x0011B724
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

	// Token: 0x06003EAA RID: 16042 RVA: 0x0011D37F File Offset: 0x0011B77F
	public void UpdateReplacements()
	{
		base.gameObject.GetComponent<TextMeshProUGUI>().text = this.replaceValues(this.RawLocalizedText);
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x0011D3A0 File Offset: 0x0011B7A0
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

	// Token: 0x06003EAC RID: 16044 RVA: 0x0011D3EC File Offset: 0x0011B7EC
	private void findInjectionContext()
	{
		StaticInject.Inject(this);
	}

	// Token: 0x06003EAD RID: 16045 RVA: 0x0011D3F4 File Offset: 0x0011B7F4
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

	// Token: 0x04002A7C RID: 10876
	public string Key;

	// Token: 0x04002A7D RID: 10877
	[ReadOnly]
	public string EnglishText;

	// Token: 0x04002A7E RID: 10878
	[ReadOnly]
	public string RawLocalizedText;

	// Token: 0x04002A7F RID: 10879
	public List<string> replacements = new List<string>();
}
