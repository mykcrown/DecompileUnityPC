using System;
using strange.extensions.injector.api;
using strange.framework.api;

namespace strange.extensions.injector.impl
{
	// Token: 0x0200024B RID: 587
	public class CrossContextInjectionBinder : InjectionBinder, ICrossContextInjectionBinder, IInjectionBinder, IInstanceProvider
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x00054624 File Offset: 0x00052A24
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x0005462C File Offset: 0x00052A2C
		public IInjectionBinder CrossContextBinder { get; set; }

		// Token: 0x06000BAB RID: 2987 RVA: 0x00054635 File Offset: 0x00052A35
		public override IInjectionBinding GetBinding<T>()
		{
			return this.GetBinding(typeof(T), null);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00054648 File Offset: 0x00052A48
		public override IInjectionBinding GetBinding<T>(object name)
		{
			return this.GetBinding(typeof(T), name);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0005465B File Offset: 0x00052A5B
		public override IInjectionBinding GetBinding(object key)
		{
			return this.GetBinding(key, null);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00054668 File Offset: 0x00052A68
		public override IInjectionBinding GetBinding(object key, object name)
		{
			IInjectionBinding binding = base.GetBinding(key, name);
			if (binding == null && this.CrossContextBinder != null)
			{
				binding = this.CrossContextBinder.GetBinding(key, name);
			}
			return binding;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x000546A0 File Offset: 0x00052AA0
		public override void ResolveBinding(IBinding binding, object key)
		{
			if (binding is IInjectionBinding)
			{
				InjectionBinding injectionBinding = (InjectionBinding)binding;
				if (injectionBinding.isCrossContext)
				{
					if (this.CrossContextBinder == null)
					{
						base.ResolveBinding(binding, key);
					}
					else
					{
						this.Unbind(key, binding.name);
						this.CrossContextBinder.ResolveBinding(binding, key);
					}
				}
				else
				{
					base.ResolveBinding(binding, key);
				}
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00054709 File Offset: 0x00052B09
		protected override IInjector GetInjectorForBinding(IInjectionBinding binding)
		{
			if (binding.isCrossContext && this.CrossContextBinder != null)
			{
				return this.CrossContextBinder.injector;
			}
			return base.injector;
		}
	}
}
