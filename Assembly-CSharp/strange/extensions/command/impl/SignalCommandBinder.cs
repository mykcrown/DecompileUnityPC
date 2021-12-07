using System;
using System.Collections.Generic;
using strange.extensions.command.api;
using strange.extensions.signal.api;
using strange.extensions.signal.impl;
using strange.framework.api;

namespace strange.extensions.command.impl
{
	// Token: 0x0200021D RID: 541
	public class SignalCommandBinder : CommandBinder
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x000526E8 File Offset: 0x00050AE8
		public override void ResolveBinding(IBinding binding, object key)
		{
			base.ResolveBinding(binding, key);
			if (this.bindings.ContainsKey(key))
			{
				IBaseSignal baseSignal = (IBaseSignal)key;
				baseSignal.AddListener(new Action<IBaseSignal, object[]>(this.ReactTo));
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00052728 File Offset: 0x00050B28
		public override void OnRemove()
		{
			foreach (object obj in this.bindings.Keys)
			{
				IBaseSignal baseSignal = (IBaseSignal)obj;
				if (baseSignal != null)
				{
					baseSignal.RemoveListener(new Action<IBaseSignal, object[]>(this.ReactTo));
				}
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x000527A4 File Offset: 0x00050BA4
		protected override ICommand invokeCommand(Type cmd, ICommandBinding binding, object data, int depth)
		{
			IBaseSignal baseSignal = (IBaseSignal)binding.key;
			ICommand command = this.createCommandForSignal(cmd, data, baseSignal.GetTypes());
			command.sequenceId = depth;
			base.trackCommand(command, binding);
			base.executeCommand(command);
			return command;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000527E4 File Offset: 0x00050BE4
		protected ICommand createCommandForSignal(Type cmd, object data, List<Type> signalTypes)
		{
			if (data != null)
			{
				object[] collection = (object[])data;
				HashSet<Type> hashSet = new HashSet<Type>();
				List<object> list = new List<object>(collection);
				foreach (Type type in signalTypes)
				{
					if (hashSet.Contains(type))
					{
						throw new SignalException(string.Concat(new object[]
						{
							"SignalCommandBinder: You have attempted to map more than one value of type: ",
							type,
							" in Command: ",
							cmd.GetType(),
							". Only the first value of a type will be injected. You may want to place your values in a VO, instead."
						}), SignalExceptionType.COMMAND_VALUE_CONFLICT);
					}
					bool flag = false;
					foreach (object obj in list)
					{
						if (obj == null)
						{
							throw new SignalException(string.Concat(new object[]
							{
								"SignalCommandBinder attempted to bind a null value from a signal to Command: ",
								cmd.GetType(),
								" to type: ",
								type
							}), SignalExceptionType.COMMAND_NULL_INJECTION);
						}
						if (type.IsAssignableFrom(obj.GetType()))
						{
							base.injectionBinder.Bind(type).ToValue(obj).ToInject(false);
							hashSet.Add(type);
							list.Remove(obj);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						throw new SignalException(string.Concat(new object[]
						{
							"Could not find an unused injectable value to inject in to Command: ",
							cmd.GetType(),
							" for Type: ",
							type
						}), SignalExceptionType.COMMAND_VALUE_NOT_FOUND);
					}
				}
			}
			ICommand command = base.getCommand(cmd);
			command.data = data;
			foreach (Type key in signalTypes)
			{
				base.injectionBinder.Unbind(key);
			}
			return command;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00052A18 File Offset: 0x00050E18
		public override ICommandBinding Bind<T>()
		{
			if (base.injectionBinder.GetBinding<T>() == null)
			{
				base.injectionBinder.Bind<T>().ToSingleton();
			}
			T instance = base.injectionBinder.GetInstance<T>();
			return base.Bind(instance);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00052A60 File Offset: 0x00050E60
		public override void Unbind<T>()
		{
			ICommandBinding commandBinding = (ICommandBinding)base.injectionBinder.GetBinding<T>();
			if (commandBinding != null)
			{
				T instance = base.injectionBinder.GetInstance<T>();
				this.Unbind(instance, null);
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00052AA0 File Offset: 0x00050EA0
		public override void Unbind(object key, object name)
		{
			if (this.bindings.ContainsKey(key))
			{
				IBaseSignal baseSignal = (IBaseSignal)key;
				baseSignal.RemoveListener(new Action<IBaseSignal, object[]>(this.ReactTo));
			}
			base.Unbind(key, name);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00052AE0 File Offset: 0x00050EE0
		public override ICommandBinding GetBinding<T>()
		{
			T instance = base.injectionBinder.GetInstance<T>();
			return base.GetBinding(instance) as ICommandBinding;
		}
	}
}
