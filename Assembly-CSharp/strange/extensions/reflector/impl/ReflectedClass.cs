using System;
using System.Collections.Generic;
using System.Reflection;
using strange.extensions.reflector.api;

namespace strange.extensions.reflector.impl
{
	// Token: 0x0200026F RID: 623
	public class ReflectedClass : IReflectedClass
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x00055E5B File Offset: 0x0005425B
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x00055E63 File Offset: 0x00054263
		public ConstructorInfo Constructor { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00055E6C File Offset: 0x0005426C
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x00055E74 File Offset: 0x00054274
		public Type[] ConstructorParameters { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00055E7D File Offset: 0x0005427D
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x00055E85 File Offset: 0x00054285
		public object[] ConstructorParameterNames { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x00055E8E File Offset: 0x0005428E
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x00055E96 File Offset: 0x00054296
		public MethodInfo[] PostConstructors { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00055E9F File Offset: 0x0005429F
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x00055EA7 File Offset: 0x000542A7
		public KeyValuePair<Type, PropertyInfo>[] Setters { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x00055EB0 File Offset: 0x000542B0
		// (set) Token: 0x06000CA9 RID: 3241 RVA: 0x00055EB8 File Offset: 0x000542B8
		public object[] SetterNames { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00055EC1 File Offset: 0x000542C1
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x00055EC9 File Offset: 0x000542C9
		public bool PreGenerated { get; set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00055ED2 File Offset: 0x000542D2
		// (set) Token: 0x06000CAD RID: 3245 RVA: 0x00055EDA File Offset: 0x000542DA
		public ConstructorInfo constructor
		{
			get
			{
				return this.Constructor;
			}
			set
			{
				this.Constructor = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x00055EE3 File Offset: 0x000542E3
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x00055EEB File Offset: 0x000542EB
		public Type[] constructorParameters
		{
			get
			{
				return this.ConstructorParameters;
			}
			set
			{
				this.ConstructorParameters = value;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00055EF4 File Offset: 0x000542F4
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x00055EFC File Offset: 0x000542FC
		public MethodInfo[] postConstructors
		{
			get
			{
				return this.PostConstructors;
			}
			set
			{
				this.PostConstructors = value;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x00055F05 File Offset: 0x00054305
		// (set) Token: 0x06000CB3 RID: 3251 RVA: 0x00055F0D File Offset: 0x0005430D
		public KeyValuePair<Type, PropertyInfo>[] setters
		{
			get
			{
				return this.Setters;
			}
			set
			{
				this.Setters = value;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x00055F16 File Offset: 0x00054316
		// (set) Token: 0x06000CB5 RID: 3253 RVA: 0x00055F1E File Offset: 0x0005431E
		public object[] setterNames
		{
			get
			{
				return this.SetterNames;
			}
			set
			{
				this.SetterNames = value;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00055F27 File Offset: 0x00054327
		// (set) Token: 0x06000CB7 RID: 3255 RVA: 0x00055F2F File Offset: 0x0005432F
		public bool preGenerated
		{
			get
			{
				return this.PreGenerated;
			}
			set
			{
				this.PreGenerated = value;
			}
		}
	}
}
