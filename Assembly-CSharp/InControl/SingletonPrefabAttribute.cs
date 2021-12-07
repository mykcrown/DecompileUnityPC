using System;

namespace InControl
{
	// Token: 0x020001D6 RID: 470
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class SingletonPrefabAttribute : Attribute
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x0004ACBC File Offset: 0x000490BC
		public SingletonPrefabAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x040005E2 RID: 1506
		public readonly string Name;
	}
}
