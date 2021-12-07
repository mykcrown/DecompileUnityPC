using System;

// Token: 0x020006CB RID: 1739
public interface ILocalization
{
	// Token: 0x06002B9C RID: 11164
	void Clear();

	// Token: 0x06002B9D RID: 11165
	void AddLocalizationData(LocalizationData regionData);

	// Token: 0x06002B9E RID: 11166
	void SetRegion(LocalizationRegion region);

	// Token: 0x06002B9F RID: 11167
	string GetText(string key);

	// Token: 0x06002BA0 RID: 11168
	string GetText(string key, string[] replace);

	// Token: 0x06002BA1 RID: 11169
	string GetText(string key, string replace0);

	// Token: 0x06002BA2 RID: 11170
	string GetText(string key, string replace0, string replace1);

	// Token: 0x06002BA3 RID: 11171
	string GetText(string key, string replace0, string replace1, string replace2);

	// Token: 0x06002BA4 RID: 11172
	string GetHardPriceString(float price);

	// Token: 0x06002BA5 RID: 11173
	string GetSoftPriceString(int price);
}
