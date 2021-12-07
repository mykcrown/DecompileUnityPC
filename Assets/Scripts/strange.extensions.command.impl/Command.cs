// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using strange.extensions.injector.api;
using strange.extensions.pool.api;
using System;

namespace strange.extensions.command.impl
{
	public class Command : ICommand, IPoolable
	{
		[Inject]
		public ICommandBinder commandBinder
		{
			get;
			set;
		}

		[Inject]
		public IInjectionBinder injectionBinder
		{
			get;
			set;
		}

		public object data
		{
			get;
			set;
		}

		public bool cancelled
		{
			get;
			set;
		}

		public bool IsClean
		{
			get;
			set;
		}

		public int sequenceId
		{
			get;
			set;
		}

		public bool retain
		{
			get;
			set;
		}

		public Command()
		{
			this.IsClean = false;
		}

		public virtual void Execute()
		{
			throw new CommandException("You must override the Execute method in every Command", CommandExceptionType.EXECUTE_OVERRIDE);
		}

		public virtual void Retain()
		{
			this.retain = true;
		}

		public virtual void Release()
		{
			this.retain = false;
			if (this.commandBinder != null)
			{
				this.commandBinder.ReleaseCommand(this);
			}
		}

		public virtual void Restore()
		{
			this.injectionBinder.injector.Uninject(this);
			this.IsClean = true;
		}

		public virtual void Fail()
		{
			if (this.commandBinder != null)
			{
				this.commandBinder.Stop(this);
			}
		}

		public void Cancel()
		{
			this.cancelled = true;
		}
	}
}
