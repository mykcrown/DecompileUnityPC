// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.framework.api;
using strange.framework.impl;
using System;
using UnityEngine;

namespace strange.extensions.mediation.impl
{
	public class MediationBinder : Binder, IMediationBinder, IBinder
	{
		[Inject]
		public IInjectionBinder injectionBinder
		{
			get;
			set;
		}

		public IBinding GetRawBinding()
		{
			return new MediationBinding(new Binder.BindingResolver(this.resolver));
		}

		public void Trigger(MediationEvent evt, IView view)
		{
			Type type = view.GetType();
			IMediationBinding mediationBinding = this.GetBinding(type) as IMediationBinding;
			if (mediationBinding != null)
			{
				if (evt != MediationEvent.AWAKE)
				{
					if (evt == MediationEvent.DESTROYED)
					{
						this.unmapView(view, mediationBinding);
					}
				}
				else
				{
					this.injectViewAndChildren(view);
					this.mapView(view, mediationBinding);
				}
			}
			else if (evt == MediationEvent.AWAKE)
			{
				this.injectViewAndChildren(view);
			}
		}

		protected virtual void injectViewAndChildren(IView view)
		{
			MonoBehaviour monoBehaviour = view as MonoBehaviour;
			Component[] componentsInChildren = monoBehaviour.GetComponentsInChildren(typeof(IView), true);
			int num = componentsInChildren.Length;
			for (int i = num - 1; i > -1; i--)
			{
				IView view2 = componentsInChildren[i] as IView;
				if (view2 != null)
				{
					if (!view2.autoRegisterWithContext || !view2.registeredWithContext)
					{
						view2.registeredWithContext = true;
						if (!view2.Equals(monoBehaviour))
						{
							this.Trigger(MediationEvent.AWAKE, view2);
						}
					}
				}
			}
			this.injectionBinder.injector.Inject(monoBehaviour, false);
		}

		public new IMediationBinding Bind<T>()
		{
			return base.Bind<T>() as IMediationBinding;
		}

		public IMediationBinding BindView<T>() where T : MonoBehaviour
		{
			return base.Bind<T>() as IMediationBinding;
		}

		protected virtual void mapView(IView view, IMediationBinding binding)
		{
			Type type = view.GetType();
			if (this.bindings.ContainsKey(type))
			{
				object[] array = binding.value as object[];
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					MonoBehaviour monoBehaviour = view as MonoBehaviour;
					Type type2 = array[i] as Type;
					if (type2 == type)
					{
						throw new MediationException(type + "mapped to itself. The result would be a stack overflow.", MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
					}
					MonoBehaviour monoBehaviour2 = monoBehaviour.gameObject.AddComponent(type2) as MonoBehaviour;
					if (monoBehaviour2 == null)
					{
						throw new MediationException(string.Concat(new string[]
						{
							"The view: ",
							type.ToString(),
							" is mapped to mediator: ",
							type2.ToString(),
							". AddComponent resulted in null, which probably means ",
							type2.ToString().Substring(type2.ToString().LastIndexOf(".") + 1),
							" is not a MonoBehaviour."
						}), MediationExceptionType.NULL_MEDIATOR);
					}
					if (monoBehaviour2 is IMediator)
					{
						((IMediator)monoBehaviour2).PreRegister();
					}
					Type key = (binding.abstraction != null && !binding.abstraction.Equals(BindingConst.NULLOID)) ? (binding.abstraction as Type) : type;
					this.injectionBinder.Bind(key).ToValue(view).ToInject(false);
					this.injectionBinder.injector.Inject(monoBehaviour2);
					this.injectionBinder.Unbind(key);
					if (monoBehaviour2 is IMediator)
					{
						((IMediator)monoBehaviour2).OnRegister();
					}
				}
			}
		}

		protected virtual void unmapView(IView view, IMediationBinding binding)
		{
			Type type = view.GetType();
			if (this.bindings.ContainsKey(type))
			{
				object[] array = binding.value as object[];
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					Type type2 = array[i] as Type;
					MonoBehaviour monoBehaviour = view as MonoBehaviour;
					IMediator mediator = monoBehaviour.GetComponent(type2) as IMediator;
					if (mediator != null)
					{
						mediator.OnRemove();
					}
				}
			}
		}

		private void enableView(IView view)
		{
		}

		private void disableView(IView view)
		{
		}
	}
}
