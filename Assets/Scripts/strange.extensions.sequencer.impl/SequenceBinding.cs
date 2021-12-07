// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.sequencer.api;
using strange.framework.api;
using strange.framework.impl;
using System;

namespace strange.extensions.sequencer.impl
{
	public class SequenceBinding : CommandBinding, ISequenceBinding, ICommandBinding, IBinding
	{
		public new bool isOneOff
		{
			get;
			set;
		}

		public SequenceBinding()
		{
		}

		public SequenceBinding(Binder.BindingResolver resolver) : base(resolver)
		{
		}

		public new ISequenceBinding Once()
		{
			this.isOneOff = true;
			return this;
		}

		public new ISequenceBinding Bind<T>()
		{
			return this.Bind<T>();
		}

		public new ISequenceBinding Bind(object key)
		{
			return this.Bind(key);
		}

		public new ISequenceBinding To<T>()
		{
			return this.To(typeof(T));
		}

		public new ISequenceBinding To(object o)
		{
			Type type = o as Type;
			Type typeFromHandle = typeof(ISequenceCommand);
			if (!typeFromHandle.IsAssignableFrom(type))
			{
				throw new SequencerException("Attempt to bind a non SequenceCommand to a Sequence. Perhaps your command needs to extend SequenceCommand or implement ISequenCommand?\n\tType: " + type.ToString(), SequencerExceptionType.COMMAND_USED_IN_SEQUENCE);
			}
			return base.To(o) as ISequenceBinding;
		}

		public new ISequenceBinding ToName<T>()
		{
			return base.ToName<T>() as ISequenceBinding;
		}

		public new ISequenceBinding ToName(object o)
		{
			return base.ToName(o) as ISequenceBinding;
		}

		public new ISequenceBinding Named<T>()
		{
			return base.Named<T>() as ISequenceBinding;
		}

		public new ISequenceBinding Named(object o)
		{
			return base.Named(o) as ISequenceBinding;
		}
	}
}
