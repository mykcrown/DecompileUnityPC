// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.framework.api;
using strange.framework.impl;
using System;

namespace strange.extensions.command.impl
{
	public class CommandBinding : Binding, ICommandBinding, IBinding
	{
		public bool isOneOff
		{
			get;
			set;
		}

		public bool isSequence
		{
			get;
			set;
		}

		public bool isPooled
		{
			get;
			set;
		}

		public CommandBinding()
		{
		}

		public CommandBinding(Binder.BindingResolver resolver) : base(resolver)
		{
		}

		public ICommandBinding Once()
		{
			this.isOneOff = true;
			return this;
		}

		public ICommandBinding InParallel()
		{
			this.isSequence = false;
			return this;
		}

		public ICommandBinding InSequence()
		{
			this.isSequence = true;
			return this;
		}

		public ICommandBinding Pooled()
		{
			this.isPooled = true;
			this.resolver(this);
			return this;
		}

		public new ICommandBinding Bind<T>()
		{
			return base.Bind<T>() as ICommandBinding;
		}

		public new ICommandBinding Bind(object key)
		{
			return base.Bind(key) as ICommandBinding;
		}

		public new ICommandBinding To<T>()
		{
			return base.To<T>() as ICommandBinding;
		}

		public new ICommandBinding To(object o)
		{
			return base.To(o) as ICommandBinding;
		}

		public new ICommandBinding ToName<T>()
		{
			return base.ToName<T>() as ICommandBinding;
		}

		public new ICommandBinding ToName(object o)
		{
			return base.ToName(o) as ICommandBinding;
		}

		public new ICommandBinding Named<T>()
		{
			return base.Named<T>() as ICommandBinding;
		}

		public new ICommandBinding Named(object o)
		{
			return base.Named(o) as ICommandBinding;
		}
	}
}
