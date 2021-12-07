using System;
using strange.extensions.command.api;

namespace strange.extensions.command.impl
{
	// Token: 0x0200021A RID: 538
	public class CommandException : Exception
	{
		// Token: 0x06000A75 RID: 2677 RVA: 0x0005257C File Offset: 0x0005097C
		public CommandException()
		{
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00052584 File Offset: 0x00050984
		public CommandException(string message, CommandExceptionType exceptionType) : base(message)
		{
			this.type = exceptionType;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00052594 File Offset: 0x00050994
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0005259C File Offset: 0x0005099C
		public CommandExceptionType type { get; set; }
	}
}
