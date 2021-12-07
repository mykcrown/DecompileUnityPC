// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameAnalyticsSDK.Events
{
	public class GA_SpecialEvents : MonoBehaviour
	{
		private sealed class _SubmitFPSRoutine_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal object _current;

			internal bool _disposing;

			internal int _PC;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public _SubmitFPSRoutine_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					break;
				case 1u:
					GA_SpecialEvents.SubmitFPS();
					break;
				default:
					return false;
				}
				if (Application.isPlaying && GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsAverage)
				{
					this._current = new WaitForSeconds(30f);
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._disposing = true;
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private sealed class _CheckCriticalFPSRoutine_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal GA_SpecialEvents _this;

			internal object _current;

			internal bool _disposing;

			internal int _PC;

			object IEnumerator<object>.Current
			{
				get
				{
					return this._current;
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			public _CheckCriticalFPSRoutine_c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					break;
				case 1u:
					this._this.CheckCriticalFPS();
					break;
				default:
					return false;
				}
				if (Application.isPlaying && GameAnalytics.SettingsGA != null && GameAnalytics.SettingsGA.SubmitFpsCritical)
				{
					this._current = new WaitForSeconds((float)GameAnalytics.SettingsGA.FpsCirticalSubmitInterval);
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				this._PC = -1;
				return false;
			}

			public void Dispose()
			{
				this._disposing = true;
				this._PC = -1;
			}

			public void Reset()
			{
				throw new NotSupportedException();
			}
		}

		private static int _frameCountAvg;

		private static float _lastUpdateAvg;

		private int _frameCountCrit;

		private float _lastUpdateCrit;

		private static int _criticalFpsCount;

		public void Start()
		{
			base.StartCoroutine(this.SubmitFPSRoutine());
			base.StartCoroutine(this.CheckCriticalFPSRoutine());
		}

		private IEnumerator SubmitFPSRoutine()
		{
			return new GA_SpecialEvents._SubmitFPSRoutine_c__Iterator0();
		}

		private IEnumerator CheckCriticalFPSRoutine()
		{
			GA_SpecialEvents._CheckCriticalFPSRoutine_c__Iterator1 _CheckCriticalFPSRoutine_c__Iterator = new GA_SpecialEvents._CheckCriticalFPSRoutine_c__Iterator1();
			_CheckCriticalFPSRoutine_c__Iterator._this = this;
			return _CheckCriticalFPSRoutine_c__Iterator;
		}

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
	}
}
