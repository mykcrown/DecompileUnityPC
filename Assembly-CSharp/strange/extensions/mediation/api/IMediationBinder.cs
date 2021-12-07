using System;
using strange.framework.api;
using UnityEngine;

namespace strange.extensions.mediation.api
{
	// Token: 0x02000256 RID: 598
	public interface IMediationBinder : IBinder
	{
		// Token: 0x06000C04 RID: 3076
		void Trigger(MediationEvent evt, IView view);

		// Token: 0x06000C05 RID: 3077
		IMediationBinding Bind<T>();

		// Token: 0x06000C06 RID: 3078
		IMediationBinding BindView<T>() where T : MonoBehaviour;
	}
}
