// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.mediation.api;
using System;
using UnityEngine;

namespace strange.extensions.context.impl
{
	public class ContextView : MonoBehaviour, IContextView, IView
	{
		public IContext context
		{
			get;
			set;
		}

		public bool requiresContext
		{
			get;
			set;
		}

		public bool registeredWithContext
		{
			get;
			set;
		}

		public bool autoRegisterWithContext
		{
			get;
			set;
		}

		protected virtual void OnDestroy()
		{
			if (this.context != null && Context.firstContext != null)
			{
				Context.firstContext.RemoveContext(this.context);
			}
		}
	}
}
