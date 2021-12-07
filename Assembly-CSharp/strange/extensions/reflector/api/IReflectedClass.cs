using System;
using System.Collections.Generic;
using System.Reflection;

namespace strange.extensions.reflector.api
{
	// Token: 0x0200026C RID: 620
	public interface IReflectedClass
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000C81 RID: 3201
		// (set) Token: 0x06000C82 RID: 3202
		ConstructorInfo Constructor { get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000C83 RID: 3203
		// (set) Token: 0x06000C84 RID: 3204
		Type[] ConstructorParameters { get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000C85 RID: 3205
		// (set) Token: 0x06000C86 RID: 3206
		object[] ConstructorParameterNames { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000C87 RID: 3207
		// (set) Token: 0x06000C88 RID: 3208
		MethodInfo[] PostConstructors { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000C89 RID: 3209
		// (set) Token: 0x06000C8A RID: 3210
		KeyValuePair<Type, PropertyInfo>[] Setters { get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000C8B RID: 3211
		// (set) Token: 0x06000C8C RID: 3212
		object[] SetterNames { get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000C8D RID: 3213
		// (set) Token: 0x06000C8E RID: 3214
		bool PreGenerated { get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000C8F RID: 3215
		// (set) Token: 0x06000C90 RID: 3216
		ConstructorInfo constructor { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000C91 RID: 3217
		// (set) Token: 0x06000C92 RID: 3218
		Type[] constructorParameters { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000C93 RID: 3219
		// (set) Token: 0x06000C94 RID: 3220
		MethodInfo[] postConstructors { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000C95 RID: 3221
		// (set) Token: 0x06000C96 RID: 3222
		KeyValuePair<Type, PropertyInfo>[] setters { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000C97 RID: 3223
		// (set) Token: 0x06000C98 RID: 3224
		object[] setterNames { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000C99 RID: 3225
		// (set) Token: 0x06000C9A RID: 3226
		bool preGenerated { get; set; }
	}
}
