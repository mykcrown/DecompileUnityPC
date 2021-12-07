using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using strange.extensions.implicitBind.api;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;

namespace strange.extensions.implicitBind.impl
{
	// Token: 0x0200023D RID: 573
	public class ImplicitBinder : IImplicitBinder
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00053C71 File Offset: 0x00052071
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x00053C79 File Offset: 0x00052079
		[Inject]
		public IInjectionBinder injectionBinder { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00053C82 File Offset: 0x00052082
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x00053C8A File Offset: 0x0005208A
		[Inject]
		public IMediationBinder mediationBinder { get; set; }

		// Token: 0x06000B54 RID: 2900 RVA: 0x00053C93 File Offset: 0x00052093
		[PostConstruct]
		public void PostConstruct()
		{
			this.assembly = Assembly.GetExecutingAssembly();
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00053CA0 File Offset: 0x000520A0
		public virtual void ScanForAnnotatedClasses(string[] usingNamespaces)
		{
			if (this.assembly != null)
			{
				IEnumerable<Type> exportedTypes = this.assembly.GetExportedTypes();
				List<Type> list = new List<Type>();
				int num = usingNamespaces.Length;
				int ns;
				for (ns = 0; ns < num; ns++)
				{
					list.AddRange(from t in exportedTypes
					where !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith(usingNamespaces[ns])
					select t);
				}
				List<ImplicitBinder.ImplicitBindingVO> list2 = new List<ImplicitBinder.ImplicitBindingVO>();
				List<ImplicitBinder.ImplicitBindingVO> list3 = new List<ImplicitBinder.ImplicitBindingVO>();
				foreach (Type type in list)
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(Implements), true);
					object[] customAttributes2 = type.GetCustomAttributes(typeof(ImplementedBy), true);
					object[] customAttributes3 = type.GetCustomAttributes(typeof(MediatedBy), true);
					object[] customAttributes4 = type.GetCustomAttributes(typeof(Mediates), true);
					if (customAttributes2.Any<object>())
					{
						ImplementedBy implementedBy = (ImplementedBy)customAttributes2.First<object>();
						if (!implementedBy.DefaultType.GetInterfaces().Contains(type))
						{
							throw new InjectionException("Default Type: " + implementedBy.DefaultType.Name + " does not implement annotated interface " + type.Name, InjectionExceptionType.IMPLICIT_BINDING_IMPLEMENTOR_DOES_NOT_IMPLEMENT_INTERFACE);
						}
						list3.Add(new ImplicitBinder.ImplicitBindingVO(type, implementedBy.DefaultType, implementedBy.Scope == InjectionBindingScope.CROSS_CONTEXT, null));
					}
					if (customAttributes.Any<object>())
					{
						Type[] interfaces = type.GetInterfaces();
						object obj = null;
						bool flag = false;
						List<Type> list4 = new List<Type>();
						foreach (Implements implements in customAttributes)
						{
							if (implements.DefaultInterface != null)
							{
								if (!interfaces.Contains(implements.DefaultInterface) && !(type == implements.DefaultInterface))
								{
									throw new InjectionException("Annotated type " + type.Name + " does not implement Default Interface " + implements.DefaultInterface.Name, InjectionExceptionType.IMPLICIT_BINDING_TYPE_DOES_NOT_IMPLEMENT_DESIGNATED_INTERFACE);
								}
								list4.Add(implements.DefaultInterface);
							}
							else
							{
								list4.Add(type);
							}
							flag = (flag || implements.Scope == InjectionBindingScope.CROSS_CONTEXT);
							obj = (obj ?? implements.Name);
						}
						ImplicitBinder.ImplicitBindingVO item = new ImplicitBinder.ImplicitBindingVO(list4, type, flag, obj);
						list2.Add(item);
					}
					Type type2 = null;
					Type type3 = null;
					if (customAttributes3.Any<object>())
					{
						type3 = type;
						type2 = ((MediatedBy)customAttributes3.First<object>()).MediatorType;
						if (type2 == null)
						{
							throw new MediationException("Cannot implicitly bind view of type: " + type.Name + " due to null MediatorType", MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
						}
					}
					else if (customAttributes4.Any<object>())
					{
						type2 = type;
						type3 = ((Mediates)customAttributes4.First<object>()).ViewType;
						if (type3 == null)
						{
							throw new MediationException("Cannot implicitly bind Mediator of type: " + type.Name + " due to null ViewType", MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
						}
					}
					if (this.mediationBinder != null && type3 != null && type2 != null)
					{
						this.mediationBinder.Bind(type3).To(type2);
					}
				}
				list3.ForEach(new Action<ImplicitBinder.ImplicitBindingVO>(this.Bind));
				list2.ForEach(new Action<ImplicitBinder.ImplicitBindingVO>(this.Bind));
				return;
			}
			throw new InjectionException("Assembly was not initialized yet for Implicit Bindings!", InjectionExceptionType.UNINITIALIZED_ASSEMBLY);
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00054090 File Offset: 0x00052490
		private void Bind(ImplicitBinder.ImplicitBindingVO toBind)
		{
			IInjectionBinding injectionBinding = this.injectionBinder.Bind(toBind.BindTypes.First<Type>());
			injectionBinding.Weak();
			for (int i = 1; i < toBind.BindTypes.Count; i++)
			{
				Type key = toBind.BindTypes.ElementAt(i);
				injectionBinding.Bind(key);
			}
			injectionBinding = ((!(toBind.ToType != null)) ? injectionBinding.ToName(toBind.Name).ToSingleton() : injectionBinding.To(toBind.ToType).ToName(toBind.Name).ToSingleton());
			if (toBind.IsCrossContext)
			{
				injectionBinding.CrossContext();
			}
		}

		// Token: 0x0400074B RID: 1867
		private Assembly assembly;

		// Token: 0x0200023E RID: 574
		private sealed class ImplicitBindingVO
		{
			// Token: 0x06000B57 RID: 2903 RVA: 0x00054142 File Offset: 0x00052542
			public ImplicitBindingVO(Type bindType, Type toType, bool isCrossContext, object name)
			{
				this.BindTypes.Add(bindType);
				this.ToType = toType;
				this.IsCrossContext = isCrossContext;
				this.Name = name;
			}

			// Token: 0x06000B58 RID: 2904 RVA: 0x00054177 File Offset: 0x00052577
			public ImplicitBindingVO(List<Type> bindTypes, Type toType, bool isCrossContext, object name)
			{
				this.BindTypes = bindTypes;
				this.ToType = toType;
				this.IsCrossContext = isCrossContext;
				this.Name = name;
			}

			// Token: 0x0400074C RID: 1868
			public List<Type> BindTypes = new List<Type>();

			// Token: 0x0400074D RID: 1869
			public Type ToType;

			// Token: 0x0400074E RID: 1870
			public bool IsCrossContext;

			// Token: 0x0400074F RID: 1871
			public object Name;
		}
	}
}
