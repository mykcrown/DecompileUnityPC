// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.api;
using strange.extensions.sequencer.api;
using strange.framework.api;
using strange.framework.impl;
using System;

namespace strange.extensions.sequencer.impl
{
	public class Sequencer : CommandBinder, ISequencer, ITriggerable, ICommandBinder, IBinder
	{
		public override IBinding GetRawBinding()
		{
			return new SequenceBinding(new Binder.BindingResolver(this.resolver));
		}

		public override void ReactTo(object key, object data)
		{
			ISequenceBinding sequenceBinding = this.GetBinding(key) as ISequenceBinding;
			if (sequenceBinding != null)
			{
				this.nextInSequence(sequenceBinding, data, 0);
			}
		}

		private void removeSequence(ISequenceCommand command)
		{
			if (this.activeSequences.ContainsKey(command))
			{
				command.Cancel();
				this.activeSequences.Remove(command);
			}
		}

		private void invokeCommand(Type cmd, ISequenceBinding binding, object data, int depth)
		{
			ISequenceCommand sequenceCommand = this.createCommand(cmd, data);
			sequenceCommand.sequenceId = depth;
			this.trackCommand(sequenceCommand, binding);
			this.executeCommand(sequenceCommand);
			this.ReleaseCommand(sequenceCommand);
		}

		protected new virtual ISequenceCommand createCommand(object cmd, object data)
		{
			base.injectionBinder.Bind<ISequenceCommand>().To(cmd);
			ISequenceCommand instance = base.injectionBinder.GetInstance<ISequenceCommand>();
			instance.data = data;
			base.injectionBinder.Unbind<ISequenceCommand>();
			return instance;
		}

		private void trackCommand(ISequenceCommand command, ISequenceBinding binding)
		{
			this.activeSequences[command] = binding;
		}

		private void executeCommand(ISequenceCommand command)
		{
			if (command == null)
			{
				return;
			}
			command.Execute();
		}

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

		private void failIf(bool condition, string message, SequencerExceptionType type)
		{
			if (condition)
			{
				throw new SequencerException(message, type);
			}
		}

		public new virtual ISequenceBinding Bind<T>()
		{
			return base.Bind<T>() as ISequenceBinding;
		}

		public new virtual ISequenceBinding Bind(object value)
		{
			return base.Bind(value) as ISequenceBinding;
		}
	}
}
