// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.extensions.dispatcher.api;
using strange.extensions.injector.api;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using strange.framework.api;
using strange.framework.impl;
using System;
using System.Collections.Generic;

namespace strange.extensions.command.impl
{
	public class CommandBinder : Binder, ICommandBinder, IPooledCommandBinder, ITriggerable, IBinder
	{
		protected Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

		protected HashSet<ICommand> activeCommands = new HashSet<ICommand>();

		protected Dictionary<ICommand, ICommandBinding> activeSequences = new Dictionary<ICommand, ICommandBinding>();

		[Inject]
		public IInjectionBinder injectionBinder
		{
			get;
			set;
		}

		public bool usePooling
		{
			get;
			set;
		}

		public CommandBinder()
		{
			this.usePooling = true;
		}

		public override IBinding GetRawBinding()
		{
			return new CommandBinding(new Binder.BindingResolver(this.resolver));
		}

		public virtual void ReactTo(object trigger)
		{
			this.ReactTo(trigger, null);
		}

		public virtual void ReactTo(object trigger, object data)
		{
			if (data is IPoolable)
			{
				(data as IPoolable).Retain();
			}
			ICommandBinding commandBinding = this.GetBinding(trigger) as ICommandBinding;
			if (commandBinding != null)
			{
				if (commandBinding.isSequence)
				{
					this.next(commandBinding, data, 0);
				}
				else
				{
					object[] array = commandBinding.value as object[];
					int num = array.Length + 1;
					for (int i = 0; i < num; i++)
					{
						this.next(commandBinding, data, i);
					}
				}
			}
		}

		protected void next(ICommandBinding binding, object data, int depth)
		{
			object[] array = binding.value as object[];
			if (depth < array.Length)
			{
				Type cmd = array[depth] as Type;
				ICommand command = this.invokeCommand(cmd, binding, data, depth);
				this.ReleaseCommand(command);
			}
			else
			{
				this.disposeOfSequencedData(data);
				if (binding.isOneOff)
				{
					this.Unbind(binding);
				}
			}
		}

		protected virtual void disposeOfSequencedData(object data)
		{
		}

		protected virtual ICommand invokeCommand(Type cmd, ICommandBinding binding, object data, int depth)
		{
			ICommand command = this.createCommand(cmd, data);
			command.sequenceId = depth;
			this.trackCommand(command, binding);
			this.executeCommand(command);
			return command;
		}

		protected virtual ICommand createCommand(object cmd, object data)
		{
			ICommand command = this.getCommand(cmd as Type);
			if (command == null)
			{
				string text = "A Command ";
				if (data != null)
				{
					text = text + "tied to data " + data.ToString();
				}
				text += " could not be instantiated.\nThis might be caused by a null pointer during instantiation or failing to override Execute (generally you shouldn't have constructor code in Commands).";
				throw new CommandException(text, CommandExceptionType.BAD_CONSTRUCTOR);
			}
			command.data = data;
			return command;
		}

		protected ICommand getCommand(Type type)
		{
			if (this.usePooling && this.pools.ContainsKey(type))
			{
				Pool pool = this.pools[type];
				ICommand command = pool.GetInstance() as ICommand;
				if (command.IsClean)
				{
					this.injectionBinder.injector.Inject(command);
					command.IsClean = false;
				}
				return command;
			}
			this.injectionBinder.Bind<ICommand>().To(type);
			ICommand instance = this.injectionBinder.GetInstance<ICommand>();
			this.injectionBinder.Unbind<ICommand>();
			return instance;
		}

		protected void trackCommand(ICommand command, ICommandBinding binding)
		{
			if (binding.isSequence)
			{
				this.activeSequences.Add(command, binding);
			}
			else
			{
				this.activeCommands.Add(command);
			}
		}

		protected void executeCommand(ICommand command)
		{
			if (command == null)
			{
				return;
			}
			command.Execute();
		}

		public virtual void Stop(object key)
		{
			if (key is ICommand && this.activeSequences.ContainsKey(key as ICommand))
			{
				this.removeSequence(key as ICommand);
			}
			else
			{
				ICommandBinding commandBinding = this.GetBinding(key) as ICommandBinding;
				if (commandBinding != null && this.activeSequences.ContainsValue(commandBinding))
				{
					foreach (KeyValuePair<ICommand, ICommandBinding> current in this.activeSequences)
					{
						if (current.Value == commandBinding)
						{
							ICommand key2 = current.Key;
							this.removeSequence(key2);
						}
					}
				}
			}
		}

		public virtual void ReleaseCommand(ICommand command)
		{
			if (!command.retain)
			{
				Type type = command.GetType();
				if (this.usePooling && this.pools.ContainsKey(type))
				{
					this.pools[type].ReturnInstance(command);
				}
				if (this.activeCommands.Contains(command))
				{
					this.activeCommands.Remove(command);
				}
				else if (this.activeSequences.ContainsKey(command))
				{
					ICommandBinding binding = this.activeSequences[command];
					object data = command.data;
					this.activeSequences.Remove(command);
					this.next(binding, data, command.sequenceId + 1);
				}
			}
		}

		public Pool<T> GetPool<T>()
		{
			Type typeFromHandle = typeof(T);
			if (this.pools.ContainsKey(typeFromHandle))
			{
				return this.pools[typeFromHandle] as Pool<T>;
			}
			return null;
		}

		private void removeSequence(ICommand command)
		{
			if (this.activeSequences.ContainsKey(command))
			{
				command.Cancel();
				this.activeSequences.Remove(command);
			}
		}

		public bool Trigger<T>(object data)
		{
			return this.Trigger(typeof(T), data);
		}

		public bool Trigger(object key, object data)
		{
			this.ReactTo(key, data);
			return true;
		}

		public new virtual ICommandBinding Bind<T>()
		{
			return base.Bind<T>() as ICommandBinding;
		}

		public new virtual ICommandBinding Bind(object value)
		{
			return base.Bind(value) as ICommandBinding;
		}

		protected override void resolver(IBinding binding)
		{
			base.resolver(binding);
			if (this.usePooling && (binding as ICommandBinding).isPooled && binding.value != null)
			{
				object[] array = binding.value as object[];
				object[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					Type type = (Type)array2[i];
					if (!this.pools.ContainsKey(type))
					{
						Pool value = this.makePoolFromType(type);
						this.pools[type] = value;
					}
				}
			}
		}

		protected virtual Pool makePoolFromType(Type type)
		{
			Type o = typeof(Pool<>).MakeGenericType(new Type[]
			{
				type
			});
			this.injectionBinder.Bind(type).To(type);
			this.injectionBinder.Bind<Pool>().To(o).ToName(CommandKeys.COMMAND_POOL);
			Pool instance = this.injectionBinder.GetInstance<Pool>(CommandKeys.COMMAND_POOL);
			this.injectionBinder.Unbind<Pool>(CommandKeys.COMMAND_POOL);
			return instance;
		}

		public new virtual ICommandBinding GetBinding<T>()
		{
			return base.GetBinding<T>() as ICommandBinding;
		}
	}
}
