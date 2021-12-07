// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.signal.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Localization : ILocalization
{
	public class LocalizationRegionChangedSignal : Signal<LocalizationRegion>
	{
	}

	private LocalizationRegion region;

	private Dictionary<string, string> textMap = new Dictionary<string, string>();

	private List<LocalizationData> localizationDataList = new List<LocalizationData>();

	private string[] scratchArray1 = new string[1];

	private string[] scratchArray2 = new string[2];

	private string[] scratchArray3 = new string[3];

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	public void Clear()
	{
		this.textMap.Clear();
		this.localizationDataList.Clear();
	}

	public void SetRegion(LocalizationRegion region)
	{
		bool flag = this.region != region;
		this.region = region;
		this.textMap.Clear();
		foreach (LocalizationData current in this.localizationDataList)
		{
			foreach (TextAsset current2 in current.GetAssets(region))
			{
				if (current2 != null)
				{
					this.loadDocument(current2);
				}
			}
		}
		if (flag)
		{
			this.signalBus.GetSignal<Localization.LocalizationRegionChangedSignal>().Dispatch(region);
		}
	}

	public void Load()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("Localization/en-US/ui");
		this.loadDocument(textAsset);
	}

	public void AddLocalizationData(LocalizationData data)
	{
		if (data == null)
		{
			UnityEngine.Debug.LogError("Attempted to add null localization region data");
			return;
		}
		this.localizationDataList.Add(data);
		foreach (TextAsset current in data.GetAssets(this.region))
		{
			if (current != null)
			{
				this.loadDocument(current);
			}
		}
	}

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
					XmlNode xmlNode = (XmlNode)enumerator.Current;
					XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("string");
					IEnumerator enumerator2 = xmlNodeList2.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							XmlNode xmlNode2 = (XmlNode)enumerator2.Current;
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
			UnityEngine.Debug.LogError("Unhandled error in loading " + textAsset.name + ":\n" + ex.ToString());
		}
	}

	public string GetTextFormat(string key, params string[] args)
	{
		return string.Format(this.GetText(key), args);
	}

	public string GetText(string key, string[] replace)
	{
		string text = this.GetText(key);
		return this.valueReplace(text, replace);
	}

	public string GetText(string key)
	{
		string result = null;
		if (key == null)
		{
			UnityEngine.Debug.LogError("Null key request");
			return null;
		}
		this.textMap.TryGetValue(key, out result);
		return result;
	}

	public string GetText(string key, string replace0)
	{
		this.scratchArray1[0] = replace0;
		string text = this.GetText(key);
		return this.valueReplace(text, this.scratchArray1);
	}

	public string GetText(string key, string replace0, string replace1)
	{
		this.scratchArray2[0] = replace0;
		this.scratchArray2[1] = replace1;
		string text = this.GetText(key);
		return this.valueReplace(text, this.scratchArray2);
	}

	public string GetText(string key, string replace0, string replace1, string replace2)
	{
		this.scratchArray3[0] = replace0;
		this.scratchArray3[1] = replace1;
		this.scratchArray3[2] = replace2;
		string text = this.GetText(key);
		return this.valueReplace(text, this.scratchArray3);
	}

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

	public string GetHardPriceString(float price)
	{
		if (this.region == LocalizationRegion.en_US)
		{
			return string.Format("${0:0.00}", price / 100f);
		}
		UnityEngine.Debug.LogError("No hard price format implemented for region");
		return "????";
	}

	public string GetSoftPriceString(int price)
	{
		if (this.region == LocalizationRegion.en_US)
		{
			return string.Format("{0:n0}", price);
		}
		UnityEngine.Debug.LogError("No soft price format implemented for region");
		return "????";
	}
}
