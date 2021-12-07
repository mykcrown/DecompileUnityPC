// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace DevConsole
{
	[Serializable]
	internal struct Settings
	{
		public AnimationCurve curve;

		[Tooltip("When checked, logs from the Debug class and exceptions will be printed to the Console")]
		public bool showDebugLog;

		[Tooltip("Shows a time stamp on each message")]
		public bool showTimeStamp;

		[Tooltip("If none is set, the default one will be used")]
		public GUISkin skin;

		[Tooltip("If none is set, the default one will be used")]
		public Font font;
	}
}
