// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.pool.api;
using System;

namespace strange.extensions.command.impl
{
	public class EventCommandBinder : CommandBinder
	{
		protected override ICommand createCommand(object cmd, object data)
		{
			base.injectionBinder.Bind<ICommand>().To(cmd);
			if (data is IEvent)
			{
				base.injectionBinder.Bind<IEvent>().ToValue(data).ToInject(false);
			}
			ICommand instance = base.injectionBinder.GetInstance<ICommand>();
			try
			{
				if (instance == null)
				{
					string text = "A Command ";
					if (data is IEvent)
					{
						IEvent @event = (IEvent)data;
						text = text + "tied to event " + @event.type;
					}
					text += " could not be instantiated.\nThis might be caused by a null pointer during instantiation or failing to override Execute (generally you shouldn't have constructor code in Commands).";
					throw new CommandException(text, CommandExceptionType.BAD_CONSTRUCTOR);
				}
				instance.data = data;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (data is IEvent)
				{
					base.injectionBinder.Unbind<IEvent>();
				}
				base.injectionBinder.Unbind<ICommand>();
			}
			return instance;
		}

		protected override void disposeOfSequencedData(object data)
		{
			if (data is IPoolable)
			{
				(data as IPoolable).Release();
			}
		}
	}
}
