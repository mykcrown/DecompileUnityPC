// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.extensions.signal.api;
using strange.extensions.signal.impl;
using strange.framework.api;
using System;
using System.Collections.Generic;

namespace strange.extensions.command.impl
{
	public class SignalCommandBinder : CommandBinder
	{
		public override void ResolveBinding(IBinding binding, object key)
		{
			base.ResolveBinding(binding, key);
			if (this.bindings.ContainsKey(key))
			{
				IBaseSignal baseSignal = (IBaseSignal)key;
				baseSignal.AddListener(new Action<IBaseSignal, object[]>(this.ReactTo));
			}
		}

		public override void OnRemove()
		{
			foreach (object current in this.bindings.Keys)
			{
				IBaseSignal baseSignal = (IBaseSignal)current;
				if (baseSignal != null)
				{
					baseSignal.RemoveListener(new Action<IBaseSignal, object[]>(this.ReactTo));
				}
			}
		}

		protected override ICommand invokeCommand(Type cmd, ICommandBinding binding, object data, int depth)
		{
			IBaseSignal baseSignal = (IBaseSignal)binding.key;
			ICommand command = this.createCommandForSignal(cmd, data, baseSignal.GetTypes());
			command.sequenceId = depth;
			base.trackCommand(command, binding);
			base.executeCommand(command);
			return command;
		}

		protected ICommand createCommandForSignal(Type cmd, object data, List<Type> signalTypes)
		{
			if (data != null)
			{
				object[] collection = (object[])data;
				HashSet<Type> hashSet = new HashSet<Type>();
				List<object> list = new List<object>(collection);
				foreach (Type current in signalTypes)
				{
					if (hashSet.Contains(current))
					{
						throw new SignalException(string.Concat(new object[]
						{
							"SignalCommandBinder: You have attempted to map more than one value of type: ",
							current,
							" in Command: ",
							cmd.GetType(),
							". Only the first value of a type will be injected. You may want to place your values in a VO, instead."
						}), SignalExceptionType.COMMAND_VALUE_CONFLICT);
					}
					bool flag = false;
					foreach (object current2 in list)
					{
						if (current2 == null)
						{
							throw new SignalException(string.Concat(new object[]
							{
								"SignalCommandBinder attempted to bind a null value from a signal to Command: ",
								cmd.GetType(),
								" to type: ",
								current
							}), SignalExceptionType.COMMAND_NULL_INJECTION);
						}
						if (current.IsAssignableFrom(current2.GetType()))
						{
							base.injectionBinder.Bind(current).ToValue(current2).ToInject(false);
							hashSet.Add(current);
							list.Remove(current2);
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
							current
						}), SignalExceptionType.COMMAND_VALUE_NOT_FOUND);
					}
				}
			}
			ICommand command = base.getCommand(cmd);
			command.data = data;
			foreach (Type current3 in signalTypes)
			{
				base.injectionBinder.Unbind(current3);
			}
			return command;
		}

		public override ICommandBinding Bind<T>()
		{
			if (base.injectionBinder.GetBinding<T>() == null)
			{
				base.injectionBinder.Bind<T>().ToSingleton();
			}
			T instance = base.injectionBinder.GetInstance<T>();
			return base.Bind(instance);
		}

		public override void Unbind<T>()
		{
			ICommandBinding commandBinding = (ICommandBinding)base.injectionBinder.GetBinding<T>();
			if (commandBinding != null)
			{
				T instance = base.injectionBinder.GetInstance<T>();
				this.Unbind(instance, null);
			}
		}

		public override void Unbind(object key, object name)
		{
			if (this.bindings.ContainsKey(key))
			{
				IBaseSignal baseSignal = (IBaseSignal)key;
				baseSignal.RemoveListener(new Action<IBaseSignal, object[]>(this.ReactTo));
			}
			base.Unbind(key, name);
		}

		public override ICommandBinding GetBinding<T>()
		{
			T instance = base.injectionBinder.GetInstance<T>();
			return base.GetBinding(instance) as ICommandBinding;
		}
	}
}
