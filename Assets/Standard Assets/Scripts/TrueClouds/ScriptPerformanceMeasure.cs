// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace TrueClouds
{
	internal class ScriptPerformanceMeasure : MonoBehaviour
	{
		private sealed class _MeasureCoroutine_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _enabledId___0;

			internal int _disabledId___0;

			internal int _i___1;

			internal float _percent___2;

			internal float _time___2;

			internal ScriptPerformanceMeasure _this;

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

			public _MeasureCoroutine_c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this._enabledId___0 = 0;
					this._disabledId___0 = 0;
					this._i___1 = 0;
					break;
				case 1u:
					this._time___2 = Time.unscaledTime;
					this._current = this._this.WaitForFrames(this._this.BatchDurationInFrames);
					if (!this._disposing)
					{
						this._PC = 2;
					}
					return true;
				case 2u:
					this._time___2 = (Time.unscaledTime - this._time___2) * 1000f / (float)this._this.BatchDurationInFrames;
					if (this._this._isScriptEnabled[this._i___1])
					{
						this._this._enabledTimes[this._enabledId___0++] = this._time___2;
					}
					else
					{
						this._this._disabledlTimes[this._disabledId___0++] = this._time___2;
					}
					this._i___1++;
					break;
				default:
					return false;
				}
				if (this._i___1 < this._this.BatchCount * 2)
				{
					this._percent___2 = (float)(100 * this._i___1 / (this._this.BatchCount * 2));
					this._this._testResult = string.Format("Measured {0}%", this._percent___2);
					this._this.Target.enabled = this._this._isScriptEnabled[this._i___1];
					this._current = null;
					if (!this._disposing)
					{
						this._PC = 1;
					}
					return true;
				}
				this._this.SetTimeString();
				this._this.Target.enabled = true;
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

		private sealed class _WaitForFrames_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			internal int _i___1;

			internal int frames;

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

			public _WaitForFrames_c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this._PC;
				this._PC = -1;
				switch (num)
				{
				case 0u:
					this._i___1 = 0;
					break;
				case 1u:
					this._i___1++;
					break;
				default:
					return false;
				}
				if (this._i___1 < this.frames)
				{
					Thread.Sleep(16);
					this._current = null;
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

		public MonoBehaviour Target;

		public int BatchDurationInFrames = 10;

		public int BatchCount = 40;

		private string _testResult = "Not measured";

		private List<bool> _isScriptEnabled = new List<bool>();

		private float[] _enabledTimes;

		private float[] _disabledlTimes;

		private GUIStyle _labelStyle;

		private bool _wasMeasureLaunched;

		private static System.Random rnd = new System.Random();

		private void Start()
		{
			this._labelStyle = new GUIStyle("label")
			{
				fontSize = 20
			};
			this._enabledTimes = new float[this.BatchCount];
			this._disabledlTimes = new float[this.BatchCount];
			for (int i = 0; i < this.BatchCount; i++)
			{
				this._isScriptEnabled.Add(true);
				this._isScriptEnabled.Add(false);
			}
			ScriptPerformanceMeasure.Shuffle<bool>(this._isScriptEnabled);
		}

		private IEnumerator MeasureCoroutine()
		{
			ScriptPerformanceMeasure._MeasureCoroutine_c__Iterator0 _MeasureCoroutine_c__Iterator = new ScriptPerformanceMeasure._MeasureCoroutine_c__Iterator0();
			_MeasureCoroutine_c__Iterator._this = this;
			return _MeasureCoroutine_c__Iterator;
		}

		private void SetTimeString()
		{
			Array.Sort<float>(this._enabledTimes);
			Array.Sort<float>(this._disabledlTimes);
			float[] array = new float[this.BatchCount];
			for (int i = 0; i < this.BatchCount; i++)
			{
				array[i] = this._enabledTimes[i] - this._disabledlTimes[i];
			}
			Array.Sort<float>(array);
			float num = array[this.BatchCount * 50 / 100];
			float num2 = array[this.BatchCount * 90 / 100];
			this._testResult = string.Format("3d Cloud rendering takes {0} ms per frame. 90% of time rendering was faster than {1} ms", num.ToString("F4"), num2.ToString("F4"));
		}

		private IEnumerator WaitForFrames(int frames)
		{
			ScriptPerformanceMeasure._WaitForFrames_c__Iterator1 _WaitForFrames_c__Iterator = new ScriptPerformanceMeasure._WaitForFrames_c__Iterator1();
			_WaitForFrames_c__Iterator.frames = frames;
			return _WaitForFrames_c__Iterator;
		}

		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect(10f, 40f, 1000f, 30f));
			if (this._wasMeasureLaunched)
			{
				GUILayout.Label(this._testResult, this._labelStyle, Array.Empty<GUILayoutOption>());
			}
			else if (GUILayout.Button("Measure Performance", new GUILayoutOption[]
			{
				GUILayout.Width(150f)
			}))
			{
				base.StartCoroutine(this.MeasureCoroutine());
				this._wasMeasureLaunched = true;
			}
			GUILayout.EndArea();
		}

		private static void Shuffle<T>(List<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = ScriptPerformanceMeasure.rnd.Next(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}
	}
}
