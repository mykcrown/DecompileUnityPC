// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.mediation.api;
using System;
using UnityEngine;

namespace strange.extensions.mediation.impl
{
	public class View : MonoBehaviour, IView
	{
		private bool _requiresContext = true;

		protected bool registerWithContext = true;

		public bool requiresContext
		{
			get
			{
				return this._requiresContext;
			}
			set
			{
				this._requiresContext = value;
			}
		}

		public virtual bool autoRegisterWithContext
		{
			get
			{
				return this.registerWithContext;
			}
			set
			{
				this.registerWithContext = value;
			}
		}

		public bool registeredWithContext
		{
			get;
			set;
		}

		protected virtual void Awake()
		{
			if (this.autoRegisterWithContext && !this.registeredWithContext)
			{
				this.bubbleToContext(this, true, false);
			}
		}

		protected virtual void Start()
		{
			if (this.autoRegisterWithContext && !this.registeredWithContext)
			{
				this.bubbleToContext(this, true, true);
			}
		}

		protected virtual void OnDestroy()
		{
			this.bubbleToContext(this, false, false);
		}

		protected virtual void bubbleToContext(MonoBehaviour view, bool toAdd, bool finalTry)
		{
			int num = 0;
			Transform transform = view.gameObject.transform;
			while (transform.parent != null && num < 100)
			{
				num++;
				transform = transform.parent;
				if (transform.gameObject.GetComponent<ContextView>() != null)
				{
					ContextView component = transform.gameObject.GetComponent<ContextView>();
					if (component.context != null)
					{
						IContext context = component.context;
						if (toAdd)
						{
							context.AddView(view);
							this.registeredWithContext = true;
							return;
						}
						context.RemoveView(view);
						return;
					}
				}
			}
			if (!this.requiresContext || !finalTry)
			{
				return;
			}
			if (Context.firstContext != null)
			{
				Context.firstContext.AddView(view);
				this.registeredWithContext = true;
				return;
			}
			string text = (num != 100) ? "A view was added with no context. Views must be added into the hierarchy of their ContextView lest all hell break loose." : "A view couldn't find a context. Loop limit reached.";
			text = text + "\nView: " + view.ToString();
			throw new MediationException(text, MediationExceptionType.NO_CONTEXT);
		}
	}
}
