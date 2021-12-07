// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ILocalization
{
	void Clear();

	void AddLocalizationData(LocalizationData regionData);

	void SetRegion(LocalizationRegion region);

	string GetText(string key);

	string GetText(string key, string[] replace);

	string GetText(string key, string replace0);

	string GetText(string key, string replace0, string replace1);

	string GetText(string key, string replace0, string replace1, string replace2);

	string GetHardPriceString(float price);

	string GetSoftPriceString(int price);
}
