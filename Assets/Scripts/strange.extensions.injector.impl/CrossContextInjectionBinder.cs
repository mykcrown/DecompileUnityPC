// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.injector.api;
using strange.framework.api;
using System;

namespace strange.extensions.injector.impl
{
	public class CrossContextInjectionBinder : InjectionBinder, ICrossContextInjectionBinder, IInjectionBinder, IInstanceProvider
	{
		public IInjectionBinder CrossContextBinder
		{
			get;
			set;
		}

		public override IInjectionBinding GetBinding<T>()
		{
			return this.GetBinding(typeof(T), null);
		}

		public override IInjectionBinding GetBinding<T>(object name)
		{
			return this.GetBinding(typeof(T), name);
		}

		public override IInjectionBinding GetBinding(object key)
		{
			return this.GetBinding(key, null);
		}

		public override IInjectionBinding GetBinding(object key, object name)
		{
			IInjectionBinding binding = base.GetBinding(key, name);
			if (binding == null && this.CrossContextBinder != null)
			{
				binding = this.CrossContextBinder.GetBinding(key, name);
			}
			return binding;
		}

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
