// Decompile from assembly: Assembly-CSharp.dll

using GameAnalyticsSDK.Utilities;
using GameAnalyticsSDK.Validators;
using GameAnalyticsSDK.Wrapper;
using System;
using System.Collections.Generic;

namespace GameAnalyticsSDK.Events
{
	public static class GA_Setup
	{
		public static void SetAvailableCustomDimensions01(List<string> customDimensions)
		{
			if (GAValidator.ValidateCustomDimensions(customDimensions.ToArray()))
			{
				string availableCustomDimensions = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions01(availableCustomDimensions);
			}
		}

		public static void SetAvailableCustomDimensions02(List<string> customDimensions)
		{
			if (GAValidator.ValidateCustomDimensions(customDimensions.ToArray()))
			{
				string availableCustomDimensions = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions02(availableCustomDimensions);
			}
		}

		public static void SetAvailableCustomDimensions03(List<string> customDimensions)
		{
			if (GAValidator.ValidateCustomDimensions(customDimensions.ToArray()))
			{
				string availableCustomDimensions = GA_MiniJSON.Serialize(customDimensions);
				GA_Wrapper.SetAvailableCustomDimensions03(availableCustomDimensions);
			}
		}

		public static void SetAvailableResourceCurrencies(List<string> resourceCurrencies)
		{
			if (GAValidator.ValidateResourceCurrencies(resourceCurrencies.ToArray()))
			{
				string availableResourceCurrencies = GA_MiniJSON.Serialize(resourceCurrencies);
				GA_Wrapper.SetAvailableResourceCurrencies(availableResourceCurrencies);
			}
		}

		public static void SetAvailableResourceItemTypes(List<string> resourceItemTypes)
		{
			if (GAValidator.ValidateResourceItemTypes(resourceItemTypes.ToArray()))
			{
				string availableResourceItemTypes = GA_MiniJSON.Serialize(resourceItemTypes);
				GA_Wrapper.SetAvailableResourceItemTypes(availableResourceItemTypes);
			}
		}

		public static void SetInfoLog(bool enabled)
		{
			GA_Wrapper.SetInfoLog(enabled);
		}

		public static void SetVerboseLog(bool enabled)
		{
			GA_Wrapper.SetVerboseLog(enabled);
		}

		public static void SetFacebookId(string facebookId)
		{
			GA_Wrapper.SetFacebookId(facebookId);
		}

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

		public static void SetBirthYear(int birthYear)
		{
			GA_Wrapper.SetBirthYear(birthYear);
		}

		public static void SetCustomDimension01(string customDimension)
		{
			GA_Wrapper.SetCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02(string customDimension)
		{
			GA_Wrapper.SetCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03(string customDimension)
		{
			GA_Wrapper.SetCustomDimension03(customDimension);
		}
	}
}
