using System;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;
using GameAnalyticsSDK.Validators;
using GameAnalyticsSDK.Wrapper;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x0200002D RID: 45
	public static class GA_Setup
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0000C640 File Offset: 0x0000AA40
		public static void SetAvailableCustomDimensions01(List<string> customDimensions)
		{
			if (GAValidator.ValidateCustomDimensions(customDimensions.ToArray()))
			{
				string availableCustomDimensions = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions01(availableCustomDimensions);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000C66C File Offset: 0x0000AA6C
		public static void SetAvailableCustomDimensions02(List<string> customDimensions)
		{
			if (GAValidator.ValidateCustomDimensions(customDimensions.ToArray()))
			{
				string availableCustomDimensions = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions02(availableCustomDimensions);
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000C698 File Offset: 0x0000AA98
		public static void SetAvailableCustomDimensions03(List<string> customDimensions)
		{
			if (GAValidator.ValidateCustomDimensions(customDimensions.ToArray()))
			{
				string availableCustomDimensions = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions03(availableCustomDimensions);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000C6C4 File Offset: 0x0000AAC4
		public static void SetAvailableResourceCurrencies(List<string> resourceCurrencies)
		{
			if (GAValidator.ValidateResourceCurrencies(resourceCurrencies.ToArray()))
			{
				string availableResourceCurrencies = GA_MiniJSON.Serialize(resourceCurrencies);
				GA_Wrapper.SetAvailableResourceCurrencies(availableResourceCurrencies);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000C6F0 File Offset: 0x0000AAF0
		public static void SetAvailableResourceItemTypes(List<string> resourceItemTypes)
		{
			if (GAValidator.ValidateResourceItemTypes(resourceItemTypes.ToArray()))
			{
				string availableResourceItemTypes = GA_MiniJSON.Serialize(resourceItemTypes);
				GA_Wrapper.SetAvailableResourceItemTypes(availableResourceItemTypes);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000C71A File Offset: 0x0000AB1A
		public static void SetInfoLog(bool enabled)
		{
			GA_Wrapper.SetInfoLog(enabled);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000C722 File Offset: 0x0000AB22
		public static void SetVerboseLog(bool enabled)
		{
			GA_Wrapper.SetVerboseLog(enabled);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000C72A File Offset: 0x0000AB2A
		public static void SetFacebookId(string facebookId)
		{
			GA_Wrapper.SetFacebookId(facebookId);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000C734 File Offset: 0x0000AB34
		public static void SetGender(GAGender gender)
		{
			if (gender != GAGender.male)
			{
				if (gender == GAGender.female)
				{
					GA_Wrapper.SetGender(GAGender.female.ToString());
				}
			}
			else
			{
				GA_Wrapper.SetGender(GAGender.male.ToString());
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000C786 File Offset: 0x0000AB86
		public static void SetBirthYear(int birthYear)
		{
			GA_Wrapper.SetBirthYear(birthYear);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000C78E File Offset: 0x0000AB8E
		public static void SetCustomDimension01(string customDimension)
		{
			GA_Wrapper.SetCustomDimension01(customDimension);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000C796 File Offset: 0x0000AB96
		public static void SetCustomDimension02(string customDimension)
		{
			GA_Wrapper.SetCustomDimension02(customDimension);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000C79E File Offset: 0x0000AB9E
		public static void SetCustomDimension03(string customDimension)
		{
			GA_Wrapper.SetCustomDimension03(customDimension);
		}
	}
}
