// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.command.api;
using System;

namespace strange.extensions.command.impl
{
	public class CommandException : Exception
	{
		public CommandExceptionType type
		{
			get;
			set;
		}

		public CommandException()
		{
		}

		public CommandException(string message, CommandExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}
	}
}
