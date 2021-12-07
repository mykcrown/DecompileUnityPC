using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using strange.extensions.signal.impl;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
public class Localization : ILocalization
{
	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000E3539 File Offset: 0x000E1939
	// (set) Token: 0x06002B8C RID: 11148 RVA: 0x000E3541 File Offset: 0x000E1941
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06002B8D RID: 11149 RVA: 0x000E354A File Offset: 0x000E194A
	public void Clear()
	{
		this.textMap.Clear();
		this.localizationDataList.Clear();
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x000E3564 File Offset: 0x000E1964
	public void SetRegion(LocalizationRegion region)
	{
		bool flag = this.region != region;
		this.region = region;
		this.textMap.Clear();
		foreach (LocalizationData localizationData in this.localizationDataList)
		{
			foreach (TextAsset textAsset in localizationData.GetAssets(region))
			{
				if (textAsset != null)
				{
					this.loadDocument(textAsset);
				}
			}
		}
		if (flag)
		{
			this.signalBus.GetSignal<Localization.LocalizationRegionChangedSignal>().Dispatch(region);
		}
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x000E3648 File Offset: 0x000E1A48
	public void Load()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("Localization/en-US/ui");
		this.loadDocument(textAsset);
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x000E366C File Offset: 0x000E1A6C
	public void AddLocalizationData(LocalizationData data)
	{
		if (data == null)
		{
			Debug.LogError("Attempted to add null localization region data");
			return;
		}
		this.localizationDataList.Add(data);
		foreach (TextAsset textAsset in data.GetAssets(this.region))
		{
			if (textAsset != null)
			{
				this.loadDocument(textAsset);
			}
		}
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x000E36F8 File Offset: 0x000E1AF8
	private void loadDocument(TextAsset textAsset)
	{
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(textAsset.text);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("resources");
			IEnumerator enumerator = xmlNodeList.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					XmlNode xmlNode = (XmlNode)obj;
					XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("string");
					IEnumerator enumerator2 = xmlNodeList2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode xmlNode2 = (XmlNode)obj2;
							string innerText = xmlNode2.Attributes["name"].InnerText;
							string innerText2 = xmlNode2.InnerText;
							if (this.textMap.ContainsKey(innerText))
							{
								DataAlert.Fatal("Duplicate localization key " + innerText + " detected from file " + textAsset.name);
							}
							this.textMap[innerText] = innerText2;
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Unhandled error in loading " + textAsset.name + ":\n" + ex.ToString());
		}
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x000E388C File Offset: 0x000E1C8C
	public string GetTextFormat(string key, params string[] args)
	{
		return string.Format(this.GetText(key), args);
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x000E389C File Offset: 0x000E1C9C
	public string GetText(string key, string[] replace)
	{
		string text = this.GetText(key);
		return this.valueReplace(text, replace);
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x000E38BC File Offset: 0x000E1CBC
	public string GetText(string key)
	{
		string result = null;
		if (key == null)
		{
			Debug.LogError("Null key request");
			return null;
		}
		this.textMap.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x000E38F0 File Offset: 0x000E1CF0
	public string GetText(string key, string replace0)
	{
		this.scratchArray1[0] = replace0;
		string text = this.GetText(key);
		return this.valueReplace(text, this.scratchArray1);
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x000E3920 File Offset: 0x000E1D20
	public string GetText(string key, string replace0, string replace1)
	{
		this.scratchArray2[0] = replace0;
		this.scratchArray2[1] = replace1;
		string text = this.GetText(key);
		return this.valueReplace(text, this.scratchArray2);
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x000E3958 File Offset: 0x000E1D58
	public string GetText(string key, string replace0, string replace1, string replace2)
	{
		this.scratchArray3[0] = replace0;
		this.scratchArray3[1] = replace1;
		this.scratchArray3[2] = replace2;
		string text = this.GetText(key);
		return this.valueReplace(text, this.scratchArray3);
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000E3998 File Offset: 0x000E1D98
	private string valueReplace(string text, string[] replace)
	{
		if (text != null)
		{
			int num = 0;
			int num2 = text.IndexOf("#*");
			while (num2 != -1 && num < replace.Length)
			{
				string text2 = text.Substring(0, num2) + replace[num] + text.Substring(num2 + 2, text.Length - (num2 + 2));
				text = text2;
				num++;
				if (num >= replace.Length)
				{
					break;
				}
				num2 = text.IndexOf("#*");
			}
		}
		return text;
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000E3A14 File Offset: 0x000E1E14
	public string GetHardPriceString(float price)
	{
		if (this.region == LocalizationRegion.en_US)
		{
			return string.Format("${0:0.00}", price / 100f);
		}
		Debug.LogError("No hard price format implemented for region");
		return "????";
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000E3A47 File Offset: 0x000E1E47
	public string GetSoftPriceString(int price)
	{
		if (this.region == LocalizationRegion.en_US)
		{
			return string.Format("{0:n0}", price);
		}
		Debug.LogError("No soft price format implemented for region");
		return "????";
	}

	// Token: 0x04001F19 RID: 7961
	private LocalizationRegion region;

	// Token: 0x04001F1A RID: 7962
	private Dictionary<string, string> textMap = new Dictionary<string, string>();

	// Token: 0x04001F1B RID: 7963
	private List<LocalizationData> localizationDataList = new List<LocalizationData>();

	// Token: 0x04001F1C RID: 7964
	private string[] scratchArray1 = new string[1];

	// Token: 0x04001F1D RID: 7965
	private string[] scratchArray2 = new string[2];

	// Token: 0x04001F1E RID: 7966
	private string[] scratchArray3 = new string[3];

	// Token: 0x020006CA RID: 1738
	public class LocalizationRegionChangedSignal : Signal<LocalizationRegion>
	{
	}
}
