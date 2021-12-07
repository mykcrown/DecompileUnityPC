// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.mediation.api;
using strange.framework.api;
using strange.framework.impl;
using System;

namespace strange.extensions.mediation.impl
{
	public class MediationBinding : Binding, IMediationBinding, IBinding
	{
		protected ISemiBinding _abstraction;

		public object abstraction
		{
			get
			{
				return (this._abstraction.value != null) ? this._abstraction.value : BindingConst.NULLOID;
			}
		}

		public MediationBinding(Binder.BindingResolver resolver) : base(resolver)
		{
			this._abstraction = new SemiBinding();
			this._abstraction.constraint = BindingConstraintType.ONE;
		}

		IMediationBinding IMediationBinding.ToMediator<T>()
		{
			return base.To(typeof(T)) as IMediationBinding;
		}

		IMediationBinding IMediationBinding.ToAbstraction<T>()
		{
			Type typeFromHandle = typeof(T);
			if (base.key != null)
			{
				Type c = base.key as Type;
				if (!typeFromHandle.IsAssignableFrom(c))
				{
					throw new MediationException(string.Concat(new string[]
					{
						"The View ",
						base.key.ToString(),
						" has been bound to the abstraction ",
						typeof(T).ToString(),
						" which the View neither extends nor implements. "
					}), MediationExceptionType.VIEW_NOT_ASSIGNABLE);
				}
			}
			this._abstraction.Add(typeFromHandle);
			return this;
		}

		public new IMediationBinding Bind<T>()
		{
			return base.Bind<T>() as IMediationBinding;
		}

		public new IMediationBinding Bind(object key)
		{
			return base.Bind(key) as IMediationBinding;
		}

		public new IMediationBinding To<T>()
		{
			return base.To<T>() as IMediationBinding;
		}

		public new IMediationBinding To(object o)
		{
			return base.To(o) as IMediationBinding;
		}
	}
}
