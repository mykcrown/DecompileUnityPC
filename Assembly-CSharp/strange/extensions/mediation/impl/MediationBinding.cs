using System;
using strange.extensions.mediation.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.mediation.impl
{
	// Token: 0x0200025F RID: 607
	public class MediationBinding : Binding, IMediationBinding, IBinding
	{
		// Token: 0x06000C2A RID: 3114 RVA: 0x0005588B File Offset: 0x00053C8B
		public MediationBinding(Binder.BindingResolver resolver) : base(resolver)
		{
			this._abstraction = new SemiBinding();
			this._abstraction.constraint = BindingConstraintType.ONE;
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000558B0 File Offset: 0x00053CB0
		IMediationBinding IMediationBinding.ToMediator<T>()
		{
			return base.To(typeof(T)) as IMediationBinding;
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000558C8 File Offset: 0x00053CC8
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

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0005595B File Offset: 0x00053D5B
		public object abstraction
		{
			get
			{
				return (this._abstraction.value != null) ? this._abstraction.value : BindingConst.NULLOID;
			}
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x00055983 File Offset: 0x00053D83
		public new IMediationBinding Bind<T>()
		{
			return base.Bind<T>() as IMediationBinding;
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00055990 File Offset: 0x00053D90
		public new IMediationBinding Bind(object key)
		{
			return base.Bind(key) as IMediationBinding;
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x0005599E File Offset: 0x00053D9E
		public new IMediationBinding To<T>()
		{
			return base.To<T>() as IMediationBinding;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000559AB File Offset: 0x00053DAB
		public new IMediationBinding To(object o)
		{
			return base.To(o) as IMediationBinding;
		}

		// Token: 0x0400078D RID: 1933
		protected ISemiBinding _abstraction;
	}
}
