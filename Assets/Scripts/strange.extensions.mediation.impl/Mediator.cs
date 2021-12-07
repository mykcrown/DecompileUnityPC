// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.mediation.api;
using System;
using UnityEngine;

namespace strange.extensions.mediation.impl
{
	public class Mediator : MonoBehaviour, IMediator
	{
		[Inject(ContextKeys.CONTEXT_VIEW)]
		public GameObject contextView
		{
			get;
			set;
		}

		public virtual void PreRegister()
		{
		}

		public virtual void OnRegister()
		{
		}

		public virtual void OnRemove()
		{
		}
	}
}
