using System;
using System.Collections;
using UnityEngine;

namespace GameAnalyticsSDK.Events
{
	// Token: 0x0200002E RID: 46
	public class GA_SpecialEvents : MonoBehaviour
	{
		// Token: 0x06000163 RID: 355 RVA: 0x0000C7AE File Offset: 0x0000ABAE
		public void Start()
		{
			base.StartCoroutine(this.SubmitFPSRoutine());
			base.StartCoroutine(this.CheckCriticalFPSRoutine());
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000C7CC File Offset: 0x0000ABCC
		private IEnumerator SubmitFPSRoutine()
		{
			while (Application.isPlaying && GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsAverage)
			{
				yield return new WaitForSeconds(30f);
				GA_SpecialEvents.SubmitFPS();
			}
			yield break;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000C7E0 File Offset: 0x0000ABE0
		private IEnumerator CheckCriticalFPSRoutine()
		{
			while (Application.isPlaying && GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsCritical)
			{
				yield return new WaitForSeconds((float)GameAnalytics.SettingsGA.FpsCirticalSubmitInterval);
				this.CheckCriticalFPS();
			}
			yield break;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000C7FC File Offset: 0x0000ABFC
		public void Update()
		{
			if (GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsAverage)
			{
				GA_SpecialEvents._frameCountAvg++;
			}
			if (GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsCritical)
			{
				this._frameCountCrit++;
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000C864 File Offset: 0x0000AC64
		public static void SubmitFPS()
		{
			if (GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsAverage)
			{
				float num = Time.unscaledTime - GA_SpecialEvents._lastUpdateAvg;
				if (num > 1f)
				{
					float num2 = (float)GA_SpecialEvents._frameCountAvg / num;
					GA_SpecialEvents._lastUpdateAvg = Time.unscaledTime;
					GA_SpecialEvents._frameCountAvg = 0;
					if (num2 > 0f)
					{
						GameAnalytics.NewDesignEvent("GA:AverageFPS", (float)((int)num2));
					}
				}
			}
			if (GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsCritical && GA_SpecialEvents._criticalFpsCount > 0)
			{
				GameAnalytics.NewDesignEvent("GA:CriticalFPS", (float)GA_SpecialEvents._criticalFpsCount);
				GA_SpecialEvents._criticalFpsCount = 0;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000C918 File Offset: 0x0000AD18
		public void CheckCriticalFPS()
		{
			if (GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsCritical)
			{
				float num = Time.unscaledTime - this._lastUpdateCrit;
				if (num >= 1f)
				{
					float num2 = (float)this._frameCountCrit / num;
					this._lastUpdateCrit = Time.unscaledTime;
					this._frameCountCrit = 0;
					if (num2 <= (float)GameAnalytics.SettingsGA.FpsCriticalThreshold)
					{
						GA_SpecialEvents._criticalFpsCount++;
					}
				}
			}
		}

		// Token: 0x04000124 RID: 292
		private static int _frameCountAvg;

		// Token: 0x04000125 RID: 293
		private static float _lastUpdateAvg;

		// Token: 0x04000126 RID: 294
		private int _frameCountCrit;

		// Token: 0x04000127 RID: 295
		private float _lastUpdateCrit;

		// Token: 0x04000128 RID: 296
		private static int _criticalFpsCount;
	}
}
