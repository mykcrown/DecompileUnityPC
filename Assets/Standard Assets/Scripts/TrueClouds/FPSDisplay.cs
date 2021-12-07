// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Diagnostics;
using UnityEngine;

namespace TrueClouds
{
	public class FPSDisplay : MonoBehaviour
	{
		private string _text;

		private Stopwatch _stopwatch;

		private float _delta;

		private void Update()
		{
			this._delta = Mathf.Lerp(this._delta, Time.unscaledDeltaTime, 1f);
			float num = 1f / this._delta;
			this._text = string.Format("{0:0.0} ms ({1:0.} fps)", this._delta * 1000f, num);
		}

		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect(10f, 10f, 300f, 20f));
			GUILayout.Label(this._text, Array.Empty<GUILayoutOption>());
			GUILayout.EndArea();
		}
	}
}
