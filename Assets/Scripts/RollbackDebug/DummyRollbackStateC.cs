// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace RollbackDebug
{
	[Serializable]
	internal class DummyRollbackStateC : RollbackState
	{
		public Vector3 vector;

		public DummyRollbackStateC()
		{
		}

		public DummyRollbackStateC(Vector3 v)
		{
			this.vector = v;
		}
	}
}
