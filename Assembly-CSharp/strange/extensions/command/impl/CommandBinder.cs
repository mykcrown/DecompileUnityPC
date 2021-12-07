using System;
using System.Collections.Generic;
using strange.extensions.command.api;
using strange.extensions.dispatcher.api;
using strange.extensions.injector.api;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using strange.framework.api;
using strange.framework.impl;

namespace strange.extensions.command.impl
{
	// Token: 0x02000218 RID: 536
	public class CommandBinder : Binder, ICommandBinder, IPooledCommandBinder, ITriggerable, IBinder
	{
		// Token: 0x06000A47 RID: 2631 RVA: 0x00051CA8 File Offset: 0x000500A8
		public CommandBinder()
		{
			this.usePooling = true;
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00051CD8 File Offset: 0x000500D8
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x00051CE0 File Offset: 0x000500E0
		[Inject]
		public IInjectionBinder injectionBinder { get; set; }

		// Token: 0x06000A4A RID: 2634 RVA: 0x00051CE9 File Offset: 0x000500E9
		public override IBinding GetRawBinding()
		{
			return new CommandBinding(new Binder.BindingResolver(this.resolver));
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00051CFD File Offset: 0x000500FD
		public virtual void ReactTo(object trigger)
		{
			this.ReactTo(trigger, null);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00051D08 File Offset: 0x00050108
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

		// Token: 0x06000A4D RID: 2637 RVA: 0x00051D84 File Offset: 0x00050184
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

		// Token: 0x06000A4E RID: 2638 RVA: 0x00051DDF File Offset: 0x000501DF
		protected virtual void disposeOfSequencedData(object data)
		{
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00051DE4 File Offset: 0x000501E4
		protected virtual ICommand invokeCommand(Type cmd, ICommandBinding binding, object data, int depth)
		{
			ICommand command = this.createCommand(cmd, data);
			command.sequenceId = depth;
			this.trackCommand(command, binding);
			this.executeCommand(command);
			return command;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00051E14 File Offset: 0x00050214
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

		// Token: 0x06000A51 RID: 2641 RVA: 0x00051E70 File Offset: 0x00050270
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

		// Token: 0x06000A52 RID: 2642 RVA: 0x00051F02 File Offset: 0x00050302
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

		// Token: 0x06000A53 RID: 2643 RVA: 0x00051F2E File Offset: 0x0005032E
		protected void executeCommand(ICommand command)
		{
			if (command == null)
			{
				return;
			}
			command.Execute();
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00051F40 File Offset: 0x00050340
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
					foreach (KeyValuePair<ICommand, ICommandBinding> keyValuePair in this.activeSequences)
					{
						if (keyValuePair.Value == commandBinding)
						{
							ICommand key2 = keyValuePair.Key;
							this.removeSequence(key2);
						}
					}
				}
			}
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00052008 File Offset: 0x00050408
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x000520BA File Offset: 0x000504BA
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x000520C2 File Offset: 0x000504C2
		public bool usePooling { get; set; }

		// Token: 0x06000A58 RID: 2648 RVA: 0x000520CC File Offset: 0x000504CC
		public Pool<T> GetPool<T>()
		{
			Type typeFromHandle = typeof(T);
			if (this.pools.ContainsKey(typeFromHandle))
			{
				return this.pools[typeFromHandle] as Pool<T>;
			}
			return null;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00052108 File Offset: 0x00050508
		private void removeSequence(ICommand command)
		{
			if (this.activeSequences.ContainsKey(command))
			{
				command.Cancel();
				this.activeSequences.Remove(command);
			}
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0005212E File Offset: 0x0005052E
		public bool Trigger<T>(object data)
		{
			return this.Trigger(typeof(T), data);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00052141 File Offset: 0x00050541
		public bool Trigger(object key, object data)
		{
			this.ReactTo(key, data);
			return true;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0005214C File Offset: 0x0005054C
		public new virtual ICommandBinding Bind<T>()
		{
			return base.Bind<T>() as ICommandBinding;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00052159 File Offset: 0x00050559
		public new virtual ICommandBinding Bind(object value)
		{
			return base.Bind(value) as ICommandBinding;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00052168 File Offset: 0x00050568
		protected override void resolver(IBinding binding)
		{
			base.resolver(binding);
			if (this.usePooling && (binding as ICommandBinding).isPooled && binding.value != null)
			{
				object[] array = binding.value as object[];
				foreach (Type type in array)
				{
					if (!this.pools.ContainsKey(type))
					{
						Pool value = this.makePoolFromType(type);
						this.pools[type] = value;
					}
				}
			}
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000521F8 File Offset: 0x000505F8
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

		// Token: 0x06000A60 RID: 2656 RVA: 0x00052273 File Offset: 0x00050673
		public new virtual ICommandBinding GetBinding<T>()
		{
			return base.GetBinding<T>() as ICommandBinding;
		}

		// Token: 0x04000708 RID: 1800
		protected Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

		// Token: 0x04000709 RID: 1801
		protected HashSet<ICommand> activeCommands = new HashSet<ICommand>();

		// Token: 0x0400070A RID: 1802
		protected Dictionary<ICommand, ICommandBinding> activeSequences = new Dictionary<ICommand, ICommandBinding>();
	}
}
