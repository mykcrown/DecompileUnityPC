// Decompile from assembly: Assembly-CSharp.dll

using strange.extensions.implicitBind.api;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using strange.extensions.mediation.api;
using strange.extensions.mediation.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace strange.extensions.implicitBind.impl
{
	public class ImplicitBinder : IImplicitBinder
	{
		private sealed class ImplicitBindingVO
		{
			public List<Type> BindTypes = new List<Type>();

			public Type ToType;

			public bool IsCrossContext;

			public object Name;

			public ImplicitBindingVO(Type bindType, Type toType, bool isCrossContext, object name)
			{
				this.BindTypes.Add(bindType);
				this.ToType = toType;
				this.IsCrossContext = isCrossContext;
				this.Name = name;
			}

			public ImplicitBindingVO(List<Type> bindTypes, Type toType, bool isCrossContext, object name)
			{
				this.BindTypes = bindTypes;
				this.ToType = toType;
				this.IsCrossContext = isCrossContext;
				this.Name = name;
			}
		}

		private sealed class _ScanForAnnotatedClasses_c__AnonStorey0
		{
			internal string[] usingNamespaces;
		}

		private sealed class _ScanForAnnotatedClasses_c__AnonStorey1
		{
			internal int ns;

			internal ImplicitBinder._ScanForAnnotatedClasses_c__AnonStorey0 __f__ref_0;

			internal bool __m__0(Type t)
			{
				return !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith(this.__f__ref_0.usingNamespaces[this.ns]);
			}
		}

		private Assembly assembly;

		[Inject]
		public IInjectionBinder injectionBinder
		{
			get;
			set;
		}

		[Inject]
		public IMediationBinder mediationBinder
		{
			get;
			set;
		}

		[PostConstruct]
		public void PostConstruct()
		{
			this.assembly = Assembly.GetExecutingAssembly();
		}

		public virtual void ScanForAnnotatedClasses(string[] usingNamespaces)
		{
			ImplicitBinder._ScanForAnnotatedClasses_c__AnonStorey0 _ScanForAnnotatedClasses_c__AnonStorey = new ImplicitBinder._ScanForAnnotatedClasses_c__AnonStorey0();
			_ScanForAnnotatedClasses_c__AnonStorey.usingNamespaces = usingNamespaces;
			if (this.assembly != null)
			{
				IEnumerable<Type> exportedTypes = this.assembly.GetExportedTypes();
				List<Type> list = new List<Type>();
				int num = _ScanForAnnotatedClasses_c__AnonStorey.usingNamespaces.Length;
				ImplicitBinder._ScanForAnnotatedClasses_c__AnonStorey1 _ScanForAnnotatedClasses_c__AnonStorey2 = new ImplicitBinder._ScanForAnnotatedClasses_c__AnonStorey1();
				_ScanForAnnotatedClasses_c__AnonStorey2.__f__ref_0 = _ScanForAnnotatedClasses_c__AnonStorey;
				_ScanForAnnotatedClasses_c__AnonStorey2.ns = 0;
				while (_ScanForAnnotatedClasses_c__AnonStorey2.ns < num)
				{
					list.AddRange(exportedTypes.Where(new Func<Type, bool>(_ScanForAnnotatedClasses_c__AnonStorey2.__m__0)));
					_ScanForAnnotatedClasses_c__AnonStorey2.ns++;
				}
				List<ImplicitBinder.ImplicitBindingVO> list2 = new List<ImplicitBinder.ImplicitBindingVO>();
				List<ImplicitBinder.ImplicitBindingVO> list3 = new List<ImplicitBinder.ImplicitBindingVO>();
				foreach (Type current in list)
				{
					object[] customAttributes = current.GetCustomAttributes(typeof(Implements), true);
					object[] customAttributes2 = current.GetCustomAttributes(typeof(ImplementedBy), true);
					object[] customAttributes3 = current.GetCustomAttributes(typeof(MediatedBy), true);
					object[] customAttributes4 = current.GetCustomAttributes(typeof(Mediates), true);
					if (customAttributes2.Any<object>())
					{
						ImplementedBy implementedBy = (ImplementedBy)customAttributes2.First<object>();
						if (!implementedBy.DefaultType.GetInterfaces().Contains(current))
						{
							throw new InjectionException("Default Type: " + implementedBy.DefaultType.Name + " does not implement annotated interface " + current.Name, InjectionExceptionType.IMPLICIT_BINDING_IMPLEMENTOR_DOES_NOT_IMPLEMENT_INTERFACE);
						}
						list3.Add(new ImplicitBinder.ImplicitBindingVO(current, implementedBy.DefaultType, implementedBy.Scope == InjectionBindingScope.CROSS_CONTEXT, null));
					}
					if (customAttributes.Any<object>())
					{
						Type[] interfaces = current.GetInterfaces();
						object obj = null;
						bool flag = false;
						List<Type> list4 = new List<Type>();
						object[] array = customAttributes;
						for (int i = 0; i < array.Length; i++)
						{
							Implements implements = (Implements)array[i];
							if (implements.DefaultInterface != null)
							{
								if (!interfaces.Contains(implements.DefaultInterface) && !(current == implements.DefaultInterface))
								{
									throw new InjectionException("Annotated type " + current.Name + " does not implement Default Interface " + implements.DefaultInterface.Name, InjectionExceptionType.IMPLICIT_BINDING_TYPE_DOES_NOT_IMPLEMENT_DESIGNATED_INTERFACE);
								}
								list4.Add(implements.DefaultInterface);
							}
							else
							{
								list4.Add(current);
							}
							flag = (flag || implements.Scope == InjectionBindingScope.CROSS_CONTEXT);
							obj = (obj ?? implements.Name);
						}
						ImplicitBinder.ImplicitBindingVO item = new ImplicitBinder.ImplicitBindingVO(list4, current, flag, obj);
						list2.Add(item);
					}
					Type type = null;
					Type type2 = null;
					if (customAttributes3.Any<object>())
					{
						type2 = current;
						type = ((MediatedBy)customAttributes3.First<object>()).MediatorType;
						if (type == null)
						{
							throw new MediationException("Cannot implicitly bind view of type: " + current.Name + " due to null MediatorType", MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
						}
					}
					else if (customAttributes4.Any<object>())
					{
						type = current;
						type2 = ((Mediates)customAttributes4.First<object>()).ViewType;
						if (type2 == null)
						{
							throw new MediationException("Cannot implicitly bind Mediator of type: " + current.Name + " due to null ViewType", MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
						}
					}
					if (this.mediationBinder != null && type2 != null && type != null)
					{
						this.mediationBinder.Bind(type2).To(type);
					}
				}
				list3.ForEach(new Action<ImplicitBinder.ImplicitBindingVO>(this.Bind));
				list2.ForEach(new Action<ImplicitBinder.ImplicitBindingVO>(this.Bind));
				return;
			}
			throw new InjectionException("Assembly was not initialized yet for Implicit Bindings!", InjectionExceptionType.UNINITIALIZED_ASSEMBLY);
		}

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
	}
}
