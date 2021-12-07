using System;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using strange.framework.api;
using strange.framework.impl;
using UnityEngine;

namespace strange.extensions.mediation.impl
{
	// Token: 0x0200025E RID: 606
	public class MediationBinder : Binder, IMediationBinder, IBinder
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x0005551C File Offset: 0x0005391C
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x00055524 File Offset: 0x00053924
		[Inject]
		public IInjectionBinder injectionBinder { get; set; }

		// Token: 0x06000C21 RID: 3105 RVA: 0x0005552D File Offset: 0x0005392D
		public override IBinding GetRawBinding()
		{
			return new MediationBinding(new Binder.BindingResolver(this.resolver));
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00055544 File Offset: 0x00053944
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

		// Token: 0x06000C23 RID: 3107 RVA: 0x000555B8 File Offset: 0x000539B8
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

		// Token: 0x06000C24 RID: 3108 RVA: 0x00055655 File Offset: 0x00053A55
		public new IMediationBinding Bind<T>()
		{
			return base.Bind<T>() as IMediationBinding;
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00055662 File Offset: 0x00053A62
		public IMediationBinding BindView<T>() where T : MonoBehaviour
		{
			return base.Bind<T>() as IMediationBinding;
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00055670 File Offset: 0x00053A70
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

		// Token: 0x06000C27 RID: 3111 RVA: 0x00055810 File Offset: 0x00053C10
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

		// Token: 0x06000C28 RID: 3112 RVA: 0x00055887 File Offset: 0x00053C87
		private void enableView(IView view)
		{
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00055889 File Offset: 0x00053C89
		private void disableView(IView view)
		{
		}
	}
}
