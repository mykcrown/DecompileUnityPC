// Decompile from assembly: Assembly-CSharp.dll

using strange.framework.api;
using System;
using UnityEngine;

namespace strange.extensions.mediation.api
{
	public interface IMediationBinder : IBinder
	{
		void Trigger(MediationEvent evt, IView view);

		IMediationBinding Bind<T>();

		IMediationBinding BindView<T>() where T : MonoBehaviour;
	}
}
