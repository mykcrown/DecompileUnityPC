using System;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.api;
using strange.extensions.sequencer.api;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.sequencer.impl
{
	// Token: 0x0200027B RID: 635
	public class Sequencer : CommandBinder, ISequencer, ITriggerable, ICommandBinder, IBinder
	{
		// Token: 0x06000CF2 RID: 3314 RVA: 0x000564B2 File Offset: 0x000548B2
		public override IBinding GetRawBinding()
		{
			return new SequenceBinding(new Binder.BindingResolver(this.resolver));
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000564C8 File Offset: 0x000548C8
		public override void ReactTo(object key, object data)
		{
			ISequenceBinding sequenceBinding = this.GetBinding(key) as ISequenceBinding;
			if (sequenceBinding != null)
			{
				this.nextInSequence(sequenceBinding, data, 0);
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000564F1 File Offset: 0x000548F1
		private void removeSequence(ISequenceCommand command)
		{
			if (this.activeSequences.ContainsKey(command))
			{
				command.Cancel();
				this.activeSequences.Remove(command);
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00056518 File Offset: 0x00054918
		private void invokeCommand(Type cmd, ISequenceBinding binding, object data, int depth)
		{
			ISequenceCommand sequenceCommand = this.createCommand(cmd, data);
			sequenceCommand.sequenceId = depth;
			this.trackCommand(sequenceCommand, binding);
			this.executeCommand(sequenceCommand);
			this.ReleaseCommand(sequenceCommand);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0005654C File Offset: 0x0005494C
		protected new virtual ISequenceCommand createCommand(object cmd, object data)
		{
			base.injectionBinder.Bind<ISequenceCommand>().To(cmd);
			ISequenceCommand instance = base.injectionBinder.GetInstance<ISequenceCommand>();
			instance.data = data;
			base.injectionBinder.Unbind<ISequenceCommand>();
			return instance;
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0005658A File Offset: 0x0005498A
		private void trackCommand(ISequenceCommand command, ISequenceBinding binding)
		{
			this.activeSequences[command] = binding;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00056599 File Offset: 0x00054999
		private void executeCommand(ISequenceCommand command)
		{
			if (command == null)
			{
				return;
			}
			command.Execute();
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000565A8 File Offset: 0x000549A8
		public void ReleaseCommand(ISequenceCommand command)
		{
			if (!command.retain && this.activeSequences.ContainsKey(command))
			{
				ISequenceBinding binding = this.activeSequences[command] as ISequenceBinding;
				object data = command.data;
				this.activeSequences.Remove(command);
				this.nextInSequence(binding, data, command.sequenceId + 1);
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00056608 File Offset: 0x00054A08
		private void nextInSequence(ISequenceBinding binding, object data, int depth)
		{
			object[] array = binding.value as object[];
			if (depth < array.Length)
			{
				Type cmd = array[depth] as Type;
				this.invokeCommand(cmd, binding, data, depth);
			}
			else if (binding.isOneOff)
			{
				this.Unbind(binding);
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00056654 File Offset: 0x00054A54
		private void failIf(bool condition, string message, SequencerExceptionType type)
		{
			if (condition)
			{
				throw new SequencerException(message, type);
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00056664 File Offset: 0x00054A64
		public new virtual ISequenceBinding Bind<T>()
		{
			return base.Bind<T>() as ISequenceBinding;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00056671 File Offset: 0x00054A71
		public new virtual ISequenceBinding Bind(object value)
		{
			return base.Bind(value) as ISequenceBinding;
		}
	}
}
