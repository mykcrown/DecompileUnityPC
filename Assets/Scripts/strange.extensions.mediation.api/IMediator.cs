// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

namespace strange.extensions.mediation.api
{
	public interface IMediator
	{
		GameObject contextView
		{
			get;
			set;
		}

		void PreRegister();

		void OnRegister();

		void OnRemove();
	}
}
