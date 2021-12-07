using System;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.sequencer.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.sequencer.impl
{
	// Token: 0x02000279 RID: 633
	public class SequenceBinding : CommandBinding, ISequenceBinding, ICommandBinding, IBinding
	{
		// Token: 0x06000CDE RID: 3294 RVA: 0x000566FF File Offset: 0x00054AFF
		public SequenceBinding()
		{
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00056707 File Offset: 0x00054B07
		public SequenceBinding(Binder.BindingResolver resolver) : base(resolver)
		{
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00056710 File Offset: 0x00054B10
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x00056718 File Offset: 0x00054B18
		public new bool isOneOff { get; set; }

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00056721 File Offset: 0x00054B21
		public new ISequenceBinding Once()
		{
			this.isOneOff = true;
			return this;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0005672B File Offset: 0x00054B2B
		public new ISequenceBinding Bind<T>()
		{
			return this.Bind<T>();
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00056733 File Offset: 0x00054B33
		public new ISequenceBinding Bind(object key)
		{
			return this.Bind(key);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0005673C File Offset: 0x00054B3C
		public new ISequenceBinding To<T>()
		{
			return this.To(typeof(T));
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00056750 File Offset: 0x00054B50
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

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0005679E File Offset: 0x00054B9E
		public new ISequenceBinding ToName<T>()
		{
			return base.ToName<T>() as ISequenceBinding;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000567AB File Offset: 0x00054BAB
		public new ISequenceBinding ToName(object o)
		{
			return base.ToName(o) as ISequenceBinding;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x000567B9 File Offset: 0x00054BB9
		public new ISequenceBinding Named<T>()
		{
			return base.Named<T>() as ISequenceBinding;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x000567C6 File Offset: 0x00054BC6
		public new ISequenceBinding Named(object o)
		{
			return base.Named(o) as ISequenceBinding;
		}
	}
}
